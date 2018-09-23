using ChessGame.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ChessGame.AI.Interface
{
    public abstract class ArtificialIntelligenceBase
    {
        public abstract Move FindMove(PiecesEnum[,] board);

        protected readonly PiecesEnum[] whites = { PiecesEnum.WhitePawn, PiecesEnum.WhiteKnight, PiecesEnum.WhiteBishop, PiecesEnum.WhiteRook, PiecesEnum.WhiteQueen, PiecesEnum.WhiteKing };
        
        protected List<PossibleBoard> GetPossibleBoards(PiecesEnum[,] board, bool color)
        {
            List<Move> moves = GetPossibleMovesForBoard(board, color);

            List<PossibleBoard> possibleBoards = new List<PossibleBoard>();

            foreach (Move m in moves)
            {
                PiecesEnum[,] temp = new PiecesEnum[8, 8];
                Array.Copy(board, temp, 64);
                PerformMove(temp, m);
                possibleBoards.Add(new PossibleBoard
                {
                    Board = temp,
                    Move = m
                });                
            }

            return possibleBoards;
        }


        protected List<Move> GetPossibleMovesForBoard(PiecesEnum[,] board, bool color)
        {
            List<Move> moves = new List<Move>();
            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    if (board[x, y] != PiecesEnum.Empty && whites.Contains(board[x, y]) == color)
                    {
                        bool[,] _moves = PossibleMoveFinder.FindMoves(board, x, y, whites.Contains(board[x, y]));

                        for (int i = 0; i < 8; i++)
                        {
                            for (int j = 0; j < 8; j++)
                            {
                                if (_moves[i, j] == true)
                                {
                                    moves.Add(new Move(board[x, y], x, y, i, j));
                                }
                            }
                        }
                    }
                }
            }

            return moves;
        }

        protected void PerformMove(PiecesEnum[,] board, Move move)
        {
            board[move.OldX, move.OldY] = PiecesEnum.Empty;
            board[move.NewX, move.NewY] = move.Piece;
        }

        protected int CalculateScoreForBoard(PiecesEnum[,] board)
        {
            int whitescore = 0;
            int blackscore = 0;

            foreach (PiecesEnum p in board)
            {
                if (p != PiecesEnum.Empty)
                {
                    if (p == PiecesEnum.WhitePawn || p == PiecesEnum.BlackPawn)
                    {
                        if (whites.Contains(p))
                        {
                            whitescore += 1;
                        }
                        else
                        {
                            blackscore += 1;
                        }
                    }
                    else if (p == PiecesEnum.WhiteKnight || p == PiecesEnum.BlackKnight || p == PiecesEnum.WhiteBishop || p == PiecesEnum.BlackBishop)
                    {
                        if (whites.Contains(p))
                        {
                            whitescore += 3;
                        }
                        else
                        {
                            blackscore += 3;
                        }
                    }
                    else if (p == PiecesEnum.WhiteRook || p == PiecesEnum.BlackRook)
                    {
                        if (whites.Contains(p))
                        {
                            whitescore += 5;
                        }
                        else
                        {
                            blackscore += 5;
                        }
                    }
                    if (p == PiecesEnum.WhiteQueen || p == PiecesEnum.BlackQueen)
                    {
                        if (whites.Contains(p))
                        {
                            whitescore += 9;
                        }
                        else
                        {
                            blackscore += 9;
                        }
                    }
                    if (p == PiecesEnum.WhiteKing || p == PiecesEnum.BlackKing)
                    {
                        if (whites.Contains(p))
                        {
                            whitescore += 1000000;
                        }
                        else
                        {
                            blackscore += 1000000;
                        }
                    }
                }
            }

            return blackscore - whitescore;
        }

    }
}
