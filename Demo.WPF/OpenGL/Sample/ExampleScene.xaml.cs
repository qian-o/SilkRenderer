using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using System;
using System.Diagnostics;
using System.Windows.Controls;

namespace SilkDemo.WPF.OpenGL.Sample;

public partial class ExampleScene : UserControl
{
    private readonly Stopwatch _stopwatch = Stopwatch.StartNew();

    public ExampleScene()
    {
        InitializeComponent();

        Game.Setting = new Settings()
        {
            MajorVersion = 4,
            MinorVersion = 5,
            GraphicsProfile = ContextProfile.Compatability
        };
        Game.Render += Game_Render;
        Game.Start();
    }

    private void Game_Render(TimeSpan obj)
    {
        float hue = (float)_stopwatch.Elapsed.TotalSeconds * 0.15f % 1;
        Color4 c = Color4.FromHsv(new Vector4(1.0f * hue, 1.0f * 0.75f, 1.0f * 0.75f, 1.0f));
        GL.ClearColor(c);
        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        GL.LoadIdentity();
        GL.Begin(PrimitiveType.Triangles);

        GL.Color4(Color4.Red);
        GL.Vertex2(0.0f, 0.5f);

        GL.Color4(Color4.Green);
        GL.Vertex2(0.58f, -0.5f);

        GL.Color4(Color4.Blue);
        GL.Vertex2(-0.58f, -0.5f);

        GL.End();
        GL.Finish();
    }
}
