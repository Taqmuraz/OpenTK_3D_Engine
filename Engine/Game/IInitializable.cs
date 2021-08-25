namespace Engine.Game
{
	public interface IInitializable<TArg>
	{
		void Initialize(TArg arg);
	}
	public interface IInitializable<TArg0, TArg1>
	{
		void Initialize(TArg0 arg0, TArg1 arg1);
	}
}
