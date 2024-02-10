
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

        public static void Error(string message, bool cleanup = true)
        {
            var color = Console.ForegroundColor;

            if (cleanup)
            {
                message = Cleanup(message);
            }

            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine(message);
            Console.ForegroundColor = color;
        }

        private static string Cleanup(string message)
        {
            // Try to cleanup lengthy error messages.
            if (message.Length > 200)
            {
                int i = message.IndexOf('.');
                int j = message.IndexOf('.', i + 1);
                if (j > 0 && j < i + 50) i = j;
                message = (i > 0 && i < 200) ? message.Substring(0, i + 1) : message.Substring(0, 200) + " ...";
            }

            return message;
        }
    }
}
