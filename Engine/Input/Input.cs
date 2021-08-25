using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;

namespace Engine
{
	public static class InputExtensions
	{
		public static KeyCode ToKeyCode(this Keys keys)
		{

			return (KeyCode)(int)keys;
		}
	}

	public sealed class Input : IInputHandler
	{
		static SafeDictionary<KeyCode, KeyInfo> keys = new SafeDictionary<KeyCode, KeyInfo>(defaultInstanceFunc: () => emptyKey);
		static List<(KeyCode key, KeyState state)> keysToUpdate = new List<(KeyCode, KeyState)>();

		static readonly KeyInfo emptyKey = new EmptyKeyInfo();

		public static Vector2 mousePosition { get; private set; }

		class EmptyKeyInfo : KeyInfo
		{
			public override KeyState keyState { get => KeyState.None; set { return; } }
		}

		public class KeyInfo
		{
			public virtual KeyState keyState { get; set; }
		}

		static Input instance;

		private Input()
		{

		}

		public static void UpdateInput()
		{
			lock (keys)
			{
				foreach (var key in keys)
				{
					switch (key.Value.keyState)
					{
						case KeyState.Down: key.Value.keyState = KeyState.Hold; break;
						case KeyState.Up: key.Value.keyState = KeyState.None; break;
					}
				}
				lock (keysToUpdate)
				{
					foreach (var key in keysToUpdate)
					{
						var info = keys[key.key];
						if (info == emptyKey) keys[key.key] = info = new KeyInfo();
						info.keyState = key.state;
					}
					keysToUpdate.Clear();
				}
			}
		}

		public static Vector2 GetWASD()
		{
			Vector2 wasd = Vector2.zero;
			if (GetKey(KeyCode.W)) wasd.y += 1f;
			if (GetKey(KeyCode.A)) wasd.x -= 1f;
			if (GetKey(KeyCode.S)) wasd.y -= 1f;
			if (GetKey(KeyCode.D)) wasd.x += 1f;
			return wasd;
		}

		public static KeyInfo GetKeyInfo(KeyCode key)
		{
			return keys[key];
		}


		public static bool GetKeyDown(KeyCode key)
		{
			return keys[key].keyState == KeyState.Down;
		}
		public static bool GetKey(KeyCode key)
		{
			return keys[key].keyState != KeyState.None;
		}
		public static bool GetKeyUp(KeyCode key)
		{
			return keys[key].keyState == KeyState.Up;
		}

		public static void StartInput()
		{
			Project.mainPanel.AddInputHandler(instance = new Input());
		}

		public void OnKeyDown(Keys key)
		{
			lock (keys)
			{
				if (keys[key.ToKeyCode()].keyState != KeyState.Hold) lock (keysToUpdate) keysToUpdate.Add((key.ToKeyCode(), KeyState.Down));
			}
		}

		public void OnKeyUp(Keys key)
		{
			lock (keysToUpdate) keysToUpdate.Add((key.ToKeyCode(), KeyState.Up));
		}

		public void OnMouseDown(PointF point, int button)
		{
			button++;
			lock (keysToUpdate) keysToUpdate.Add(((KeyCode)button, KeyState.Down));
			mousePosition = point;
		}

		public void OnMouseMove(PointF point)
		{
			mousePosition = point;
		}

		public void OnMouseUp(PointF point, int button)
		{
			button++;
			lock (keysToUpdate) keysToUpdate.Add(((KeyCode)button, KeyState.Up));
			mousePosition = point;
		}
	}
}
