﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AI_ONITAMA_2022
{
    
    public class GameState
    {
        Char[,] state;
        bool isPlayerMove;

        Card[] playerCard;
        Card[] enemyCard;
        Card neutralCard;
        private void Reset()
        {
            // init card
            playerCard = new Card[2];
            enemyCard = new Card[2];
            int[] randShuffle = new int[16];
            for (int i = 0; i < 16; i++)
            {
                randShuffle[i] = i;
            }
            Random rand = new Random();
            for (int i = 0; i < 16; i++)
            {
                int randIdx = rand.Next(i, randShuffle.Length);
                int temp = randShuffle[i];
                randShuffle[i] = randShuffle[randIdx];
                randShuffle[randIdx] = temp;
            }
            playerCard[0] = CardList.GetCard(randShuffle[0]);
            playerCard[1] = CardList.GetCard(randShuffle[1]);
            enemyCard[0] = CardList.GetCard(randShuffle[2]);
            enemyCard[1] = CardList.GetCard(randShuffle[3]);
            neutralCard = CardList.GetCard(randShuffle[4]);

            // init state
            state = new char[5, 5];
            isPlayerMove = neutralCard.IsPlayer();
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    if (i == 0)
                    {
                        state[i, j] = 'b';
                        if (j == 2)
                        {
                            state[i, j] = 'B';
                        }
                    }
                    else if (i == 4)
                    {
                        state[i, j] = 'p';
                        if (j == 2)
                        {
                            state[i, j] = 'P';
                        }
                    }
                    else
                    {
                        state[i, j] = '-';
                    }
                }
            }
        }

        public GameState()
        {
            Reset();
            
        }

        private GameState CloneState(GameState toCLone)
        {
            // clone card
            GameState temp = new GameState();
            temp.playerCard = new Card[2];
            temp.enemyCard = new Card[2];

            for (int i = 0; i < 2; i++)
            {
                playerCard[i] = toCLone.playerCard[i];
                enemyCard[i] = toCLone.enemyCard[i];
            }
            temp.neutralCard = toCLone.neutralCard;

            //clone state
            temp.state = new char[5, 5];
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    temp.state[i, j] = toCLone.state[i, j];
                }
            }
            return temp;
        }

        public GameState GetStateCloned()
        {
            return CloneState(this);
        }

        public GameState GetState()
        {
            return this;
        }

        

        public (bool, String) Step(Coordinate startCoor, Card card, int cardMoveIndex )
        {
            /// return isWin and info
            /// 

            Char temp = this.state[startCoor.Y, startCoor.X];
            bool canReplace = true;

            if (this.isPlayerMove)
            {
                Coordinate[] playerMove = card.GetPlayerMove();
                if (startCoor.X + playerMove[cardMoveIndex].X < 0 ||
                    startCoor.Y + playerMove[cardMoveIndex].Y < 0 ||
                    startCoor.X + playerMove[cardMoveIndex].X > 4 ||
                    startCoor.Y + playerMove[cardMoveIndex].Y > 4)
                {
                    canReplace = false;
                }
                else if (this.state[startCoor.Y + playerMove[cardMoveIndex].Y, startCoor.X + playerMove[cardMoveIndex].X] == 'p' ||
                   this.state[startCoor.Y + playerMove[cardMoveIndex].Y, startCoor.X + playerMove[cardMoveIndex].X] == 'P')
                {
                    canReplace = false;
                }
            }
            else if (!this.isPlayerMove)
            {
                Coordinate[] enemyMove = card.GetEnemyMove();
                if (startCoor.X + enemyMove[cardMoveIndex].X < 0 ||
                    startCoor.Y + enemyMove[cardMoveIndex].Y < 0 ||
                    startCoor.X + enemyMove[cardMoveIndex].X > 4 ||
                    startCoor.Y + enemyMove[cardMoveIndex].Y > 4)
                {
                    canReplace = false;
                }
                else if (this.state[startCoor.Y + enemyMove[cardMoveIndex].Y, startCoor.X + enemyMove[cardMoveIndex].X] == 'b' ||
                   this.state[startCoor.Y + enemyMove[cardMoveIndex].Y, startCoor.X + enemyMove[cardMoveIndex].X] == 'B')
                {
                    canReplace = false;
                }
            }
            if(canReplace)
            {
                if (this.isPlayerMove)
                {
                    Coordinate[] playerMove = card.GetPlayerMove();
                    this.state[startCoor.Y + playerMove[cardMoveIndex].Y, startCoor.X + playerMove[cardMoveIndex].X] = temp;
                    this.state[startCoor.Y, startCoor.X] = '-';

                    if (playerCard[0].Equals(card) )
                    {
                        Card tempCard = playerCard[0];
                        playerCard[0] = neutralCard;
                        neutralCard = tempCard;
                    }
                    else if(playerCard[1].Equals(card) )
                    {
                        Card tempCard = playerCard[1];
                        playerCard[1] = neutralCard;
                        neutralCard = tempCard;
                    }
                }
                else
                {
                    Coordinate[] enemyMove = card.GetEnemyMove();
                    this.state[startCoor.Y + enemyMove[cardMoveIndex].Y, startCoor.X + enemyMove[cardMoveIndex].X] = temp;
                    this.state[startCoor.Y, startCoor.X] = '-';

                    if (enemyCard[0].Equals(card))
                    {
                        Card tempCard = enemyCard[0];
                        enemyCard[0] = neutralCard;
                        neutralCard = tempCard;
                    }
                    else if (enemyCard[1].Equals(card) )
                    {
                        Card tempCard = enemyCard[1];
                        enemyCard[1] = neutralCard;
                        neutralCard = tempCard;
                    }
                }
                isPlayerMove = !isPlayerMove;
            }
            
            return (false, "");
        }



    }
}
