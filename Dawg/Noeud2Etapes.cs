using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dawg
{
    public class Noeud2Etapes:Noeud
    {
        /// <summary>
        /// Initialise un noeud pour la méthode à 2 étapes
        /// </summary>
        public Noeud2Etapes()
        {
            Profondeur = 0;
        }


        /// <summary>
        /// La notion de rang n'existe pas dans le tuto, il s'agit d'une représentation des 3 condtions fixes d'une équivalence.
        /// </summary>
        /// <remarks>Dans le traitement en 2 temps, un GroupBy sur cette propriété divise le temps d'exécution par un facteur > 10</remarks>
        public string Rang
        {
            get { return string.Format("{0:D3}-{1:D6}{2}", Profondeur, NbreEnfants, IsTerminal ? "#" : ""); }
        }

        /// <summary>
        /// Profondeur du noeud dans l'arbre du premier temps de la création
        /// </summary>
        public int Profondeur { get; set; }


        /// <summary>
        /// Retourne si le noeud est équivalent à celui en paramètre
        /// </summary>
        /// <param name="Autre"></param>
        /// <returns></returns>
        public bool IsEquivalent(Noeud2Etapes Autre)
        {
            /* Pour être équivalents deux noeuds doivent:
             * -avoir la même profondeur (condition fixe 1)
             * -être tous les 2 terminaux ou pas (condition fixe 2)
             * -avoir le même nombre d'enfants (condition fixe 3) avec le groupement ces 3 conditions sont déjà vérifiée lors de la construction en 2 temps
             * -les arcs sortants doivent avoir les mêmes lettres allant vers les mêmes destinations.Dans la construction en 2 temps, les destinations étant redirigées au fur et à mesure de la reduction ctte condition sera évaluée au moment nécessaire
             */
            if (this.Profondeur == Autre.Profondeur && this.IsTerminal == Autre.IsTerminal && this.NbreEnfants == Autre.NbreEnfants)
            {
                IEnumerable<string> destinationsThis = this.Sortants.Select(a => a.Serialize).Distinct().OrderBy(i => i);
                IEnumerable<string> destinationsAutre = Autre.Sortants.Select(a => a.Serialize).Distinct().OrderBy(i => i);
                return destinationsThis.SequenceEqual(destinationsAutre);
            }
            else
                return false;
        }


        public override string ToString()
        {
            return "Noeud n°" + Numero + ", profondeur: " + Profondeur + ", nbr enfants: " + NbreEnfants;
        }


    }
}
