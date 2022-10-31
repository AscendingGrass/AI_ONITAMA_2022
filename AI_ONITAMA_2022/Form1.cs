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
            //currentState = new GameState();
            
            Form2 formNow = new Form2(this);
            formNow.Show();
            

        }

        public void ResetGame()
        {

        }

        private void OnitamaForm_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
            this.ShowInTaskbar = false;

        }
    }
}