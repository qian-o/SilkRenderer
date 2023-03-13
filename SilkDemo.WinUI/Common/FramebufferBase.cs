using System;

namespace SilkDemo.WinUI.Common;
public abstract class FramebufferBase : IDisposable
{
    public abstract int FramebufferWidth { get; protected set; }

    public abstract int FramebufferHeight { get; protected set; }

    public abstract IntPtr SwapChainHandle { get; protected set; }

    public abstract void Dispose();
}
