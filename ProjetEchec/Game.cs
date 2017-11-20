using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ProjetEchec
{
    class Game
    {
        public Piece[,] GameBoard;
        public Piece.Color colorRound;
        public int round;


        public void StartGame()
        {
            GameBoard = new Piece[8, 8];
            round = 1;
            Set setBlack = new Set(Piece.Color.black);
            Set setWhite = new Set(Piece.Color.white);

            setBlack.set.ForEach(AddPieceOnGameBoard);
            setWhite.set.ForEach(AddPieceOnGameBoard);
            //TODO Faire le tableau et le remplis
            //TODO Lancer la partie
            while (!IsGameOver())
            {
                PrintBoard();
                PlayTurn();
            }
        }

        private void AddPieceOnGameBoard(Piece piece)
        {
            GameBoard[piece.Coord.x, piece.Coord.y] = piece;
        }

        public void PlayTurn()
        {
            List<Coord> ValidMoves = new List<Coord>();
            List<Piece> pieceToMustMove = new List<Piece>();
            Piece validPiece;


            Console.Write(" Tour: " + round);
            if (round % 2 == 1)
            {
                Console.WriteLine("  -  Manche Blanche");
                colorRound = Piece.Color.white;
            }
            if (round % 2 == 0)
            {
                Console.WriteLine("  -  Manche Noire");
                colorRound = Piece.Color.black;
            }
            Console.Write("\n");
            Roi king = GetKing(colorRound);
            if (king.isEchec)
            {
                Console.Write(" Votre roi est en echec. Vous devez jouer une des pieces suivantes : ");
                pieceToMustMove = IsPieceWhoCanDisableEchec(king.pieceMakeEchec, king.color);

                if (pieceToMustMove != null)
                {
                    pieceToMustMove.ForEach(delegate (Piece p)
                    {
                        Console.Write(" " + p.name + " ");
                    });
                }
                Console.Write("\n");
            }
            while (true)
            {
                Console.Write(" Quelle Piece Avancer ? ");
                string pieceToMove = Console.ReadLine();

                if (king.isEchec)
                    validPiece = IsValidPieceEchec(pieceToMove, pieceToMustMove);
                else
                    validPiece = IsValidPiece(pieceToMove);

                if (validPiece != null)
                {
                    if (king.isEchec)
                        ValidMoves = validPiece.GetPossibleMovesWhenInEchec(GameBoard, validPiece.Coord, king);
                    else
                        ValidMoves = validPiece.GetPossibleMoves(GameBoard, validPiece.Coord);

                    if (validPiece.name.Contains("P"))
                    {
                        Pion pion = (Pion)validPiece;
                        pion.IsFirst = false;
                    }

                    if (ValidMoves != null)
                    {
                        Console.Write(" Mouvement Possible: ");
                        ValidMoves.ForEach(delegate (Coord c)
                        {
                            Console.Write(" " + c.x + "-" + c.y + " ");
                        });
                        Console.Write("\n");

                        Console.Write(" Deplacer cette piece ? [y/n] ");
                        string validPieceToMove = Console.ReadLine();

                        if (validPieceToMove == "y")
                            break;

                    }
                    else
                    {
                        Console.WriteLine(" Cette piece ne peut pas se deplacer. Reselectionnez en une !");
                    }

                }
                else
                {
                    Console.WriteLine(" La piece selectionnee est invalide. Reselectionnez en une !");
                }

            }

            while (true)
            {
                Console.Write(" Ou deplacer la piece ? Format: X-Y ");
                string placeToMove = Console.ReadLine();

                Coord validPlace = IsValidPlace(placeToMove, ValidMoves);

                if (validPlace != null)
                {
                    MovePiece(validPiece.Coord, validPlace);
                    validPiece.Coord = validPlace;
                    round++;
                    break;
                }
                else
                {
                    Console.WriteLine(" Coordonnees invalides ! Reessayer !");
                }

            }
        }

        public bool IsGameOver()
        {
            Roi king_black = GetKing(Piece.Color.black);
            king_black.isEchec = false;
            Roi king_white = GetKing(Piece.Color.white);
            king_white.isEchec = false;
            //TODO Verifier si la partie est terminé
            IsPieceWhoMakeEchec();

            if (king_black.isEchec)
            {
                return IsEchecMat(king_black);
            }

            if (king_white.isEchec)
            {
                return IsEchecMat(king_white);
            }

            return false;
        }

        public bool IsEchecMat(Roi king)
        {
            if (king.isEchec)
            {
                List<Coord> moveKing = king.GetPossibleMovesWhenInEchec(GameBoard, king.Coord, king);
                List<Piece> pieceToMustMove = IsPieceWhoCanDisableEchec(king.pieceMakeEchec, king.color);
                string winColor = "";

                if (king.color == Piece.Color.white)
                    winColor = "noirs";
                if (king.color == Piece.Color.black)
                    winColor = "blancs";

                if (moveKing == null && pieceToMustMove == null)
                {
                    PrintBoard();
                    Console.WriteLine(" Echec et Mat !");
                    Console.WriteLine(" Les " + winColor + " ont gagne !");
                    return true;
                }
            }

            return false;
        }

        public void IsPieceWhoMakeEchec()
        {
            Roi king = null;

            foreach (Piece p in GameBoard)
            {
                if (p != null)
                {
                    if (p.color == Piece.Color.black)
                        king = GetKing(Piece.Color.white);
                    if (p.color == Piece.Color.white)
                        king = GetKing(Piece.Color.black);

                    List<Coord> moves = p.GetPossibleMoves(GameBoard, p.Coord);

                    if (moves != null)
                    {
                        moves.ForEach(delegate (Coord c)
                        {
                            if (c.x == king.Coord.x && c.y == king.Coord.y)
                            {
                                king.isEchec = true;
                                king.pieceMakeEchec.Add(p);
                            }

                        });
                    }
                }
            }

        }

        public List<Piece> IsPieceWhoCanDisableEchec(List<Piece> pieceMakeEchec, Piece.Color color)
        {
            List<Piece> pieceToMustMove = new List<Piece>();

            Roi king = GetKing(color);

            if (king.GetPossibleMovesWhenInEchec(GameBoard, king.Coord, king) != null)
                pieceToMustMove.Add(king);


            foreach (Piece pieceGame in GameBoard)
            {
                if (pieceGame != null && pieceGame.color == color && pieceGame.name != "Roi")
                {
                    List<Coord> movePieceGame = pieceGame.GetPossibleMoves(GameBoard, pieceGame.Coord);

                    if (movePieceGame != null)
                    {
                        movePieceGame.ForEach(delegate (Coord coordPieceGame)
                        {
                            pieceMakeEchec.ForEach(delegate (Piece pieceEchec)
                            {
                                if (coordPieceGame.x == pieceEchec.Coord.x && coordPieceGame.y == pieceEchec.Coord.y)
                                {
                                    if (!pieceToMustMove.Contains(pieceGame))
                                        pieceToMustMove.Add(pieceGame);
                                }

                                List<Coord> movePieceEchec = pieceEchec.GetPossibleMovesWhenMakeEchec(GameBoard, pieceEchec.Coord, king.Coord);

                                if (movePieceEchec != null)
                                {
                                    movePieceEchec.ForEach(delegate (Coord coordPieceEchec)
                                    {
                                        if (coordPieceGame.x == coordPieceEchec.x && coordPieceGame.y == coordPieceEchec.y)
                                        {
                                            if (!pieceToMustMove.Contains(pieceGame))
                                                pieceToMustMove.Add(pieceGame);
                                        }
                                    });
                                }

                            });

                        });
                    }
                }
            }

            if (pieceToMustMove.Count == 0)
                return null;

            return pieceToMustMove;
        }

        public Roi GetKing(Piece.Color color)
        {
            foreach (Piece p in GameBoard)
            {
                if (p != null && p.color == color && p.name == "Roi")
                    return (Roi)p;
            }

            return null;
        }

        public void PrintBoard()
        {
            Console.WriteLine("");
            Console.WriteLine("    |   0   |   1   |   2   |   3   |   4   |   5   |   6   |   7   |");
            Console.WriteLine(" --------------------------------------------------------------------");

            for (int i = 0; i < 8; i++)
            {
                Console.Write("  " + i);

                for (int j = 0; j < 8; j++)
                {
                    Console.Write(" | ");
                    Piece piece = GameBoard[j, i];
                    if (piece == null)
                        Console.Write("     ");
                    else
                    {
                        if (piece.color == Piece.Color.black)
                        {
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.BackgroundColor = ConsoleColor.Black;
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Black;
                            Console.BackgroundColor = ConsoleColor.White;
                        }
                        Console.Write(" " + piece.name + " ");
                        Console.ResetColor();
                    }
                }

                Console.Write(" |\n --------------------------------------------------------------------\n");
            }
            //TODO afficher le plateau
        }

        public Piece IsValidPiece(string namePiece)
        {
            foreach (Piece p in GameBoard)
            {
                if (p != null && p.name == namePiece.ToUpper() && p.color == colorRound)
                    return p;
            }

            return null;
        }

        public Piece IsValidPieceEchec(string namePiece, List<Piece> pieceToMustMove)
        {
            Piece choosePiece = null;

            pieceToMustMove.ForEach(delegate (Piece p)
            {
                if (p != null && namePiece.ToUpper() == p.name)
                    choosePiece = p;

            });

            return choosePiece;
        }

        public Coord IsValidPlace(string place, List<Coord> validMoves)
        {
            Coord coordPlace = ConvertPlace(place);

            bool valid = false;

            if (coordPlace != null)
            {
                validMoves.ForEach(delegate (Coord c)
                {
                    if (coordPlace.x == c.x && coordPlace.y == c.y)
                    {
                        valid = true;
                    }
                });
            }

            if (valid)
                return coordPlace;
            else
                return null;
        }

        private Coord ConvertPlace(string place)
        {
            Regex r = new Regex(@"\d-\d");

            if (r.Match(place).Success)
            {
                string[] stringCoordPlace = place.Split('-');
                int placeX = Convert.ToInt32(stringCoordPlace[0]);
                int placeY = Convert.ToInt32(stringCoordPlace[1]);
                Coord coordPlace = new Coord(placeX, placeY);
                return coordPlace;
            }
            return null;
        }

        public void MovePiece(Coord CoordPiece, Coord Destination)
        {
            Piece Temp = GameBoard[CoordPiece.x, CoordPiece.y];
            GameBoard[CoordPiece.x, CoordPiece.y] = null;
            GameBoard[Destination.x, Destination.y] = Temp;
        }

    }
}
