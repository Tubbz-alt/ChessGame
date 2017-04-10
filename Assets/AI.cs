using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets
{
    interface AI
    {
        Move FindMove(Piece[,] board);
        List<Move> IterateMoves(Piece[,] board, bool isWhite);

        Piece[,] PerformMove(Piece[,] board, Move move);


    }
}
