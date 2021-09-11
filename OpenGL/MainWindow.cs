using System;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Collections.Generic;
using OpenTK.Graphics;
using OpenTK.Input;
using Engine;
using Engine.Rendering;
using System.Linq;

namespace WinGL
{

	class MainWindow : GameWindow, IMainPanel
    {
        public static float WindowWidth { get; private set; }
        public static float WindowHeight { get; private set; }

        public MainWindow(int width, int height, string title)
        {
            WindowWidth = Width = width;
            WindowHeight = Height = height;
            WindowState = WindowState.Maximized;
            Title = title;
			/*
            GL.Enable(EnableCap.PointSmooth);
            GL.Enable(EnableCap.LineSmooth);
            GL.Enable(EnableCap.PolygonSmooth);
            GL.Enable(EnableCap.Multisample);
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.Texture2D);
            GL.Enable(EnableCap.AlphaTest);
            GL.Enable(EnableCap.CullFace);
            GL.Enable(EnableCap.Blend);
            GL.DepthFunc(DepthFunction.Lequal);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
            GL.FrontFace(FrontFaceDirection.Cw);
            GL.CullFace(CullFaceMode.Front);
            */

			try
			{
                MasterRenderer.CreateMasterRenderer();
            }
			catch (Exception ex)
			{
                Debug.LogError(ex.ToString());
			}

            sample = Engine.Rendering.OBJFileLoader.LoadOBJ("./Data/Models/Soldier.obj");

            //vertexBufferId = GL.GenBuffer();
            //indexBufferId = GL.GenBuffer();
        }

        protected override void OnResize(EventArgs e)
        {
            WindowWidth = Width;
            WindowHeight = Height;
        }

        Mesh sample;
        int vertexBufferId, indexBufferId;

        void DrawFrame()
		{
            var camera = Engine.Game.Camera.mainCamera;
            Title = $"Field of view : {camera.fieldOfView}, Camera position : {camera.transform.position}, Camera rotation : {camera.transform.rotation}";

            Loader.UpdateLoader();

            GL.Viewport(0, 0, Width, Height);

            MasterRenderer.masterRenderer.Render(null, Engine.Game.Camera.mainCamera);

            /*GL.UseProgram(Shader.defaultShader.GetProgramID());

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, indexBufferId);
            GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(indexCount * sizeof(ushort)), (IntPtr)pIndex, BufferUsageHint.StaticDraw);

            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferId);

            GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, Vertex2D.SizeInBytes, 0);
            GL.VertexAttribPointer(1, 4, VertexAttribPointerType.UnsignedByte, true, Vertex2D.SizeInBytes, 8);
            GL.VertexAttribPointer(2, 2, VertexAttribPointerType.Float, false, Vertex2D.SizeInBytes, 8 + 4);

            GL.EnableVertexAttribArray(0);
            GL.EnableVertexAttribArray(1);
            GL.EnableVertexAttribArray(2);

            fixed (float* ptr = &projection.M11)
            {
                GL.UniformMatrix4(transformLocation, 1, false, ptr);
            }
            */


            /*GL.Begin(PrimitiveType.Triangles);
            
            float s = 0.1f;

            var m = camera.transform.localToWorld * camera.projectionMatrix;

            foreach (var i in sample.indices)
            {
                int indexV = i * 3;
                int indexT = i * 2;

                Engine.Vector3 vert = new Engine.Vector3(sample.vertices[indexV] * s, sample.vertices[indexV + 1] * s, sample.vertices[indexV + 2] * s);

                vert = m.MultiplyPoint_With_WDevision(vert);
                
                GL.Color4(sample.textureCoords[indexT], sample.textureCoords[indexT + 1], 0f, 1f);
                GL.Vertex3(vert.x, vert.y, vert.z);
            }

            GL.End();
            */

            SwapBuffers();
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            DrawFrame();
        }

        List<IInputHandler> inputHandlers = new List<IInputHandler>();

		void IMainPanel.AddInputHandler(IInputHandler handler)
		{
            inputHandlers.Add(handler);
		}

        Engine.KeyCode ToKeyCode(Key key)
		{
            if (key >= Key.A && key < Key.Z) return (KeyCode)((int)key - (int)Key.A + (int)KeyCode.A);
            else if (key >= Key.Keypad0 && key <= Key.Keypad9) return (KeyCode)((int)key - (int)Key.Keypad0 + (int)KeyCode.N0);
            else
            {
                switch (key)
                {
                    case Key.LShift:return KeyCode.ShiftKey;
                    case Key.RShift:return KeyCode.ShiftKey;
                    case Key.LControl: return KeyCode.ControlKey;
                    case Key.RControl: return KeyCode.ControlKey;

                    case Key.Left: return KeyCode.Left;
                    case Key.Right: return KeyCode.Right;
                    case Key.Down: return KeyCode.Down;
                    case Key.Up: return KeyCode.Up;
                }
            }
            return KeyCode.None;
        }

		protected override void OnKeyDown(KeyboardKeyEventArgs e)
		{
			base.OnKeyDown(e);
            foreach (var handler in inputHandlers) handler.OnKeyDown((Keys)(int)ToKeyCode(e.Key));
		}

		protected override void OnKeyUp(KeyboardKeyEventArgs e)
		{
            foreach (var handler in inputHandlers) handler.OnKeyUp((Keys)(int)ToKeyCode(e.Key));
        }

        int ToMouseButton(MouseButton btn)
		{
			switch (btn)
			{
				case MouseButton.Left: return 0;
				case MouseButton.Middle: return 2;
				case MouseButton.Right: return 1;
			}
            return 3;
		}

		protected override void OnMouseDown(MouseButtonEventArgs e)
		{
            foreach (var handler in inputHandlers) handler.OnMouseDown(e.Position, ToMouseButton(e.Button));
        }
		protected override void OnMouseUp(MouseButtonEventArgs e)
		{
            foreach (var handler in inputHandlers) handler.OnMouseUp(e.Position, ToMouseButton(e.Button));
        }
        protected override void OnMouseMove(MouseMoveEventArgs e)
        {
            foreach (var handler in inputHandlers) handler.OnMouseMove(e.Position);
        }

		void IMainPanel.RemoveInputHandler(IInputHandler handler)
		{
            inputHandlers.Remove(handler);
        }

        List<string> texturesToLoad = new List<string>();

		public Texture LoadTexture(string fileName)
		{
            return Loader.LoadTexture(fileName);
		}

		public Model LoadModel(string fileName)
		{
            return MasterRenderer.LoadModel(fileName);
		}

		public void RegisterRenderer(Engine.Game.Renderer renderer)
		{
            MasterRenderer.renderers.Add(renderer);
		}

		public void UnregisterRenderer(Engine.Game.Renderer renderer)
		{
            MasterRenderer.renderers.Remove(renderer);
		}

        public int screenWidth => Width;
        public int screenHeight => Height;
	}
}