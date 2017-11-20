using System;
using System.Collections.Generic;
using System.Text;

namespace ProjetEchec
{
    class Pion : Piece
    {
        public bool IsFirst = true;
        public Pion(string n, Color c, Coord coord) : base(n, c, coord)
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

            Piece CaseMovement1 = null, CaseMovement2 = null, CaseAtk1 = null, CaseAtk2 = null;

            if (IsFirst)
            {
                if (IsInGameBoard(coord.x, coord.y + SensMovement * 2))
                    CaseMovement2 = GameBoard[coord.x, coord.y + SensMovement * 2];
                if (CaseMovement2 == null && IsInGameBoard(coord.x, coord.y + SensMovement * 2))
                    listCoord.Add(new Coord(coord.x, coord.y + SensMovement * 2));
            }

            if (IsInGameBoard(coord.x, coord.y + SensMovement))
                CaseMovement1 = GameBoard[coord.x, coord.y + SensMovement];
            if (IsInGameBoard(coord.x + 1, coord.y + SensMovement))
                CaseAtk1 = GameBoard[coord.x + 1, coord.y + SensMovement];
            if (IsInGameBoard(coord.x - 1, coord.y + SensMovement))
                CaseAtk2 = GameBoard[coord.x - 1, coord.y + SensMovement];

            if (CaseMovement1 == null && IsInGameBoard(coord.x, coord.y + SensMovement))
                listCoord.Add(new Coord(coord.x, coord.y + SensMovement));

            if (CaseAtk1 != null && CaseAtk1.color == ColorAdversary)
                listCoord.Add(new Coord(coord.x + 1, coord.y + SensMovement));

            if (CaseAtk2 != null && CaseAtk2.color == ColorAdversary)
                listCoord.Add(new Coord(coord.x - 1, coord.y + SensMovement));

            if (listCoord.Count == 0)
                return null;

            return listCoord;
            //TODO return valid positions fot the piece
        }
    }
}
