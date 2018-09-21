using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets
{
    public class Move
    {

        private int oldX;
        private int oldY;
        private int newX;
        private int newY;

        private Piece chessman;

        public Move(Piece cm, int _newX, int _newY)
        {
            oldX = cm.CurrentX;
            oldY = cm.CurrentY;
            newX = _newX;
            newY = _newY;

            if(cm.isWhite == true)
            {
                if(cm.GetType() == typeof(Queen))
                {
                    chessman = (Queen)cm;
                } else if(cm.GetType() == typeof(King))
                {
                    chessman = (King)cm;
                }
                else if(cm.GetType() == typeof(Rook))
                {
                    chessman = (Rook)cm;
                }
                else if(cm.GetType() == typeof(Bishop))
                {
                    chessman = (Bishop)cm;
                }
                else if (cm.GetType() == typeof(Knight))
                {
                    chessman = (Knight)cm;
                }
                else if (cm.GetType() == typeof(Pawn))
                {
                    chessman = (Pawn)cm;
                }
            }
            else
            {
                if (cm.GetType() == typeof(Queen))
                {
                    chessman = (Queen)cm;
                }
                else if (cm.GetType() == typeof(King))
                {
                    chessman = (King)cm;
                }
                else if (cm.GetType() == typeof(Rook))
                {
                    chessman = (Rook)cm;
                }
                else if (cm.GetType() == typeof(Bishop))
                {
                    chessman = (Bishop)cm;
                }
                else if (cm.GetType() == typeof(Knight))
                {
                    chessman = (Knight)cm;
                }
                else if (cm.GetType() == typeof(Pawn))
                {
                    chessman = (Pawn)cm;
                }
            }
        }


        public int GetOldX()
        {
            return oldX;
        }

        public int GetOldY()
        {
            return oldY;
        }

        public int GetNewX()
        {
            return newX;
        }

        public int GetNewY()
        {
            return newY;
        }

        public Piece GetChessman()
        {
            return chessman;
        }



    }
}
