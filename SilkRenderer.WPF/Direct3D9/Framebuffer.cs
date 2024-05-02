using System;
using System.Windows.Interop;
using Silk.NET.Direct3D9;
using SilkRenderer.WPF.Common;

namespace SilkRenderer.WPF.Direct3D9;

public unsafe class Framebuffer : FramebufferBase
{
    public RenderContext Context { get; }

    public override int FramebufferWidth { get; }

    public override int FramebufferHeight { get; }

    public override D3DImage D3dImage { get; }

    public Framebuffer(RenderContext context, int framebufferWidth, int framebufferHeight)
    {
        Context = context;
        FramebufferWidth = framebufferWidth;
        FramebufferHeight = framebufferHeight;

        IDirect3DSurface9* surface;
        context.Device->CreateRenderTarget((uint)FramebufferWidth, (uint)FramebufferHeight, context.Format, MultisampleType.MultisampleNone, 0, 0, &surface, null);
        context.Device->SetRenderTarget(0, surface);

        D3dImage = new D3DImage();
        D3dImage.Lock();
        D3dImage.SetBackBuffer(D3DResourceType.IDirect3DSurface9, (IntPtr)surface);
        D3dImage.Unlock();
    }

    public override void Dispose()
    {
        GC.SuppressFinalize(this);
    }
}
