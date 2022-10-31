using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.AxHost;

namespace AI_ONITAMA_2022
{
    public partial class Form2 : Form
    {
        Button[,] btnArr;
        GameState gameState;
        Coordinate start;

        public Form2(OnitamaForm form)
        {
            
            InitializeComponent();
            gameState = new GameState();
            start = new Coordinate(-1, -1);

            btnArr = new Button[5, 5];
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    btnArr[i, j] = new Button();
                    btnArr[i, j].SetBounds(j * 50 + 100, i * 50 + 100, 50, 50);
                    btnArr[i, j].Text = gameState.state[i, j] + "";
                    btnArr[i, j].Show();
                    btnArr[i, j].Tag = i + ";" + j;
                    btnArr[i, j].Parent = this;
                    btnArr[i, j].Click += btnClick;

                }
            }

            renderNow();



        }

        private void renderNow()
        {

            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    btnArr[i, j].Text = gameState.state[i, j] + "";
                    btnArr[i, j].BackColor = Color.White;

                }
            }
            playerBox.Items.Clear();
            botBox.Items.Clear();

            playerBox.Items.Add(gameState.playerCard[0].ToString());
            playerBox.Items.Add(gameState.playerCard[1].ToString());
            botBox.Items.Add(gameState.enemyCard[0].ToString());
            botBox.Items.Add(gameState.enemyCard[1].ToString());
            textBox1.Text = gameState.neutralCard.ToString();

            label1.Text = gameState.isPlayerMove + "" ;

            if(start.X != -1)
            {
                List<Coordinate> posCoor = new List<Coordinate>();
                List<int> cardIdx = new List<int>();
                if (gameState.isPlayerMove)
                {
                    if (playerBox.Text == gameState.playerCard[0].ToString())
                    {
                        
                        (posCoor, cardIdx) = gameState.getPosCoor(start, gameState.playerCard[0]);

                    }
                    else if (playerBox.Text == gameState.playerCard[1].ToString())
                    {
                        (posCoor, cardIdx) = gameState.getPosCoor(start, gameState.playerCard[1]);
                    }
                } else
                {
                    if (botBox.Text == gameState.enemyCard[0].ToString())
                    {
                        (posCoor, cardIdx) = gameState.getPosCoor(start, gameState.enemyCard[0]);

                    }
                    else if (botBox.Text == gameState.enemyCard[1].ToString())
                    {
                        (posCoor, cardIdx) = gameState.getPosCoor(start, gameState.enemyCard[1]);
                    }
                }
                for(int i = 0;i < posCoor.Count;i++)
                {
                    if (cardIdx[i] == 0)
                    {
                        btnArr[posCoor[i].Y, posCoor[i].X].BackColor = Color.Red;
                    }
                    if (cardIdx[i] == 1)
                    {
                        btnArr[posCoor[i].Y, posCoor[i].X].BackColor = Color.Green;
                    }
                    if (cardIdx[i] == 2)
                    {
                        btnArr[posCoor[i].Y, posCoor[i].X].BackColor = Color.Blue;
                    }
                    if (cardIdx[i] == 3)
                    {
                        btnArr[posCoor[i].Y, posCoor[i].X].BackColor = Color.Yellow;
                    }

                }
            }

        }

        private void btnClick(object sender, EventArgs e)
        {
            Button now = (Button)sender;
            int idxI = int.Parse(now.Tag.ToString().Split(';')[0]);
            int idxJ = int.Parse(now.Tag.ToString().Split(';')[1]);
            
            if(now.BackColor != Color.White)
            {
                int cardMove = -1;
                if(now.BackColor == Color.Red)
                {
                    cardMove = 0;
                } else if (now.BackColor == Color.Green)
                {
                    cardMove = 1;
                } else if (now.BackColor == Color.Blue)
                {
                    cardMove = 2;
                } else if (now.BackColor == Color.Yellow)
                {
                    cardMove = 3;
                }
                
                if (gameState.isPlayerMove)
                {
                    if (playerBox.Text == gameState.playerCard[0].ToString())
                    {
                        gameState.Step(start, gameState.playerCard[0], cardMove);

                    }
                    else if (playerBox.Text == gameState.playerCard[1].ToString())
                    {
                        gameState.Step(start, gameState.playerCard[1], cardMove);
                    }
                }
                else
                {
                    if (botBox.Text == gameState.enemyCard[0].ToString())
                    {
                        gameState.Step(start, gameState.enemyCard[0], cardMove);

                    }
                    else if (botBox.Text == gameState.enemyCard[1].ToString())
                    {
                        gameState.Step(start, gameState.enemyCard[1], cardMove);
                    }
                }
                    
                
                
            } else
            {
                start.X = idxJ;
                start.Y = idxI;
            }
            renderNow();

        }
    }
}
