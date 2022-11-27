using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AI_ONITAMA_2022
{
    class BoardPanel : Panel
    {
        public static Color HighlightColor = Color.FromArgb(192, 0, 192);
        public static Color HighlightColorHover = Color.Fuchsia;
        public static Color DefaultColor = SystemColors.ControlDarkDark;
        public static Color DefaultColorHover = SystemColors.ControlDark;

        public readonly int X, Y;

        private bool highlighted = false;
        public bool Highlighted 
        {
            get => highlighted;
            set { highlighted = value; BackColor = value ? HighlightColor : DefaultColor; }
        }

        public BoardPanel(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}
