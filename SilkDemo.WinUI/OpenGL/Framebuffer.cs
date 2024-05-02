using System;
using System.Reflection;
using Microsoft.UI.Xaml.Media;
using Silk.NET.Core.Native;
using Silk.NET.Direct3D11;
using Silk.NET.DXGI;
using Silk.NET.OpenGL;
using Silk.NET.WGL.Extensions.NV;
using SilkDemo.WinUI.Common;

namespace SilkDemo.WinUI.OpenGL;

public unsafe class Framebuffer : FramebufferBase
{
    public RenderContext Context { get; }

    public override int FramebufferWidth { get; protected set; }

    public override int FramebufferHeight { get; protected set; }

    public uint GLColorRenderbufferHandle { get; set; }

    public uint GLDepthRenderBufferHandle { get; set; }

    public uint GLFramebufferHandle { get; set; }

    public IntPtr DxInteropColorHandle { get; set; }

    public override IntPtr SwapChainHandle { get; protected set; }

    public TranslateTransform TranslateTransform { get; }

    public ScaleTransform FlipYTransform { get; }

    public Framebuffer(RenderContext context, int framebufferWidth, int framebufferHeight)
    {
        Context = context;
        FramebufferWidth = framebufferWidth;
        FramebufferHeight = framebufferHeight;

        IDXGISwapChain1* swapChain;

        // SwapChain
        {
            SwapChainDesc1 swapChainDesc = new()
            {
                Width = (uint)FramebufferWidth,
                Height = (uint)FramebufferHeight,
                Format = Format.FormatB8G8R8A8Unorm,
                Stereo = 0,
                SampleDesc = new SampleDesc()
                {
                    Count = 1,
                    Quality = 0
                },
                BufferUsage = DXGI.UsageRenderTargetOutput,
                BufferCount = 2,
                Scaling = Scaling.Stretch,
                SwapEffect = SwapEffect.FlipSequential,
                Flags = 0,
            };

            ((IDXGIFactory2*)Context.DxDeviceFactory)->CreateSwapChainForComposition((IUnknown*)Context.DxDeviceHandle, &swapChainDesc, null, &swapChain);

            SwapChainHandle = (IntPtr)swapChain;
        }

        GLFramebufferHandle = RenderContext.GL.GenFramebuffer();

        TranslateTransform = new TranslateTransform
        {
            X = 0,
            Y = FramebufferHeight
        };
        FlipYTransform = new ScaleTransform
        {
            ScaleX = 1,
            ScaleY = -1
        };
    }

    public void Begin()
    {
        ID3D11Texture2D* colorbuffer;

        RenderContext.GL.BindFramebuffer(FramebufferTarget.Framebuffer, GLFramebufferHandle);

        // Texture2D
        {
            Guid guid = typeof(ID3D11Texture2D).GetTypeInfo().GUID;
            ((IDXGISwapChain1*)SwapChainHandle)->GetBuffer(0, &guid, (void**)&colorbuffer);
        }

        // GL
        {
            GLColorRenderbufferHandle = RenderContext.GL.GenRenderbuffer();
            GLDepthRenderBufferHandle = RenderContext.GL.GenRenderbuffer();

            DxInteropColorHandle = RenderContext.NVDXInterop.DxregisterObject(Context.GlDeviceHandle, (void*)colorbuffer, (uint)GLColorRenderbufferHandle, (NV)RenderbufferTarget.Renderbuffer, NV.AccessReadWriteNV);
            RenderContext.GL.FramebufferRenderbuffer(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, RenderbufferTarget.Renderbuffer, GLColorRenderbufferHandle);

            RenderContext.GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, GLDepthRenderBufferHandle);
            RenderContext.GL.RenderbufferStorage(GLEnum.Renderbuffer, GLEnum.Depth24Stencil8, (uint)FramebufferWidth, (uint)FramebufferHeight);
            RenderContext.GL.FramebufferRenderbuffer(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthAttachment, RenderbufferTarget.Renderbuffer, (uint)GLDepthRenderBufferHandle);
            RenderContext.GL.FramebufferRenderbuffer(FramebufferTarget.Framebuffer, FramebufferAttachment.StencilAttachment, RenderbufferTarget.Renderbuffer, (uint)GLDepthRenderBufferHandle);
        }

        colorbuffer->Release();

        RenderContext.NVDXInterop.DxlockObjects(Context.GlDeviceHandle, 1, new[] { DxInteropColorHandle });

        RenderContext.GL.BindFramebuffer(FramebufferTarget.Framebuffer, GLFramebufferHandle);
        RenderContext.GL.Viewport(0, 0, (uint)FramebufferWidth, (uint)FramebufferHeight);
    }

    public void End()
    {
        RenderContext.GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);

        RenderContext.NVDXInterop.DxunlockObjects(Context.GlDeviceHandle, 1, new[] { DxInteropColorHandle });

        RenderContext.NVDXInterop.DxunregisterObject(Context.GlDeviceHandle, DxInteropColorHandle);

        RenderContext.GL.DeleteRenderbuffer(GLColorRenderbufferHandle);
        RenderContext.GL.DeleteRenderbuffer(GLDepthRenderBufferHandle);

        ((IDXGISwapChain1*)SwapChainHandle)->Present(1, 0);
    }

    public void UpdateSize(int framebufferWidth, int framebufferHeight)
    {
        FramebufferWidth = framebufferWidth;
        FramebufferHeight = framebufferHeight;

        ((IDXGISwapChain1*)SwapChainHandle)->ResizeBuffers(2, (uint)framebufferWidth, (uint)framebufferHeight, Format.FormatUnknown, 0);

        TranslateTransform.Y = FramebufferHeight;
    }

    public override void Dispose()
    {
        RenderContext.GL.DeleteFramebuffer(GLFramebufferHandle);

        RenderContext.NVDXInterop.DxunregisterObject(Context.GlDeviceHandle, DxInteropColorHandle);
        RenderContext.GL.DeleteRenderbuffer(GLColorRenderbufferHandle);
        RenderContext.GL.DeleteRenderbuffer(GLDepthRenderBufferHandle);

        GC.SuppressFinalize(this);
    }
}
