using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Dawg
{
    /// <summary>
    /// Représente un noeud du DAWG
    /// </summary>
    [Serializable]
    public class Noeud
    {
        //Compteur statique qui va permettre lors de la création du DAWG d'affecter un numéro à chaque noeud
        private static int compteur = 0;

        /// <summary>
        /// Initialise un noeud lors d'une compression de DAWG
        /// </summary>
        public Noeud()
        {
            IsTerminal = false;
            Numero = ++compteur;//comme le tuto commence par le noeud 1, je fais le ++ d'abord
            Entrants = new List<Arc>();
            Sortants = new List<Arc>();
       }

        /// <summary>
        /// Initialise un noeud lors du chargement d'un fichier déjà compressé
        /// </summary>
        /// <param name="Numero">Numéro du noeud à créer</param>
        public Noeud(int Numero)
        {
            this.Numero = Numero;
            Entrants = new List<Arc>();
            Sortants = new List<Arc>();

       }

        /// <summary>
        /// Nombre total de noeuds enfants 
        /// </summary>
        public int NbreEnfants { get; set; }

        /// <summary>
        /// Retourne ou définit si ce mot finit des mots
        /// </summary>
        public bool IsTerminal { get; set; }

        /// <summary>
        /// Numéro du noeud
        /// </summary>
        public int Numero { get; set; }

        /// <summary>
        /// Liste des arcs sortants du noeud
        /// </summary>
        /// <remarks>C'est par eux que l'on passe de noeuds en noued pour "écrire un mot"</remarks>
        public List<Arc> Sortants { get; set; }

        /// <summary>
        /// Liste des arcs entrants
        /// </summary>
        /// <remarks>Sert utile lors de la réduction de l'arbre dans la construction en deux étapes, chaque arc étant présant dans la liste Sortants d'un noued et la liste Entrant d'un autre.
        /// Quand on redigire les entrants vers le noeud fusionnant tous les équivalents, ces arcs avec une destination unique serviront plus loin dans la réduction à déterminer de nouvelles équivalences </remarks>
        public List<Arc> Entrants { get; set; }

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
        public bool IsEquivalent(Noeud Autre)
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

        /// <summary>
        /// Renumérote le noeud après la rédution dans la construction en deux temps
        /// </summary>
        /// <param name="NouveauNumero">Nouveau numéro</param>
        /// <returns>Le même noeud, cette méthode étant utilisée dans une requête linq</returns>
        public Noeud ReIndex(int NouveauNumero)
        {
            Numero = NouveauNumero;
            return this;
        }

        /// <summary>
        /// Méthode statique sériliazant le DAWG vers un fichier ASCII
        /// </summary>
        /// <param name="Noeuds">Liste des Noeuds du DAWG</param>
        /// <param name="NbrMots">Nombre de mots total du dictionnaire</param>
        /// <param name="Filename">Chemin du fichier de sortie</param>
        public static void Serialize(IEnumerable<Noeud> Noeuds, int NbrMots, string Filename)
        {
            List<string> lignes = (from n in Noeuds.OrderBy(nn=> nn.Numero)
                                   select string.Join("-",n.Sortants.Select(a=> a.Serialize)) + (n.IsTerminal ? "#" : "")
                                       ).ToList();
            
            lignes.Insert(0, "NBMOTS : " + NbrMots);
            lignes.Insert(1, "NBNOEUDS : " + Noeuds.Count());
            File.WriteAllLines(Filename, lignes);
        }

        /// <summary>
        /// Remets à Zéro le compteur, pour le cas ou il y ait déjà eu un chargement ou une constrution
        /// </summary>
        public static void ResetCompteur()
        {
            compteur = 0;
        }

 
    }
}
