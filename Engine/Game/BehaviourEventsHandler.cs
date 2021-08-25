using System;
using System.Collections.Generic;

namespace Engine.Game
{
	public abstract class BehaviourEventsHandler
	{
		SafeDictionary<string, List<Action>> actions;

		public BehaviourEventsHandler()
		{
			actions = new SafeDictionary<string, List<Action>>();
			List<System.Reflection.MethodInfo> methods = new List<System.Reflection.MethodInfo>();

			var type = GetType();

			while (type != typeof(object))
			{
				methods.AddRange(type.GetMethods(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic));
				type = type.BaseType;
			}

			var zeroParam = new object[0];
			for (int i = 0; i < methods.Count; i++)
			{
				var method = methods[i];
				if (Attribute.IsDefined(method, typeof(BehaviourEventAttribute)) && method.GetParameters().Length == 0)
				{
					Action action = () => method.Invoke(this, zeroParam);
					if (actions.ContainsKey(method.Name))
					{
						actions[method.Name].Add(action);
					}
					else
					{
						actions.Add(method.Name, new List<Action> { action });
					}
				}
			}
		}

		public bool ContainsAction(string name)
		{
			return actions.ContainsKey(name);
		}

		public void CallEvent(string name)
		{
			OnEventCall(name);

			if (actions.ContainsKey(name))
			{
				var actionsList = actions[name];
				for (int i = actionsList.Count - 1; i >= 0; i--) actionsList[i].Invoke();
			}
		}

		protected virtual void OnEventCall(string name)
		{

		}
	}
}
