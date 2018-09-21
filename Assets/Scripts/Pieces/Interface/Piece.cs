using UnityEngine;

namespace ChessGame.Pieces.Interface
{
    public abstract class Piece : MonoBehaviour
    {
        public int CurrentX { set; get; }
        public int CurrentY { set; get; }
        public bool isWhite { get; set; }

        public void SetPosition(int x, int y)
        {
            CurrentX = x;
            CurrentY = y;
        }

        public abstract bool[,] PossibleMove();

        public abstract string GetPieceType();
    }
}

