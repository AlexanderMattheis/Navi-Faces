using System;

namespace Navi
{
#if WINDOWS || LINUX

    public static class Program
    {
        [STAThread]
        public static void Main()
        {
            using (var game = new Plant())
                game.Run();
        }
    }
#endif
}
