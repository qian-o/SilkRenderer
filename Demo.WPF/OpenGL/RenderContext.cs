using OpenTK.Graphics.Wgl;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using Silk.NET.Direct3D9;
using System;
using System.Threading;

namespace SilkDemo.WPF.OpenGL;

public unsafe class RenderContext
{
    private static IGraphicsContext _sharedContext;
    private static Settings _sharedContextSettings;
    private static int _sharedContextReferenceCount;

    public Format Format { get; }

    public IntPtr DxDeviceHandle { get; }

    public IntPtr GlDeviceHandle { get; }

    public IGraphicsContext GraphicsContext { get; }

    public RenderContext(Settings settings)
    {
        IDirect3D9Ex* direct3D9;
        IDirect3DDevice9Ex* device;
        D3D9.GetApi().Direct3DCreate9Ex(D3D9.SdkVersion, &direct3D9);

        Displaymodeex pMode = new((uint)sizeof(Displaymodeex));
        direct3D9->GetAdapterDisplayModeEx(D3D9.AdapterDefault, ref pMode, null);
        Format = pMode.Format;

        PresentParameters presentParameters = new()
        {
            Windowed = 1,
            SwapEffect = Swapeffect.Discard,
            HDeviceWindow = 0,
            PresentationInterval = 0,
            BackBufferFormat = Format,
            BackBufferWidth = 1,
            BackBufferHeight = 1,
            AutoDepthStencilFormat = Format.Unknown,
            BackBufferCount = 1,
            EnableAutoDepthStencil = 0,
            Flags = 0,
            FullScreenRefreshRateInHz = 0,
            MultiSampleQuality = 0,
            MultiSampleType = MultisampleType.MultisampleNone
        };
        direct3D9->CreateDeviceEx(D3D9.AdapterDefault, Devtype.Hal, 0, D3D9.CreateHardwareVertexprocessing | D3D9.CreateMultithreaded | D3D9.CreatePuredevice, ref presentParameters, (Displaymodeex*)IntPtr.Zero, &device);

        DxDeviceHandle = (IntPtr)device;

        GraphicsContext = GetOrCreateSharedOpenGLContext(settings);
        GlDeviceHandle = Wgl.DXOpenDeviceNV((IntPtr)device);
    }

    private static IGraphicsContext GetOrCreateSharedOpenGLContext(Settings settings)
    {
        if (_sharedContext == null)
        {
            NativeWindowSettings windowSettings = NativeWindowSettings.Default;
            windowSettings.StartFocused = false;
            windowSettings.StartVisible = false;
            windowSettings.NumberOfSamples = 0;
            windowSettings.APIVersion = new Version(settings.MajorVersion, settings.MinorVersion);
            windowSettings.Flags = ContextFlags.Offscreen | settings.GraphicsContextFlags;
            windowSettings.Profile = settings.GraphicsProfile;
            windowSettings.WindowBorder = WindowBorder.Hidden;
            windowSettings.WindowState = WindowState.Minimized;
            NativeWindow nativeWindow = new(windowSettings);
            Wgl.LoadBindings(new GLFWBindingsContext());

            _sharedContext = nativeWindow.Context;
            _sharedContextSettings = settings;

            _sharedContext.MakeCurrent();
        }
        else
        {
            if (!Settings.WouldResultInSameContext(settings, _sharedContextSettings))
            {
                throw new ArgumentException($"The provided {nameof(Settings)} would result" +
                                                $"in a different context creation to one previously created. To fix this," +
                                                $" either ensure all of your context settings are identical, or provide an " +
                                                $"external context via the '{nameof(Settings.ContextToUse)}' field.");
            }
        }

        Interlocked.Increment(ref _sharedContextReferenceCount);

        return _sharedContext;
    }
}
