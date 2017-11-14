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
        public static Game lastGame;
        public static readonly Random RNG = new Random();

        static void Main(string[] args)
        {
            games = new List<Game>();

            AppState state = new MainMenu();

            state.UpdateState();
        }
    }
}
