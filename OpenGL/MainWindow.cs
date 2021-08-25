using System;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Collections.Generic;
using OpenTK.Graphics;
using OpenTK.Input;
using Engine;
using Engine.Rendering;

namespace WinGL
{

	class MainWindow : GameWindow, IMainPanel
    {
        private Matrix4 ortho;
        public MainWindow(int width, int height, string title)
        {
            Width = width;
            Height = height;
            WindowState = WindowState.Maximized;
            Title = title;
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
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(0, 0, Width, Height);
            ortho = Matrix4.CreateOrthographicOffCenter(0, Width, Height, 0, -100, 100);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref ortho);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
        }

        void DrawFrame()
		{
            GL.Clear(ClearBufferMask.DepthBufferBit | ClearBufferMask.ColorBufferBit);
            GL.ClearColor(Color4.Gray);
            GL.LoadIdentity();

            // draw

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
	}
}