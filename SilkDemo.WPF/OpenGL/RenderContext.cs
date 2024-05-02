using System;
using System.Threading;
using Silk.NET.Core.Contexts;
using Silk.NET.Direct3D9;
using Silk.NET.GLFW;
using Silk.NET.OpenGL;
using Silk.NET.WGL;
using Silk.NET.WGL.Extensions.NV;
using Silk.NET.Windowing;

namespace SilkDemo.WPF.OpenGL;

public unsafe class RenderContext
{
    private static Settings _sharedContextSettings;
    private static int _sharedContextReferenceCount;

    public static GL GL { get; private set; }

    public static NVDXInterop NVDXInterop { get; private set; }

    public Format Format { get; }

    public IntPtr DxDeviceHandle { get; }

    public IntPtr GlDeviceHandle { get; }

    public RenderContext(Settings settings)
    {
        IDirect3D9Ex* direct3D9;
        IDirect3DDevice9Ex* device;
        D3D9.GetApi(null).Direct3DCreate9Ex(D3D9.SdkVersion, &direct3D9);

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

        GetOrCreateSharedOpenGLContext(settings);

        GlDeviceHandle = NVDXInterop.DxopenDevice(device);
    }

    private static void GetOrCreateSharedOpenGLContext(Settings settings)
    {
        if (_sharedContextSettings == null)
        {
            WindowOptions options = WindowOptions.Default;

            options.API = new GraphicsAPI(ContextAPI.OpenGL, settings.GraphicsProfile, settings.GraphicsContextFlags, new APIVersion(settings.MajorVersion, settings.MinorVersion));
            options.IsVisible = false;

            IWindow window = Window.Create(options);

            window.CreateWindow(options);
            window.Initialize();

            GL = window.CreateOpenGL();
            NVDXInterop = new(new LamdaNativeContext((name) =>
            {
                return GL.Context.GetProcAddress(name);
            }));

            _sharedContextSettings = settings;
        }

        Interlocked.Increment(ref _sharedContextReferenceCount);
    }
}
