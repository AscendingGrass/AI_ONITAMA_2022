using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AI_ONITAMA_2022
{
    public partial class MainForm : Form
    {

        public GameState currentState;

        private int myVar;

        public int MyProperty
        {
            get { return myVar; }
            set { myVar = value; }
        }


        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr one, int two, int three, int four);


        public MainForm()
        {
            InitializeComponent();
            currentState = new GameState();

            Form2 formNow = new Form2();
            formNow.ShowDialog();
            formNow.Dispose();


        }

        public void ResetGame()
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Button_Enter(object sender, EventArgs e)
        {
            ((Control)sender).BackColor = Color.DarkMagenta;
        }

        private void Button_Leave(object sender, EventArgs e)
        {
            ((Control)sender).BackColor = Color.Transparent;
        }


        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(Handle, 0x112, 0xf012, 0);
        }

        private void panel5_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {
            this.WindowState = this.WindowState == FormWindowState.Normal ? FormWindowState.Maximized : FormWindowState.Normal;
        }
    }
}
