
namespace ChessGame.BusinessObjects
{
    public class Move
    {
        public PiecesEnum Piece { get; private set; }
        public int OldX { get; private set; }
        public int OldY { get; private set; }
        public int NewX { get; private set; }
        public int NewY { get; private set; }        

        public Move(PiecesEnum cm, int _oldX, int _oldY, int _newX, int _newY)
        {
            Piece = cm;
            OldX = _oldX;
            OldY = _oldY;
            NewX = _newX;
            NewY = _newY;            
        }
    }
}
