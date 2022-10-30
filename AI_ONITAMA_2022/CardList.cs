using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AI_ONITAMA_2022
{
    
    public class CardList
    {
        public static readonly Card[] cardList;

        public static Card GetCard(int idx)
        {
            return cardList[idx];
        }

        static CardList()
        {
            cardList = new Card[16];
            Coordinate[] tempCoor0 =  {
                new Coordinate(0, -2),
                new Coordinate(0, 1)

            };
            cardList[0] = new Card("Tiger", tempCoor0, true);

            Coordinate[] tempCoor1 =  {
                new Coordinate(0, -1),
                new Coordinate(2, 0),
                new Coordinate(-2, 0)
            };
            cardList[1] = new Card("Crab", tempCoor1, true);

            Coordinate[] tempCoor2 =  {
                new Coordinate(1, -1),
                new Coordinate(1, 1),
                new Coordinate(-1, 1),
                new Coordinate(-1, -1),
            };
            cardList[2] = new Card("Monkey", tempCoor2, true);

            Coordinate[] tempCoor3 =  {
                new Coordinate(0, -1),
                new Coordinate(1, 1),
                new Coordinate(-1, 1),

            };
            cardList[3] = new Card("Crane", tempCoor3, true);

            Coordinate[] tempCoor4 =  {
                new Coordinate(2, -1),
                new Coordinate(1, 1),
                new Coordinate(-1, 1),
                new Coordinate(-2, -1),

            };
            cardList[4] = new Card("Dragon", tempCoor4, false);

            Coordinate[] tempCoor5 =  {
                new Coordinate(1, -1),
                new Coordinate(1, 0),
                new Coordinate(-1, 0),
                new Coordinate(-1, -1),

            };
            cardList[5] = new Card("Elephant", tempCoor5, false);

            Coordinate[] tempCoor6 =  {
                new Coordinate(1, -1),
                new Coordinate(0, 1),
                new Coordinate(-1, -1),

            };
            cardList[6] = new Card("Mantis", tempCoor6, false);


            Coordinate[] tempCoor7 =  {
                new Coordinate(0, -1),
                new Coordinate(1, 0),
                new Coordinate(-1, 0),

            };
            cardList[7] = new Card("Boar", tempCoor7, false);

            Coordinate[] tempCoor8 =  {
                new Coordinate(1, 1),
                new Coordinate(-2, 0),
                new Coordinate(-1, -1),

            };
            cardList[8] = new Card("Frog", tempCoor8, false);

            Coordinate[] tempCoor9 =  {
                new Coordinate(1, 0),
                new Coordinate(1, 1),
                new Coordinate(-1, 0),
                new Coordinate(-1, -1),

            };
            cardList[9] = new Card("Goose", tempCoor9, true);

            Coordinate[] tempCoor10 =  {
                new Coordinate(0, -1),
                new Coordinate(0, 1),
                new Coordinate(-1, 0),

            };
            cardList[10] = new Card("Horse", tempCoor10, false);

            Coordinate[] tempCoor11 =  {
                new Coordinate(1, 0),
                new Coordinate(-1, 1),
                new Coordinate(-1, -1),

            };
            cardList[11] = new Card("Eel", tempCoor11, true);

            Coordinate[] tempCoor12 =  {
                new Coordinate(1, -1),
                new Coordinate(2, 0),
                new Coordinate(-1, 1),

            };
            cardList[12] = new Card("Rabbit", tempCoor12, true);

            Coordinate[] tempCoor13 =  {
                new Coordinate(1, -1),
                new Coordinate(1, 0),
                new Coordinate(-1, 1),
                new Coordinate(-1, 0),

            };
            cardList[13] = new Card("Rooster", tempCoor13, false);

            Coordinate[] tempCoor14 =  {
                new Coordinate(0, -1),
                new Coordinate(1, 0),
                new Coordinate(0, 1),

            };
            cardList[14] = new Card("Ox", tempCoor14, true);

            Coordinate[] tempCoor15 =  {
                new Coordinate(-1, 1),
                new Coordinate(1, 1 ),
                new Coordinate(0, 1),

            };
            cardList[15] = new Card("Cobra", tempCoor15, false);
        }
         
    }
}
