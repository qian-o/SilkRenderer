using System;
using Silk.NET.Direct3D9;

namespace SilkDemo.WPF.Direct3D9;

public unsafe class RenderContext
{
    public IDirect3DDevice9Ex* Device { get; }

    public Format Format { get; }

    public RenderContext()
    {
        IDirect3D9Ex* direct3D9;
        IDirect3DDevice9Ex* device;
        D3D9.GetApi(null).Direct3DCreate9Ex(D3D9.SdkVersion, &direct3D9);

        Displaymodeex pMode = new((uint)sizeof(Displaymodeex));
        direct3D9->GetAdapterDisplayModeEx(D3D9.AdapterDefault, ref pMode, null);

        PresentParameters presentParameters = new()
        {
            Windowed = 1,
            SwapEffect = Swapeffect.Discard,
            HDeviceWindow = 0,
            PresentationInterval = 0,
            BackBufferFormat = pMode.Format,
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

        Device = device;
        Format = pMode.Format;
    }

    public static Format MakeFourCC(byte c1, byte c2, byte c3, byte c4)
    {
        return (Format)((((((c4 << 8) | c3) << 8) | c2) << 8) | c1);
    }
}
