using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DraughtsCmd
{
    class HistoryItem
    {
        public List<Coord> P1UnitCoords { get; set; }
        public List<int> P1UnitTypes { get; set; }
        public int P1Kills { get; set; }

        public List<Coord> P2UnitCoords { get; set; }
        public List<int> P2UnitTypes { get; set; }
        public int P2Kills { get; set; }

        public int Turn { get; set; }

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
