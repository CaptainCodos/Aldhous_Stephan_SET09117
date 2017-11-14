using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DraughtsCmd
{
    class Program
    {
        // games played this session
        public static List<Game> games;
        // last game played
        public static Game lastGame;

        // RNG for use in AI
        public static readonly Random RNG = new Random();

        static void Main(string[] args)
        {
            games = new List<Game>();

            // Program uses a state machine
            AppState state = new MainMenu();
            state.UpdateState();
        }
    }
}
