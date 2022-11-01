using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AI_ONITAMA_2022
{
    public enum WinCondition : byte
    {
        KingDeath,
        TempleTakenOver,
        None
    }
    public class GameOverEventArgs : EventArgs
    {
        public string Winner { get; set; }
        public WinCondition Condition { get; set; }
    }
}
