using AI_ONITAMA_2022.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace AI_ONITAMA_2022
{
    public partial class MainForm : Form
    {

        public GameState currentState;


        //private GameState test = new GameState()
        //{
        //    state = new char[5, 5] {
        //        {'-','-','-','-','-' },
        //        {'b','-','-','p','-' },
        //        {'b','B','-','P','-' },
        //        {'-','-','-','-','-' },
        //        {'p','-','-','-','-' },
        //    },
        //    playerCard = new Card[2] { CardList.cardList[3], CardList.cardList[12] },
        //    enemyCard = new Card[2] { CardList.cardList[11], CardList.cardList[7] },
        //    neutralCard = CardList.cardList[10],
        //    isPlayerMove = true
        //};

        private Stack<GameState> undoStack = new Stack<GameState>();
        private Stack<GameState> redoStack = new Stack<GameState>();
        private Coordinate selectedCell = Coordinate.Minus;
        private BoardPanel[,] board = new BoardPanel[5, 5];
        private string message = string.Empty;
        private bool navCollapsed = false;
        private bool mboxCollapsed = true;
        private bool maxPlyFocused = false;
        private int animationSpeed = 1;
        private int selectedCard = -1;
        private Timer tMBox = null;
        private Timer tNav  = null;

        private string Message 
        {
            get => message;
            set 
            {
                message = value;
                if (value != string.Empty)
                {
                    lb_message.Text = value;
                }
                ShowMessageBox(value != string.Empty);
            } 
        }

        private int SelectedCard
        {
            get => selectedCard;
            set
            {
                selectedCard = value;
                tableLayoutPanel1.Enabled = value != -1;
                panel14.BackColor = value == 0 ? BoardPanel.HighlightColor : SystemColors.ControlDarkDark;
                panel15.BackColor = value == 1 ? BoardPanel.HighlightColor : SystemColors.ControlDarkDark;
                if(selectedCell != Coordinate.Minus) HighlightMoves();
            }
        }

        public bool MaxPlyFocused {
            get => maxPlyFocused;
            set 
            {
                if (maxPlyFocused == value) return;

                maxPlyFocused = value;
                if (value)
                {
                    textBox1.Focus();
                    label8.BackColor = Color.FromArgb(192, 0, 192);
                    panel27.BackColor = Color.FromArgb(162, 0, 162);
                    textBox1.BackColor = Color.FromArgb(162, 0, 162);
                    if(navCollapsed)ToggleCollapse();
                    return;
                }

                this.ActiveControl = null;
                label8_MouseLeave(this, null);
                MaxPlyEntered();
                
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

            //currentState = test;
            //RefreshBoard();

            
            //bug visual nggk tau kenapa harus diginiin dulu
            Button_Leave(label6,null);
            Button_Leave(label7,null);
            
            
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

        private bool CheckGameOver(out bool status)
        {
            int temp = currentState.staticBoardEvaluatorValue();
            status = false;
            if(temp == 1000000 || temp == -1000000)
            {
                status = temp == -1000000;
                return true;
            }
            return false;
        }

        private void BoardCellClick(object sender, EventArgs e)
        {
            var temp = (BoardPanel)sender;
            if (SelectedCard == -1) return;
            if (board[temp.Y,temp.X].Highlighted && (int)board[temp.Y, temp.X].Tag!=-1)
            {
                PushUndo(currentState.GetStateCloned());
                ClearRedo();
                currentState.Step(selectedCell, currentState.playerCard[SelectedCard], (int)board[temp.Y, temp.X].Tag);
                RefreshBoard();
                if (CheckGameOver(out _)) //player wins
                {
                    Message = "You Won!";
                    panel9.Enabled = false;
                    return;
                }
                AIMove();
                if (CheckGameOver(out _)) //AI wins
                {
                    Message = "You Lost!";
                    panel9.Enabled = false;
                    return;
                }
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
            panel9.Enabled = true;
            currentState = new GameState() { isPlayerMove = true};
            Message = string.Empty;
            ClearUndo();
            ClearRedo();
            RefreshBoard();
        }

        private GameState PopUndo()
        {
            var temp = undoStack.Pop();
            if (undoStack.Count == 0) label6.Enabled = false;
            return temp;
        }

        private void PushUndo(GameState state)
        {
            label6.Enabled = true;
            undoStack.Push(state);
        }

        private void ClearUndo()
        {
            undoStack.Clear();
            label6.Enabled = false;
        }

        private GameState PopRedo()
        {
            var temp = redoStack.Pop();
            if (redoStack.Count == 0) label7.Enabled = false;
            return temp;
        }

        private void PushRedo(GameState state)
        {
            label7.Enabled = true;
            redoStack.Push(state);
        }

        private void ClearRedo()
        {
            redoStack.Clear();
            label7.Enabled = false;
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
            MaxPlyFocused = false;
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

        private void ToggleCollapse()
        {
            if (tNav != null) tNav.Stop();
            navCollapsed = !navCollapsed;
            tNav = new Timer
            {
                Interval = 10
            };
            // open : 280
            // collapsed : 68
            int accel = 0;
            if (navCollapsed)
            {
                tNav.Tick += (snd, evt) => TranslateNavWidth(62);
            }
            else
            {
                tNav.Tick += (snd, evt) => TranslateNavWidth(280);
            }
            tNav.Start();

            void TranslateNavWidth(int target)
            {
                accel += animationSpeed;
                if (panel4.Width > target)
                {
                    int temp = panel4.Width - accel;
                    if (temp <= target)
                    {
                        panel4.Width = target;
                        tNav.Stop();
                        tNav = null;
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
                        tNav.Stop();
                        tNav = null;
                    }
                    else
                    {
                        panel4.Width = temp;
                    }
                }
            }
        }

        private void expandCollapse_Clicked(object sender, EventArgs e)
        {
            MaxPlyFocused = false;
            ToggleCollapse();
        }

        private void ShowMessageBox(bool value)
        {
            if (tMBox != null) tMBox.Stop();
            mboxCollapsed = !value;
            tMBox = new Timer
            {
                Interval = 10
            };

            int accel = 0;
            if (mboxCollapsed)
            {
                tMBox.Tick += (snd, evt) => TranslateMessageBox(191);
            }
            else
            {
                tMBox.Tick += (snd, evt) => TranslateMessageBox(0);
            }
            tMBox.Start();

            void TranslateMessageBox(int target)
            {
                target += panel25.Parent.Width-panel25.Width;
                accel += animationSpeed;
                if (panel25.Left > target)
                {
                    int temp = panel25.Left - accel;
                    if (temp <= target)
                    {
                        panel25.Left = target;
                        tMBox.Stop();
                        tMBox = null;
                        //if (mboxCollapsed) lb_message.Text = string.Empty;
                    }
                    else
                    {
                        panel25.Left = temp;
                    }
                }
                else
                {
                    int temp = panel25.Left + accel;
                    if (temp >= target)
                    {
                        panel25.Left = target;
                        tMBox.Stop();
                        tMBox = null;
                        //if (mboxCollapsed) lb_message.Text = string.Empty;
                    }
                    else
                    {
                        panel25.Left = temp;
                    }
                }
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            label4_Click(this, null);

            //saya juga tidak paham
            Timer t = new Timer { Interval = 100 };
            t.Tick += (snd, evt) => { MaxPlyEntered(); t.Stop(); t.Dispose();};
            t.Start();


        }

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

        private void label2_Click(object sender, EventArgs e)
        {
            MaxPlyFocused = false;
            ResetGame();
        }

        private void panel18_Click(object sender, EventArgs e) => SelectedCard = 0;

        private void panel19_Click(object sender, EventArgs e) => SelectedCard = 1;

        private void label6_Click(object sender, EventArgs e)
        {
            MaxPlyFocused = false;
            PushRedo(currentState.GetStateCloned());
            currentState = PopUndo();
            Message = string.Empty;
            panel9.Enabled = true;
            RefreshBoard();
        }

        private void label7_Click(object sender, EventArgs e)
        {
            MaxPlyFocused = false;
            PushUndo(currentState.GetStateCloned());
            currentState = PopRedo();
            RefreshBoard();
            if (CheckGameOver(out bool playerWon))
            {
                Message = playerWon?"You Won!":"You Lost!";
                panel9.Enabled = false;
                return;
            }
        }

        private void label8_MouseEnter(object sender, EventArgs e)
        {
            if (maxPlyFocused) return;
            Button_Enter(label8,e);
            MaxPlyMouseEnter();
        }

        private void label8_MouseLeave(object sender, EventArgs e)
        {
            if (maxPlyFocused) return;
            Button_Leave(label8, e);
            MaxPlyMouseLeave();
        }

        private void MaxPlyMouseEnter()
        {
            panel27.BackColor = Color.FromArgb(0x7B,0x00,0x7B);
            textBox1.BackColor = Color.FromArgb(0x7B, 0x00, 0x7B);
        }

        private void MaxPlyMouseLeave()
        {
            panel27.BackColor = Color.FromArgb(49, 49, 49);
            textBox1.BackColor = Color.FromArgb(49,49,49);
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
                MaxPlyFocused = false;

            }
        }

        private void MaxPlyEntered()
        {
            this.ActiveControl = null;

            string temp = textBox1.Text;
            if (int.TryParse(temp, out int result) && result > 0)
            {
                AiMove.MAX_PLY = result - 1;

                return;
            }

            textBox1.Text = (AiMove.MAX_PLY + 1).ToString();
        }

        private void textBox1_MouseUp(object sender, MouseEventArgs e)
        {
            textBox1.SelectionLength = 0;
        }

        private void maxPly_Clicked(object sender, EventArgs e)
        {
            textBox1.Text = "";
            MaxPlyFocused = true;
        }
    }
}
