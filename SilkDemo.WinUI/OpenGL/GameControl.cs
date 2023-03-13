using SilkDemo.WinUI.Common;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using System;
using WinRT;

namespace SilkDemo.WinUI.OpenGL;

public unsafe class GameControl : GameBase<Framebuffer>
{
    private RenderContext _context;
    private SwapChainPanel _swapChainPanel;

    public Settings Setting { get; set; } = new Settings();

    public override event Action Ready;
    public override event Action<TimeSpan> Render;
    public override event Action<object, TimeSpan> UpdateFrame;

    protected override void OnStart()
    {
        if (_context == null)
        {
            _context = new RenderContext(Setting);
            _swapChainPanel = new SwapChainPanel();

            HorizontalContentAlignment = HorizontalAlignment.Stretch;
            VerticalContentAlignment = VerticalAlignment.Stretch;
            Content = _swapChainPanel;

            Ready?.Invoke();
        }
    }

    protected override void OnSizeChanged(SizeChangedEventArgs sizeInfo)
    {
        if (_context != null && sizeInfo.NewSize.Width > 0 && sizeInfo.NewSize.Height > 0)
        {
            if (Framebuffer == null)
            {
                Framebuffer = new Framebuffer(_context, (int)ActualWidth, (int)ActualHeight);

                TransformGroup transformGroup = new();
                transformGroup.Children.Add(Framebuffer.FlipYTransform);
                transformGroup.Children.Add(Framebuffer.TranslateTransform);
                _swapChainPanel.RenderTransform = transformGroup;

                _swapChainPanel.As<ISwapChainPanelNative>().SetSwapChain(Framebuffer.SwapChainHandle);
            }
            else
            {
                Framebuffer?.UpdateSize((int)ActualWidth, (int)ActualHeight);
            }
        }
    }

    protected override void OnDraw()
    {
        Framebuffer.Begin();

        Render?.Invoke(_stopwatch.Elapsed - _lastFrameStamp);

        Framebuffer.End();

        UpdateFrame?.Invoke(this, _stopwatch.Elapsed - _lastFrameStamp);
    }
}
