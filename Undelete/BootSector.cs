using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;

namespace Undelete
{
    /// <summary>
    /// Partition boot sector structure for both FAT12 and FAT16.
    /// </summary>
    public sealed class BootSector
    {
        [ArrayLength(3)]
        public byte[] jmp;

        [ArrayLength(8)]
        public string OemName;

        /// <summary>
        /// The BIOS parameter block (BPB).
        /// </summary>
        public ushort BytesPerSector;
        public byte SectorsPerCluster;
        public ushort ReservedSectors;
        public byte NumberOfFATs;
        public ushort RootEntries;
        public ushort Sectors16;
        public byte MediaType;
        public ushort SectorsPerFAT;
        public ushort SectorsPerTrack;
        public ushort Heads;
        /// <summary>
        /// Same as RelativeSectors in the Partition Table
        /// </summary>
        public uint HiddenSectors;
        public uint Sectors32;
        public byte DiskNumber;
        public byte Unused;
        public byte Signature;
        public uint VolumeId;

        [ArrayLength(11)]
        public string VolumeLabel;

        /// <value>
        /// Either "FAT12" or "FAT16".
        /// </value>
        [ArrayLength(8)]
        public string SystemId;
    }
}
