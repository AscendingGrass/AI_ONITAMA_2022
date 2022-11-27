using AI_ONITAMA_2022.Properties;
using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace AI_ONITAMA_2022
{
    public partial class MainForm : Form
    {

        public GameState currentState;

        private Coordinate selectedCell = Coordinate.Minus;
        private int selectedCard;
        private BoardPanel[,] board = new BoardPanel[5, 5];
        private bool navCollapsed = false;
        private int animationSpeed = 1;
        private Timer t = null;

        private int SelectedCard
        {
            get => selectedCard;
            set
            {
                selectedCard = value;
                tableLayoutPanel1.Enabled = value != -1;
                panel14.BackColor = value == 0 ? BoardPanel.HighlightColor : SystemColors.ControlDarkDark;
                panel15.BackColor = value == 1 ? BoardPanel.HighlightColor : SystemColors.ControlDarkDark;
                if (selectedCell != Coordinate.Minus) HighlightMoves();
            }
        }



        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(IntPtr one, int two, int three, int four);


        public MainForm()
        {
            InitializeComponent();
            InitializeBoard();
            ResetGame();

            Form2 formNow = new Form2();
            formNow.ShowDialog();
            formNow.Dispose();
        }

        private void InitializeBoard()
        {
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    var temp = new BoardPanel(j, i)
                    {
                        Dock = DockStyle.Fill,
                        Margin = new Padding(1),
                        BackgroundImageLayout = ImageLayout.Center,
                        Tag = -1
                        //Highlighted = (i+j)%2==0
                    };
                    temp.MouseEnter += BoardCellMouseEnter;
                    temp.MouseLeave += BoardCellMouseLeave;
                    temp.Click += BoardCellClick;
                    board[i, j] = temp;
                    tableLayoutPanel1.Controls.Add(temp, j, i);
                }
            }
        }

        private void RefreshBoard()
        {
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    switch (currentState.state[i, j])
                    {
                        case 'b':
                            board[i, j].BackgroundImage = Resources.redpion;
                            break;
                        case 'B':
                            board[i, j].BackgroundImage = Resources.redking;
                            break;
                        case 'p':
                            board[i, j].BackgroundImage = Resources.pion;
                            break;
                        case 'P':
                            board[i, j].BackgroundImage = Resources.king;
                            break;
                        default:
                            board[i, j].BackgroundImage = null;
                            break;
                    }
                }
            }

            panel22.BackgroundImage = currentState.neutralCard.playerIcon;
            panel18.BackgroundImage = currentState.playerCard[0].playerIcon;
            panel19.BackgroundImage = currentState.playerCard[1].playerIcon;
            panel20.BackgroundImage = currentState.enemyCard[0].enemyIcon;
            panel21.BackgroundImage = currentState.enemyCard[1].enemyIcon;

            

            selectedCell = Coordinate.Minus;
            SelectedCard = -1;
            ResetHighlights();
        }

        private void ResetHighlights()
        {
            
            foreach (var item in board)
            {
                item.Enabled = true;
                item.Highlighted = false;
                item.Tag = -1;
            }
        }

        private void HighlightMoves()
        {
            ResetHighlights();
            board[selectedCell.Y, selectedCell.X].Highlighted = true;
            board[selectedCell.Y, selectedCell.X].Enabled = false;
            var (posCoor, cardIdx) = currentState.getPosCoor(selectedCell, currentState.playerCard[SelectedCard]);
            for (int i = 0; i < posCoor.Count; i++)
            {
                char temp = currentState.state[posCoor[i].Y, posCoor[i].X];
                if (temp == 'p' || temp == 'P') continue;
                board[posCoor[i].Y, posCoor[i].X].Highlighted = true;
                board[posCoor[i].Y, posCoor[i].X].Tag = cardIdx[i];
            }
        }

        private void AIMove()
        {
            Card cardUseBot = null;
            int cardIdxUseBot = -1;
            Coordinate startCoorBot = Coordinate.Minus;
            (startCoorBot, cardUseBot, cardIdxUseBot) = AiMove.getMove(currentState);
            currentState.Step(startCoorBot, cardUseBot, cardIdxUseBot);
            RefreshBoard();
        }

        private void BoardCellClick(object sender, EventArgs e)
        {
            var temp = (BoardPanel)sender;
            if (SelectedCard == -1) return;
            if (board[temp.Y,temp.X].Highlighted && (int)board[temp.Y, temp.X].Tag!=-1)
            {
                currentState.Step(selectedCell, currentState.playerCard[SelectedCard], (int)board[temp.Y, temp.X].Tag);
                RefreshBoard();
                AIMove();
                return;
            }
            if (currentState.state[temp.Y, temp.X] != 'p' && currentState.state[temp.Y, temp.X] != 'P') return;

            selectedCell = new Coordinate(temp.X, temp.Y);
            HighlightMoves();
            
        }

        private void BoardCellMouseEnter(object sender, EventArgs e)
        {
            var temp = (BoardPanel)sender;
            temp.BackColor = temp.Highlighted ? BoardPanel.HighlightColorHover : temp.DefaultColorHover;
        }

        private void BoardCellMouseLeave(object sender, EventArgs e)
        {
            var temp = (BoardPanel)sender;
            temp.BackColor = temp.Highlighted ? BoardPanel.HighlightColor : temp.DefaultColor;
        }

        public void ResetGame()
        {
            currentState = new GameState() { isPlayerMove = true};
            RefreshBoard();
        }

        private void label1_Click(object sender, EventArgs e) => this.Close();

        private void Button_Enter(object sender, EventArgs e) => ((Control)sender).BackColor = Color.DarkMagenta;

        private void Button_Leave(object sender, EventArgs e) => ((Control)sender).BackColor = Color.Transparent;

        private void header_MouseDown(object sender, MouseEventArgs e)
        {
            if (this.WindowState == FormWindowState.Maximized) return;
            ReleaseCapture();
            SendMessage(Handle, 0x112, 0xf012, 0);
        }

        private void label4_Click(object sender, EventArgs e)
        {
            //450,550
            double ratioY = panel10.Parent.Height;

            if (this.WindowState == FormWindowState.Normal)
                this.WindowState = FormWindowState.Maximized;
            else
                this.WindowState = FormWindowState.Normal;

            ratioY = panel10.Parent.Height / ratioY;
            panel10.Width  = (int)(panel10.Width * ratioY);
            panel10.Height = (int)(panel10.Height * ratioY);
            panel10.Top  = (panel10.Parent.Height - panel10.Height) / 2;
            panel10.Left = (panel10.Parent.Width - panel10.Width) / 2 - (panel6.Width + 20) / 2 + 30;

            panel6.Width  = (int)(panel6.Width * ratioY);
            panel6.Height = (int)(panel6.Height * ratioY);
            panel6.Top  = (panel6.Parent.Height - panel6.Height) / 2;
            panel6.Left = panel10.Left + panel10.Width + 10;

            panel11.Height = panel6.Height;
            panel12.Height = panel6.Height;

            panel14.Width = panel14.Height;
            panel15.Width = panel15.Height;

            panel16.Width = panel16.Height;
            panel17.Width = panel17.Height;

            panel14.Left = (panel14.Parent.Width - (panel14.Width + panel15.Width + 20)) / 2;
            panel15.Left = panel14.Right + 20;

            panel16.Left = (panel16.Parent.Width - (panel16.Width + panel17.Width + 20)) / 2;
            panel17.Left = panel16.Right + 20;
        }

        private void ToggleCollapse(object sender, EventArgs e)
        {
            if (t != null) t.Stop();
            navCollapsed = !navCollapsed;
            t = new Timer
            {
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
                if (panel4.Width > target)
                {
                    int temp = panel4.Width - accel;
                    if (temp <= target)
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

        private void MainForm_Load(object sender, EventArgs e) => label4_Click(this, null);

        private void CardMouseEnter(object sender, EventArgs e)
        {
            var temp = (Control)sender;
            if(!(selectedCard==0 && temp == panel18) && !(selectedCard == 1 && temp == panel19)) 
                temp.Parent.BackColor = SystemColors.ControlDark;
        }

        private void CardMouseLeave(object sender, EventArgs e)
        {
            var temp = (Control)sender;
            if (!((selectedCard == 0 && temp == panel18) || (selectedCard == 1 && temp == panel19))) 
                temp.Parent.BackColor = SystemColors.ControlDarkDark;
            
        }

        private void label2_Click(object sender, EventArgs e) => ResetGame();

        private void panel18_Click(object sender, EventArgs e) => SelectedCard = 0;

        private void panel19_Click(object sender, EventArgs e) => SelectedCard = 1;
    }
}
