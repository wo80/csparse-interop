using System;

namespace CSparse.Interop.Tests.AMD
{
    internal class TestRunner
    {
        public static void Run()
        {
            var amd = new TestAMD();
            RunSafe("AMD", () => amd.TestAMD1() && amd.TestAMD2());
            RunSafe("CAMD", () => amd.TestCAMD1() && amd.TestCAMD2());

            var colamd = new TestCOLAMD();
            RunSafe("COLAMD", colamd.TestCOLAMD1);
            RunSafe("CCOLAMD", colamd.TestCCOLAMD1);

            Console.WriteLine();
        }

        private static void RunSafe(string name, Func<bool> action)
        {
            try
            {
                Console.Write("Running {0} tests ... ", name);

                if (action())
                {
                    Display.Ok("OK");
                }
                else
                {
                    Display.Error("failed");
                }
            }
            catch (Exception e)
            {
                Display.Error(e.Message);
            }
        }
    }
}
