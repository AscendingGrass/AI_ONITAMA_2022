using AI_ONITAMA_2022.Properties;
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
            cardList[0] = new Card("Tiger", tempCoor0, true) {
                playerIcon = Resources.tiger,
                enemyIcon  = Resources.tigeropp
            };

            Coordinate[] tempCoor1 =  {
                new Coordinate(0, -1),
                new Coordinate(2, 0),
                new Coordinate(-2, 0)
            };
            cardList[1] = new Card("Crab", tempCoor1, true)
            {
                playerIcon = Resources.crab,
                enemyIcon = Resources.crabopp
            };

            Coordinate[] tempCoor2 =  {
                new Coordinate(1, -1),
                new Coordinate(1, 1),
                new Coordinate(-1, 1),
                new Coordinate(-1, -1),
            };
            cardList[2] = new Card("Monkey", tempCoor2, true)
            {
                playerIcon = Resources.monkeyFinal,
                enemyIcon = Resources.monkeyFinal
            };

            Coordinate[] tempCoor3 =  {
                new Coordinate(0, -1),
                new Coordinate(1, 1),
                new Coordinate(-1, 1),

            };
            cardList[3] = new Card("Crane", tempCoor3, true)
            {
                playerIcon = Resources.crane,
                enemyIcon = Resources.craneopp
            };

            Coordinate[] tempCoor4 =  {
                new Coordinate(2, -1),
                new Coordinate(1, 1),
                new Coordinate(-1, 1),
                new Coordinate(-2, -1),

            };
            cardList[4] = new Card("Dragon", tempCoor4, false) //salah
            {
                playerIcon = Resources.dragon,
                enemyIcon = Resources.dragonopp
            };

            Coordinate[] tempCoor5 =  {
                new Coordinate(1, -1),
                new Coordinate(1, 0),
                new Coordinate(-1, 0),
                new Coordinate(-1, -1),

            };
            cardList[5] = new Card("Elephant", tempCoor5, false)
            {
                playerIcon = Resources.elephant,
                enemyIcon = Resources.elephantopp
            };

            Coordinate[] tempCoor6 =  {
                new Coordinate(1, -1),
                new Coordinate(0, 1),
                new Coordinate(-1, -1),

            };
            cardList[6] = new Card("Mantis", tempCoor6, false)//salah
            {
                playerIcon = Resources.mantis,
                enemyIcon = Resources.mantisopp
            };


            Coordinate[] tempCoor7 =  {
                new Coordinate(0, -1),
                new Coordinate(1, 0),
                new Coordinate(-1, 0),

            };
            cardList[7] = new Card("Boar", tempCoor7, false)
            {
                playerIcon = Resources.boar,
                enemyIcon = Resources.boaropp
            };

            Coordinate[] tempCoor8 =  {
                new Coordinate(1, 1),
                new Coordinate(-2, 0),
                new Coordinate(-1, -1),

            };
            cardList[8] = new Card("Frog", tempCoor8, false)
            {
                playerIcon = Resources.frog,
                enemyIcon = Resources.frogopp
            };

            Coordinate[] tempCoor9 =  {
                new Coordinate(1, 0),
                new Coordinate(1, 1),
                new Coordinate(-1, 0),
                new Coordinate(-1, -1),

            };
            cardList[9] = new Card("Goose", tempCoor9, true)
            {
                playerIcon = Resources.goose,
                enemyIcon = Resources.gooseopp
            };

            Coordinate[] tempCoor10 =  {
                new Coordinate(0, -1),
                new Coordinate(0, 1),
                new Coordinate(-1, 0),

            };
            cardList[10] = new Card("Horse", tempCoor10, false)
            {
                playerIcon = Resources.horse,
                enemyIcon = Resources.horseopp
            };

            Coordinate[] tempCoor11 =  {
                new Coordinate(1, 0),
                new Coordinate(-1, 1),
                new Coordinate(-1, -1),

            };
            cardList[11] = new Card("Eel", tempCoor11, true)
            {
                playerIcon = Resources.eel,
                enemyIcon = Resources.eelopp
            };

            Coordinate[] tempCoor12 =  {
                new Coordinate(1, -1),
                new Coordinate(2, 0),
                new Coordinate(-1, 1),

            };
            cardList[12] = new Card("Rabbit", tempCoor12, true)
            {
                playerIcon = Resources.rabbit,
                enemyIcon = Resources.rabbitop
            };

            Coordinate[] tempCoor13 =  {
                new Coordinate(1, -1),
                new Coordinate(1, 0),
                new Coordinate(-1, 1),
                new Coordinate(-1, 0),

            };
            cardList[13] = new Card("Rooster", tempCoor13, false)
            {
                playerIcon = Resources.rooster,
                enemyIcon = Resources.roosteropp
            };

            Coordinate[] tempCoor14 =  {
                new Coordinate(0, -1),
                new Coordinate(1, 0),
                new Coordinate(0, 1),

            };
            cardList[14] = new Card("Ox", tempCoor14, true)
            {
                playerIcon = Resources.ox,
                enemyIcon = Resources.oxopp
            };

            Coordinate[] tempCoor15 =  {
                new Coordinate(1, -1),
                new Coordinate(1, 1 ),
                new Coordinate(-1, 0),

            };
            cardList[15] = new Card("Cobra", tempCoor15, false)
            {
                playerIcon = Resources.cobra,
                enemyIcon = Resources.cobraopp
            };
        }
         
    }
}
