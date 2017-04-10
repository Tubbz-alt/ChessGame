using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets
{
    class RandomMoveGenerator : AI
    {

        public Move FindMove(Piece[,] board)
        {
            return null;
        }

        public Piece[,] PerformMove(Piece[,] board, Move m)
        {
            return null;
        }

        public List<Move> IterateMoves(Piece[,] board, bool isWhite = false)
        {
            List<Move> moves = new List<Move>();

            foreach (Piece c in board)
            {
                if (c != null && c.isWhite == false)
                {
                    bool[,] _moves = c.PossibleMove();

                    for (int i = 0; i < 8; i++)
                    {
                        for (int j = 0; j < 8; j++)
                        {
                            if (_moves[i, j] == true)
                            {

                                moves.Add(new Assets.Move(c, i, j));
                            }
                        }
                    }
                }
            }

            return moves;
        }



        public Move GetRandomMove(List<Move> moves)
        {
            System.Random random = new System.Random();
            int randomNumber = random.Next(0, moves.Count);

            Move moveToMake = moves[randomNumber];

            return moveToMake;


        }

    }
}
