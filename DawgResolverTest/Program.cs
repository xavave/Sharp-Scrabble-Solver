using Dawg;
using System.Linq;

namespace DawgResolverTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var dr = new Dawg.Resolver();
            var dic = new Dictionnaire();
            dic.ChargerFichierDAWG();
           //var ret =  dic.AllWordsStartingWith("ANN");
            dr.NewGame();
            dr.NewDraught("[TBERLE");

            var test = dr.Dictionnaire.dawg.Find(n => n.Numero == 131).IsTerminal;
            dr.FindMoves();
        }
    }
}

