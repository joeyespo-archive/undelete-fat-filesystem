using System;
using System.IO;
using System.Runtime.InteropServices;


namespace Undelete
{
    /// <summary>
    /// Represents a win32 file stream.
    /// </summary>
    public sealed class Win32FileStream : FileStream
    {
        public Win32FileStream(IntPtr handle, FileAccess access)
            : base(handle, access)
        {
        }
        public Win32FileStream(IntPtr handle, FileAccess access, bool ownsHandle)
            : base(handle, access, ownsHandle)
        {
        }

        /// <summary>
        /// Gets the length, which will always have a value of zero.
        /// </summary>
        /// <returns>0</returns>
        public override long Length
        {
            get
            {
                return 0;
            }
        }
    }
}
