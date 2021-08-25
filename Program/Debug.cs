using System;

public static class Debug
{
    public static void Log(object message)
    {
        Console.ForegroundColor = ConsoleColor.Gray;
        Console.WriteLine(message);
    }
    public static void LogError(object message)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(message);
    }
}
