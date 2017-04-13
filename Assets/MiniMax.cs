using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets
{
    class MiniMax : AI
    {

        private readonly int DEPTH = 1;


        /** white = true, black = false **/


        public Move FindMove(Piece[,] board)
        {

            //get possible moves given this board
            List<Move> moves = IterateMoves(board, false);

            List<Piece[,]> possibleBoards = GetPossibleBoards(board, moves, false);
            Move bestMove = moves[0];
            int bestMoveScore = EvaluatePosition(board, Int32.MinValue, Int32.MaxValue, DEPTH, false);

            List<Move> bestMoves = new List<Move>();

            for (int i = 1; i < possibleBoards.Count; i++)
            {
                /*
				 * calls evaluatePosition on each possible board and if the score is higher than previous,
				 * reset the bestMove
				 */
                int score = EvaluatePosition(possibleBoards[i], Int32.MinValue, Int32.MaxValue, DEPTH, true);
                if (score > bestMoveScore)
                {
                    bestMove = moves[i];
                    bestMoveScore = score;
                    bestMoves.Clear();
                    bestMoves.Add(bestMove);
                }
                else if(score == bestMoveScore)
                {
                    bestMoves.Add(moves[i]);
                    UnityEngine.Debug.Log("------------------");
                    int j =  0;
                    foreach (Piece p in possibleBoards[i])
                    {
                        if(p != null && p.GetType() == typeof(Pawn))
                        {
                            j++;
                        }
                        
                    }
                    UnityEngine.Debug.Log("num of pawns = " + j);
                }
                score = 0;           
            }

            Random random = new Random();
            int rand = random.Next(0, bestMoves.Count - 1);



            return bestMoves[rand];
        }

        public List<Piece[,]> GetPossibleBoards(Piece[,] board, List<Move> moves, bool color)
        {       

            //create a new list of possible boards, which will be based on those earlier moves
            List<Piece[,]> possibleBoards = new List<Piece[,]>();

            foreach (Move m in moves)
            {
                Piece[,] temp = CopyBoard(board);
                PerformMove(temp, m);
                possibleBoards.Add(temp);
            }

            return possibleBoards;

        }

        public List<Move> IterateMoves(Piece[,] board, bool color)
        {
            List<Move> moves = new List<Move>();
            foreach (Piece c in board)
            {
                if (c != null && c.isWhite == color)
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


        public int EvaluatePosition(Piece[,] board, int alpha, int beta, int depth, bool isWhite)
        {
            if (depth == 0)
            {
                int evaluation = evaluate(board);
                return evaluation;
            }

            List<Move> moves = IterateMoves(board, isWhite);
            List<Piece[,]> boards = GetPossibleBoards(board, moves, isWhite);

            if (isWhite == true)
            {
                int newBeta = beta;
                foreach (Piece[,] _board in boards)
                {  
                                        
                    newBeta = Math.Min(newBeta, EvaluatePosition(_board, alpha, beta, depth - 1, !isWhite)); //think about how to change moves
                    /*
                    if (newBeta <= alpha)
                    {
                        break;
                    }
                    */
                }
                return newBeta; //returns the highest score of the possible moves

            }
            else
            {
                
                int newAlpha = alpha;
                foreach(Piece[,] _board in boards)
                {
                    newAlpha = Math.Max(newAlpha, EvaluatePosition(_board, alpha, beta, depth - 1, !isWhite)); //think about how to change moves
                    /*
                    if (beta <= newAlpha)
                    {
                        break;
                    }
                    */
                }
                return newAlpha; //returns the highest score of the possible moves
            }
        }

        public Piece[,] PerformMove(Piece[,] board, Move move)
        {
            board[move.GetOldX(), move.GetOldY()] = null;
            board[move.GetNewX(), move.GetNewY()] = move.GetChessman();

            return board;
        }

        public Piece[,] CopyBoard(Piece[,] board)
        {
            Piece[,] copy = new Piece[8, 8];

            for(int i = 0; i < 8; i++)
            {
                for(int j = 0; j < 8; j++)
                {
                    if(board[i,j] != null && board[i,j].isWhite == true)
                    {

                        copy[i, j] = board[i, j];
                        /*
                        if (board[i,j].GetType() == typeof(Queen))
                        {
                            Queen q = new Queen();
                            q.CurrentX = i;
                            q.CurrentY = j;
                            q.isWhite = true;
                            copy[i, j] = q;
                        }
                        else if (board[i, j].GetType() == typeof(Rook))
                        {
                            Rook q = new Rook();
                            q.CurrentX = i;
                            q.CurrentY = j;
                            q.isWhite = true;
                            copy[i, j] = q;
                        }
                        else if (board[i, j].GetType() == typeof(Knight))
                        {
                            Knight q = new Knight();
                            q.CurrentX = i;
                            q.CurrentY = j;
                            q.isWhite = true;
                            copy[i, j] = q;
                        }
                        else if(board[i,j].GetType() == typeof(Bishop))
                        {
                            Bishop q = new Bishop();
                            q.CurrentX = i;
                            q.CurrentY = j;
                            q.isWhite = true;
                            copy[i, j] = q;
                        }
                        else if (board[i, j].GetType() == typeof(Pawn))
                        {
                            Pawn q = new Pawn();
                            q.CurrentX = i;
                            q.CurrentY = j;
                            q.isWhite = true;
                            copy[i, j] = q;
                        }
                        else if (board[i, j].GetType() == typeof(King))
                        {
                            King q = new King();
                            q.CurrentX = i;
                            q.CurrentY = j;
                            q.isWhite = true;
                            copy[i, j] = q;
                        }
                        */
                    }
                    else if(board[i,j] != null && board[i,j].isWhite == false)
                    {
                        copy[i, j] = board[i, j];
                        /*
                        if (board[i, j].GetType() == typeof(Queen))
                        {
                            Queen q = new Queen();
                            q.CurrentX = i;
                            q.CurrentY = j;
                            q.isWhite = false;
                            copy[i, j] = q;
                        }
                        else if (board[i, j].GetType() == typeof(Rook))
                        {
                            Rook q = new Rook();
                            q.CurrentX = i;
                            q.CurrentY = j;
                            q.isWhite = false;
                            copy[i, j] = q;
                        }
                        else if (board[i, j].GetType() == typeof(Knight))
                        {
                            Knight q = new Knight();
                            q.CurrentX = i;
                            q.CurrentY = j;
                            q.isWhite = false;
                            copy[i, j] = q;
                        }
                        else if (board[i, j].GetType() == typeof(Bishop))
                        {
                            Bishop q = new Bishop();
                            q.CurrentX = i;
                            q.CurrentY = j;
                            q.isWhite = false;
                            copy[i, j] = q;
                        }
                        else if (board[i, j].GetType() == typeof(Pawn))
                        {
                            Pawn q = new Pawn();
                            q.CurrentX = i;
                            q.CurrentY = j;
                            q.isWhite = false;
                            copy[i, j] = q;
                        }
                        else if (board[i, j].GetType() == typeof(King))
                        {
                            King q = new King();
                            q.CurrentX = i;
                            q.CurrentY = j;
                            q.isWhite = false;
                            copy[i, j] = q;
                        }
                        */
                    }
                }
            }

            return copy;
        }



        public int evaluate(Piece[,] board)
        {
            int whitescore = 0;
            int blackscore = 0;

            /*
             * Iterates through entire board.   
             */

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

            return blackscore - whitescore; //returns blackscore-whitescore, black player tries to maximize, white player tries to minimize
        }
    }
}
