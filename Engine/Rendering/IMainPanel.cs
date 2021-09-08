using System;
using Engine.Game;

namespace Engine.Rendering
{
	public interface IMainPanel
	{
		void AddInputHandler(IInputHandler handler);
		void RemoveInputHandler(IInputHandler handler);
		Texture LoadTexture (string fileName);
		Model LoadModel(string fileName);
		void RegisterRenderer(Renderer renderer);
		void UnregisterRenderer(Renderer renderer);
	}
}
