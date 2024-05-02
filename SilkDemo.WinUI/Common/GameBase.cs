using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;

namespace SilkDemo.WinUI.Common;

public abstract class GameBase<TFrame> : ContentControl where TFrame : FramebufferBase
{
    public static readonly DependencyProperty FpsProperty = DependencyProperty.Register(nameof(Fps), typeof(int), typeof(GameBase<TFrame>), new PropertyMetadata(0));

    protected Stopwatch _stopwatch = Stopwatch.StartNew();
    private readonly List<int> _fpsSample = new();

    protected TimeSpan _lastRenderTime = TimeSpan.FromSeconds(-1);
    protected TimeSpan _lastFrameStamp;

    protected TFrame Framebuffer { get; set; }
    public int Fps
    {
        get { return (int)GetValue(FpsProperty); }
        set { SetValue(FpsProperty, value); }
    }

    public abstract event Action Ready;
    public abstract event Action<TimeSpan> Render;
    public abstract event Action<object, TimeSpan> UpdateFrame;

    protected abstract void OnStart();
    protected abstract void OnDraw();
    protected abstract void OnSizeChanged(SizeChangedEventArgs sizeInfo);

    protected GameBase()
    {
        SizeChanged += GameBase_SizeChanged;
    }

    private void GameBase_SizeChanged(object sender, SizeChangedEventArgs e)
    {
        OnSizeChanged(e);
    }

    private void CompositionTarget_Rendering(object sender, object e)
    {
        RenderingEventArgs args = (RenderingEventArgs)e;

        if (_lastRenderTime != args.RenderingTime)
        {
            InvalidateVisual();

            _fpsSample.Add(Convert.ToInt32(1000.0d / (args.RenderingTime.TotalMilliseconds - _lastRenderTime.TotalMilliseconds)));
            // 样本数 30
            if (_fpsSample.Count == 30)
            {
                Fps = Convert.ToInt32(_fpsSample.Average());
                _fpsSample.Clear();
            }

            _lastRenderTime = args.RenderingTime;
        }
    }

    private void InvalidateVisual()
    {
        if (Framebuffer != null)
        {
            OnDraw();

            _stopwatch.Restart();
        }
    }

    public void Start()
    {
        Unloaded += (_, _) =>
        {
            EffectiveViewportChanged -= GameBase_EffectiveViewportChanged;

            CompositionTarget.Rendering -= CompositionTarget_Rendering;
        };

        Loaded += (_, _) =>
        {
            EffectiveViewportChanged += GameBase_EffectiveViewportChanged;

            InvalidateVisual();
        };

        OnStart();
    }

    private void GameBase_EffectiveViewportChanged(FrameworkElement sender, EffectiveViewportChangedEventArgs args)
    {
        CompositionTarget.Rendering -= CompositionTarget_Rendering;

        if (args.EffectiveViewport.Width != 1 && args.EffectiveViewport.Height != 1)
        {
            CompositionTarget.Rendering += CompositionTarget_Rendering;
        }
    }
}
