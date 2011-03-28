using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;

namespace Undelete
{
    /// <summary>
    /// Represents the partition entry structure.
    /// </summary>
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

        /// <summary>
        /// Gets whether the file system is FAT (that is, not FAT32).
        /// </summary>
        /// <value>true if the file system is FAT12 or FAT16; otherwise, false.</value>
        public bool IsFAT
        {
            get
            {
                return PartitionId == PartitionId.FAT12 || PartitionId == PartitionId.FAT16;
            }
        }

        public PartitionId PartitionId
        {
            get
            {
                switch(SysId)
                {
                    case 0x00:
                        return PartitionId.None;
                    case 0x01:
                        return PartitionId.FAT12;
                    case 0x05:
                        return PartitionId.Extended;
                    case 0x04:
                    case 0x06:
                    case 0x0E:
                        return PartitionId.FAT16;
                    case 0x0B:
                    case 0x0C:
                        return PartitionId.FAT32;
                    case 0x07:
                        return PartitionId.NTFS;
                    default:
                        return PartitionId.Unknown;
                }
            }
        }

        public string PartitionLabel
        {
            get
            {
                switch(PartitionId)
                {
                    case PartitionId.None:
                        return "None";
                    case PartitionId.FAT12:
                        return "FAT12";
                    case PartitionId.FAT16:
                        return "FAT16";
                    case PartitionId.FAT32:
                        return "FAT32";
                    case PartitionId.Extended:
                        return "Extended";
                    case PartitionId.NTFS:
                        return "NTFS";
                    default:
                        return "Unknown";
                }
            }
        }

        public ushort StartingSector
        {
            get
            {
                return (ushort)(StartingSectorCylinder & 0x3F);
            }
        }

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
        {
            get
            {
                return (ushort)(EndingSectorCylinder & 0x3F);
            }
        }

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
        {
            get
            {
                return (ushort)(EndingCylinder - StartingCylinder);
            }
        }

        public ushort Heads
        {
            get
            {
                return (ushort)(EndingHead - StartingHead);
            }
        }

        public ushort Sectors
        {
            get
            {
                return (ushort)(EndingSector - StartingSector);
            }
        }
    }
}
