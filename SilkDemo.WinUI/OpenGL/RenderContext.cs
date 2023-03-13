using OpenTK.Graphics.Wgl;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using Silk.NET.Core.Native;
using Silk.NET.Direct3D11;
using Silk.NET.DXGI;
using System;
using System.Reflection;
using System.Threading;

namespace SilkDemo.WinUI.OpenGL;

public unsafe class RenderContext
{
    private static IGraphicsContext _sharedContext;
    private static Settings _sharedContextSettings;
    private static int _sharedContextReferenceCount;

    public Format Format { get; }

    public IntPtr DxDeviceFactory { get; }

    public IntPtr DxDeviceHandle { get; }

    public IntPtr DxDeviceContext { get; }

    public IntPtr GlDeviceHandle { get; }

    public IGraphicsContext GraphicsContext { get; }

    public RenderContext(Settings settings)
    {
        IDXGIFactory2* factory;
        ID3D11Device* device;
        ID3D11DeviceContext* devCtx;

        // Factory
        {
            Guid guid = typeof(IDXGIFactory2).GetTypeInfo().GUID;
            DXGI.GetApi().CreateDXGIFactory2(0, &guid, (void**)&factory);
        }

        // Device
        {
            D3D11.GetApi().CreateDevice(null, D3DDriverType.Hardware, 0, 0, null, 0, D3D11.SdkVersion, &device, null, &devCtx);
        }

        DxDeviceFactory = (IntPtr)factory;
        DxDeviceHandle = (IntPtr)device;
        DxDeviceContext = (IntPtr)devCtx;

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
