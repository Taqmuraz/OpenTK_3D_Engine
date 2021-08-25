using System;

namespace Engine.Rendering
{
	public interface IMainPanel
	{
		void AddInputHandler(IInputHandler handler);
		void RemoveInputHandler(IInputHandler handler);
	}
}
