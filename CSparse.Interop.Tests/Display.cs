
namespace CSparse.Interop.Tests
{
    using System;
    using System.Globalization;

    static class Display
    {
        public static void Time(TimeSpan time)
        {
            var color = Console.ForegroundColor;

            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write(string.Format(CultureInfo.InvariantCulture, "[{0:0.000s}] ", time.TotalSeconds));
            Console.ForegroundColor = color;
        }
        
        public static void Ok(string message)
        {
            var color = Console.ForegroundColor;

            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine(message);
            Console.ForegroundColor = color;
        }

        public static void Warning(string message)
        {
            var color = Console.ForegroundColor;

            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine(message);
            Console.ForegroundColor = color;
        }

        public static void Error(string message)
        {
            var color = Console.ForegroundColor;

            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine(message);
            Console.ForegroundColor = color;
        }
    }
}
