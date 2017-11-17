using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DraughtsCmd
{
    class Player
    {
        // Player's army
        public List<Unit> ArmyUnits { get; set; }

        public int Kills { get; set; } // Player kill count
        public int Units { get { return ArmyUnits.Count; } } // Quick army size access

        public string BoardSign { get; set; } // Board symbol

        public bool IsAIPlayer { get; set; } // AI flag

        public Player(string sign, bool isAI)
        {
            ArmyUnits = new List<Unit>();
            BoardSign = sign;
            
            IsAIPlayer = isAI;
        }
    }
}
