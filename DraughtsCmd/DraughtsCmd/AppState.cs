using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DraughtsCmd
{
    class AppState
    {
        public virtual void UpdateState()
        {
        }

        public virtual void Confirm(out int val)
        {
            Console.WriteLine("\nAre you sure? y/n");

            val = 0;

            switch (Input.GetLine())
            {
                case "y":
                    val = 1;
                    break;
                case "n":
                    val = 0;
                    break;
                default:
                    Confirm(out val);
                    break;
            }
        }
    }

    class MainMenu : AppState
    {
        public MainMenu()
        {

        }

        public override void UpdateState()
        {
            Console.Clear();
            Console.WriteLine("Crazy Draughts");
            Console.WriteLine("Start New Game: ng");
            Console.WriteLine("Load Game: lg");
            Console.WriteLine("Exit: e");
            Console.WriteLine("\nPlease choose action: ");

            int confirmVal = 0;
            
            switch(Input.GetLine())
            {
                case "ng":
                    Confirm(out confirmVal);

                    if (confirmVal == 1)
                    {
                        Game game = new Game();

                        AppState state = this;
                        state = new GamePlay(game);
                        state.UpdateState();
                    }
                    else
                    {
                        UpdateState();
                    }
                    break;
                case "lg":
                    Confirm(out confirmVal);

                    if (confirmVal == 1)
                    {
                        Console.WriteLine("\nPlease enter the game you want to load! [enter game number]");

                        string input = Input.GetLine();

                        int val;
                        int.TryParse(input, out val);
                        val -= 1;

                        if (val < Program.games.Count && Program.games.Count > 0)
                        {
                            AppState state = this;
                            state = new GamePlay(val);
                            state.UpdateState();
                        }
                        else
                        {
                            Console.WriteLine("\nThis game does not exist! Press enter to continue...");
                            Input.GetLine();
                            UpdateState();
                        }
                    }
                    else
                    {
                        UpdateState();
                    }
                    break;
                case "e":
                    Confirm(out confirmVal);

                    if (confirmVal == 1)
                    {
                        Environment.Exit(0);
                    }
                    else
                    {
                        UpdateState();
                    }
                    break;
                default:
                    UpdateState();
                    break;
            }
        }
    }

    class GamePlay : AppState
    {
        private Game m_game;

        public GamePlay(int game)
        {
            m_game = Program.games[game];
        }

        public GamePlay(Game game)
        {
            m_game = game;
            Program.games.Add(game);
        }

        public override void UpdateState()
        {
            ConstantDisplay();

            Console.WriteLine("\nPlay turn: p");
            Console.WriteLine("\nUndo: u");
            Console.WriteLine("\nRedo: r");
            Console.WriteLine("Exit Game: e");

            int confirmVal;

            switch (Input.GetLine())
            {
                case "p":
                    break;
                case "e":
                    Confirm(out confirmVal);

                    if (confirmVal == 1)
                    {
                        AppState state = this;
                        state = new MainMenu();
                        state.UpdateState();
                    }
                    else
                    {
                        UpdateState();
                    }
                    break;
                default:
                    UpdateState();
                    break;
            }
        }

        public void ConstantDisplay()
        {
            Console.Clear();
            Console.WriteLine("Game On!");
            Console.WriteLine("\nTurn " + (m_game.GetManager().Turn + 1));
            Console.WriteLine("Player: " + ((m_game.GetManager().Turn % 2) + 1));
            Console.WriteLine("");

            m_game.GetManager().Board.DrawBoard();

            Console.WriteLine("\nPlayer1 Units Remaining: " + m_game.GetManager().GetPlayer(0).Units);
            Console.WriteLine("Player1 Kills: " + m_game.GetManager().GetPlayer(0).Kills);

            Console.WriteLine("\nPlayer2 Units Remaining: " + m_game.GetManager().GetPlayer(1).Units);
            Console.WriteLine("Player2 Kills: " + m_game.GetManager().GetPlayer(1).Kills);
        }

        public void SelectPiece()
        {
            ConstantDisplay();

            Console.WriteLine("\nPlay turn: p");
            Console.WriteLine("\nUndo: u");
            Console.WriteLine("\nRedo: r");
            Console.WriteLine("Exit Game: e");

            int confirmVal;

            switch (Input.GetLine())
            {

            }
        }
    }
}
