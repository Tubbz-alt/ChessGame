namespace ChessGame.BusinessObjects
{
    public class PossibleBoard
    {
        public PiecesEnum[,] Board { get; set; }

        public Move Move { get; set; }
    }
}
