using ChessGame.AI.Interface;
using ChessGame.BusinessObjects;
using System.Collections.Generic;

namespace ChessGame.AI
{
    class RandomMoveGenerator : ArtificialIntelligenceBase
    {   
        public override Move FindMove(PiecesEnum[,] board)
        {
            return null;
        }       
        
        private Move GetRandomMove(List<Move> moves)
        {
            System.Random random = new System.Random();
            int randomNumber = random.Next(0, moves.Count);

            return moves[randomNumber];            
        }
    }
}
