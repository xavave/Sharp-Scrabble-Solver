using Dawg;

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
            dr.FindMoves();
        }
    }
}

