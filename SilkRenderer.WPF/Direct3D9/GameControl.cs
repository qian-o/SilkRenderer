using System;
using System.Windows;
using System.Windows.Media;
using Silk.NET.Direct3D9;
using SilkRenderer.WPF.Common;
using Rect = System.Windows.Rect;

namespace SilkRenderer.WPF.Direct3D9;

public unsafe class GameControl : GameBase<Framebuffer>
{
    private RenderContext _context;

    public IDirect3DDevice9Ex* Device { get; private set; }
    public Format Format { get; private set; }

    public override event Action Ready;
    public override event Action<TimeSpan> Render;
    public override event Action<object, TimeSpan> UpdateFrame;

    protected override void OnStart()
    {
        if (_context == null)
        {
            _context = new RenderContext();
            Device = _context.Device;
            Format = _context.Format;

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

        Render?.Invoke(_stopwatch.Elapsed - _lastFrameStamp);

        Framebuffer.D3dImage.AddDirtyRect(new Int32Rect(0, 0, Framebuffer.FramebufferWidth, Framebuffer.FramebufferHeight));
        Framebuffer.D3dImage.Unlock();

        Rect rect = new(0, 0, Framebuffer.D3dImage.Width, Framebuffer.D3dImage.Height);
        drawingContext.DrawImage(Framebuffer.D3dImage, rect);

        UpdateFrame?.Invoke(this, _stopwatch.Elapsed - _lastFrameStamp);
    }
}
