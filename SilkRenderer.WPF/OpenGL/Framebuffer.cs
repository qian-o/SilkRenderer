using System;
using System.Windows.Interop;
using System.Windows.Media;
using Silk.NET.Direct3D9;
using Silk.NET.OpenGL;
using Silk.NET.WGL.Extensions.NV;
using SilkRenderer.WPF.Common;

namespace SilkRenderer.WPF.OpenGL;

public unsafe class Framebuffer : FramebufferBase
{
    public RenderContext Context { get; }

    public override int FramebufferWidth { get; }

    public override int FramebufferHeight { get; }

    public uint GLFramebufferHandle { get; }

    public uint GLSharedTextureHandle { get; }

    public uint GLDepthRenderBufferHandle { get; }

    public IntPtr DxInteropRegisteredHandle { get; }

    public override D3DImage D3dImage { get; }

    public TranslateTransform TranslateTransform { get; }

    public ScaleTransform FlipYTransform { get; }

    public Framebuffer(RenderContext context, int framebufferWidth, int framebufferHeight)
    {
        Context = context;
        FramebufferWidth = framebufferWidth;
        FramebufferHeight = framebufferHeight;

        IDirect3DDevice9Ex* device = (IDirect3DDevice9Ex*)context.DxDeviceHandle;
        IDirect3DSurface9* surface;
        void* surfacePtr = (void*)IntPtr.Zero;
        device->CreateRenderTarget((uint)FramebufferWidth, (uint)FramebufferHeight, context.Format, MultisampleType.MultisampleNone, 0, 0, &surface, &surfacePtr);

        RenderContext.NVDXInterop.DxsetResourceShareHandle(surface, (nint)surfacePtr);

        GLFramebufferHandle = RenderContext.GL.GenFramebuffer();
        GLSharedTextureHandle = RenderContext.GL.GenTexture();

        DxInteropRegisteredHandle = RenderContext.NVDXInterop.DxregisterObject(context.GlDeviceHandle, surface, GLSharedTextureHandle, (NV)TextureTarget.Texture2D, NV.AccessReadWriteNV);

        RenderContext.GL.BindFramebuffer(FramebufferTarget.Framebuffer, GLFramebufferHandle);
        RenderContext.GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, TextureTarget.Texture2D, GLSharedTextureHandle, 0);

        GLDepthRenderBufferHandle = RenderContext.GL.GenRenderbuffer();
        RenderContext.GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, GLDepthRenderBufferHandle);
        RenderContext.GL.RenderbufferStorage((GLEnum)RenderbufferTarget.Renderbuffer, GLEnum.Depth24Stencil8, (uint)FramebufferWidth, (uint)FramebufferHeight);

        RenderContext.GL.FramebufferRenderbuffer(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthAttachment, RenderbufferTarget.Renderbuffer, GLDepthRenderBufferHandle);
        RenderContext.GL.FramebufferRenderbuffer(FramebufferTarget.Framebuffer, FramebufferAttachment.StencilAttachment, RenderbufferTarget.Renderbuffer, GLDepthRenderBufferHandle);
        RenderContext.GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);

        D3dImage = new D3DImage();
        D3dImage.Lock();
        D3dImage.SetBackBuffer(D3DResourceType.IDirect3DSurface9, (IntPtr)surface);
        D3dImage.Unlock();

        TranslateTransform = new TranslateTransform(0, FramebufferHeight);
        FlipYTransform = new ScaleTransform(1, -1);
    }

    public override void Dispose()
    {
        RenderContext.GL.DeleteFramebuffer(GLFramebufferHandle);
        RenderContext.GL.DeleteRenderbuffer(GLDepthRenderBufferHandle);
        RenderContext.GL.DeleteTexture(GLSharedTextureHandle);
        RenderContext.NVDXInterop.DxunregisterObject(Context.GlDeviceHandle, DxInteropRegisteredHandle);

        GC.SuppressFinalize(this);
    }
}