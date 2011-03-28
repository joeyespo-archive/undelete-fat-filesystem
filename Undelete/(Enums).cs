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
        None = 0x00,
        ReadOnly = 0x01,
        Hidden = 0x02,
        System = 0x04,
        VolumeId = 0x08,
        Directory = 0x10,
        Archive = 0x20,
        LongFileName = 0x0F,
    }
}
