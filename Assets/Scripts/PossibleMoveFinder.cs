using ChessGame;
using ChessGame.BusinessObjects;
using System.Linq;

namespace Assets.Scripts
{
    public static class PossibleMoveFinder
    {
        private static PiecesEnum[] whites = { PiecesEnum.WhitePawn, PiecesEnum.WhiteKnight, PiecesEnum.WhiteBishop, PiecesEnum.WhiteRook, PiecesEnum.WhiteQueen, PiecesEnum.WhiteKing };

        public static bool[,] FindMoves(PiecesEnum[,] board, int x, int y, bool isWhite)
        {
            if(board[x, y] == PiecesEnum.WhitePawn || board[x, y] == PiecesEnum.BlackPawn)
            {
                return FindMovesForPawn(board, x, y, isWhite);
            }
            else if (board[x, y] == PiecesEnum.WhiteKnight || board[x, y] == PiecesEnum.BlackKnight)
            {
                return FindMovesForKnight(board, x, y, isWhite);
            }
            else if (board[x, y] == PiecesEnum.WhiteBishop || board[x, y] == PiecesEnum.BlackBishop)
            {
                return FindMovesForBishop(board, x, y, isWhite);
            }
            else if (board[x, y] == PiecesEnum.WhiteRook || board[x, y] == PiecesEnum.BlackRook)
            {
                return FindMovesForRook(board, x, y, isWhite);
            }
            else if (board[x, y] == PiecesEnum.WhiteQueen || board[x, y] == PiecesEnum.BlackQueen)
            {
                return FindMovesForQueen(board, x, y, isWhite);
            }
            else if (board[x, y] == PiecesEnum.WhiteKing || board[x, y] == PiecesEnum.BlackKing)
            {
                return FindMovesForKing(board, x, y, isWhite);
            }
            else
            {
                return null;
            }
        }

        private static bool[,] FindMovesForPawn(PiecesEnum[,] board, int currentX, int currentY, bool isWhite)
        {
            bool[,] r = new bool[8, 8];
            PiecesEnum c, c2;
            int[] e = BoardManager.Instance.EnPassantMove;

            //White team move
            if (isWhite)
            {
                //Diagonal Left
                if (currentX != 0 && currentY != 7)
                {
                    if (e[0] == currentX - 1 && e[1] == currentY + 1)
                    {
                        r[currentX - 1, currentY + 1] = true;
                    }

                    c = board[currentX - 1, currentY + 1];
                    if (c != PiecesEnum.Empty && !whites.Contains(c))
                    {
                        r[currentX - 1, currentY + 1] = true;
                    }
                }
                //Diagonal Right
                if (currentX != 7 && currentY != 7)
                {
                    if (e[0] == currentX + 1 && e[1] == currentY + 1)
                    {
                        r[currentX + 1, currentY + 1] = true;
                    }

                    c = board[currentX + 1, currentY + 1];
                    if (c != PiecesEnum.Empty && !whites.Contains(c))
                        r[currentX + 1, currentY + 1] = true;
                }
                //Middle
                if (currentY != 7)
                {
                    c = board[currentX, currentY + 1];
                    if (c == PiecesEnum.Empty)
                        r[currentX, currentY + 1] = true;
                }
                //Middle on first move
                if (currentY == 1)
                {
                    c = board[currentX, currentY + 1];
                    c2 = board[currentX, currentY + 2];
                    if (c == PiecesEnum.Empty && c2 == PiecesEnum.Empty)
                        r[currentX, currentY + 2] = true;
                }
            }
            else //Black Team Move
            {
                //Diagonal Left
                if (currentX != 0 && currentY != 0)
                {
                    if (e[0] == currentX - 1 && e[1] == currentY - 1)
                    {
                        r[currentX - 1, currentY - 1] = true;
                    }

                    c = board[currentX - 1, currentY - 1];
                    if (c != PiecesEnum.Empty && whites.Contains(c))
                        r[currentX - 1, currentY - 1] = true;
                }
                //Diagonal Right
                if (currentX != 7 && currentY != 0)
                {

                    if (e[0] == currentX + 1 && e[1] == currentY - 1)
                    {
                        r[currentX + 1, currentY - 1] = true;
                    }

                    c = board[currentX + 1, currentY - 1];
                    if (c != PiecesEnum.Empty && whites.Contains(c))
                        r[currentX + 1, currentY - 1] = true;
                }
                //Middle
                if (currentY != 0)
                {
                    c = board[currentX, currentY - 1];
                    if (c == PiecesEnum.Empty)
                        r[currentX, currentY - 1] = true;
                }
                //Middle on first move
                if (currentY == 6)
                {
                    c = board[currentX, currentY - 1];
                    c2 = board[currentX, currentY - 2];
                    if (c == PiecesEnum.Empty && c2 == PiecesEnum.Empty)
                        r[currentX, currentY - 2] = true;
                }
            }

            return r;
        }

