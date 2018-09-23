using UnityEngine;

namespace ChessGame.PiecesGameObjects.Interface
{
    public abstract class Piece : MonoBehaviour
    {
        public int CurrentX { set; get; }
        public int CurrentY { set; get; }
        public bool IsWhite { get; set; }

        public void SetPosition(int x, int y)
        {
            CurrentX = x;
            CurrentY = y;
        }
    }
}

