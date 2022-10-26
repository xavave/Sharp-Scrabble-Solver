using System;
using System.Diagnostics;

using Dawg.Solver.Winform;

namespace DawgResolverTest
{
    class Program
    {
        static void Main(string[] args)
        {
           
            //Pour mes tests 
            var t = Game.DefaultInstance.Grid[7, 5];
            //t.SetWord(g.Player1, "famille", MovementDirection.Across);
            //t.SetWord(g.Player1, "foin", MovementDirection.Down);

            Game.DefaultInstance.Bag.GetLetters(Game.DefaultInstance.Player1, "???OTES");

            var sw = Stopwatch.StartNew();
            var ret = Game.DefaultInstance.Solver.FindMoves();
            sw.Stop();
            Debug.WriteLine(string.Join(Environment.NewLine, ret));
        }
    }
}

