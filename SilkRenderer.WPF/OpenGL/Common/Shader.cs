using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using Silk.NET.OpenGL;

namespace SilkRenderer.WPF.OpenGL.Common;

// A simple class meant to help create shaders.
public class Shader
{
    public readonly uint Handle;

    private readonly Dictionary<string, int> _uniformLocations;

    // This is how you create a simple shader.
    // Shaders are written in GLSL, which is a language very similar to C in its semantics.
    // The GLSL source is compiled *at runtime*, so it can optimize itself for the graphics card it's currently being used on.
    // A commented example of GLSL can be found in shader.vert.
    public Shader(string vertPath, string fragPath)
    {
        // There are several different types of shaders, but the only two you need for basic rendering are the vertex and fragment shaders.
        // The vertex shader is responsible for moving around vertices, and uploading that data to the fragment shader.
        //   The vertex shader won't be too important here, but they'll be more important later.
        // The fragment shader is responsible for then converting the vertices to "fragments", which represent all the data OpenGL needs to draw a pixel.
        //   The fragment shader is what we'll be using the most here.

        // Load vertex shader and compile
        string shaderSource = File.ReadAllText(vertPath);

        // GL.CreateShader will create an empty shader (obviously). The ShaderType enum denotes which type of shader will be created.
        uint vertexShader = RenderContext.GL.CreateShader(ShaderType.VertexShader);

        // Now, bind the GLSL source code
        RenderContext.GL.ShaderSource(vertexShader, shaderSource);

        // And then compile
        CompileShader(vertexShader);

        // We do the same for the fragment shader.
        shaderSource = File.ReadAllText(fragPath);
        var fragmentShader = RenderContext.GL.CreateShader(ShaderType.FragmentShader);
        RenderContext.GL.ShaderSource(fragmentShader, shaderSource);
        CompileShader(fragmentShader);

        // These two shaders must then be merged into a shader program, which can then be used by OpenGL.
        // To do this, create a program...
        Handle = RenderContext.GL.CreateProgram();

        // Attach both shaders...
        RenderContext.GL.AttachShader(Handle, vertexShader);
        RenderContext.GL.AttachShader(Handle, fragmentShader);

        // And then link them together.
        LinkProgram(Handle);

        // When the shader program is linked, it no longer needs the individual shaders attached to it; the compiled code is copied into the shader program.
        // Detach them, and then delete them.
        RenderContext.GL.DetachShader(Handle, vertexShader);
        RenderContext.GL.DetachShader(Handle, fragmentShader);
        RenderContext.GL.DeleteShader(fragmentShader);
        RenderContext.GL.DeleteShader(vertexShader);

        // The shader is now ready to go, but first, we're going to cache all the shader uniform locations.
        // Querying this from the shader is very slow, so we do it once on initialization and reuse those values
        // later.

        // First, we have to get the number of active uniforms in the shader.
        RenderContext.GL.GetProgram(Handle, GLEnum.ActiveUniforms, out var numberOfUniforms);

        // Next, allocate the dictionary to hold the locations.
        _uniformLocations = new Dictionary<string, int>();

        // Loop over all the uniforms,
        for (uint i = 0; i < numberOfUniforms; i++)
        {
            // get the name of this uniform,
            var key = RenderContext.GL.GetActiveUniform(Handle, i, out _, out _);

            // get the location,
            var location = RenderContext.GL.GetUniformLocation(Handle, key);

            // and then add it to the dictionary.
            _uniformLocations.Add(key, location);
        }
    }

    private static void CompileShader(uint shader)
    {
        // Try to compile the shader
        RenderContext.GL.CompileShader(shader);

        // Check for compilation errors
        RenderContext.GL.GetShader(shader, GLEnum.CompileStatus, out var code);
        if (code != (int)GLEnum.True)
        {
            // We can use `GL.GetShaderInfoLog(shader)` to get information about the error.
            var infoLog = RenderContext.GL.GetShaderInfoLog(shader);
            throw new Exception($"Error occurred whilst compiling Shader({shader}).\n\n{infoLog}");
        }
    }

    private static void LinkProgram(uint program)
    {
        // We link the program
        RenderContext.GL.LinkProgram(program);

        // Check for linking errors
        RenderContext.GL.GetProgram(program, GLEnum.LinkStatus, out var code);
        if (code != (int)GLEnum.True)
        {
            // We can use `GL.GetProgramInfoLog(program)` to get information about the error.
            throw new Exception($"Error occurred whilst linking Program({program})");
        }
    }

    // A wrapper function that enables the shader program.
    public void Use()
    {
        RenderContext.GL.UseProgram(Handle);
    }

    public void Discard()
    {
        if (RenderContext.GL.IsProgram(Handle))
        {
            RenderContext.GL.UseProgram(0);
        }
    }

    // The shader sources provided with this project use hardcoded layout(location)-s. If you want to do it dynamically,
    // you can omit the layout(location=X) lines in the vertex shader, and use this in VertexAttribPointer instead of the hardcoded values.
    public int GetAttribLocation(string attribName)
    {
        return RenderContext.GL.GetAttribLocation(Handle, attribName);
    }

    // Uniform setters
    // Uniforms are variables that can be set by user code, instead of reading them from the VBO.
    // You use VBOs for vertex-related data, and uniforms for almost everything else.

    // Setting a uniform is almost always the exact same, so I'll explain it here once, instead of in every method:
    //     1. Bind the program you want to set the uniform on
    //     2. Get a handle to the location of the uniform with GL.GetUniformLocation.
    //     3. Use the appropriate GL.Uniform* function to set the uniform.

    /// <summary>
    /// Set a uniform int on this shader.
    /// </summary>
    /// <param name="name">The name of the uniform</param>
    /// <param name="data">The data to set</param>
    public void SetInt(string name, int data)
    {
        RenderContext.GL.UseProgram(Handle);
        RenderContext.GL.Uniform1(_uniformLocations[name], data);
    }

    /// <summary>
    /// Set a uniform float on this shader.
    /// </summary>
    /// <param name="name">The name of the uniform</param>
    /// <param name="data">The data to set</param>
    public void SetFloat(string name, float data)
    {
        RenderContext.GL.UseProgram(Handle);
        RenderContext.GL.Uniform1(_uniformLocations[name], data);
    }

    /// <summary>
    /// Set a uniform Matrix4 on this shader
    /// </summary>
    /// <param name="name">The name of the uniform</param>
    /// <param name="data">The data to set</param>
    /// <remarks>
    ///   <para>
    ///   The matrix is transposed before being sent to the shader.
    ///   </para>
    /// </remarks>
    public unsafe void SetMatrix4(string name, Matrix4x4 data)
    {
        RenderContext.GL.UseProgram(Handle);
        RenderContext.GL.UniformMatrix4(_uniformLocations[name], 1, true, (float*)&data);
    }

    /// <summary>
    /// Set a uniform Vector3 on this shader.
    /// </summary>
    /// <param name="name">The name of the uniform</param>
    /// <param name="data">The data to set</param>
    public void SetVector3(string name, Vector3 data)
    {
        RenderContext.GL.UseProgram(Handle);
        RenderContext.GL.Uniform3(_uniformLocations[name], data);
    }
}
