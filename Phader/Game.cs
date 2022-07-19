using System.Collections.Specialized;
using System.Reflection.Metadata.Ecma335;
using System.Reflection.PortableExecutable;
using System.Data;
using Microsoft.VisualBasic.CompilerServices;
using System.Runtime.Versioning;
using System.Security;
using System.Numerics;
using Silk.NET.Input;
using Silk.NET.Maths;
using Silk.NET.Windowing;
using Silk.NET.OpenGL;
using Phader.GlObjects;

namespace Phader;
public struct GameOptions
{
    public Vector2D<int> Size { get; init; }
    public string Title { get; init; }
    public Scene.Scene Scene { get; init; }

    public GameOptions(int width, int height, string title, Scene.Scene scene)
    {
        Size = new Vector2D<int>(width, height);
        Title = title;
        Scene = scene;
    }
}

public class Game
{
    private IWindow window;
    private GL Gl;

    public Phader.Rendering.SpriteRenderer SpriteRenderer { get; private set; }

    public int width { get; init; }
    public int height { get; init; }

    public Scene.Scene Scene;

    private readonly float[] ColorVertices =
    {
        //X    Y      Z     R  G  B  A
         0.5f,  0.5f, 0.0f, 1, 0, 0, 1,
         0.5f, -0.5f, 0.0f, 0, 0, 0, 1,
        -0.5f, -0.5f, 0.0f, 0, 0, 1, 1,
        -0.5f,  0.5f, 0.5f, 0, 0, 0, 1
    };

    public Game(GameOptions gameOptions)
    {
        var options = WindowOptions.Default;
        options.Size = gameOptions.Size;
        options.Title = gameOptions.Title;
        options.WindowBorder = WindowBorder.Fixed;
        // options.FramesPerSecond = 1; // DOES NOT WORK OnRender called as fast as possible
        // options.UpdatesPerSecond = 10; // DOES WORK OnUpdate called 10 times a second
        window = Window.Create(options);

        width = gameOptions.Size.X;
        height = gameOptions.Size.Y;

        window.Load += OnLoad;
        window.Render += OnRender;
        window.Update += OnUpdate;
        window.Closing += OnClose;
        // window.Resize += OnResize;

        Scene = gameOptions.Scene;

        window.Run();
    }

    // private void OnResize(Vector2D<int> newSize)
    // {
    //     width = newSize.X;
    //     height = newSize.Y;

    //     Shader.Use();
    //     Matrix4x4 projection = Matrix4x4.CreateOrthographic(width, height, 1f, -1f);
    //     Shader.SetUniform("uProjection", projection);

    //     Vector3 CameraPosition = new Vector3(width / 2f, -height / 2f, 3.0f);
    //     Vector3 CameraTarget = new Vector3(width / 2f, -height / 2f, 0.0f);
    //     Vector3 CameraDirection = Vector3.Normalize(CameraPosition - CameraTarget);
    //     Vector3 CameraRight = Vector3.Normalize(Vector3.Cross(Vector3.UnitY, CameraDirection));
    //     Vector3 CameraUp = Vector3.Cross(CameraDirection, CameraRight);
    //     var view = Matrix4x4.CreateLookAt(CameraPosition, CameraTarget, CameraUp);
    //     Shader.SetUniform("uView", view);
    // }

    private void OnLoad()
    {
        Console.WriteLine("START LOAD");
        DateTime time = DateTime.Now;

        Gl = GL.GetApi(window);
        Gl.Enable(GLEnum.Blend);
        Gl.BlendFunc(GLEnum.SrcAlpha, GLEnum.OneMinusSrcAlpha);
        Gl.ClearColor(0, 0, 0, 0);

        Scene.Init(Gl, this);

        IInputContext input = window.CreateInput();
        for (int i = 0; i < input.Keyboards.Count; i++)
        {
            input.Keyboards[i].KeyDown += KeyDown;
        }

        SpriteRenderer = new Rendering.SpriteRenderer(Gl);
        SpriteRenderer.SetOrthoProjection(width, height);
        SpriteRenderer.SetView(width, height);

        Scene.Preload();
        Scene.Create();

        Console.WriteLine($"LOAD TOOK: {DateTime.Now - time}");
    }

    private unsafe void OnRender(double dt)
    {
        // Console.WriteLine($"OnRender {obj}");
        Gl.Clear((uint)ClearBufferMask.ColorBufferBit);

        Scene.Render(dt);
    }

    private void OnUpdate(double dt)
    {
        // Console.WriteLine($"OnUpdate {obj}");
        Scene.Update(dt);
    }

    private void OnClose()
    {
        //Remember to dispose all the instances.
        Scene.End();
        SpriteRenderer.Dispose();
    }

    private void KeyDown(IKeyboard arg1, Key arg2, int arg3)
    {
        if (arg2 == Key.Escape)
        {
            window.Close();
        }
    }
}
