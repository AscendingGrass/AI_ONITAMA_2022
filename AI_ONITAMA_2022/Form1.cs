using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AI_ONITAMA_2022
{
    public partial class OnitamaForm : Form
    {
        public GameState currentState;

        public OnitamaForm()
        {
            InitializeComponent();
            currentState = new GameState();
        }

        public void ResetGame()
        {

        }

        
        
    }
}
