using System;
using System.Collections.Generic;
using System.Text;

namespace ProjetEchec
{
    class Roi : Piece
    {
        public bool isEchec;
        public List<Piece> pieceMakeEchec = new List<Piece>();
        public Roi(string n, Color c, Coord coord) : base(n, c, coord)
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

            listCoord = MovementHorizontal(listCoord, GameBoard, ColorAdversary, coord.x, coord.y, SensMovement, true);
            listCoord = MovementHorizontal(listCoord, GameBoard, ColorAdversary, coord.x, coord.y, -SensMovement, true);
            listCoord = MovementVertical(listCoord, GameBoard, ColorAdversary, coord.x, coord.y, SensMovement, true);
            listCoord = MovementVertical(listCoord, GameBoard, ColorAdversary, coord.x, coord.y, -SensMovement, true);
            listCoord = MovementDiagonal(listCoord, GameBoard, ColorAdversary, coord.x, coord.y, SensMovement, SensMovement, true);
            listCoord = MovementDiagonal(listCoord, GameBoard, ColorAdversary, coord.x, coord.y, SensMovement, -SensMovement, true);
            listCoord = MovementDiagonal(listCoord, GameBoard, ColorAdversary, coord.x, coord.y, -SensMovement, SensMovement, true);
            listCoord = MovementDiagonal(listCoord, GameBoard, ColorAdversary, coord.x, coord.y, -SensMovement, -SensMovement, true);

            if (listCoord.Count == 0)
                return null;

            return listCoord;
        }

        public override List<Coord> GetPossibleMovesWhenInEchec(Piece[,] GameBoard, Coord coord, Roi king)
        {
            List<Coord> listMove = GetPossibleMoves(GameBoard, coord);

            if (listMove != null)
            {
                listMove.ForEach(delegate (Coord c)
                {
                    king.pieceMakeEchec.ForEach(delegate (Piece p)
                    {
                        List<Coord> moveP = p.GetPossibleMovesWhenMakeEchec(GameBoard, p.Coord, king.Coord);

                        if (moveP != null)
                        {
                            moveP.ForEach(delegate (Coord coordP)
                            {
                                if (c.x == coordP.x && c.y == coordP.y)
                                    if (listMove.Contains(c))
                                        listMove.Remove(c);
                            });
                        }
                    });
                });
            }

            if (listMove.Count == 0)
                return null;

            return listMove;

        }



    }
}
