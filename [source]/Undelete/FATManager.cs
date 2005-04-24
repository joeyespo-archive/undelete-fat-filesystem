using System;
using System.IO;
using System.Text;
using System.Collections;


namespace Undelete
{
  public class FATManager
  {
    private LogicalDevice owner;
    private uint currentCluster;
    
    public FATManager (LogicalDevice owner)
    {
      if (!owner.Partition.IsFAT) throw new ArgumentException("Logical device must be a compatible FAT device.");
      this.owner = owner;
      currentCluster = 0;
    }
    
    public LogicalDevice Owner
    { get { return owner; } }
    
    public Stream DeviceStream
    { get { return owner.DeviceStream; } }
    
    public string Path
    {
      get
      {
        while (true)
        {
          string path = @"\";
          foreach (FileEntry file in EnumerateDirectories())
          {
            if (file.FormattedName == "..")
            {
              currentCluster = (uint)(file.FirstCluster);
              return path;
            }
          }
        }
      }
    }
    
    
    public FileEntry CurrentDirectory
    { get { return EnumerateFileEntries(currentCluster, false, false, false, true, false)[0]; } }
    
    public void ChangeDirectory (string name)
    {
      foreach (FileEntry file in EnumerateDirectories())
      {
        if (file.FormattedName == name.ToLower())
        {
          currentCluster = (uint)(file.FirstCluster);
          return;
        }
      }
      throw new DirectoryNotFoundException("Directory was not found");
    }
    
    public FileEntry [] EnumerateFileEntries ()
    { return EnumerateFileEntries(currentCluster, true, true, false, false, false); }
    public FileEntry [] EnumerateFileEntries (bool includeFiles, bool includeDirectories, bool deleted)
    { return EnumerateFileEntries(currentCluster, includeFiles, includeDirectories, deleted, false, false); }
    public FileEntry [] EnumerateFiles ()
    { return EnumerateFileEntries(currentCluster, true, false, false, false, false); }
    public FileEntry [] EnumerateDirectories ()
    { return EnumerateFileEntries(currentCluster, false, true, false, false, false); }
    public FileEntry [] EnumerateDeletedFileEntries ()
    { return EnumerateFileEntries(currentCluster, true, true, true, false, false); }
    public FileEntry [] EnumerateDeletedFiles ()
    { return EnumerateFileEntries(currentCluster, true, false, true, false, false); }
    public FileEntry [] EnumerateDeletedDirectories ()
    { return EnumerateFileEntries(currentCluster, false, true, true, false, false); }
    
    private FileEntry [] EnumerateFileEntries (uint clusterNumber, bool includeFiles, bool includeDirectories, bool deleted, bool currentOnly, bool parentOnly)
    {
      ArrayList files = new ArrayList();
      uint sector = (uint)(owner.RootDirectorySector + (( (( clusterNumber == 0 )?( 0 ):( clusterNumber + 2 )) ) * owner.BootSector.SectorsPerCluster));
      
      // Jump to next directory sector
      DeviceStream.Seek((owner.Partition.RelativeSector + sector) * owner.BootSector.BytesPerSector, SeekOrigin.Begin);
      while (true)
      {
        FileEntry file = (FileEntry)StreamActivator.CreateInstance(typeof(FileEntry), DeviceStream);
        if (file.FileAttributes == FileAttributes.LongFileName) continue;
        if (file.Name[0] == 0x00) break;  // No more entries
        
        if (currentOnly)
        {
          if ((clusterNumber == 0) && ((file.FileAttributes & FileAttributes.VolumeId) != 0)) files.Add(file);
          else if (file.Name == ".          ") files.Add(file);
          continue;
        }
        if (parentOnly)
        {
          if (file.Name == "..         ") files.Add(file);
          continue;
        }
        // Skip the volume id
        if ((file.FileAttributes & FileAttributes.VolumeId) != 0) continue;
        
        // Skip deleted/non-deleted entries
        if ((file.Name[0] == '?') != deleted) continue;
        
        // Show info
        bool isDirectory = ((file.FileAttributes & FileAttributes.Directory) != 0);
        if ((isDirectory) && (!includeDirectories)) continue;
        if ((!isDirectory) && (!includeFiles)) continue;
        files.Add(file);
      }
      return (FileEntry [])files.ToArray(typeof(FileEntry));
    }
    
    public string ReadFileAsString (FileEntry file)
    {
      byte [] data = ReadFile(file);
      char [] contents = new char[data.Length];
      Encoding.ASCII.GetDecoder().GetChars(data, 0, data.Length, contents, 0);
      return new string(contents);
    }
    
    public byte [] ReadFile (FileEntry file)
    {
      ushort nextCluster = file.FirstCluster;
      uint sizeLeft = file.FileSize;
      uint clusterSize = (uint)(PhysicalDevice.SectorSize * owner.BootSector.SectorsPerCluster);
      uint sector;
      
      int offset = 0;
      byte [] data = new Byte [file.FileSize];
      while (true)
      {
        // Calculate next sector
        sector = (uint)(owner.RootDirectorySector + (( (( nextCluster == 0 )?( 0 ):( nextCluster + 2 )) ) * owner.BootSector.SectorsPerCluster));
        // Jump to data sector
        DeviceStream.Seek((owner.Partition.RelativeSector + sector) * owner.BootSector.BytesPerSector, SeekOrigin.Begin);
        // Read correct size
        if (sizeLeft < clusterSize)
        { DeviceStream.Read(data, offset, (int)sizeLeft); break; }
        else
        { DeviceStream.Read(data, offset, (int)clusterSize); offset += (int)clusterSize; sizeLeft -= clusterSize; }
        // Get next cluster
        nextCluster = owner.FAT[nextCluster];
        if ((nextCluster == 0x0000) || (nextCluster == 0xFFF7)) break;    // Bad cluster pointer
        if ((nextCluster >= 0xFFF8) && (nextCluster <= 0xFFFF)) break;    // End of cluster of file
      }
      return data;
    }
    
    public void SaveFile (FileEntry file, Stream stream)
    {
      ushort nextCluster = file.FirstCluster;
      uint sizeLeft = file.FileSize;
      uint clusterSize = (uint)(PhysicalDevice.SectorSize * owner.BootSector.SectorsPerCluster);
      uint sector;
      
      byte [] data = new Byte [clusterSize];
      while (true)
      {
        // Calculate next sector
        sector = (uint)(owner.RootDirectorySector + (( (( nextCluster == 0 )?( 0 ):( nextCluster + 2 )) ) * owner.BootSector.SectorsPerCluster));
        // Jump to data sector
        DeviceStream.Seek((owner.Partition.RelativeSector + sector) * owner.BootSector.BytesPerSector, SeekOrigin.Begin);
        // Read correct size
        if (sizeLeft < clusterSize)
        {
          DeviceStream.Read(data, 0, (int)sizeLeft);
          stream.Write(data, 0, (int)sizeLeft);
          break;
        }
        else
        {
          DeviceStream.Read(data, 0, (int)clusterSize);
          stream.Write(data, 0, (int)clusterSize);
          sizeLeft -= clusterSize;
        }
        // Get next cluster
        nextCluster = owner.FAT[nextCluster];
        if ((nextCluster == 0x0000) || (nextCluster == 0xFFF7)) break;    // Bad cluster pointer
        if ((nextCluster >= 0xFFF8) && (nextCluster <= 0xFFFF)) break;    // End of cluster of file
      }
    }
  }
}
