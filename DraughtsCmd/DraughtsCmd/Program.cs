using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DraughtsCmd
{
    class Program
    {
        public static List<Game> games;

        static void Main(string[] args)
        {
            games = new List<Game>();

            AppState state = new MainMenu();

            state.UpdateState();
        }
    }
}
