using System;
using System.Collections.Generic;
using System.Linq;

namespace Dawg
{
    /// <summary>
    /// Décrit un arc reliant un noeud à un autre en représentant une lettre
    /// </summary>
    [Serializable]
    public class Arc
    {
        /// <summary>
        /// Construit un arc lors de la construction en deux étapes
        /// </summary>
        /// <param name="Lettre">Lettre réprésentée</param>
        /// <param name="Origine">Noeud de départ</param>
        /// <param name="Destination">Noeud d'arrivée</param>
        public Arc(char Lettre, Noeud Origine, Noeud Destination)
        {
            this.Lettre = Lettre;
            this.Origine = Origine;
            this.Destination = Destination;
            Destination.Entrants.Add(this);
            Origine.Sortants.Add(this);
        }

        /// <summary>
        /// Construit un arc lors de la lecture du fichier DAWG
        /// </summary>
        /// <param name="Origine">Noeud d'origine</param>
        /// <param name="Serialization">Texte sérializé</param>
        /// <param name="Dawg">Tableau des noeuds en cours de création</param>
        public Arc(Noeud Origine, string Serialization, Noeud[] Dawg)
        {
            this.Lettre = Serialization[0];
            this.Origine = Origine;
            int numDest = Convert.ToInt32(Serialization.Substring(1));

            if (Dawg[numDest - 1] == null)//on vérifie si le noeud de destination existe déjà
                Dawg[numDest - 1] = new Noeud(numDest);//on l'initialise si nécessaire

            this.Destination = Dawg[numDest - 1];
            this.Destination.Entrants.Add(this);
        }


        /// <summary>
        /// Noeud d'arrivée de l'arc
        /// </summary>
        public Noeud Destination { get;  private set; }

        /// <summary>
        /// Noued de départ de l'arc
        /// </summary>
        public Noeud Origine { get; private set; }

        /// <summary>
        /// Lettre représentée
        /// </summary>
        public char Lettre { get; private set; }

        /// <summary>
        /// Retourne cet arc aprés avoir changé la destination
        /// </summary>
        /// <param name="N">Nouvelle destination</param>
        /// <returns>Le même arc, pour pouvoir être utilisé dans une requête Linq</returns>
        public Arc SetDestination(Noeud N)
        {
            Destination = N;
            return this;
        }

        /// <summary>
        /// Retourne cet arc aprés avoir changé l'origine
        /// </summary>
        /// <param name="N">Nouvelle origine</param>
        /// <returns>Le même arc, pour pouvoir être utilisé dans une requête Linq</returns>
        public Arc SetOrigine(Noeud N)
        {
            Origine = N;
            return this;
        }

        /// <summary>
        /// Sérialize l'arc sous la forme LettreNumeroNoeudDestination
        /// </summary>
        public string Serialize
        {
            get { return Lettre.ToString() + Destination.Numero; }
        }

        public override string ToString()
        {
            return Lettre.ToString() + ", du noeud n°" + Origine.Numero +" vers le n°" + Destination.Numero;
        }

        /// <summary>
        /// Méthode statique retournant un mot en fonction des arcs qui le composent
        /// </summary>
        /// <param name="Arcs">Liste des arcs</param>
        /// <returns>Mot écrit</returns>
        public static string RetourneMot(List<Arc> Arcs)
        {
            return string.Join("",Arcs.Select(a => a.Lettre));
        }
    }
}
