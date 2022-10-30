using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AI_ONITAMA_2022
{
    public class Card
    {
        Coordinate[] playerMove;
        Coordinate[] enemyMove;
        bool isPlayer;
        String name;
        public Card(String name,Coordinate[] playerMove, bool isPlayer)
        {
            this.playerMove = playerMove;
            this.enemyMove = new Coordinate[playerMove.Length];
            for (int i = 0; i < playerMove.Length; i++)
            {
                enemyMove[i] = playerMove[i] * -1;
            }
            this.name = name;
            this.isPlayer = isPlayer;
        }
    }
}
