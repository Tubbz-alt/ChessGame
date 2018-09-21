using ChessGame.AI.Interface;
using ChessGame.Pieces;
using ChessGame.Pieces.Interface;
using System;
using System.Collections.Generic;

namespace ChessGame.AI
{
    /** white = true, black = false **/
    public class MiniMax : IArtificialIntelligence
    {
        private const int DEPTH = 10;

        public Move FindMove(Piece[,] board)
        {
            Move bestMove = null;
            List<Move> moves = IterateMoves(board, false);
            List<Piece[,]> possibleBoards = GetPossibleBoards(board, moves);

            int currentBestMoveScore = Int32.MinValue;
            List<Move> bestMoves = new List<Move>();

            for (int i = 0; i < possibleBoards.Count; i++)
            {
                int score = FindMinScoreForBoard(possibleBoards[i], Int32.MinValue, Int32.MaxValue, DEPTH);
                if (score > currentBestMoveScore)
                {
                    bestMove = moves[i];
                    currentBestMoveScore = score;
                    bestMoves.Clear();
                    bestMoves.Add(bestMove);
                }
                else if(score == currentBestMoveScore)
                {
                    bestMoves.Add(moves[i]);
                }
            }

            Random random = new Random();
            int rand = 0;

            if (bestMoves.Count > 0)
            {
                rand = random.Next(0, bestMoves.Count);
            }
            
            return bestMoves[rand];
        }

        public List<Piece[,]> GetPossibleBoards(Piece[,] board, List<Move> moves)
        {
            List<Piece[,]> possibleBoards = new List<Piece[,]>();

            foreach (Move m in moves)
            {
                Piece[,] temp = new Piece[8, 8];
                Array.Copy(board, temp, 64);                
                temp = PerformMove(temp, m);
                possibleBoards.Add(temp);
            }
            
            return possibleBoards;
        }

        public List<Move> IterateMoves(Piece[,] board, bool color)
        {
            List<Move> moves = new List<Move>();
            foreach (Piece p in board)
            {
                if (p != null && p.isWhite == color)
                {
                    bool[,] _moves = p.PossibleMove();

                    for (int i = 0; i < 8; i++)
                    {
                        for (int j = 0; j < 8; j++)
                        {
                            if (_moves[i, j] == true)
                            {
                                moves.Add(new Move(p, i, j));
                            }
                        }
                    }
                }
            }     
            
            return moves;
        }

        private int FindMinScoreForBoard(Piece[,] board, int alpha, int beta, int depth) // white minimizes
        {
            if (depth == 0)
            {
                return CalculateScoreForBoard(board);
            }

            List<Move> moves = IterateMoves(board, true);
            List<Piece[,]> boards = GetPossibleBoards(board, moves);

            int value = Int32.MaxValue;

            foreach (Piece[,] _board in boards)
            {
                value = Math.Min(value, FindMaxScoreForBoard(_board, alpha, beta, depth - 1));

                if (value <= alpha)
                {
                    break;
                }

                if (value > alpha)
                {
                    alpha = value;
                }
            }

            return value;
        }


        private int FindMaxScoreForBoard(Piece[,] board, int alpha, int beta, int depth) // black maximizes
        {
            if (depth == 0)
            {
                return CalculateScoreForBoard(board);
            }

            List<Move> moves = IterateMoves(board, false);
            List<Piece[,]> boards = GetPossibleBoards(board, moves);

            int value = Int32.MinValue;

            foreach (Piece[,] _board in boards)
            {
                value = Math.Max(value, FindMinScoreForBoard(_board, alpha, beta, depth - 1));

                if (value >= beta)
                {
                    break;
                }

                if (value < beta)
                {
                    beta = value;
                }
            }

            return value;
        }

        public Piece[,] PerformMove(Piece[,] board, Move move)
        {
            board[move.GetOldX(), move.GetOldY()] = null;
            board[move.GetNewX(), move.GetNewY()] = move.GetChessman();
            
            return board;
        }
        
        public int CalculateScoreForBoard(Piece[,] board)
        {
            int whitescore = 0;
            int blackscore = 0;
            
            foreach(Piece p in board)
            {
                if(p != null && p.isWhite == true)
                {
                    if(p.GetType() == typeof(Queen))
                    {
                        whitescore += 9;
                    }
                    else if(p.GetType() == typeof(Rook))
                    {
                        whitescore += 5;
                    }
                    else if(p.GetType() == typeof(Knight) || p.GetType() == typeof(Bishop))
                    {
                        whitescore += 3;
                    }
                    else if (p.GetType() == typeof(Pawn))
                    {
                        whitescore += 1;
                    }
                    else if (p.GetType() == typeof(King))
                    {
                        whitescore += 1000000;
                    }
                }
                else if (p != null && p.isWhite == false)
                {
                    if (p.GetType() == typeof(Queen))
                    {
                        blackscore += 9;
                    }
                    else if (p.GetType() == typeof(Rook))
                    {
                        blackscore += 5;
                    }
                    else if (p.GetType() == typeof(Knight) || p.GetType() == typeof(Bishop))
                    {
                        blackscore += 3;
                    }
                    else if (p.GetType() == typeof(Pawn))
                    {
                        blackscore += 1;
                    }
                    else if (p.GetType() == typeof(King))
                    {
                        blackscore += 1000000;
                    }
                }
            }

            return blackscore - whitescore;             
        }
    }
}