        private static bool[,] FindMovesForKnight(PiecesEnum[,] board, int CurrentX, int CurrentY, bool isWhite)
        {
            bool[,] r = new bool[8, 8];

            //Up Left
            KnightMove(board, CurrentX - 1, CurrentY + 2, ref r, isWhite);

            //Up Right
            KnightMove(board, CurrentX + 1, CurrentY + 2, ref r, isWhite);

            //Right Up
            KnightMove(board, CurrentX + 2, CurrentY + 1, ref r, isWhite);

            //Right Down
            KnightMove(board, CurrentX + 2, CurrentY - 1, ref r, isWhite);

            //Down Left
            KnightMove(board, CurrentX - 1, CurrentY - 2, ref r, isWhite);

            //Down Right
            KnightMove(board, CurrentX + 1, CurrentY - 2, ref r, isWhite);

            //Left Up
            KnightMove(board, CurrentX - 2, CurrentY + 1, ref r, isWhite);

            //Left Down
            KnightMove(board, CurrentX - 2, CurrentY - 1, ref r, isWhite);

            return r;
        }

        private static void KnightMove(PiecesEnum[,] board, int x, int y, ref bool[,] r, bool isWhite)
        {
            PiecesEnum c;
            if (x >= 0 && x < 8 && y >= 0 && y < 8)
            {
                c = board[x, y];
                if (c == PiecesEnum.Empty)
                    r[x, y] = true;
                else if (isWhite != whites.Contains(c))
                    r[x, y] = true;
            }
        }

        private static bool[,] FindMovesForBishop(PiecesEnum[,] board, int CurrentX, int CurrentY, bool isWhite)
        {
            bool[,] r = new bool[8, 8];
            PiecesEnum c;
            int i, j;

            //Top Left
            i = CurrentX;
            j = CurrentY;
            while (true)
            {
                i--;
                j++;
                if (i < 0 || j >= 8)
                    break;

                c = board[i, j];
                if (c == PiecesEnum.Empty)
                    r[i, j] = true;
                else
                {
                    if (isWhite != whites.Contains(c))
                        r[i, j] = true;

                    break;
                }
            }

            //Top Right
            i = CurrentX;
            j = CurrentY;
            while (true)
            {
                i++;
                j++;
                if (i >= 8 || j >= 8)
                    break;

                c = board[i, j];
                if (c == PiecesEnum.Empty)
                    r[i, j] = true;
                else
                {
                    if (isWhite != whites.Contains(c))
                        r[i, j] = true;

                    break;
                }
            }

            //Down Left
            i = CurrentX;
            j = CurrentY;
            while (true)
            {
                i--;
                j--;
                if (i < 0 || j < 0)
                    break;

                c = board[i, j];
                if (c == PiecesEnum.Empty)
                    r[i, j] = true;
                else
                {
                    if (isWhite != whites.Contains(c))
                        r[i, j] = true;

                    break;
                }
            }

            //Down Right
            i = CurrentX;
            j = CurrentY;
            while (true)
            {
                i++;
                j--;
                if (i >= 8 || j < 0)
                    break;

                c = board[i, j];
                if (c == PiecesEnum.Empty)
                    r[i, j] = true;
                else
                {
                    if (isWhite != whites.Contains(c))
                        r[i, j] = true;

                    break;
                }
            }

            return r;
        }

        private static bool[,] FindMovesForRook(PiecesEnum[,] board, int CurrentX, int CurrentY, bool isWhite)
        {
            bool[,] r = new bool[8, 8];
            PiecesEnum c;
            int i;

            //Right
            i = CurrentX;
            while (true)
            {
                i++;
                if (i >= 8)
                    break;

                c = board[i, CurrentY];
                if (c == PiecesEnum.Empty)
                    r[i, CurrentY] = true;
                else
                {
                    if (isWhite != whites.Contains(c))
                        r[i, CurrentY] = true;
                    break;
                }
            }

            //Left
            i = CurrentX;
            while (true)
            {
                i--;
                if (i < 0)
                    break;

                c = board[i, CurrentY];
                if (c == PiecesEnum.Empty)
                    r[i, CurrentY] = true;
                else
                {
                    if (isWhite != whites.Contains(c))
                        r[i, CurrentY] = true;
                    break;
                }
            }

            //Up
            i = CurrentY;
            while (true)
            {
                i++;
                if (i >= 8)
                    break;

                c = board[CurrentX, i];
                if (c == PiecesEnum.Empty)
                    r[CurrentX, i] = true;
                else
                {
                    if (isWhite != whites.Contains(c))
                        r[CurrentX, i] = true;
                    break;
                }
            }


            //Down
            i = CurrentY;
            while (true)
            {
                i--;
                if (i < 0)
                    break;

                c = board[CurrentX, i];
                if (c == PiecesEnum.Empty)
                    r[CurrentX, i] = true;
                else
                {
                    if (isWhite != whites.Contains(c))
                        r[CurrentX, i] = true;
                    break;
                }
            }

            return r;
        }

