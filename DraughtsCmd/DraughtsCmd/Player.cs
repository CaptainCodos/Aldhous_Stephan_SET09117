using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DraughtsCmd
{
    class Player
    {
        public List<Unit> ArmyUnits { get; set; }

        public int Kills { get; set; }
        public int Units { get { return ArmyUnits.Count; } }

        public string BoardSign { get; set; }

        public bool IsAIPlayer { get; set; }

        public Player(string sign, bool isAI)
        {
            ArmyUnits = new List<Unit>();
            BoardSign = sign;
            
            IsAIPlayer = isAI;
        }
    }
}
