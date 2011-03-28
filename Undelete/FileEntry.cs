using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;

namespace Undelete
{
    /// <summary>
    /// Represents a file/directory entry structure.
    /// </summary>
    public sealed class FileEntry
    {
        [ArrayLength(11)]
        public string Name;
        public byte Attributes;
        public byte Reserved;
        public byte CreateTimeMili;
        public ushort CreateTime;
        public ushort CreateDate;
        public ushort LastAccessDate;
        /// <value>
        /// Always 0 for both FAT12 and FAT16.
        /// </value>
        public ushort FirstCluster32;
        public ushort LastWriteTime;
        public ushort LastWriteDate;
        public ushort FirstCluster;
        public uint FileSize;

        public FileAttributes FileAttributes
        {
            get
            {
                return (FileAttributes)Attributes;
            }
        }

        public bool IsDirectory
        {
            get
            {
                return (FileAttributes & FileAttributes.Directory) != 0;
            }
        }

        public bool IsFile
        {
            get
            {
                return (FileAttributes & FileAttributes.Directory) == 0;
            }
        }

        public string FormattedName
        {
            get
            {
                if(IsDirectory)
                    return Name.Substring(0, 8).Trim().ToLower();
                return Name.Substring(0, 8).Trim().ToLower() + "." + Name.Substring(8, 3).Trim().ToLower();
            }
        }

        public string FormattedSize
        {
            get
            {
                if(IsDirectory)
                    return "";
                if(FileSize > 1024 * 1024 * 1024)
                    return ((uint)((ulong)((FileSize / (double)(1024 * 1024 * 1024)) * 10) / 10)) + " GB";
                if(FileSize > 1024 * 1024)
                    return ((uint)((ulong)((FileSize / (double)(1024 * 1024)) * 10) / 10)) + " MB";
                if(FileSize > 1024)
                    return ((uint)((ulong)((FileSize / (double)(1024)) * 10) / 10)) + " KB";
                return FileSize + " bytes";
            }
        }
    }
}
