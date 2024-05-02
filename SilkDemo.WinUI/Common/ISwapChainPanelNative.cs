using System;
using System.Runtime.InteropServices;
using Silk.NET.Core.Native;

namespace SilkDemo.WinUI.Common;

[ComImport]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
[Guid("63aad0b8-7c24-40ff-85a8-640d944cc325")]
public interface ISwapChainPanelNative
{
    [PreserveSig] HResult SetSwapChain([In] IntPtr swapChain);
    [PreserveSig] ulong Release();
}
