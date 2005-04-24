using System;
using System.IO;
using System.Collections;

namespace Undelete
{
  public class PhysicalDevice
  {
    public static readonly int SectorSize = 512;
    
    private Stream deviceStream;
    private LogicalDevice [] logicalDevices;
    
    public PhysicalDevice (Stream deviceStream)
    {
      this.deviceStream = deviceStream;
      Refresh();
    }
    
    public static PhysicalDevice FromPhysicalDrive (int deviceNum)
    {
      string physicalPath = @"\\.\PhysicalDrive" + deviceNum.ToString();
      Stream stream = null;
      try
      { stream = File.Open(physicalPath, FileMode.Open, FileAccess.Read, FileShare.Read); }
      catch (FileNotFoundException) {}
      if (stream == null) return null;
      return new PhysicalDevice(stream);
    }
    
    public Stream DeviceStream
    { get { return deviceStream; } }
    
    public LogicalDevice [] LogicalDevices
    { get { return logicalDevices; } }
    
    public void Refresh ()
    {
      logicalDevices = null;
      ArrayList logicalDeviceList = new ArrayList();
      deviceStream.Seek(0, SeekOrigin.Begin);
      BinaryReader br = new BinaryReader(deviceStream);
      // Read the MBR
      byte [] sector = br.ReadBytes(SectorSize);
      // Failsafe .. be sure MBR is vaild
      if ((sector[0x01FE] != 0x55) || (sector[0x01FF] != 0xAA))
      { Console.WriteLine("Corrupt MBR; exiting"); return; }
      
      // Partition table
      int p = 0x01BE;   // Beginning of partition table
      for (int i = 0; i < 4; i++, p += 16)
      {
        PartitionTable partition = (PartitionTable)StreamActivator.CreateInstance(typeof(PartitionTable), sector, p);
        if (partition.SysId == 0) continue;
        logicalDeviceList.Add(new LogicalDevice(this, partition));
      }
      
      logicalDevices = (LogicalDevice [])logicalDeviceList.ToArray(typeof(LogicalDevice));
    }
  }
}
