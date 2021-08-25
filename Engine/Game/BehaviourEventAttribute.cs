using System;

namespace Engine.Game
{
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
	public sealed class BehaviourEventAttribute : Attribute
	{
	}
}