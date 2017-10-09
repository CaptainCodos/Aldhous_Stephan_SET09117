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
            Console.WriteLine("\nAre you sure? Y/N");

            val = 0;

            switch (Input.GetLine())
            {
                case "Y":
                    val = 1;
                    break;
                case "N":
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
            Console.WriteLine("Start New Game: NG");
            Console.WriteLine("Load Game: LG");
            Console.WriteLine("Exit: E");
            Console.WriteLine("\nPlease choose action: ");

            int confirmVal = 0;
            
            switch(Input.GetLine())
            {
                case "NG":
                    Confirm(out confirmVal);

                    if (confirmVal == 1)
                    {
                        AppState state = this;
                        state = new GamePlay();
                        state.UpdateState();
                    }
                    else
                    {
                        UpdateState();
                    }
                    break;
                case "LG":
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
                case "E":
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
        public GamePlay()
        {

        }

        public override void UpdateState()
        {
            Console.Clear();
            Console.WriteLine("Started Crazy Draughts");
            Console.WriteLine("Start New Game: NG");
            Console.WriteLine("Exit: E");
            Console.WriteLine("\nPlease choose action: ");

            GameBoard b = new GameBoard();

            b.DrawBoard();

            int confirmVal = 0;
            
            switch (Input.GetLine())
            {
                case "NG":
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
                case "E":
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
}
