using System;

namespace ProjetEchec
{
    class Program
    {
        static void Main(string[] args)
        {
            Game g = new Game();

            g.StartGame();
            
            Console.Read();
        }
    }
}
