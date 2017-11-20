using System;
using System.Collections.Generic;
using System.Text;

namespace ProjetEchec
{
    abstract class Piece
    {
        public enum Color { white, black };
        public enum Position { Left, Right, Top, Bottom };
        public Color color;
        public Coord Coord;
        public string name;

        public Piece(string n, Color c, Coord coord)
        {
            name = n;
            color = c;
            Coord = coord;
        }

        public virtual List<Coord> GetPossibleMoves(Piece[,] GameBoard, Coord coord)
        {
            List<Coord> listCoord = new List<Coord>();
            return listCoord;
        }

        public virtual List<Coord> GetPossibleMovesWhenInEchec(Piece[,] GameBoard, Coord coord, Roi king)
        {
            List<Coord> listMove = GetPossibleMoves(GameBoard, coord);
            List<Coord> listValidMove = new List<Coord>();

            if (listMove != null)
            {
                listMove.ForEach(delegate (Coord c)
                {
                    king.pieceMakeEchec.ForEach(delegate (Piece p)
                    {
                        if (c.x == p.Coord.x && c.y == p.Coord.y)
                            if (!listValidMove.Contains(c))
                                listValidMove.Add(c);

                        List<Coord> moveP = p.GetPossibleMovesWhenMakeEchec(GameBoard, p.Coord, king.Coord);

                        if (moveP != null)
                        {
                            moveP.ForEach(delegate (Coord coordP)
                            {
                                if (c.x == coordP.x && c.y == coordP.y)
                                    if (!listValidMove.Contains(c))
                                        listValidMove.Add(c);
                            });
                        }
                    });
                });
            }

            if (listValidMove.Count == 0)
                return null;

            return listValidMove;

        }

        public List<Coord> GetPossibleMovesWhenMakeEchec(Piece[,] GameBoard, Coord coordPieceMakeEchec, Coord coordKingInEchec)
        {
            List<Position> pos = new List<Position>();

            if (coordKingInEchec.x > coordPieceMakeEchec.x)
                pos.Add(Position.Right);
            if (coordKingInEchec.x < coordPieceMakeEchec.x)
                pos.Add(Position.Left);
            if (coordKingInEchec.y > coordPieceMakeEchec.y)
                pos.Add(Position.Bottom);
            if (coordKingInEchec.y < coordPieceMakeEchec.y)
                pos.Add(Position.Top);

            List<Coord> listCoordPossible = GetPossibleMoves(GameBoard, coordPieceMakeEchec);
            List<Coord> listCoordPossibleInEchec = new List<Coord>();

            listCoordPossible.ForEach(delegate (Coord c)
            {
                if (pos.Contains(Position.Right) && pos.Contains(Position.Bottom))
                {
                    if (c.x > coordPieceMakeEchec.x && c.y > coordPieceMakeEchec.y)
                        listCoordPossibleInEchec.Add(c);
                }

                if (pos.Contains(Position.Right) && pos.Contains(Position.Top))
                {
                    if (c.x > coordPieceMakeEchec.x && c.y < coordPieceMakeEchec.y)
                        listCoordPossibleInEchec.Add(c);
                }

                if (pos.Contains(Position.Left) && pos.Contains(Position.Bottom))
                {
                    if (c.x < coordPieceMakeEchec.x && c.y > coordPieceMakeEchec.y)
                        listCoordPossibleInEchec.Add(c);
                }

                if (pos.Contains(Position.Left) && pos.Contains(Position.Top))
                {
                    if (c.x < coordPieceMakeEchec.x && c.y < coordPieceMakeEchec.y)
                        listCoordPossibleInEchec.Add(c);
                }
            });

            if (listCoordPossibleInEchec.Count == 0)
                return null;

            return listCoordPossibleInEchec;

        }

        protected bool IsInGameBoard(int x, int y)
        {
            if ((x <= 7 && x >= 0) && (y <= 7 && y >= 0))
                return true;
            return false;
        }

        protected List<Coord> MovementVertical(List<Coord> listCoord, Piece[,] GameBoard, Color ColorAdversary, int x, int y, int SensMovement, bool limit = false)
        {
            Piece CaseMovement = null;
            int i = 0;
            bool validCase = true;

            while (validCase)
            {
                validCase = false;

                if (IsInGameBoard(x, y + SensMovement + i * SensMovement))
                {
                    validCase = true;
                    CaseMovement = GameBoard[x, y + SensMovement + i * SensMovement];
                }


                if (CaseMovement == null && validCase == true)
                {
                    listCoord.Add(new Coord(x, y + SensMovement + i * SensMovement));
                    i++;
                }

                if (CaseMovement != null)
                {
                    if (CaseMovement.color == ColorAdversary)
                        listCoord.Add(new Coord(x, y + SensMovement + i * SensMovement));

                    validCase = false;
                }

                if (limit)
                    break;
            }
            return listCoord;
        }

        protected List<Coord> MovementHorizontal(List<Coord> listCoord, Piece[,] GameBoard, Color ColorAdversary, int x, int y, int SensMovement, bool limit = false)
        {
            Piece CaseMovement = null;
            int i = 0;
            bool validCase = true;

            while (validCase)
            {
                validCase = false;

                if (IsInGameBoard(x + SensMovement + SensMovement * i, y))
                {
                    validCase = true;
                    CaseMovement = GameBoard[x + SensMovement + SensMovement * i, y];
                }


                if (CaseMovement == null && validCase == true)
                {
                    listCoord.Add(new Coord(x + SensMovement + SensMovement * i, y));
                    i++;
                }

                if (CaseMovement != null)
                {
                    if (CaseMovement.color == ColorAdversary)
                        listCoord.Add(new Coord(x + SensMovement + SensMovement * i, y));

                    validCase = false;
                }

                if (limit)
                    break;
            }
            return listCoord;
        }

        protected List<Coord> MovementDiagonal(List<Coord> listCoord, Piece[,] GameBoard, Color ColorAdversary, int x, int y, int SensMovementX, int SensMovementY, bool limit = false)
        {
            Piece CaseMovement = null;
            int i = 0;
            bool validCase = true;

            while (validCase)
            {
                validCase = false;

                if (IsInGameBoard(x + SensMovementX + SensMovementX * i, y + SensMovementY + SensMovementY * i))
                {
                    validCase = true;
                    CaseMovement = GameBoard[x + SensMovementX + SensMovementX * i, y + SensMovementY + SensMovementY * i];
                }


                if (CaseMovement == null && validCase == true)
                {
                    listCoord.Add(new Coord(x + SensMovementX + SensMovementX * i, y + SensMovementY + SensMovementY * i));
                    i++;
                }

                if (CaseMovement != null)
                {
                    if (CaseMovement.color == ColorAdversary)
                        listCoord.Add(new Coord(x + SensMovementX + SensMovementX * i, y + SensMovementY + SensMovementY * i));

                    validCase = false;
                }

                if (limit)
                    break;

            }
            return listCoord;
        }

    }
}