        private static bool[,] FindMovesForQueen(PiecesEnum[,] board, int CurrentX, int CurrentY, bool isWhite)
        {
            bool[,] r = new bool[8, 8];
            PiecesEnum c;
            int i, j;

            //Right
            i = CurrentX;
            while (true)
            {
                i++;
                if (i >= 8)
                    break;

                c = board[i, CurrentY];
                if (c == PiecesEnum.Empty)
                    r[i, CurrentY] = true;
                else
                {
                    if (isWhite != whites.Contains(c))
                        r[i, CurrentY] = true;
                    break;
                }
            }

            //Left
            i = CurrentX;
            while (true)
            {
                i--;
                if (i < 0)
                    break;

                c = board[i, CurrentY];
                if (c == PiecesEnum.Empty)
                    r[i, CurrentY] = true;
                else
                {
                    if (isWhite != whites.Contains(c))
                        r[i, CurrentY] = true;
                    break;
                }
            }

            //Up
            i = CurrentY;
            while (true)
            {
                i++;
                if (i >= 8)
                    break;

                c = board[CurrentX, i];
                if (c == PiecesEnum.Empty)
                    r[CurrentX, i] = true;
                else
                {
                    if (isWhite != whites.Contains(c))
                        r[CurrentX, i] = true;
                    break;
                }
            }


            //Down
            i = CurrentY;
            while (true)
            {
                i--;
                if (i < 0)
                    break;

                c = board[CurrentX, i];
                if (c == PiecesEnum.Empty)
                    r[CurrentX, i] = true;
                else
                {
                    if (isWhite != whites.Contains(c))
                        r[CurrentX, i] = true;
                    break;
                }
            }

            //Top Left
            i = CurrentX;
            j = CurrentY;
            while (true)
            {
                i--;
                j++;
                if (i < 0 || j >= 8)
                    break;

                c = board[i, j];
                if (c == PiecesEnum.Empty)
                    r[i, j] = true;
                else
                {
                    if (isWhite != whites.Contains(c))
                        r[i, j] = true;

                    break;
                }
            }

            //Top Right
            i = CurrentX;
            j = CurrentY;
            while (true)
            {
                i++;
                j++;
                if (i >= 8 || j >= 8)
                    break;

                c = board[i, j];
                if (c == PiecesEnum.Empty)
                    r[i, j] = true;
                else
                {
                    if (isWhite != whites.Contains(c))
                        r[i, j] = true;

                    break;
                }
            }

            //Down Left
            i = CurrentX;
            j = CurrentY;
            while (true)
            {
                i--;
                j--;
                if (i < 0 || j < 0)
                    break;

                c = board[i, j];
                if (c == PiecesEnum.Empty)
                    r[i, j] = true;
                else
                {
                    if (isWhite != whites.Contains(c))
                        r[i, j] = true;

                    break;
                }
            }

            //Down Right
            i = CurrentX;
            j = CurrentY;
            while (true)
            {
                i++;
                j--;
                if (i >= 8 || j < 0)
                    break;

                c = board[i, j];
                if (c == PiecesEnum.Empty)
                    r[i, j] = true;
                else
                {
                    if (isWhite != whites.Contains(c))
                        r[i, j] = true;

                    break;
                }
            }

            return r;
        }

        private static bool[,] FindMovesForKing(PiecesEnum[,] board, int CurrentX, int CurrentY, bool isWhite)
        {
            bool[,] r = new bool[8, 8];

            PiecesEnum c;
            int i, j;

            //Top Side
            i = CurrentX - 1;
            j = CurrentY + 1;
            if (CurrentY != 7)
            {
                for (int k = 0; k < 3; k++)
                {
                    if (i >= 0 && i < 8)
                    {
                        c = board[i, j];
                        if (c == PiecesEnum.Empty)
                            r[i, j] = true;
                        else if (isWhite != whites.Contains(c))
                            r[i, j] = true;
                    }

                    i++;
                }
            }

            //Down Side
            i = CurrentX - 1;
            j = CurrentY - 1;
            if (CurrentY != 0)
            {
                for (int k = 0; k < 3; k++)
                {
                    if (i >= 0 && i < 8)
                    {
                        c = board[i, j];
                        if (c == PiecesEnum.Empty)
                            r[i, j] = true;
                        else if (isWhite != whites.Contains(c))
                            r[i, j] = true;
                    }

                    i++;
                }
            }

            //Middle Left
            if (CurrentX != 0)
            {
                c = board[CurrentX - 1, CurrentY];
                if (c == PiecesEnum.Empty)
                    r[CurrentX - 1, CurrentY] = true;
                else if (isWhite != whites.Contains(c))
                    r[CurrentX - 1, CurrentY] = true;
            }

            //Middle Right
            if (CurrentX != 7)
            {
                c = board[CurrentX + 1, CurrentY];
                if (c == PiecesEnum.Empty)
                    r[CurrentX + 1, CurrentY] = true;
                else if (isWhite != whites.Contains(c))
                    r[CurrentX + 1, CurrentY] = true;
            }

            return r;
        }
    }
}
