using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DawgResolverTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var dr = new Dawg.Resolver();
            dr.NewGame();
            dr.NewDraught("[TBERLE");
            dr.FindMoves();
        }
    }
}
