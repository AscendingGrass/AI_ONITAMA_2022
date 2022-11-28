using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AI_ONITAMA_2022
{

    public class AiMove
    {
        private static int MAX_PLY = 4;
        
        public static int maxValueAi(GameState stateNow, int depthNow, int alpha, int beta)
        {
            int sbeNow = stateNow.staticBoardEvaluatorValue();
            if (Math.Abs(sbeNow) >= 100000 || depthNow == 0)
            {
                int tempSBE = (int)(sbeNow * Math.Pow((0.99), MAX_PLY - depthNow));
                return tempSBE;
            }
            else
            {
                int maxMoveValue = -1000000;

                List<GameState> gameStateList = new List<GameState>();
                List<Card> gameCardList = new List<Card>();
                List<int> gameCardIdxList = new List<int>();
                List<Coordinate> startFromList = new List<Coordinate>();

                (gameStateList, startFromList, gameCardList, gameCardIdxList) = stateNow.getAllPossibleMove();


                for (int i = 0; i < gameStateList.Count; i++)
                {
                    int valueNow = minValueAi(gameStateList[i], depthNow - 1, alpha, beta);
                    if (valueNow > maxMoveValue)
                    {
                        maxMoveValue = valueNow;
                    }
                    if (maxMoveValue > alpha)
                    {
                        alpha = maxMoveValue;
                    }

                    if (alpha >= beta)
                    {
                        return maxMoveValue;
                    }
                }
                return maxMoveValue;

            }
        }

        public static int minValueAi(GameState stateNow, int depthNow, int alpha, int beta)
        {
            int sbeNow = stateNow.staticBoardEvaluatorValue();
            
            if (Math.Abs(sbeNow) >= 100000 || depthNow == 0)
            {
                int tempSBE = (int)(sbeNow * Math.Pow((0.99), MAX_PLY - depthNow));
                return tempSBE;
            }
            else
            {
                int minMoveValue = 1000000;

                List<GameState> gameStateList = new List<GameState>();
                List<Card> gameCardList = new List<Card>();
                List<int> gameCardIdxList = new List<int>();
                List<Coordinate> startFromList = new List<Coordinate>();

                (gameStateList, startFromList, gameCardList, gameCardIdxList) = stateNow.getAllPossibleMove();


                for (int i = 0; i < gameStateList.Count; i++)
                {
                    int valueNow = maxValueAi(gameStateList[i], depthNow - 1, alpha, beta);
                    
                    if (valueNow < minMoveValue)
                    {
                        minMoveValue = valueNow;
                    }
                    if (beta > minMoveValue)
                    {
                        beta = minMoveValue;
                    }

                    if (alpha >= beta)
                    {
                        return minMoveValue;
                    }
                    
                }
                
                return minMoveValue;

            }
        }

        public static (Coordinate from,Card card, int cardIdx) getMove(GameState stateNow)
        {
            Coordinate startCoor = new Coordinate(-1,-1);
            Card cardUse = null;
            int cardIdxUse = -1;
            int maxMoveValue = -1000000;

            List<GameState> gameStateList = new List<GameState>();
            List<Card> gameCardList = new List<Card>();
            List<int> gameCardIdxList = new List<int>();
            List<Coordinate> startFromList = new List<Coordinate>();

            (gameStateList, startFromList, gameCardList, gameCardIdxList) = stateNow.getAllPossibleMove();


            for (int i = 0;i < gameStateList.Count;i++)
            {
                int valueNow = minValueAi(gameStateList[i], MAX_PLY, -2000000,2000000);
                
                if(valueNow >= maxMoveValue)
                {
                    maxMoveValue = valueNow;
                    cardUse = gameCardList[i];
                    cardIdxUse = gameCardIdxList[i];
                    startCoor = startFromList[i];
                }
            }

            return (startCoor, cardUse, cardIdxUse);
        }
    }

    
}
