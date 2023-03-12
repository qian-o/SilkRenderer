using Demo.WinUI.Common;
using Microsoft.UI.Xaml.Media;
using OpenTK.Graphics.OpenGL;
using OpenTK.Graphics.Wgl;
using OpenTK.Platform.Windows;
using Silk.NET.Core.Native;
using Silk.NET.Direct3D11;
using Silk.NET.DXGI;
using System;
using System.Reflection;

namespace Demo.WinUI.OpenGL;

public unsafe class Framebuffer : FramebufferBase
{
    public RenderContext Context { get; }

    public override int FramebufferWidth { get; protected set; }

    public override int FramebufferHeight { get; protected set; }

    public int GLColorRenderbufferHandle { get; set; }

    public int GLDepthRenderBufferHandle { get; set; }

    public int GLFramebufferHandle { get; set; }

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

        GLFramebufferHandle = GL.GenFramebuffer();

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

        GL.BindFramebuffer(FramebufferTarget.Framebuffer, GLFramebufferHandle);

        // Texture2D
        {
            Guid guid = typeof(ID3D11Texture2D).GetTypeInfo().GUID;
            ((IDXGISwapChain1*)SwapChainHandle)->GetBuffer(0, &guid, (void**)&colorbuffer);
        }

        // GL
        {
            GLColorRenderbufferHandle = GL.GenRenderbuffer();
            GLDepthRenderBufferHandle = GL.GenRenderbuffer();

            DxInteropColorHandle = Wgl.DXRegisterObjectNV(Context.GlDeviceHandle, (nint)colorbuffer, (uint)GLColorRenderbufferHandle, (uint)RenderbufferTarget.Renderbuffer, WGL_NV_DX_interop.AccessReadWrite);
            GL.FramebufferRenderbuffer(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, RenderbufferTarget.Renderbuffer, (uint)GLColorRenderbufferHandle);

            GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, GLDepthRenderBufferHandle);
            GL.RenderbufferStorage(RenderbufferTarget.Renderbuffer, RenderbufferStorage.Depth24Stencil8, FramebufferWidth, FramebufferHeight);
            GL.FramebufferRenderbuffer(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthAttachment, RenderbufferTarget.Renderbuffer, (uint)GLDepthRenderBufferHandle);
            GL.FramebufferRenderbuffer(FramebufferTarget.Framebuffer, FramebufferAttachment.StencilAttachment, RenderbufferTarget.Renderbuffer, (uint)GLDepthRenderBufferHandle);
        }

        colorbuffer->Release();

        Wgl.DXLockObjectsNV(Context.GlDeviceHandle, 1, new[] { DxInteropColorHandle });

        GL.BindFramebuffer(FramebufferTarget.Framebuffer, GLFramebufferHandle);
        GL.Viewport(0, 0, FramebufferWidth, FramebufferHeight);
    }

    public void End()
    {
        GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);

        Wgl.DXUnlockObjectsNV(Context.GlDeviceHandle, 1, new[] { DxInteropColorHandle });

        Wgl.DXUnregisterObjectNV(Context.GlDeviceHandle, DxInteropColorHandle);

        GL.DeleteRenderbuffer(GLColorRenderbufferHandle);
        GL.DeleteRenderbuffer(GLDepthRenderBufferHandle);

        ((IDXGISwapChain1*)SwapChainHandle)->Present(1, 0);
    }

    public void UpdateSize(int framebufferWidth, int framebufferHeight)
    {
        FramebufferWidth = framebufferWidth;
        FramebufferHeight = framebufferHeight;

        ((IDXGISwapChain1*)SwapChainHandle)->ResizeBuffers(2, (uint)framebufferWidth, (uint)framebufferHeight, Format.FormatUnknown, 0);
    }

    public override void Dispose()
    {
        GL.DeleteFramebuffer(GLFramebufferHandle);

        Wgl.DXUnregisterObjectNV(Context.GlDeviceHandle, DxInteropColorHandle);
        GL.DeleteRenderbuffer(GLColorRenderbufferHandle);
        GL.DeleteRenderbuffer(GLDepthRenderBufferHandle);

        GC.SuppressFinalize(this);
    }
}
