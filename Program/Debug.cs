using System;

public static class Debug
{
    static System.IO.StreamWriter log;

    public static void DebugStart()
	{
        log = new System.IO.StreamWriter("./log.txt");
	}
    public static void DebugEnd()
	{
        log.Close();
	}

    public static void Log(object message)
    {
        log.WriteLine(message);
    }
    public static void LogError(object message)
    {
        log.WriteLine(message);
    }
}
