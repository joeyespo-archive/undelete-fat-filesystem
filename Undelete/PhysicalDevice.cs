using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Collections;
using System.Windows.Forms;

namespace Undelete
{
    /// <summary>
    /// Represents a physical file system device.
    /// </summary>
    public sealed class PhysicalDevice
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PhysicalDevice"/> class.
        /// </summary>
        /// <param name="deviceStream">The device stream.</param>
        public PhysicalDevice(Stream deviceStream)
        {
            this.deviceStream = deviceStream;
            Refresh();
        }

        #region Static Members

        /// <summary>
        /// Gets a physical device from the specified device number.
        /// </summary>
        public static PhysicalDevice FromPhysicalDrive(int deviceNum)
        {
            string physicalPath = @"\\.\PHYSICALDRIVE" + deviceNum.ToString();
            Stream stream = null;
            try
            {
                // Wordaround for: File.Open(physicalPath, FileMode.Open, FileAccess.Read, FileShare.Read);
                IntPtr handle = CreateFile(physicalPath, FileAccess.Read, FileShare.Read, 0, FileMode.Open, FILE_FLAG_NO_BUFFERING, IntPtr.Zero);
                if((uint)handle != 0xFFFFFFFF)
                    stream = new Win32FileStream(handle, FileAccess.ReadWrite, true);
            }
            catch(FileNotFoundException)
            {
            }
            catch(IOException e)
            {
                MessageBox.Show(e.Message, "Undelete", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            if(stream == null)
                return null;

            try
            {
                stream.ReadByte();
            }
            catch(IOException)
            {
                return new PhysicalDevice(null);
            }
            return new PhysicalDevice(stream);
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the device stream.
        /// </summary>
        public Stream DeviceStream
        {
            get
            {
                return deviceStream;
            }
        }

        /// <summary>
        /// Gets the logical devices.
        /// </summary>
        public LogicalDevice[] LogicalDevices
        {
            get
            {
                return logicalDevices;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Refreshes the device.
        /// </summary>
        public void Refresh()
        {
            if(deviceStream == null)
                return;

            logicalDevices = null;
            ArrayList logicalDeviceList = new ArrayList();
            deviceStream.Seek(0, SeekOrigin.Begin);
            BinaryReader br = new BinaryReader(deviceStream);
            // Read the MBR
            byte[] sector = br.ReadBytes(SectorSize);
            // Failsafe .. be sure MBR is vaild
            if((sector[0x01FE] != 0x55) || (sector[0x01FF] != 0xAA))
            {
                Console.WriteLine("Corrupt MBR; exiting");
                return;
            }

            // Partition table
            int p = 0x01BE;   // Beginning of partition table
            for(int i = 0; i < 4; i++, p += 16)
            {
                PartitionTable partition = (PartitionTable)StreamActivator.CreateInstance(typeof(PartitionTable), sector, p);
                if(partition.SysId == 0)
                    continue;
                logicalDeviceList.Add(new LogicalDevice(this, partition));
            }

            logicalDevices = (LogicalDevice[])logicalDeviceList.ToArray(typeof(LogicalDevice));
        }

        #endregion

        /// <summary>
        /// Defines the sector size of the file system. (512)
        /// </summary>
        public static readonly int SectorSize = 512;

        [DllImport("kernel32")]
        private static extern IntPtr CreateFile(string filename, [MarshalAs(UnmanagedType.U4)]FileAccess fileaccess, [MarshalAs(UnmanagedType.U4)]FileShare fileshare, int securityattributes, [MarshalAs(UnmanagedType.U4)]FileMode creationdisposition, int flags, IntPtr template);
        private static readonly int FILE_FLAG_NO_BUFFERING = 0x20000000;

        private Stream deviceStream;
        private LogicalDevice[] logicalDevices;
    }
}
