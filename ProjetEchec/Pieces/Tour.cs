using System;
using System.Collections.Generic;
using System.Text;

namespace ProjetEchec
{
    class Tour : Piece
    {
        public Tour(string n, Color c, Coord coord) : base(n, c, coord)
        {

        }

        public override List<Coord> GetPossibleMoves(Piece[,] GameBoard, Coord coord)
        {
            List<Coord> listCoord = new List<Coord>();

            Color ColorAdversary;
            int SensMovement;

            if (color == Color.black)
            {
                ColorAdversary = Color.white;
                SensMovement = 1;
            }
            else
            {
                ColorAdversary = Color.black;
                SensMovement = -1;
            }

            listCoord = MovementHorizontal(listCoord, GameBoard, ColorAdversary, coord.x, coord.y, SensMovement);
            listCoord = MovementHorizontal(listCoord, GameBoard, ColorAdversary, coord.x, coord.y, -SensMovement);
            listCoord = MovementVertical(listCoord, GameBoard, ColorAdversary, coord.x, coord.y, SensMovement);
            listCoord = MovementVertical(listCoord, GameBoard, ColorAdversary, coord.x, coord.y, -SensMovement);



            if (listCoord.Count == 0)
                return null;

            return listCoord;
        }

    }
}
