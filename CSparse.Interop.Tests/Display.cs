
namespace CSparse.Interop.Tests
{
    using System;
    using System.Globalization;

    static class Display
    {
        public static void Time(TimeSpan time)
        {
            Info(string.Format(CultureInfo.InvariantCulture, "[{0:0.000s}] ", time.TotalSeconds), ConsoleColor.DarkGray, false);
        }
        
        public static void Ok(string message)
        {
            Info(message, ConsoleColor.DarkGreen);
        }

        public static void Warning(string message)
        {
            Info(message, ConsoleColor.DarkYellow);
        }

        public static void Error(string message, bool cleanup = true)
        {
            if (cleanup)
            {
                message = Cleanup(message);
            }

            Info(message, ConsoleColor.DarkRed);
        }

        public static void Info(string message, ConsoleColor color, bool newline = true)
        {
            var colorSave = Console.ForegroundColor;

            Console.ForegroundColor = color;

            if (newline)
            {
                Console.WriteLine(message);
            }
            else
            {
                Console.Write(message);
            }

            Console.ForegroundColor = colorSave;
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
