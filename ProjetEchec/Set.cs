using System;
using System.Collections.Generic;
using System.Text;

namespace ProjetEchec
{
    class Set
    {
        public Piece.Color colorPiece;
        public List<Piece> set;
        private int startLine, startLineP;

        public Set(Piece.Color c)
        {
            colorPiece = c;

            if (colorPiece == Piece.Color.black)
            {
                startLine = 0;
                startLineP = 1;
            }
            if (colorPiece == Piece.Color.white)
            {
                startLine = 7;
                startLineP = 6;
            }

            set = new List<Piece>();

            for (int i = 0; i < 8; i++)
            {
                set.Add(new Pion("P-" + i, colorPiece, new Coord(i, startLineP)));
            }

            set.Add(new Tour("T-1", colorPiece, new Coord(0, startLine)));
            set.Add(new Cavalier("C-1", colorPiece, new Coord(1, startLine)));
            set.Add(new Fou("F-1", colorPiece, new Coord(2, startLine)));
            set.Add(new Reine("Rei", colorPiece, new Coord(3, startLine)));
            set.Add(new Roi("Roi", colorPiece, new Coord(4, startLine)));
            set.Add(new Fou("F-2", colorPiece, new Coord(5, startLine)));
            set.Add(new Cavalier("C-2", colorPiece, new Coord(6, startLine)));
            set.Add(new Tour("T-2", colorPiece, new Coord(7, startLine)));

        }

    }
}
