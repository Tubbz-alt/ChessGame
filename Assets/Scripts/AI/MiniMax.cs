using ChessGame.AI.Interface;
using System;
using System.Collections.Generic;
using ChessGame.BusinessObjects;

namespace ChessGame.AI
{
    /** 
     *  
     *  white = true, 
     *  black = false 
     *  alpha = minimum score gaurenteed for the maximizing player
     *  beta  = maximum score gaurenteed for the minimizing player
     *  
     *  **/

    public class MiniMax : ArtificialIntelligenceBase
    {
        private readonly int _depth;        

        public MiniMax(int depth)
        {
            _depth = depth;
        }

        public override Move FindMove(PiecesEnum[,] board)
        {
            Move bestMove = null;
                        
            List<PossibleBoard> possibleBoards = GetPossibleBoards(board, false);

            int currentBestMoveScore = Int32.MinValue;
            List<Move> bestMoves = new List<Move>();

            foreach (var p in possibleBoards)
            {
                int score = FindMinScoreForBoard(p.Board, Int32.MinValue, Int32.MaxValue, _depth);
                if (score > currentBestMoveScore)
                {
                    bestMove = p.Move;
                    currentBestMoveScore = score;
                    bestMoves.Clear();
                    bestMoves.Add(bestMove);
                }
                else if(score == currentBestMoveScore)
                {
                    bestMoves.Add(p.Move);
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
        
        private int FindMinScoreForBoard(PiecesEnum[,] board, int alpha, int beta, int depth) // white minimizes
        {
            if (depth == 0)
            {
                return CalculateScoreForBoard(board);
            }
            
            List<PossibleBoard> boards = GetPossibleBoards(board, true);

            int value = Int32.MaxValue;

            foreach (var p in boards)
            {
                value = Math.Min(value, FindMaxScoreForBoard(p.Board, alpha, beta, depth - 1));

                if (value <= alpha)
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


        private int FindMaxScoreForBoard(PiecesEnum[,] board, int alpha, int beta, int depth) // black maximizes
        {
            if (depth == 0)
            {
                return CalculateScoreForBoard(board);
            }
            
            List<PossibleBoard> boards = GetPossibleBoards(board, false);

            int value = Int32.MinValue;

            foreach (var p in boards)
            {
                value = Math.Max(value, FindMinScoreForBoard(p.Board, alpha, beta, depth));
                                
                if (value >= beta)
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
    }
}
