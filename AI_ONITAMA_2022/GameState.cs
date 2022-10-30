using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AI_ONITAMA_2022
{
    
    public class GameState
    {
        public Char[,] state;
        bool isPlayerMove;

        private void reset()
        {
            state = new char[5, 5];
            isPlayerMove = false;
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
            reset();
        }

    }
}
