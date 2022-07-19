using System.Numerics;
using System;
using System.IO;
using Silk.NET.OpenGL;

namespace Phader.GlObjects
{
    public class Shader : IDisposable
    {
        public static string defaultVert = @"
#version 330 core
layout (location = 0) in vec3 vPos;
layout (location = 1) in vec2 vUv;

uniform mat4 uModel;
uniform mat4 uView;
uniform mat4 uProjection;

out vec2 fUv;

void main()
{
    gl_Position = uProjection * uView * uModel * vec4(vPos, 1.0);
    fUv = vUv;
}";

        public static string defaultFrag = @"
#version 330 core
in vec2 fUv;

uniform sampler2D uTexture0;
uniform vec3 uTint;
uniform float uAlpha;

out vec4 FragColor;

void main()
{
    vec4 pixelColor = texture(uTexture0, fUv) * vec4(uTint, uAlpha);
    FragColor = pixelColor;
}";

        //Our handle and the GL instance this class will use, these are private because they have no reason to be public.
        //Most of the time you would want to abstract items to make things like this invisible.
        private uint _handle;
        private GL _gl;

        public Shader(GL gl, string vertexPath, string fragmentPath, bool isRaw = false)
        {
            _gl = gl;

            //Load the individual shaders.
            uint vertex = LoadShader(ShaderType.VertexShader, vertexPath, isRaw);
            uint fragment = LoadShader(ShaderType.FragmentShader, fragmentPath, isRaw);
            //Create the shader program.
            _handle = _gl.CreateProgram();
            //Attach the individual shaders.
            _gl.AttachShader(_handle, vertex);
            _gl.AttachShader(_handle, fragment);
            _gl.LinkProgram(_handle);
            //Check for linking errors.
            _gl.GetProgram(_handle, GLEnum.LinkStatus, out var status);
            if (status == 0)
            {
                throw new Exception($"Program failed to link with error: {_gl.GetProgramInfoLog(_handle)}");
            }
            //Detach and delete the shaders
            _gl.DetachShader(_handle, vertex);
            _gl.DetachShader(_handle, fragment);
            _gl.DeleteShader(vertex);
            _gl.DeleteShader(fragment);
        }

        public Shader(GL gl)
        {
            _gl = gl;

            //Load the individual shaders.
            uint vertex = LoadShader(ShaderType.VertexShader, defaultVert, true);
            uint fragment = LoadShader(ShaderType.FragmentShader, defaultFrag, true);
            //Create the shader program.
            _handle = _gl.CreateProgram();
            //Attach the individual shaders.
            _gl.AttachShader(_handle, vertex);
            _gl.AttachShader(_handle, fragment);
            _gl.LinkProgram(_handle);
            //Check for linking errors.
            _gl.GetProgram(_handle, GLEnum.LinkStatus, out var status);
            if (status == 0)
            {
                throw new Exception($"Program failed to link with error: {_gl.GetProgramInfoLog(_handle)}");
            }
            //Detach and delete the shaders
            _gl.DetachShader(_handle, vertex);
            _gl.DetachShader(_handle, fragment);
            _gl.DeleteShader(vertex);
            _gl.DeleteShader(fragment);
        }

        public void Use()
        {
            //Using the program
            _gl.UseProgram(_handle);
        }

        //Uniforms are properties that applies to the entire geometry
        public void SetUniform(string name, int value)
        {
            //Setting a uniform on a shader using a name.
            int location = _gl.GetUniformLocation(_handle, name);
            if (location == -1) //If GetUniformLocation returns -1 the uniform is not found.
            {
                throw new Exception($"{name} uniform not found on shader.");
            }
            _gl.Uniform1(location, value);
        }

        public void SetUniform(string name, float value)
        {
            int location = _gl.GetUniformLocation(_handle, name);
            if (location == -1)
            {
                throw new Exception($"{name} uniform not found on shader.");
            }
            _gl.Uniform1(location, value);
        }

        public void SetUniform(string name, Vector3 value)
        {
            int location = _gl.GetUniformLocation(_handle, name);
            if (location == -1)
            {
                throw new Exception($"{name} uniform not found on shader");
            }
            _gl.Uniform3(location, value);
        }

        public unsafe void SetUniform(string name, Matrix4x4 value)
        {
            int location = _gl.GetUniformLocation(_handle, name);
            if (location == -1)
            {
                throw new Exception($"{name} uniform not found on shader");
            }
            _gl.UniformMatrix4(location, 1, false, (float*)&value);
        }

        public void Dispose()
        {
            //Remember to delete the program when we are done.
            _gl.DeleteProgram(_handle);
        }

        private uint LoadShader(ShaderType type, string source, bool isRaw = false)
        {
            //To load a single shader we need to:
            //1) Load the shader from a file.
            //2) Create the handle.
            //3) Upload the source to opengl.
            //4) Compile the shader.
            //5) Check for errors.
            string src = isRaw ? source : File.ReadAllText(source);
            uint handle = _gl.CreateShader(type);
            _gl.ShaderSource(handle, src);
            _gl.CompileShader(handle);
            string infoLog = _gl.GetShaderInfoLog(handle);
            if (!string.IsNullOrWhiteSpace(infoLog))
            {
                throw new Exception($"Error compiling shader of type {type}, failed with error {infoLog}");
            }

            return handle;
        }
    }
}