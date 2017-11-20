using System;
using System.Collections.Generic;
using System.Text;

namespace ProjetEchec
{
    class Cavalier : Piece
    {
        public Cavalier(string n, Color c, Coord coord) : base(n, c, coord)
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

            listCoord = MovementL(listCoord, GameBoard, ColorAdversary, coord.x, coord.y, SensMovement, SensMovement);
            listCoord = MovementL(listCoord, GameBoard, ColorAdversary, coord.x, coord.y, -SensMovement, SensMovement);
            listCoord = MovementL(listCoord, GameBoard, ColorAdversary, coord.x, coord.y, SensMovement, -SensMovement);
            listCoord = MovementL(listCoord, GameBoard, ColorAdversary, coord.x, coord.y, -SensMovement, -SensMovement);
            listCoord = MovementLShift(listCoord, GameBoard, ColorAdversary, coord.x, coord.y, SensMovement, SensMovement);
            listCoord = MovementLShift(listCoord, GameBoard, ColorAdversary, coord.x, coord.y, -SensMovement, SensMovement);
            listCoord = MovementLShift(listCoord, GameBoard, ColorAdversary, coord.x, coord.y, SensMovement, -SensMovement);
            listCoord = MovementLShift(listCoord, GameBoard, ColorAdversary, coord.x, coord.y, -SensMovement, -SensMovement);

            if (listCoord.Count == 0)
                return null;

            return listCoord;
        }
        private List<Coord> MovementL(List<Coord> listCoord, Piece[,] GameBoard, Color ColorAdversary, int x, int y, int SensMovementX, int SensMovementY)
        {
            Piece CaseMovement = null;

            bool validCase = false;

            if (IsInGameBoard(x + SensMovementX * 2, y + SensMovementY))
            {
                validCase = true;
                CaseMovement = GameBoard[x + SensMovementX * 2, y + SensMovementY];
            }


            if (CaseMovement == null && validCase == true)
                listCoord.Add(new Coord(x + SensMovementX * 2, y + SensMovementY));

            if (CaseMovement != null && CaseMovement.color == ColorAdversary)
                listCoord.Add(new Coord(x + SensMovementX * 2, y + SensMovementY));

            return listCoord;
        }
        private List<Coord> MovementLShift(List<Coord> listCoord, Piece[,] GameBoard, Color ColorAdversary, int x, int y, int SensMovementX, int SensMovementY)
        {
            Piece CaseMovement = null;

            bool validCase = false;

            if (IsInGameBoard(x + SensMovementX, y + SensMovementY * 2))
            {
                validCase = true;
                CaseMovement = GameBoard[x + SensMovementX, y + SensMovementY * 2];
            }


            if (CaseMovement == null && validCase == true)
                listCoord.Add(new Coord(x + SensMovementX, y + SensMovementY * 2));

            if (CaseMovement != null && CaseMovement.color == ColorAdversary)
                listCoord.Add(new Coord(x + SensMovementX, y + SensMovementY * 2));

            return listCoord;
        }

    }
}
