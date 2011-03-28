using System;
using System.IO;
using System.Text;
using System.Collections;


namespace Undelete
{
    /// <summary>
    /// Provides methods for interfacing with a FAT file system.
    /// </summary>
    public sealed class FATManager
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FATManager"/> class.
        /// </summary>
        /// <param name="device">The logical FAT12 or FAT16 device.</param>
        public FATManager(LogicalDevice device)
        {
            if(!device.Partition.IsFAT)
                throw new ArgumentException("Logical device must be a compatible FAT (i.e. FAT12 or FAT16) device.");
            this.device = device;
            currentCluster = 0;
        }

        #region Public Methods

        /// <summary>
        /// Gets the device of the manager.
        /// </summary>
        public LogicalDevice Device
        {
            get
            {
                return device;
            }
        }

        /// <summary>
        /// Gets the device stream of the manager.
        /// </summary>
        public Stream DeviceStream
        {
            get
            {
                return device.DeviceStream;
            }
        }

        /// <summary>
        /// Gets the path of the current directory in the file system.
        /// </summary>
        public string Path
        {
            get
            {
                while(true)
                {
                    string path = @"\";
                    foreach(FileEntry file in EnumerateDirectories())
                    {
                        if(file.FormattedName == "..")
                        {
                            currentCluster = (uint)(file.FirstCluster);
                            return path;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Gets the current directory in the file system.
        /// </summary>
        public FileEntry CurrentDirectory
        {
            get
            {
                return EnumerateFileEntries(currentCluster, false, false, false, true, false)[0];
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Changes the current directory.
        /// </summary>
        /// <param name="name">The name of the directory to change to. A value of ".." indicates the parent directory.</param>
        public void ChangeDirectory(string name)
        {
            foreach(FileEntry file in EnumerateDirectories())
            {
                if(file.FormattedName == name.ToLower())
                {
                    currentCluster = (uint)(file.FirstCluster);
                    return;
                }
            }
            throw new DirectoryNotFoundException("Directory was not found");
        }

        /// <summary>
        /// Enumerates the files and directories contained in the current directory.
        /// </summary>
        public FileEntry[] EnumerateFileEntries()
        {
            return EnumerateFileEntries(currentCluster, true, true, false, false, false);
        }

        /// <summary>
        /// Enumerates the existing or deleted files and/or directories contained in the current directory.
        /// </summary>
        public FileEntry[] EnumerateFileEntries(bool includeFiles, bool includeDirectories, bool deleted)
        {
            return EnumerateFileEntries(currentCluster, includeFiles, includeDirectories, deleted, false, false);
        }

        /// <summary>
        /// Enumerates the files contained in the current directory.
        /// </summary>
        public FileEntry[] EnumerateFiles()
        {
            return EnumerateFileEntries(currentCluster, true, false, false, false, false);
        }

        /// <summary>
        /// Enumerates the directories contained in the current directory.
        /// </summary>
        public FileEntry[] EnumerateDirectories()
        {
            return EnumerateFileEntries(currentCluster, false, true, false, false, false);
        }

        /// <summary>
        /// Enumerates the deleted files and directories contained in the current directory.
        /// </summary>
        public FileEntry[] EnumerateDeletedFileEntries()
        {
            return EnumerateFileEntries(currentCluster, true, true, true, false, false);
        }

        /// <summary>
        /// Enumerates the deleted files contained in the current directory.
        /// </summary>
        public FileEntry[] EnumerateDeletedFiles()
        {
            return EnumerateFileEntries(currentCluster, true, false, true, false, false);
        }

        /// <summary>
        /// Enumerates the deleted directories contained in the current directory.
        /// </summary>
        public FileEntry[] EnumerateDeletedDirectories()
        {
            return EnumerateFileEntries(currentCluster, false, true, true, false, false);
        }

        /// <summary>
        /// Enumerates the file entries contained in the current directory.
        /// </summary>
        private FileEntry[] EnumerateFileEntries(uint clusterNumber, bool includeFiles, bool includeDirectories, bool deleted, bool currentOnly, bool parentOnly)
        {
            ArrayList files = new ArrayList();
            uint sector = (uint)(device.RootDirectorySector + ((((clusterNumber == 0) ? (0) : (clusterNumber + 2))) * device.BootSector.SectorsPerCluster));

            // Jump to next directory sector
            DeviceStream.Seek((device.Partition.RelativeSector + sector) * device.BootSector.BytesPerSector, SeekOrigin.Begin);
            while(true)
            {
                FileEntry file = (FileEntry)StreamActivator.CreateInstance(typeof(FileEntry), DeviceStream);
                if(file.FileAttributes == FileAttributes.LongFileName)
                    continue;
                if(file.Name[0] == 0x00)
                    break;  // No more entries

                if(currentOnly)
                {
                    if((clusterNumber == 0) && ((file.FileAttributes & FileAttributes.VolumeId) != 0))
                        files.Add(file);
                    else if(file.Name == ".          ")
                        files.Add(file);
                    continue;
                }
                if(parentOnly)
                {
                    if(file.Name == "..         ")
                        files.Add(file);
                    continue;
                }
                // Skip the volume id
                if((file.FileAttributes & FileAttributes.VolumeId) != 0)
                    continue;

                // Skip deleted/non-deleted entries
                if((file.Name[0] == '?') != deleted)
                    continue;

                // Show info
                bool isDirectory = ((file.FileAttributes & FileAttributes.Directory) != 0);
                if((isDirectory) && (!includeDirectories))
                    continue;
                if((!isDirectory) && (!includeFiles))
                    continue;
                files.Add(file);
            }
            return (FileEntry[])files.ToArray(typeof(FileEntry));
        }

        /// <summary>
        /// Reads the specified file entry as string.
        /// </summary>
        public string ReadFileAsString(FileEntry file)
        {
            byte[] data = ReadFile(file);
            char[] contents = new char[data.Length];
            Encoding.ASCII.GetDecoder().GetChars(data, 0, data.Length, contents, 0);
            return new string(contents);
        }

        /// <summary>
        /// Reads the specified file entry.
        /// </summary>
        public byte[] ReadFile(FileEntry file)
        {
            ushort nextCluster = file.FirstCluster;
            uint sizeLeft = file.FileSize;
            uint clusterSize = (uint)(PhysicalDevice.SectorSize * device.BootSector.SectorsPerCluster);
            uint sector;

            int offset = 0;
            byte[] data = new Byte[file.FileSize];
            while(true)
            {
                // Calculate next sector
                sector = (uint)(device.RootDirectorySector + ((((nextCluster == 0) ? (0) : (nextCluster + 2))) * device.BootSector.SectorsPerCluster));
                // Jump to data sector
                DeviceStream.Seek((device.Partition.RelativeSector + sector) * device.BootSector.BytesPerSector, SeekOrigin.Begin);
                // Read correct size
                if(sizeLeft < clusterSize)
                {
                    DeviceStream.Read(data, offset, (int)sizeLeft);
                    break;
                }
                else
                {
                    DeviceStream.Read(data, offset, (int)clusterSize);
                    offset += (int)clusterSize;
                    sizeLeft -= clusterSize;
                }
                // Get next cluster
                nextCluster = device.Data[nextCluster];
                if((nextCluster == 0x0000) || (nextCluster == 0xFFF7))
                    break;    // Bad cluster pointer
                if((nextCluster >= 0xFFF8) && (nextCluster <= 0xFFFF))
                    break;    // End of cluster of file
            }
            return data;
        }

        /// <summary>
        /// Saves the specified stream to the specified file entry.
        /// </summary>
        public void SaveFile(FileEntry file, Stream stream)
        {
            ushort nextCluster = file.FirstCluster;
            uint sizeLeft = file.FileSize;
            uint clusterSize = (uint)(PhysicalDevice.SectorSize * device.BootSector.SectorsPerCluster);
            uint sector;

            byte[] data = new Byte[clusterSize];
            while(true)
            {
                // Calculate next sector
                sector = (uint)(device.RootDirectorySector + ((((nextCluster == 0) ? (0) : (nextCluster + 2))) * device.BootSector.SectorsPerCluster));
                // Jump to data sector
                DeviceStream.Seek((device.Partition.RelativeSector + sector) * device.BootSector.BytesPerSector, SeekOrigin.Begin);
                // Read correct size
                if(sizeLeft < clusterSize)
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
                nextCluster = device.Data[nextCluster];
                if((nextCluster == 0x0000) || (nextCluster == 0xFFF7))
                    break;    // Bad cluster pointer
                if((nextCluster >= 0xFFF8) && (nextCluster <= 0xFFFF))
                    break;    // End of cluster of file
            }
        }

        #endregion

        private LogicalDevice device;
        private uint currentCluster;
    }
}
