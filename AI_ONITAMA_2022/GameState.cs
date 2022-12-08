using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AI_ONITAMA_2022
{
    
    public class GameState
    {
        public Char[,] state;
        public bool isPlayerMove;

        public Card[] playerCard;
        public Card[] enemyCard;
        public Card neutralCard;


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
           
            GameState temp = new GameState();
            temp.isPlayerMove = toCLone.isPlayerMove;

            // clone card
            temp.playerCard = new Card[2];
            temp.enemyCard = new Card[2];


            for (int i = 0; i < 2; i++)
            {
                temp.playerCard[i] = toCLone.playerCard[i];
                temp.enemyCard[i] = toCLone.enemyCard[i];
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

        public (List<Coordinate>,List<int>) getPosCoor(Coordinate startCoor,Card card)
        {
            Coordinate[] enemyMoveTMp = card.GetEnemyMove();
            Coordinate[] plaMoveTMp = card.GetPlayerMove();


            List<Coordinate> coor = new List<Coordinate>();
            List<int> cardIdx = new List<int>();
            for (int i = 0;i < card.GetPlayerMove().Length;i++)
            {
                int cardMoveIndex = i;
                bool canAdd = true;
                if (this.isPlayerMove)
                {
                    Coordinate[] playerMove = card.GetPlayerMove();
                    if (startCoor.X + playerMove[cardMoveIndex].X < 0 ||
                        startCoor.Y + playerMove[cardMoveIndex].Y < 0 ||
                        startCoor.X + playerMove[cardMoveIndex].X > 4 ||
                        startCoor.Y + playerMove[cardMoveIndex].Y > 4)
                    {
                        canAdd = false;
                    }
                    else if (this.state[startCoor.Y + playerMove[cardMoveIndex].Y, startCoor.X + playerMove[cardMoveIndex].X] == 'p' ||
                       this.state[startCoor.Y + playerMove[cardMoveIndex].Y, startCoor.X + playerMove[cardMoveIndex].X] == 'P')
                    {
                        canAdd = false;
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
                        canAdd = false;
                    }
                    else if (this.state[startCoor.Y + enemyMove[cardMoveIndex].Y, startCoor.X + enemyMove[cardMoveIndex].X] == 'b' ||
                       this.state[startCoor.Y + enemyMove[cardMoveIndex].Y, startCoor.X + enemyMove[cardMoveIndex].X] == 'B')
                    {
                        canAdd = false;
                    }
                }
                if (canAdd)
                {
                    cardIdx.Add(cardMoveIndex);
                    if (this.isPlayerMove)
                    {
                        Coordinate temp = card.GetPlayerMove()[cardMoveIndex];
                        coor.Add((startCoor + temp));
                    }
                    else
                    {
                        Coordinate temp = card.GetEnemyMove()[cardMoveIndex];
                        coor.Add((startCoor + temp));

                    }
                    
                }
            }
            return (coor, cardIdx);
        }
        

        public (bool, String) Step(Coordinate startCoor, Card card, int cardMoveIndex )
        {
            /// return moveSucess and info
            /// 
            staticBoardEvaluatorValue();
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
            
            return (canReplace, "");
        }

        //get the first found coordinate of a pawn based of its character, will return (-1,-1) if not found
        //searches from (x = 0, y = 0) -> (x = n, y = 0) -> (x = n, y = n)
        public Coordinate GetPositionOf(char character)
        {
            for (int i = 0; i < state.GetLength(0); i++)
            {
                for (int j = 0; j < state.GetLength(1); j++)
                {
                    if (state[i, j] == character) return new Coordinate(j, i);
                }
            }
            return Coordinate.Minus;
        }

        public int staticBoardEvaluatorValue()
        {
            // bot + , player -
            int countBot = 0;
            int countPlayer = 0;
            Coordinate playerCoor = new Coordinate(-1000000, -1000000);
            Coordinate botCoor = new Coordinate(-1000000, -1000000);
            Coordinate playerWinCoor = new Coordinate(2,0);
            Coordinate botWinCoor = new Coordinate(2, 4);

            bool playerAlive = false;
            bool botAlive = false;
            for (int i = 0;i < 5;i++)
            {
                for(int j = 0;j < 5;j++)
                {
                    if(state[i, j] == 'p' || state[i,j] == 'P')
                    {
                        if(state[i, j] == 'P')
                        {
                            playerCoor.Y = i;
                            playerCoor.X = j;
                            playerAlive = true;
                        }
                        countPlayer++;
                    }
                    if (state[i,j] == 'b' || state[i,j] == 'B')
                    {
                        if (state[i, j] == 'B')
                        {
                            botCoor.Y = i;
                            botCoor.X = j;
                            botAlive = true;
                        }
                        countBot++; 
                    }
                }
            }

            Coordinate disPlayerCoor = playerCoor - playerWinCoor;
            Coordinate disBotCoor = botCoor - botWinCoor;
            int playerDistValue = Math.Abs(disPlayerCoor.Y) + Math.Abs(disPlayerCoor.X);
            int botDistValue = -Math.Abs(disBotCoor.Y) - Math.Abs(disBotCoor.X);

            if(playerDistValue == 0)
            {
                botAlive = false;
            }
            if (botDistValue == 0)
            {
                playerAlive = false;
            }

            int sbeValNow = 3*countBot + botDistValue + playerDistValue - 3*countPlayer;
            if(!playerAlive)
            {
                sbeValNow = 1000000;
            }
            if(!botAlive)
            {
                sbeValNow = -1000000;
            }

            return sbeValNow;
        }

        public (List<GameState>, List<Coordinate> ,List<Card>, List<int>) getAllPossibleMove()
        {
            List<GameState> gameStateList = new List<GameState>();
            List<Coordinate> startFromList = new List<Coordinate>();
            List<Card> gameCardList = new List<Card>();
            List<int> gameCardIdxList = new List<int>();

            int cardLength = this.enemyCard.Length;
            if (this.isPlayerMove)
            {
                cardLength = this.playerCard.Length;
            }


            for (int cardIdx = 0; cardIdx < cardLength; cardIdx++)
            {
                Card cardNow = this.enemyCard[cardIdx];
                if (this.isPlayerMove)
                {
                    cardNow = this.playerCard[cardIdx];
                } 
                Coordinate[] cardPosMove = cardNow.GetEnemyMove();
                if(this.isPlayerMove)
                {
                    cardPosMove = cardNow.GetPlayerMove();
                }

                for (int cardMoveIdx = 0; cardMoveIdx < cardPosMove.Length; cardMoveIdx++)
                {
                    for (int i = 0; i < 5; i++)
                    {
                        for (int j = 0; j < 5; j++)
                        {

                            if (this.state[i, j] != '-')
                            {
                                bool canMoveThisPiece = false;
                                if (this.isPlayerMove)
                                {
                                    if (this.state[i, j] == 'p' || this.state[i, j] == 'P')
                                    {
                                        canMoveThisPiece = true;
                                    }
                                }
                                else
                                {
                                    if (this.state[i, j] == 'b' || this.state[i, j] == 'B')
                                    {
                                        canMoveThisPiece = true;
                                    }
                                }
                                
                                if(canMoveThisPiece)
                                {
                                    bool canMove;
                                    String info;

                                    GameState copyState = this.GetStateCloned();
                                    Coordinate nowCoor = new Coordinate(j, i);

                                    (canMove, info) = copyState.Step(nowCoor, cardNow, cardMoveIdx);

                                    if (canMove)
                                    {
                                        gameStateList.Add(copyState);
                                        gameCardList.Add(cardNow);
                                        gameCardIdxList.Add(cardMoveIdx);
                                        startFromList.Add(nowCoor);
                                    }
                                }
                            }
                            

                        }
                    }

                }
            }

            
            return (gameStateList, startFromList, gameCardList, gameCardIdxList);
        }

    }
}
