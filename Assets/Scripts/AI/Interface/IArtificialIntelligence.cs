using ChessGame.Pieces.Interface;
using System.Collections.Generic;

namespace ChessGame.AI.Interface
{
    public interface IArtificialIntelligence
    {
        Move FindMove(Piece[,] board);
        List<Move> IterateMoves(Piece[,] board, bool isWhite);
        Piece[,] PerformMove(Piece[,] board, Move move);
    }
}
