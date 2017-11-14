using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DraughtsCmd
{
    class HistoryItem
    {
        public List<Coord> P1UnitCoords { get; set; } // list of coordinates where units reside
        public List<int> P1UnitTypes { get; set; } // lines up with unit coordinates to determine unit type (man or king)
        public int P1Kills { get; set; } // player's kill count

        public List<Coord> P2UnitCoords { get; set; } // p2 coords
        public List<int> P2UnitTypes { get; set; } // p2 types
        public int P2Kills { get; set; } // p2 kills

        public int Turn { get; set; } // current turn

        public HistoryItem()
        {
            P1UnitCoords = new List<Coord>();
            P2UnitCoords = new List<Coord>();

            P1UnitTypes = new List<int>();
            P2UnitTypes = new List<int>();
        }

        public void AddUnit(int player, Coord location, int unitType, int kills, int turn)
        {
            Turn = turn;
            switch(player)
            {
                case 1:
                    P2UnitCoords.Add(location);
                    P2UnitTypes.Add(unitType);
                    P2Kills = kills;
                    break;
                default:
                    P1UnitCoords.Add(location);
                    P1UnitTypes.Add(unitType);
                    P1Kills = kills;
                    break;
            }
        }
    }
}
