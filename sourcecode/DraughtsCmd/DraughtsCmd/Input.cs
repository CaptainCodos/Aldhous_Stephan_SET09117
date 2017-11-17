using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DraughtsCmd
{
    public static class Input
    {
        public static string GetLine()
        {
            return Console.ReadLine();
        }

        public static ConsoleKey GetKey()
        {
            return Console.ReadKey(true).Key;
        }
    }
}
