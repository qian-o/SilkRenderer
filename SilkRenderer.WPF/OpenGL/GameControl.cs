using System;
using System.Windows;
using System.Windows.Media;
using Silk.NET.Maths;
using Silk.NET.OpenGL;
using Silk.NET.WGL.Extensions.NV;
using SilkRenderer.WPF.Common;

namespace SilkRenderer.WPF.OpenGL;

public class GameControl : GameBase<Framebuffer>
{
    private RenderContext _context;

    public Settings Setting { get; set; } = new Settings();

    public override event Action Ready;
    public override event Action<TimeSpan> Render;
    public override event Action<object, TimeSpan> UpdateFrame;

    protected override void OnStart()
    {
        if (_context == null)
        {
            _context = new RenderContext(Setting);

            Ready?.Invoke();
        }
    }

    protected override void OnSizeChanged(SizeChangedInfo sizeInfo)
    {
        if (_context != null && sizeInfo.NewSize.Width > 0 && sizeInfo.NewSize.Height > 0)
        {
            Framebuffer?.Dispose();
            Framebuffer = new Framebuffer(_context, (int)sizeInfo.NewSize.Width, (int)sizeInfo.NewSize.Height);
        }
    }

    protected override void OnDraw(DrawingContext drawingContext)
    {
        Framebuffer.D3dImage.Lock();

        RenderContext.NVDXInterop.DxlockObjects(_context.GlDeviceHandle, 1, new[] { Framebuffer.DxInteropRegisteredHandle });
        RenderContext.GL.BindFramebuffer(FramebufferTarget.Framebuffer, Framebuffer.GLFramebufferHandle);

        RenderContext.GL.Viewport(new Rectangle<int>(0, 0, Framebuffer.FramebufferWidth, Framebuffer.FramebufferHeight));
        Render?.Invoke(_stopwatch.Elapsed - _lastFrameStamp);

        RenderContext.GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
        RenderContext.NVDXInterop.DxunlockObjects(_context.GlDeviceHandle, 1, new[] { Framebuffer.DxInteropRegisteredHandle });

        Framebuffer.D3dImage.AddDirtyRect(new Int32Rect(0, 0, Framebuffer.FramebufferWidth, Framebuffer.FramebufferHeight));
        Framebuffer.D3dImage.Unlock();

        drawingContext.PushTransform(Framebuffer.TranslateTransform);
        drawingContext.PushTransform(Framebuffer.FlipYTransform);

        Rect rect = new(0, 0, Framebuffer.D3dImage.Width, Framebuffer.D3dImage.Height);
        drawingContext.DrawImage(Framebuffer.D3dImage, rect);

        drawingContext.Pop();
        drawingContext.Pop();

        UpdateFrame?.Invoke(this, _stopwatch.Elapsed - _lastFrameStamp);
    }
}
