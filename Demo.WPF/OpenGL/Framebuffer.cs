using Demo.WPF.Common;
using OpenTK.Graphics.OpenGL;
using OpenTK.Graphics.Wgl;
using OpenTK.Platform.Windows;
using Silk.NET.Direct3D9;
using System;
using System.Windows.Interop;
using System.Windows.Media;

namespace Demo.WPF.OpenGL;

public unsafe class Framebuffer : FramebufferBase
{
    public RenderContext Context { get; }

    public override int FramebufferWidth { get; }

    public override int FramebufferHeight { get; }

    public int GLFramebufferHandle { get; }

    public int GLSharedTextureHandle { get; }

    public int GLDepthRenderBufferHandle { get; }

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

        Wgl.DXSetResourceShareHandleNV((IntPtr)surface, (IntPtr)surfacePtr);
        GLFramebufferHandle = GL.GenFramebuffer();
        GLSharedTextureHandle = GL.GenTexture();

        DxInteropRegisteredHandle = Wgl.DXRegisterObjectNV(context.GlDeviceHandle, (IntPtr)surface, (uint)GLSharedTextureHandle, (uint)TextureTarget.Texture2D, WGL_NV_DX_interop.AccessReadWrite);

        GL.BindFramebuffer(FramebufferTarget.Framebuffer, GLFramebufferHandle);
        GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, TextureTarget.Texture2D, GLSharedTextureHandle, 0);

        GLDepthRenderBufferHandle = GL.GenRenderbuffer();
        GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, GLDepthRenderBufferHandle);
        GL.RenderbufferStorage(RenderbufferTarget.Renderbuffer, RenderbufferStorage.Depth24Stencil8, FramebufferWidth, FramebufferHeight);

        GL.FramebufferRenderbuffer(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthAttachment, RenderbufferTarget.Renderbuffer, GLDepthRenderBufferHandle);
        GL.FramebufferRenderbuffer(FramebufferTarget.Framebuffer, FramebufferAttachment.StencilAttachment, RenderbufferTarget.Renderbuffer, GLDepthRenderBufferHandle);
        GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);

        D3dImage = new D3DImage();
        D3dImage.Lock();
        D3dImage.SetBackBuffer(D3DResourceType.IDirect3DSurface9, (IntPtr)surface);
        D3dImage.Unlock();

        TranslateTransform = new TranslateTransform(0, FramebufferHeight);
        FlipYTransform = new ScaleTransform(1, -1);
    }

    public override void Dispose()
    {
        GL.DeleteFramebuffer(GLFramebufferHandle);
        GL.DeleteRenderbuffer(GLDepthRenderBufferHandle);
        GL.DeleteTexture(GLSharedTextureHandle);
        Wgl.DXUnregisterObjectNV(Context.GlDeviceHandle, DxInteropRegisteredHandle);

        GC.SuppressFinalize(this);
    }
}