using System;
using System.IO;
using System.Text;

namespace Undelete
{
  public enum PartitionId
  {
    None,
    Unknown,
    FAT12,
    FAT16,
    FAT32,
    Extended,
    NTFS,
  }
  
  [Flags]
  public enum FileAttributes : byte
  {
    None      = 0x00,
    ReadOnly  = 0x01,
    Hidden    = 0x02,
    System    = 0x04,
    VolumeId  = 0x08,
    Directory = 0x10,
    Archive   = 0x20,
    LongFileName = 0x0F,
  }
  
  // Partition entry structure
  public struct PartitionTable
  {
    public byte BootInd;
    public byte StartingHead;
    public ushort StartingSectorCylinder;
    public byte SysId;
    public byte EndingHead;
    public ushort EndingSectorCylinder;
    public uint RelativeSector;
    public uint NumberSectors;
    
    // Returns true if files system is FAT (NOT FAT32)
    public bool IsFAT
    { get { return (PartitionId == PartitionId.FAT12) || (PartitionId == PartitionId.FAT16); } }
    
    public PartitionId PartitionId
    {
      get
      {
        switch (SysId)
        {
          case 0x00: return PartitionId.None;
          case 0x01: return PartitionId.FAT12;
          case 0x05: return PartitionId.Extended;
          case 0x04: case 0x06: case 0x0E: return PartitionId.FAT16;
          case 0x0B: case 0x0C: return PartitionId.FAT32;
          case 0x07: return PartitionId.NTFS;
          default: return PartitionId.Unknown;
        }
      }
    }
    
    public string PartitionLabel
    {
      get
      {
        switch (PartitionId)
        {
          case PartitionId.None: return "None";
          case PartitionId.FAT12: return "FAT12";
          case PartitionId.FAT16: return "FAT16";
          case PartitionId.FAT32: return "FAT32";
          case PartitionId.Extended: return "Extended";
          case PartitionId.NTFS: return "NTFS";
          default: return "Unknown";
        }
      }
    }
    
    public ushort StartingSector
    { get { return (ushort)(StartingSectorCylinder & 0x3F); } }
    public ushort StartingCylinder
    {
      get
      {
        byte hi = ((byte)(((ushort)(StartingSectorCylinder) >> 8) & 0xFF));
        byte lo = ((byte)(StartingSectorCylinder));
        return (ushort)(hi | ((lo & 0xC0) << 2));
      }
    }
    
    public ushort EndingSector
    { get { return (ushort)(EndingSectorCylinder & 0x3F); } }
    public ushort EndingCylinder
    {
      get
      {
        byte hi = ((byte)(((ushort)(EndingSectorCylinder) >> 8) & 0xFF));
        byte lo = ((byte)(EndingSectorCylinder));
        return (ushort)(hi | ((lo & 0xC0) << 2));
      }
    }
    
    public ushort Cylinders
    { get { return (ushort)(EndingCylinder - StartingCylinder); } }
    public ushort Heads
    { get { return (ushort)(EndingHead - StartingHead); } }
    public ushort Sectors
    { get { return (ushort)(EndingSector - StartingSector); } }
  }
  
  // Partition boot sector structure (FAT12 or FAT16)
  public class BootSector
  {
    [ArrayLength(3)]
    public byte []jmp;
    [ArrayLength(8)]
    public string OemName;
    
    // BIOS parameter block (BPB)
    public ushort BytesPerSector;
    public byte   SectorsPerCluster;
    public ushort ReservedSectors;
    public byte   NumberOfFATs;
    public ushort RootEntries;
    public ushort Sectors16;
    public byte   MediaType;
    public ushort SectorsPerFAT;
    public ushort SectorsPerTrack;
    public ushort Heads;
    public uint   HiddenSectors;  // Same as RelativeSectors in the Partition Table
    public uint   Sectors32;
    public byte   DiskNumber;
    public byte   Unused;
    public byte   Signature;
    public uint   VolumeId;
    [ArrayLength(11)]
    public string VolumeLabel;
    [ArrayLength(8)]
    public string SystemId;       // Either FAT12 or FAT16
  }
  
  // Directory entry structure
  public class FileEntry
  {
    [ArrayLength(11)]
    public string Name;
    public byte   Attributes;
    public byte   Reserved;
    public byte   CreateTimeMili;
    public ushort CreateTime;
    public ushort CreateDate;
    public ushort LastAccessDate;
    public ushort FirstCluster32; // Always 0 for FAT12, FAT16
    public ushort LastWriteTime;
    public ushort LastWriteDate;
    public ushort FirstCluster;
    public uint   FileSize;
    
    public FileAttributes FileAttributes
    { get { return (FileAttributes)Attributes; } }
    
    public bool IsDirectory
    { get { return (FileAttributes & FileAttributes.Directory) != 0; } }
    
    public bool IsFile
    { get { return (FileAttributes & FileAttributes.Directory) == 0; } }
    
    public string FormattedName
    {
      get
      {
        if (IsDirectory) return Name.Substring(0, 8).Trim().ToLower();
        return Name.Substring(0, 8).Trim().ToLower() + "." + Name.Substring(8, 3).Trim().ToLower();
      }
    }
    
    public string FormattedSize
    {
      get
      {
        if (IsDirectory) return "";
        if (FileSize > 1024*1024*1024)
          return ((uint)((ulong)((FileSize / (double)(1024*1024*1024)) * 10) / 10)) + " GB";
        if (FileSize > 1024*1024)
          return ((uint)((ulong)((FileSize / (double)(1024*1024)) * 10) / 10)) + " MB";
        if (FileSize > 1024)
          return ((uint)((ulong)((FileSize / (double)(1024)) * 10) / 10)) + " KB";
        return FileSize + " bytes";
      }
    }
  }
}
