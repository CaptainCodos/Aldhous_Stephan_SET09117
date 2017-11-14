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

        public List<Coord> P2UnitCoords { get; set; }
        public List<int> P2UnitTypes { get; set; }

        public HistoryItem()
        {
            P1UnitCoords = new List<Coord>();
            P2UnitCoords = new List<Coord>();

            P1UnitTypes = new List<int>();
            P2UnitTypes = new List<int>();
        }
    }
}
