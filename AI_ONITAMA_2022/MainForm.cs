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

        private bool navCollapsed = false;
        private int animationSpeed = 1;
        private Timer t = null;



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

        private void label4_Click(object sender, EventArgs e)
        {
            //450,550
            double ratioX = panel10.Parent.Width;
            double ratioY = panel10.Parent.Height;
            if (this.WindowState == FormWindowState.Normal)
            {
                this.WindowState = FormWindowState.Maximized;
            }
            else
            {
                this.WindowState = FormWindowState.Normal;
            }
            //ratioX = panel10.Parent.Width / ratioY;
            ratioY = panel10.Parent.Height / ratioY;
            panel10.Width = (int)(panel10.Width * ratioY);
            panel10.Height = (int)(panel10.Height * ratioY);
            panel10.Top = (panel10.Parent.Height - panel10.Height) / 2;
            panel10.Left = (panel10.Parent.Width - panel10.Width) / 2 - (panel6.Width+20)/2 + 30;

            panel6.Width = (int)(panel6.Width * ratioY);
            panel6.Height = (int)(panel6.Height * ratioY);
            panel6.Top = (panel6.Parent.Height - panel6.Height) / 2;
            panel6.Left = panel10.Left + panel10.Width + 10;

            panel11.Height = panel6.Height;
            panel12.Height = panel6.Height;
        }

        private void ToggleCollapse(object sender, EventArgs e)
        {
            if (t != null) t.Stop();
            navCollapsed = !navCollapsed;
            t = new Timer {
                Interval = 10
            };
            // open : 280
            // collapsed : 68
            int accel = 0;
            if (navCollapsed)
            {
                t.Tick += (snd, evt) => TranslateNavWidth(62);
            }
            else
            {
                t.Tick += (snd, evt) => TranslateNavWidth(280);
            }
            t.Start();

            void TranslateNavWidth(int target)
            {
                accel += animationSpeed;
                if(panel4.Width > target)
                {
                    int temp = panel4.Width - accel;
                    if(temp <= target)
                    {
                        panel4.Width = target;
                        t.Stop();
                        t = null;
                    }
                    else
                    {
                        panel4.Width = temp;
                    }
                }
                else
                {
                    int temp = panel4.Width + accel;
                    if (temp >= target)
                    {
                        panel4.Width = target;
                        t.Stop();
                        t = null;
                    }
                    else
                    {
                        panel4.Width = temp;
                    }
                }
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            label4_Click(this, null);
        }
    }
}
