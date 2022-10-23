using Dawg;

using DawgResolver.Model;

using System;
using System.Diagnostics;

namespace DawgResolverTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var g = new Game(Dictionnaire.NomDicoDawgEN_Collins);
            //Pour mes tests 
            var t = g.Grid[7, 5];
            //t.SetWord(g.Player1, "famille", MovementDirection.Across);
            //t.SetWord(g.Player1, "foin", MovementDirection.Down);

            g.Bag.GetLetters(g.Player1, "???OTES");
            
            var sw = Stopwatch.StartNew();
            var ret = g.Resolver.FindMoves(g);
            sw.Stop();
            Debug.WriteLine(string.Join(Environment.NewLine, ret));
        }
    }
}

