using ChessGame.BusinessObjects;
using ChessGame.PiecesGameObjects;
using ChessGame.PiecesGameObjects.Interface;

namespace ChessGame
{
    public static class BoardMapper
    {
        public static PiecesEnum[,] MapFromGameObjects(Piece[,] board)
        {
            PiecesEnum[,] mapped = new PiecesEnum[8, 8];

            for(int i = 0; i < board.GetLength(0); i++)
            {
                for(int j = 0; j < board.GetLength(1); j++)
                {
                    if(board[i, j] == null)
                    {
                        mapped[i, j] = PiecesEnum.Empty;
                    }
                    else if(board[i, j].GetType() == typeof(Pawn))
                    {
                        if(board[i, j].IsWhite)
                        {
                            mapped[i, j] = PiecesEnum.WhitePawn;
                        }
                        else
                        {
                            mapped[i, j] = PiecesEnum.BlackPawn;
                        }
                    }
                    else if (board[i, j].GetType() == typeof(Knight))
                    {
                        if (board[i, j].IsWhite)
                        {
                            mapped[i, j] = PiecesEnum.WhiteKnight;
                        }
                        else
                        {
                            mapped[i, j] = PiecesEnum.BlackKnight;
                        }
                    }
                    else if (board[i, j].GetType() == typeof(Bishop))
                    {
                        if (board[i, j].IsWhite)
                        {
                            mapped[i, j] = PiecesEnum.WhiteBishop;
                        }
                        else
                        {
                            mapped[i, j] = PiecesEnum.BlackBishop;
                        }
                    }
                    else if (board[i, j].GetType() == typeof(Rook))
                    {
                        if (board[i, j].IsWhite)
                        {
                            mapped[i, j] = PiecesEnum.WhiteRook;
                        }
                        else
                        {
                            mapped[i, j] = PiecesEnum.BlackRook;
                        }
                    }
                    else if (board[i, j].GetType() == typeof(Queen))
                    {
                        if (board[i, j].IsWhite)
                        {
                            mapped[i, j] = PiecesEnum.WhiteQueen;
                        }
                        else
                        {
                            mapped[i, j] = PiecesEnum.BlackQueen;
                        }
                    }
                    else if (board[i, j].GetType() == typeof(King))
                    {
                        if (board[i, j].IsWhite)
                        {
                            mapped[i, j] = PiecesEnum.WhiteKing;
                        }
                        else
                        {
                            mapped[i, j] = PiecesEnum.BlackKing;
                        }
                    }
                }
            }

            return mapped;
        }
    }
}
