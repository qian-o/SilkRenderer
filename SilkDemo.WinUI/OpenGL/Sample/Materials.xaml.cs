using System;
using System.IO;
using System.Numerics;
using System.Runtime.InteropServices;
using Microsoft.UI.Xaml.Controls;
using Silk.NET.OpenGL;
using Silk.NET.Windowing;
using SilkDemo.WinUI.OpenGL.Common;
using Shader = SilkDemo.WinUI.OpenGL.Common.Shader;

namespace SilkDemo.WinUI.OpenGL.Sample;

public sealed partial class Materials : UserControl
{
    [LibraryImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static partial bool GetAsyncKeyState(VirtualKeyCodes vKey);

    [LibraryImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static partial bool GetCursorPos(out POINT pt);

    [StructLayout(LayoutKind.Sequential)]
    public struct POINT
    {
        public int X;
        public int Y;
    }

    public enum VirtualKeyCodes
    {
        VK_LBUTTON = 0x01,
        VK_RBUTTON = 0x02,

        W = 0x57,
        A = 0x41,
        S = 0x53,
        D = 0x44,
        E = 0x45,
        Q = 0x51
    }

    const float cameraSpeed = 1.5f;
    const float sensitivity = 0.2f;

    private readonly float[] _vertices =
    {
             // Position          Normal
        -0.5f, -0.5f, -0.5f,  0.0f,  0.0f, -1.0f, // Front face
         0.5f, -0.5f, -0.5f,  0.0f,  0.0f, -1.0f,
         0.5f,  0.5f, -0.5f,  0.0f,  0.0f, -1.0f,
         0.5f,  0.5f, -0.5f,  0.0f,  0.0f, -1.0f,
        -0.5f,  0.5f, -0.5f,  0.0f,  0.0f, -1.0f,
        -0.5f, -0.5f, -0.5f,  0.0f,  0.0f, -1.0f,

        -0.5f, -0.5f,  0.5f,  0.0f,  0.0f,  1.0f, // Back face
         0.5f, -0.5f,  0.5f,  0.0f,  0.0f,  1.0f,
         0.5f,  0.5f,  0.5f,  0.0f,  0.0f,  1.0f,
         0.5f,  0.5f,  0.5f,  0.0f,  0.0f,  1.0f,
        -0.5f,  0.5f,  0.5f,  0.0f,  0.0f,  1.0f,
        -0.5f, -0.5f,  0.5f,  0.0f,  0.0f,  1.0f,

        -0.5f,  0.5f,  0.5f, -1.0f,  0.0f,  0.0f, // Left face
        -0.5f,  0.5f, -0.5f, -1.0f,  0.0f,  0.0f,
        -0.5f, -0.5f, -0.5f, -1.0f,  0.0f,  0.0f,
        -0.5f, -0.5f, -0.5f, -1.0f,  0.0f,  0.0f,
        -0.5f, -0.5f,  0.5f, -1.0f,  0.0f,  0.0f,
        -0.5f,  0.5f,  0.5f, -1.0f,  0.0f,  0.0f,

         0.5f,  0.5f,  0.5f,  1.0f,  0.0f,  0.0f, // Right face
         0.5f,  0.5f, -0.5f,  1.0f,  0.0f,  0.0f,
         0.5f, -0.5f, -0.5f,  1.0f,  0.0f,  0.0f,
         0.5f, -0.5f, -0.5f,  1.0f,  0.0f,  0.0f,
         0.5f, -0.5f,  0.5f,  1.0f,  0.0f,  0.0f,
         0.5f,  0.5f,  0.5f,  1.0f,  0.0f,  0.0f,

        -0.5f, -0.5f, -0.5f,  0.0f, -1.0f,  0.0f, // Bottom face
         0.5f, -0.5f, -0.5f,  0.0f, -1.0f,  0.0f,
         0.5f, -0.5f,  0.5f,  0.0f, -1.0f,  0.0f,
         0.5f, -0.5f,  0.5f,  0.0f, -1.0f,  0.0f,
        -0.5f, -0.5f,  0.5f,  0.0f, -1.0f,  0.0f,
        -0.5f, -0.5f, -0.5f,  0.0f, -1.0f,  0.0f,

        -0.5f,  0.5f, -0.5f,  0.0f,  1.0f,  0.0f, // Top face
         0.5f,  0.5f, -0.5f,  0.0f,  1.0f,  0.0f,
         0.5f,  0.5f,  0.5f,  0.0f,  1.0f,  0.0f,
         0.5f,  0.5f,  0.5f,  0.0f,  1.0f,  0.0f,
        -0.5f,  0.5f,  0.5f,  0.0f,  1.0f,  0.0f,
        -0.5f,  0.5f, -0.5f,  0.0f,  1.0f,  0.0f
    };

    private readonly Vector3 _lightPos = new(1.2f, 1.0f, 2.0f);

    private uint _vertexBufferObject;

    private uint _vaoModel;

    private uint _vaoLamp;

    private Shader _lampShader;

    private Shader _lightingShader;

    private Camera _camera;

    private bool _firstMove = true;

    private Vector2 _lastPos;

    public Materials()
    {
        InitializeComponent();

        Game.Setting = new Settings()
        {
            MajorVersion = 4,
            MinorVersion = 5,
            GraphicsProfile = ContextProfile.Compatability
        };
        Game.Ready += Game_Ready;
        Game.Render += Game_Render;
        Game.UpdateFrame += Game_UpdateFrame;
        Game.Start();
    }

    private void Game_Ready()
    {
        RenderContext.GL.Enable(EnableCap.DepthTest);

        _vertexBufferObject = RenderContext.GL.GenBuffer();
        RenderContext.GL.BindBuffer(GLEnum.ArrayBuffer, _vertexBufferObject);
        RenderContext.GL.BufferData(GLEnum.ArrayBuffer, (uint)_vertices.Length * sizeof(float), ref _vertices[0], GLEnum.StaticDraw);

        string path = AppDomain.CurrentDomain.BaseDirectory;

        _lightingShader = new Shader(Path.Combine(path, "OpenGL/Sample/Shaders/shader.vert"), Path.Combine(path, "OpenGL/Sample/Shaders/lighting.frag"));
        _lampShader = new Shader(Path.Combine(path, "OpenGL/Sample/Shaders/shader.vert"), Path.Combine(path, "OpenGL/Sample/Shaders/shader.frag"));

        {
            _vaoModel = RenderContext.GL.GenVertexArray();
            RenderContext.GL.BindVertexArray(_vaoModel);

            var positionLocation = _lightingShader.GetAttribLocation("aPos");
            RenderContext.GL.EnableVertexAttribArray((uint)positionLocation);
            RenderContext.GL.VertexAttribPointer((uint)positionLocation, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 0);

            var normalLocation = _lightingShader.GetAttribLocation("aNormal");
            RenderContext.GL.EnableVertexAttribArray((uint)normalLocation);
            RenderContext.GL.VertexAttribPointer((uint)normalLocation, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 3 * sizeof(float));
        }

        {
            _vaoLamp = RenderContext.GL.GenVertexArray();
            RenderContext.GL.BindVertexArray(_vaoLamp);

            var positionLocation = _lampShader.GetAttribLocation("aPos");
            RenderContext.GL.EnableVertexAttribArray((uint)positionLocation);
            RenderContext.GL.VertexAttribPointer((uint)positionLocation, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 0);
        }

        _camera = new Camera(Vector3.UnitZ * 3, 0);
    }

    private void Game_Render(TimeSpan obj)
    {
        _camera.AspectRatio = (float)(ActualWidth / ActualHeight);

        RenderContext.GL.Enable(EnableCap.DepthTest);

        RenderContext.GL.ClearColor(0.5f, 0.5f, 0.5f, 1.0f);
        RenderContext.GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        RenderContext.GL.BindVertexArray(_vaoModel);
        _lightingShader.Use();

        _lightingShader.SetMatrix4("model", Matrix4x4.Identity);
        _lightingShader.SetMatrix4("view", _camera.GetViewMatrix());
        _lightingShader.SetMatrix4("projection", _camera.GetProjectionMatrix());

        _lightingShader.SetVector3("viewPos", _camera.Position);

        // Here we set the material values of the cube, the material struct is just a container so to access
        // the underlying values we simply type "material.value" to get the location of the uniform
        _lightingShader.SetVector3("material.ambient", new Vector3(1.0f, 0.5f, 0.31f));
        _lightingShader.SetVector3("material.diffuse", new Vector3(1.0f, 0.5f, 0.31f));
        _lightingShader.SetVector3("material.specular", new Vector3(0.5f, 0.5f, 0.5f));
        _lightingShader.SetFloat("material.shininess", 32.0f);

        // This is where we change the lights color over time using the sin function
        Vector3 lightColor;
        float time = DateTime.Now.Second + DateTime.Now.Millisecond / 1000f;
        lightColor.X = (MathF.Sin(time * 2.0f) + 1) / 2f;
        lightColor.Y = (MathF.Sin(time * 0.7f) + 1) / 2f;
        lightColor.Z = (MathF.Sin(time * 1.3f) + 1) / 2f;

        // The ambient light is less intensive than the diffuse light in order to make it less dominant
        Vector3 ambientColor = lightColor * new Vector3(0.2f);
        Vector3 diffuseColor = lightColor * new Vector3(0.5f);

        _lightingShader.SetVector3("light.position", _lightPos);
        _lightingShader.SetVector3("light.ambient", ambientColor);
        _lightingShader.SetVector3("light.diffuse", diffuseColor);
        _lightingShader.SetVector3("light.specular", new Vector3(1.0f, 1.0f, 1.0f));

        RenderContext.GL.DrawArrays(PrimitiveType.Triangles, 0, 36);

        RenderContext.GL.BindVertexArray(_vaoLamp);

        _lampShader.Use();

        Matrix4x4 lampMatrix = Matrix4x4.Identity;
        lampMatrix *= Matrix4x4.CreateScale(0.2f);
        lampMatrix *= Matrix4x4.CreateTranslation(_lightPos);

        _lampShader.SetMatrix4("model", lampMatrix);
        _lampShader.SetMatrix4("view", _camera.GetViewMatrix());
        _lampShader.SetMatrix4("projection", _camera.GetProjectionMatrix());

        RenderContext.GL.DrawArrays(PrimitiveType.Triangles, 0, 36);

        _lightingShader.Discard();
        _lampShader.Discard();
    }

    private void Game_UpdateFrame(object arg1, TimeSpan arg2)
    {
        if (GetAsyncKeyState(VirtualKeyCodes.W))
        {
            _camera.Position += _camera.Front * cameraSpeed * (float)arg2.TotalSeconds;
        }
        if (GetAsyncKeyState(VirtualKeyCodes.S))
        {
            _camera.Position -= _camera.Front * cameraSpeed * (float)arg2.TotalSeconds;
        }
        if (GetAsyncKeyState(VirtualKeyCodes.A))
        {
            _camera.Position -= _camera.Right * cameraSpeed * (float)arg2.TotalSeconds;
        }
        if (GetAsyncKeyState(VirtualKeyCodes.D))
        {
            _camera.Position += _camera.Right * cameraSpeed * (float)arg2.TotalSeconds;
        }
        if (GetAsyncKeyState(VirtualKeyCodes.E))
        {
            _camera.Position += _camera.Up * cameraSpeed * (float)arg2.TotalSeconds;
        }
        if (GetAsyncKeyState(VirtualKeyCodes.Q))
        {
            _camera.Position -= _camera.Up * cameraSpeed * (float)arg2.TotalSeconds;
        }

        if (GetAsyncKeyState(VirtualKeyCodes.VK_RBUTTON))
        {
            GetCursorPos(out POINT point);

            float x = Convert.ToSingle(point.X);
            float y = Convert.ToSingle(point.Y);

            if (_firstMove)
            {
                _lastPos = new Vector2(x, y);
                _firstMove = false;
            }
            else
            {
                var deltaX = x - _lastPos.X;
                var deltaY = y - _lastPos.Y;
                _lastPos = new Vector2(x, y);

                _camera.Yaw += deltaX * sensitivity;
                _camera.Pitch -= deltaY * sensitivity;
            }
        }
        else
        {
            _firstMove = true;
        }
    }
}
