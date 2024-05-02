using System;
using System.Diagnostics;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Windows.Controls;
using Silk.NET.Direct3D9;
using Silk.NET.Maths;
using SilkDemo.WPF.Common;

namespace SilkDemo.WPF.Direct3D9.Sample;

public unsafe partial class MiniTri : UserControl
{
    [StructLayout(LayoutKind.Sequential)]
    struct Vertex
    {
        public Vector4 Position;
        public uint Color;
    }

    private readonly Stopwatch _stopwatch = Stopwatch.StartNew();
    private readonly Vertex[] _vertices =
    {
        new Vertex() { Color = (uint)SilkColor.Red.ToBgra(), Position = new Vector4(400.0f, 100.0f, 0.5f, 1.0f) },
        new Vertex() { Color = (uint)SilkColor.Blue.ToBgra(), Position = new Vector4(650.0f, 500.0f, 0.5f, 1.0f) },
        new Vertex() { Color = (uint)SilkColor.Green.ToBgra(), Position = new Vector4(150.0f, 500.0f, 0.5f, 1.0f) }
    };
    private readonly Vertexelement9[] _vertexelements =
    {
        new Vertexelement9(0, 0, 3, 0, 9, 0),
        new Vertexelement9(0, 16, 4, 0, 10, 0),
        new Vertexelement9(255, 0, 17, 0, 0, 0)
    };

    private IDirect3DVertexBuffer9* _ppVertexBuffer;
    private IDirect3DVertexDeclaration9* _ppDecl;

    public MiniTri()
    {
        InitializeComponent();

        Game.Ready += Game_Ready;
        Game.Render += Game_Render;
        Game.Start();
    }

    private void Game_Ready()
    {
        fixed (Vertex* ptr = &_vertices[0])
        {
            fixed (Vertexelement9* vertexElems = &_vertexelements[0])
            {
                void* ppbData;
                Game.Device->CreateVertexBuffer(3 * 20, D3D9.UsageWriteonly, 0, Pool.Default, ref _ppVertexBuffer, null);
                _ppVertexBuffer->Lock(0, 0, &ppbData, 0);
                System.Runtime.CompilerServices.Unsafe.CopyBlockUnaligned(ppbData, ptr, (uint)(sizeof(Vertex) * _vertices.Length));
                _ppVertexBuffer->Unlock();

                Game.Device->CreateVertexDeclaration(vertexElems, ref _ppDecl);
            }
        }
    }

    private void Game_Render(TimeSpan obj)
    {
        float hue = (float)_stopwatch.Elapsed.TotalSeconds * 0.15f % 1;
        Vector4 vector = new(1.0f * hue, 1.0f * 0.75f, 1.0f * 0.75f, 1.0f);

        Game.Device->Clear(0, null, D3D9.ClearTarget, (uint)SilkColor.FromHsv(vector).ToBgra(), 1.0f, 0);
        Game.Device->BeginScene();

        Game.Device->SetStreamSource(0, _ppVertexBuffer, 0, 20);
        Game.Device->SetVertexDeclaration(_ppDecl);
        Game.Device->DrawPrimitive(Primitivetype.Trianglelist, 0, 1);

        Game.Device->EndScene();
        Game.Device->Present((Box2D<int>*)IntPtr.Zero, (Box2D<int>*)IntPtr.Zero, 1, (RGNData*)IntPtr.Zero);
    }
}
