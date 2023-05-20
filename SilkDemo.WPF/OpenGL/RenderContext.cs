using Silk.NET.Core.Contexts;
using Silk.NET.Direct3D9;
using Silk.NET.GLFW;
using Silk.NET.OpenGL;
using Silk.NET.WGL.Extensions.NV;
using System;
using System.Threading;
using Monitor = Silk.NET.GLFW.Monitor;

namespace SilkDemo.WPF.OpenGL;

public unsafe class RenderContext
{
    private static Settings _sharedContextSettings;
    private static int _sharedContextReferenceCount;

    public static Glfw Glfw { get; private set; }

    public static GL Gl { get; private set; }

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
            Glfw = Glfw.GetApi();

            Glfw.Init();

            Glfw.WindowHint(WindowHintBool.Decorated, false);

            Glfw.WindowHint(WindowHintClientApi.ClientApi, ClientApi.OpenGL);

            Glfw.WindowHint(WindowHintInt.ContextVersionMajor, settings.MajorVersion);
            Glfw.WindowHint(WindowHintInt.ContextVersionMinor, settings.MinorVersion);

            Glfw.WindowHint(WindowHintOpenGlProfile.OpenGlProfile, settings.OpenGlProfile);

            Glfw.WindowHint(WindowHintBool.Focused, false);
            Glfw.WindowHint(WindowHintBool.Visible, false);
            Glfw.WindowHint(WindowHintInt.Samples, 0);
            Glfw.WindowHint(WindowHintBool.SrgbCapable, false);

            Monitor* monitor = Glfw.GetPrimaryMonitor();
            VideoMode* videoMode = Glfw.GetVideoMode(monitor);

            Glfw.WindowHint(WindowHintInt.RedBits, videoMode->RedBits);
            Glfw.WindowHint(WindowHintInt.GreenBits, videoMode->GreenBits);
            Glfw.WindowHint(WindowHintInt.BlueBits, videoMode->BlueBits);
            Glfw.WindowHint(WindowHintInt.RefreshRate, videoMode->RefreshRate);

            WindowHandle* window = Glfw.CreateWindow(640, 360, "OpenGL Window", null, null);
            Glfw.MakeContextCurrent(window);
            Glfw.SetWindowSizeLimits(window, -1, -1, -1, -1);

            Gl = GL.GetApi(new LamdaNativeContext(Glfw.GetProcAddress));

            NVDXInterop = new(new LamdaNativeContext(Glfw.GetProcAddress));

            _sharedContextSettings = settings;
        }

        Interlocked.Increment(ref _sharedContextReferenceCount);
    }
}
