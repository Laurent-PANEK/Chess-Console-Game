using System;
using System.Collections.Generic;
using System.Text;

namespace ProjetEchec
{
    class Fou : Piece
    {
        public Fou(string n, Color c, Coord coord) : base(n, c, coord)
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

            listCoord = MovementDiagonal(listCoord, GameBoard, ColorAdversary, coord.x, coord.y, SensMovement, SensMovement);
            listCoord = MovementDiagonal(listCoord, GameBoard, ColorAdversary, coord.x, coord.y, SensMovement, -SensMovement);
            listCoord = MovementDiagonal(listCoord, GameBoard, ColorAdversary, coord.x, coord.y, -SensMovement, SensMovement);
            listCoord = MovementDiagonal(listCoord, GameBoard, ColorAdversary, coord.x, coord.y, -SensMovement, -SensMovement);

            if (listCoord.Count == 0)
                return null;

            return listCoord;
        }
    }
}
