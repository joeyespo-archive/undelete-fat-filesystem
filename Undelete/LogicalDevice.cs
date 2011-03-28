using System;
using System.IO;

namespace Undelete
{
    /// <summary>
    /// Represents a logical file system device, which is based on a physical device.
    /// </summary>
    public class LogicalDevice
    {
        public LogicalDevice(PhysicalDevice owner, PartitionTable partition)
        {
            this.physicalDevice = owner;
            this.partition = partition;
            Refresh();
        }

        #region Public Properties

        /// <summary>
        /// Gets the physical device.
        /// </summary>
        public PhysicalDevice PhysicalDevice
        {
            get
            {
                return physicalDevice;
            }
        }

        /// <summary>
        /// Gets the device stream.
        /// </summary>
        public Stream DeviceStream
        {
            get
            {
                return physicalDevice.DeviceStream;
            }
        }

        /// <summary>
        /// Gets the partition.
        /// </summary>
        public PartitionTable Partition
        {
            get
            {
                return partition;
            }
        }

        /// <summary>
        /// Gets the boot sector.
        /// </summary>
        public BootSector BootSector
        {
            get
            {
                return bootSector;
            }
        }

        /// <summary>
        /// Gets the root directory sector.
        /// </summary>
        public uint RootDirectorySector
        {
            get
            {
                return (uint)(bootSector.ReservedSectors + (bootSector.NumberOfFATs * bootSector.SectorsPerFAT));
            }
        }
        /// <summary>
        /// Gets the root directory sector count.
        /// </summary>
        public uint RootDirectorySectorCount
        {
            get
            {
                return (uint)(((bootSector.RootEntries * 32) + (bootSector.BytesPerSector - 1)) / bootSector.BytesPerSector);
            }
        }
        /// <summary>
        /// Gets the data sector.
        /// </summary>
        public uint DataSector
        {
            get
            {
                return (uint)(bootSector.ReservedSectors + (bootSector.NumberOfFATs * bootSector.SectorsPerFAT) + RootDirectorySectorCount);
            }
        }

        /// <summary>
        /// Gets the device data.
        /// </summary>
        public ushort[] Data
        {
            get
            {
                return data;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Refreshes the logical device.
        /// </summary>
        public void Refresh()
        {
            bootSector = null;
            data = new UInt16[0];

            // Jump to FAT partition
            DeviceStream.Seek(partition.RelativeSector * PhysicalDevice.SectorSize, SeekOrigin.Begin);
            if(!partition.IsFAT)
                return;

            // Boot sector
            bootSector = (BootSector)StreamActivator.CreateInstance(typeof(BootSector), DeviceStream);
            DeviceStream.Seek(448, SeekOrigin.Current);

            // Failsafe .. be sure FAT partition table is valid
            if((DeviceStream.ReadByte() != 0x55) || (DeviceStream.ReadByte() != 0xAA))
            {
                bootSector = null;
                return;
            }

            // Get FAT sector
            BinaryReader br = new BinaryReader(DeviceStream);
            data = new UInt16[bootSector.NumberOfFATs * ((bootSector.SectorsPerFAT * PhysicalDevice.SectorSize) / 2)];
            for(int i = 0; i < data.Length; i++)
                data[i] = br.ReadUInt16();
        }

        #endregion

        private PhysicalDevice physicalDevice;
        private PartitionTable partition;
        private BootSector bootSector;    // Logical boot sector of the volume
        private ushort[] data;
    }
}
