using System;
using System.IO;
using System.Runtime.InteropServices;


namespace Undelete
{
  public class Win32FileStream : FileStream
  {
    public Win32FileStream (IntPtr handle, FileAccess access) : base(handle, access)
    {}
    public Win32FileStream (IntPtr handle, FileAccess access, bool ownsHandle) : base(handle, access, ownsHandle)
    {}
    public override long Length
    { get { return 0; } }
  }
}
