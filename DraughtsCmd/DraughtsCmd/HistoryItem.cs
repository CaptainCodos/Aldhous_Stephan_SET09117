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
        public List<Coord> P2UnitCoords { get; set; }

        public HistoryItem(List<Coord> p1UnitCs, List<Coord> p2UnitCs)
        {
            P1UnitCoords = p1UnitCs;
            P2UnitCoords = p2UnitCs;
        }
    }
}
