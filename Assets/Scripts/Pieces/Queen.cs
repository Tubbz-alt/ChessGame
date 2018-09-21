using ChessGame.Pieces.Interface;

namespace ChessGame.Pieces
{
    public class Queen : Piece
    {
        public override string GetPieceType()
        {
            return "Queen";
        }

        public override bool[,] PossibleMove()
        {
            bool[,] r = new bool[8, 8];
            Piece c;
            int i, j;

            //Right
            i = CurrentX;
            while (true)
            {
                i++;
                if (i >= 8)
                    break;

                c = BoardManager.Instance.CurrentBoard[i, CurrentY];
                if (c == null)
                    r[i, CurrentY] = true;
                else
                {
                    if (c.isWhite != isWhite)
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

                c = BoardManager.Instance.CurrentBoard[i, CurrentY];
                if (c == null)
                    r[i, CurrentY] = true;
                else
                {
                    if (c.isWhite != isWhite)
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

                c = BoardManager.Instance.CurrentBoard[CurrentX, i];
                if (c == null)
                    r[CurrentX, i] = true;
                else
                {
                    if (c.isWhite != isWhite)
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

                c = BoardManager.Instance.CurrentBoard[CurrentX, i];
                if (c == null)
                    r[CurrentX, i] = true;
                else
                {
                    if (c.isWhite != isWhite)
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

                c = BoardManager.Instance.CurrentBoard[i, j];
                if (c == null)
                    r[i, j] = true;
                else
                {
                    if (isWhite != c.isWhite)
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

                c = BoardManager.Instance.CurrentBoard[i, j];
                if (c == null)
                    r[i, j] = true;
                else
                {
                    if (isWhite != c.isWhite)
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

                c = BoardManager.Instance.CurrentBoard[i, j];
                if (c == null)
                    r[i, j] = true;
                else
                {
                    if (isWhite != c.isWhite)
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

                c = BoardManager.Instance.CurrentBoard[i, j];
                if (c == null)
                    r[i, j] = true;
                else
                {
                    if (isWhite != c.isWhite)
                        r[i, j] = true;

                    break;
                }
            }

            return r;
        }
    }
}