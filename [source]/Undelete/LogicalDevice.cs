using System;
using System.IO;

namespace Undelete
{
  public class LogicalDevice
  {
    private PhysicalDevice owner;
    private PartitionTable partition;
    private BootSector bootSector;    // Logical boot sector of the volume
    private ushort [] fat;
    
    public LogicalDevice (PhysicalDevice owner, PartitionTable partition)
    {
      this.owner = owner;
      this.partition = partition;
      Refresh();
    }
    
    public PhysicalDevice Owner
    { get { return owner; } }
    
    public Stream DeviceStream
    { get { return owner.DeviceStream; } }
    
    public PartitionTable Partition
    { get { return partition; } }
    
    public BootSector BootSector
    { get { return bootSector; } }
    
    public uint RootDirectorySector
    { get { return (uint)(bootSector.ReservedSectors + (bootSector.NumberOfFATs * bootSector.SectorsPerFAT)); } }
    public uint RootDirectorySectorCount
    { get { return (uint)(((bootSector.RootEntries * 32) + (bootSector.BytesPerSector - 1)) / bootSector.BytesPerSector); } }
    public uint DataSector
    { get { return (uint)(bootSector.ReservedSectors + (bootSector.NumberOfFATs * bootSector.SectorsPerFAT) + RootDirectorySectorCount); } }
    
    public ushort [] FAT
    { get { return fat; } }
    
    public void Refresh ()
    {
      bootSector = null;
      fat = new UInt16 [0];
      
      // Jump to FAT partition
      DeviceStream.Seek(partition.RelativeSector * PhysicalDevice.SectorSize, SeekOrigin.Begin);
      if (!partition.IsFAT) return;
      
      // Boot sector
      bootSector = (BootSector)StreamActivator.CreateInstance(typeof(BootSector), DeviceStream);
      DeviceStream.Seek(448, SeekOrigin.Current);
      
      // Failsafe .. be sure FAT partition table is valid
      if ((DeviceStream.ReadByte() != 0x55) || (DeviceStream.ReadByte() != 0xAA))
      { bootSector = null; return; }
      
      // Get FAT sector
      BinaryReader br = new BinaryReader(DeviceStream);
      fat = new UInt16 [bootSector.NumberOfFATs * ((bootSector.SectorsPerFAT * PhysicalDevice.SectorSize) / 2)];
      for (int i = 0; i < fat.Length; i++) fat[i] = br.ReadUInt16();
    }
  }
}
