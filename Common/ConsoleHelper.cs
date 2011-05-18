using System;

namespace Common
{
    public static class ConsoleHelper
    {
        public static void WriteLine(ConsoleColor color, string message, params object[] args)
        {
            lock (typeof(ConsoleHelper))
            {
                ConsoleColor currentColor = Console.ForegroundColor;
                Console.ForegroundColor = color;
                Console.WriteLine(DateTime.Now.ToString("HH:mm:ss > ") + message, args);
                Console.ForegroundColor = currentColor;
            }
        }
    }
}
