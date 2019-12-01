using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dawg
{
    /// <summary>
    /// Décrit un noeud utlisé lors de la construction en 1 temps
    /// </summary>
    public class Noeud1Temps:Noeud
    {
        /// <summary>
        /// Retourne si le noeud est équivalent à celui en paramètre
        /// </summary>
        /// <param name="Autre"></param>
        /// <returns></returns>
        public bool IsEquivalent(Noeud1Temps Autre)
        {
            /* Pour être équivalents deux noeuds doivent:
             * -être tous les 2 terminaux ou pas (condition fixe 1)
             * -avoir le même nombre d'enfants (condition fixe 2)
             * -les arcs sortants doivent avoir les mêmes lettres allant vers les mêmes destinations.
             */
            if (this.IsTerminal == Autre.IsTerminal && this.NbreEnfants == Autre.NbreEnfants)
            {
                IEnumerable<string> destinationsThis = this.Sortants.Select(a => a.Serialize).Distinct().OrderBy(i => i);
                IEnumerable<string> destinationsAutre = Autre.Sortants.Select(a => a.Serialize).Distinct().OrderBy(i => i);
                return destinationsThis.SequenceEqual(destinationsAutre);
            }
            else
                return false;
        }
    }
}
