using System;
using System.Diagnostics;
using System.Numerics;
using System.Windows;
using System.Windows.Controls;
using Silk.NET.OpenGL;
using SilkRenderer.WPF.Common;

namespace SilkRenderer.WPF.OpenGL.Sample;

public partial class ExampleScene : UserControl
{
    private readonly Stopwatch _stopwatch = Stopwatch.StartNew();
    uint vao;
    uint shaderProgram;

    public ExampleScene()
    {
        InitializeComponent();

        Game.Setting = new Settings()
        {
            MajorVersion = 4,
            MinorVersion = 5,
            OpenGlProfile = Silk.NET.GLFW.OpenGlProfile.Compat
        };
        Game.Loaded += Game_Loaded;
        Game.Render += Game_Render;
        Game.Start();
    }

    private unsafe void Game_Loaded(object sender, RoutedEventArgs e)
    {
        GL gl = RenderContext.GL;
        gl.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);

        float[] vertices = {
                -0.5f, -0.5f, 0.0f,
                 0.5f, -0.5f, 0.0f,
                 0.0f,  0.5f, 0.0f
            };

        gl.GenBuffers(1, out uint vbo);
        gl.BindBuffer(GLEnum.ArrayBuffer, vbo);
        gl.BufferData<float>(GLEnum.ArrayBuffer, (nuint)vertices.Length * sizeof(float), vertices, GLEnum.StaticDraw);

        gl.GenVertexArrays(1, out vao);
        gl.BindVertexArray(vao);
        gl.VertexAttribPointer(0, 3, GLEnum.Float, false, 3 * sizeof(float), null);
        gl.EnableVertexAttribArray(0);

        string vertexShaderSource = @"
                #version 330 core
                layout (location = 0) in vec3 aPos;
                void main()
                {
                    gl_Position = vec4(aPos.x, aPos.y, aPos.z, 1.0);
                }
            ";

        string fragmentShaderSource = @"
                #version 330 core
                out vec4 FragColor;
                void main()
                {
                    FragColor = vec4(1.0f, 0.5f, 0.2f, 1.0f);
                }
            ";

        uint vertexShader = gl.CreateShader(GLEnum.VertexShader);
        gl.ShaderSource(vertexShader, vertexShaderSource);
        gl.CompileShader(vertexShader);

        uint fragmentShader = gl.CreateShader(GLEnum.FragmentShader);
        gl.ShaderSource(fragmentShader, fragmentShaderSource);
        gl.CompileShader(fragmentShader);

        shaderProgram = gl.CreateProgram();
        gl.AttachShader(shaderProgram, vertexShader);
        gl.AttachShader(shaderProgram, fragmentShader);
        gl.LinkProgram(shaderProgram);

        gl.DeleteShader(vertexShader);
        gl.DeleteShader(fragmentShader);

    }

    private void Game_Render(TimeSpan obj)
    {
        GL gl = RenderContext.GL;

        float hue = (float)_stopwatch.Elapsed.TotalSeconds * 0.15f % 1;

        gl.Disable(EnableCap.DepthTest);

        gl.ClearColor(SilkColor.ByDrawingColor(SilkColor.FromHsv(new Vector4(1.0f * hue, 1.0f * 0.75f, 1.0f * 0.75f, 1.0f))));
        gl.Clear(ClearBufferMask.ColorBufferBit);

        gl.UseProgram(shaderProgram);
        gl.BindVertexArray(vao);
        gl.DrawArrays(GLEnum.Triangles, 0, 3);
    }
}
