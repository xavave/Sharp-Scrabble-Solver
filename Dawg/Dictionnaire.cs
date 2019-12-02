using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows.Threading;

namespace Dawg
{
    /// <summary>
    /// Cette classe permet de construire, enregistrer et utiliser un dictionnaire DAWG.
    /// Le principe de focntionnement est l'adapation en C# du tutoriel de CarlVB http://codes-sources.commentcamarche.net/faq/10903-compression-d-un-dictionnaire-sous-forme-de-premiereEtape#construction-directe-du-premiereEtape
    /// </summary>
    public class Dictionnaire
    {
        public const int AscShift = 64;
        /// <summary>
        /// Chronomètre utilisé uniquement pour comparer les performances des 2 méthodes de construtions
        /// </summary>
        private Stopwatch chrono = new Stopwatch();

        private Dispatcher dispatcher = Dispatcher.CurrentDispatcher;

        /// <summary>
        /// Cette liste est consituée lors de la construction ou lors de la lecture du fichier compressé.
        /// Elle est nécessaire à l'écriture du fichier après construction ou modification
        /// </summary>
        List<Noeud> dawg;

        #region Propriétés

        /// <summary>
        /// Retourne le nombre de mots contenus dans le dictionnaire
        /// </summary>
        public int NombreMots { get { return Mots.Count; } }

        /// <summary>
        /// Liste des mots dans le dictionnaire, cette liste est issue du fichier ASCII servant de base à la construction
        /// </summary>
        public List<string> Mots { get;  set; }

        /// <summary>
        /// Noeud père de tout le graphe
        /// </summary>
        public Noeud DAWG { get;  set; }

        /// <summary>
        /// Retourne quel travail est en cours dans le dictionnaire
        /// </summary>
        public TravailEnCours TravailEnCours { get; private set; }

        /// <summary>
        /// Nombre de noeuds contenus dans le DAWG
        /// </summary>
        public int NombreNoeuds { get;  set; }

        #endregion

        #region Communication sur la progression de la construction
        /// <summary>
        /// Génère l'évènement EtapeAtteinte
        /// </summary>
        /// <param name="Texte"></param>
        private void AnnonceEtape(string Texte)
        {
            long duree = chrono.ElapsedMilliseconds;
            if (this.EtapeAtteinte != null)
                dispatcher.Invoke(new Action(delegate
                    {
                        this.EtapeAtteinte(Texte + " Chrono: ", duree);
                    }), DispatcherPriority.Send);
        }

        /// <summary>
        /// Génère l'évènement Progression
        /// </summary>
        /// <param name="Pourcent"></param>
        private void AnnonceProgression(int Pourcent)
        {
            if (this.Progression != null)
                dispatcher.Invoke(new Action(delegate
                    {
                        this.Progression(Pourcent);
                    }), DispatcherPriority.Normal);
        }

        /// <summary>
        /// Délégué pour l'évènement EtapeAtteinte
        /// </summary>
        /// <param name="Etape">Texte décrivant l'étape</param>
        /// <param name="TempsExecution">en ms</param>
        public delegate void DawgEvent(string Etape, long TempsExecution);
        /// <summary>
        /// Annonce une étape du traitement
        /// Le temps d'exécution est donné en millisecondes
        /// </summary>
        public event DawgEvent EtapeAtteinte;

        /// <summary>
        /// Délégué pour l'évènment Progression
        /// </summary>
        /// <param name="Pourcent">Pourcentage du traitement effectué</param>
        public delegate void ProgressionEvent(int Pourcent);
        /// <summary>
        /// Annonce la progression d'un traitement
        /// </summary>
        public event ProgressionEvent Progression;

        /// <summary>
        /// Permet de vérifier que la construction ou la modification ont été correctement exécutées, utile principalement en débug, et pour la démo
        /// </summary>
        private void ComparerListeMotsEtDAWG()
        {
            chrono.Restart();

            AnnonceEtape("Comparaison entre la liste de mots et le DAWG");

            List<string> lesMotsDansLeDawg = RetournerTouslesMots();
            List<string> lesMotsEnTropDansleDAwg = lesMotsDansLeDawg.Except(Mots).ToList();
            List<string> lesMotsPerdusDansLeDawg = Mots.Except(lesMotsDansLeDawg).ToList();

            if (lesMotsPerdusDansLeDawg.Count != 0 || lesMotsEnTropDansleDAwg.Count != 0)
                AnnonceEtape("La liste de mots et le DAWG sont différents.");
            else
                AnnonceEtape("La liste de mots et le DAWG sont identiques");

            chrono.Stop();
        }

        #endregion

        #region Construction en 2 temps

        /// <summary>
        /// Construit le DAWG par la méthode dite "en deux temps", à partir de la liste de mots issues du fichier ASCII
        /// </summary>
        public void ConstruireDawgEnDeuxTemps()
        {
            chrono.Restart();

            this.TravailEnCours = TravailEnCours.CreationDawgEn2Etapes;
            Noeud.ResetCompteur();

            //Le Dawg est crée par un thread, ainsi le reste du programme n'est pas bloqué par le processus.
            //Cela permet par exemple de faire défiler une barre de progression sans figer l'interface.
            Thread construire = new Thread(new ThreadStart(leThread2Etapes));
            construire.Start();
        }

        /// <summary>
        /// Thread de la construction en 2 étapes
        /// </summary>
        private void leThread2Etapes()
        {
            List<Noeud> noeuds = ConstruireArbre();

            dawg = AnalyserEtReduire(ref noeuds);

            //Affectation du résultat
            DAWG = dawg[0];

            AnnonceEtape("Ecriture du fichier.");
            Noeud.Serialize(dawg, Mots.Count, "DawgEn2Temps.txt");
            chrono.Stop();

            //==================Ici la construction est finie

            //Cette étape n'est utile que pour la démo et le débug
            ComparerListeMotsEtDAWG();


            TravailEnCours = Dawg.TravailEnCours.Aucun;
        }

        /// <summary>
        /// Construit l'arbre à partir de la liste de mots
        /// </summary>
        /// <param name="Bgw"></param>
        private List<Noeud> ConstruireArbre()
        {
            List<Noeud> noeuds = new List<Noeud> { new Noeud() };

            AnnonceEtape("Début de la création de l'arbre.");
            int p = -1;
            for (int i = 0; i < Mots.Count; i++)
            {
                string mot = Mots[i];

                List<Arc> prefixe = ExtrairePrefixeCommun(mot, noeuds[0]);
                int longueur = prefixe.Count;


                if (longueur > 0)//si un suffixe existe, on continue à partir de là
                {
                    string suffixe = mot.Substring(longueur);
                    AjouterSuffixe(prefixe.Last().Destination, suffixe, ref noeuds);

                }
                else
                    AjouterSuffixe(noeuds[0], mot, ref noeuds);//sinon on crée une nouvelle branche de l'arbre

                int x = 100 * i / Mots.Count;//progression en pourcent casté en int
                if (p < x)
                {
                    p = x;
                    AnnonceProgression(p);
                }
            }

            return noeuds;
        }

        /// <summary>
        /// Ajoute un suffixe au noeud désigné
        /// </summary>
        /// <param name="noeud"></param>
        private void AjouterSuffixe(Noeud LeNoeud, string Suffixe, ref List<Noeud> Noeuds)
        {
            Noeud enCours = LeNoeud;
            foreach (char c in Suffixe)
            {
                Arc a = new Arc(c, enCours, new Noeud());
                enCours = (Noeud)a.Destination;
                Noeuds.Add(enCours);
            }
            enCours.IsTerminal = true;

            MettreAjourProfondeur(enCours, ref Noeuds);
        }

        /// <summary>
        /// Affecte la profodeur et le dernier enfant du nouveau suffixe et met à jour les noeuds "au-dessus"
        /// </summary>
        /// <param name="Final">Noeud Final du nouveau suffixe</param>
        private void MettreAjourProfondeur(Noeud Final, ref List<Noeud> Noeuds)
        {
            //A ce moment là le dictionnaire à encore la forme d'un arbre, donc chaque noeud ne possède qu'un arc entrant
            Noeud enCours = (Noeud)Final.Entrants[0].Origine;

            int i = 1;
            do
            {
                if (enCours.Profondeur < i)//si le noeud a une profondeur inférieure à la celle induite par le nouveau suffixe, on met à jour
                    enCours.Profondeur = i;

                i++;
                enCours.NbreEnfants = enCours.Sortants.Count() + enCours.Sortants.Sum(a => a.Destination.NbreEnfants);

                enCours = enCours != Noeuds[0] ? (Noeud)enCours.Entrants[0].Origine : null;
            } while (enCours != null);
        }

        /// <summary>
        /// Analyse l'arbre issu de la première étape et le réduit
        /// </summary>
        /// <param name="LeNoeud"></param>
        private List<Noeud> AnalyserEtReduire(ref List<Noeud> Noeuds)
        {
            /*
             * J'ai testé plusieurs algorithmes, celui retenu est le plus rapide que j'ai trouvé 58 secondes contre 18 minutes pour le plus lent.
             * 
             * J'utilise des List<T> et non des IEnumerable, car chaque collection est évaluée plusieurs fois, au final le cast en List est un gain de temps important.
             * 
             * Pour ne pas prendre en modèle, un(des) noeud(s) équivalent(s) à un noeud déjà dans le dawg2etapes, la solution la plus efficace est de retirer ce(s) noeud(s) à la collection d'origine, le fait d'enlever des items au fur et à mesure rend chaque requête de plus en plus rapide.
             * Malgré cela l'exécution prend encore plus de 10 minutes si on travaille sur la liste "noeuds", qui contient au départ plus de 786 000 noeuds.      
             * 
             * Le fait de faire un groupBy sur la propriété "Rang" fait tomber le temps d'éxécution de façon considérable (+ de 10 minutes à 1 minute)
             * En effet deux noeuds de même rang sont quasiement équivalents. Ils ont la même profondeur, le même nombre d'enfants et ont le même état terminal. Pour l'équivalence, il reste à comparer les arcs sortants, la destination des arcs évoluant au fur et à mesure de la réduction, cette dernière comparaison ne peut pas être anticipée
             * Ce groupement crée 3742 collections dont la plus importante contient tous les noeuds terminaux sans enfants (254645 noeuds) qui de fait sont équivalents, donc une seule itération suffit à la réduire.
             */
            AnnonceEtape("Début de la réduction de l'arbre en graphe.");
            int pourcent = 1;
            int progress = 0;
            int nbreNoeuds = Noeuds.Count;

            List<Noeud> dawg2etapes = new List<Noeud>();

            List<IGrouping<string, Noeud>> parProfondeur = Noeuds.GroupBy(n => ((Noeud)n).Rang).OrderBy(x => x.Key).ToList();

            foreach (IGrouping<string, Noeud> item in parProfondeur)
            {
                List<Noeud> lesNoeuds = item.ToList();
                while (lesNoeuds.Count > 0)
                {
                    Noeud model = (Noeud)lesNoeuds[0];//on prend en modèle le premier noeud (encore) disponible pour le rang en cours

                    dawg2etapes.Add(FusionEquivalents(model, ref lesNoeuds, ref progress));

                    int x = progress * 100 / nbreNoeuds;
                    if (pourcent < x)
                    {
                        pourcent = x;
                        AnnonceProgression(pourcent);
                    }

                }
            }

            //on réindexe les noeuds
            return dawg2etapes.OrderBy(n => n.Numero).Select((n, i) => (Noeud)n.ReIndex(i + 1)).ToList();

        }

        /// <summary>
        /// Fusion des noeuds équivalents en un seul
        /// </summary>
        /// <param name="Modele">Noeud modèle pour la recherche des équivalents</param>
        /// <param name="Noeuds">Collection de noeuds dans laquelle est faite la recherche</param>
        /// <param name="Progress">Decompte du nomnbre de noeuds total traités pour le suivi de la progression de la réduction</param>
        /// <returns>Le noeud fusionnant tous les équivalents</returns>
        private Noeud FusionEquivalents(Noeud Modele, ref List<Noeud> Noeuds, ref int Progress)
        {
            //on crée un noeud "clone"
            Noeud nouveauNoeud = new Noeud
            {
                IsTerminal = Modele.IsTerminal,
                NbreEnfants = Modele.NbreEnfants,
                Profondeur = Modele.Profondeur,
                Numero = Modele.Numero
            };


            //si un tel noeud existe, on cherche les équivalents
            List<Noeud> equivalents = Noeuds.Where(nn => ((Noeud)nn).IsEquivalent(Modele)).ToList();


            //On mets à jours l'origine des noeuds
            nouveauNoeud.Sortants = (from a in Modele.Sortants
                                     select a.SetOrigine(nouveauNoeud)).ToList();


            //On redirige tous les arcs entrants dans les noeuds équivalents vers le noeud clone
            nouveauNoeud.Entrants = (from n in equivalents
                                     from a in n.Entrants
                                     select a.SetDestination(nouveauNoeud)
                                     ).ToList();

            //on enlève tous les noeuds déjà traités
            Noeuds = Noeuds.Except(equivalents).ToList();

            Progress += equivalents.Count;
            return nouveauNoeud;
        }

        /// <summary>
        /// Retourne le préfixe du nouveau mot, s'il existe déja dans le dictionnaire
        /// </summary>
        /// <param name="Mot">Mot à préfixer</param>
        /// <returns></returns>
        private List<Arc> ExtrairePrefixeCommun(string Mot, Noeud Noeud0)
        {
            List<Arc> prefixe = new List<Arc>();
            Noeud enCours = Noeud0;

            for (int i = 0; i < Mot.Length; i++)
            {
                Arc a = enCours.Sortants.SingleOrDefault(aa => aa.Lettre == Mot[i] && aa.Destination.Entrants.Count == 1);//on recherche si le noeud en cours possède la lettre suivante comme arc sortant
                if (a == null)//si ça n'est pas le cas on sort du for.
                    break;

                prefixe.Add(a);
                enCours = a.Destination;
            }

            return prefixe;
        }

        /// <summary>
        /// Retourne le préfixe commun dans le DAWG déjà consitué
        /// </summary>
        /// <param name="Mot"></param>
        /// <returns></returns>
        private List<Arc> ExtrairePrefixeCommun(string Mot)
        {
            return ExtrairePrefixeCommun(Mot, DAWG);
        }

        #endregion

        #region Utilisation DAWG

        /// <summary>
        /// Charge le fichier ASCII
        /// </summary>
        /// <param name="FileName">Chemin du fichier</param>
        public void ChargerDictionnaireAscii(string FileName)
        {
            chrono.Restart();
            this.TravailEnCours = TravailEnCours.ChargementFichierASCII;

            AnnonceEtape("Début du chargement du dictionnaire Ascii.");
            //Chargemenent et tri par ordre alphabétique du dictionnaire Ascii
            //Le cast en List<string> "coute" un peu de temps cependant, par la suite l'accès à certaines infos (le nombre d'enregistrement, le contenu de la collection en débug, etc.) est instanné
            Mots = File.ReadAllLines(FileName).OrderBy(m => m).ToList();
            AnnonceEtape(string.Format("Dictionnaire de {0:#,0} mots, chargé et trié.", Mots.Count));

            this.TravailEnCours = TravailEnCours.Aucun;
            chrono.Stop();
        }
       
        /// <summary>
        /// Parcours le DAWG pour en extraire tous les mots
        /// </summary>
        /// <returns>Liste contenant tous les mots</returns>
        public List<string> RetournerTouslesMots()
        {
            List<string> mots = new List<string>();

            foreach (Arc a in DAWG.Sortants)
                ParcoursDAWG(new List<Arc> { a }, mots);

            return mots;
        }

        /// <summary>
        /// Méthode récursive parcourant tous les noeuds du DAWG de façon à trouver chaque mot.
        /// </summary>
        /// <param name="LesMots">Liste de mots dans laquelle les mots trouvés sont ajoutés</param>
        /// <param name="Arcs">Liste des arcs du chemin en cours</param>
        private void ParcoursDAWG(List<Arc> Arcs, List<string> LesMots)
        {
            Noeud enCours = Arcs.Last().Destination;

            if (enCours.IsTerminal)
                LesMots.Add(Arc.RetourneMot(Arcs));

            foreach (Arc a in enCours.Sortants)
                ParcoursDAWG(Arcs.Concat(new List<Arc> { a }).ToList(), LesMots);
        }

        /// <summary>
        /// Charge le DAWG compressé dans un fichier.
        /// Ensuite compare la liste de mots avec celle du fichier ASCII
        /// </summary>
        /// <param name="FileName">Adresse du fichier compressé</param>
        /// <returns>DAWG</returns>
        public Noeud ChargerFichierDAWG()
        {
            TravailEnCours = Dawg.TravailEnCours.ChargementFichierDAWG;

            chrono.Restart();
            AnnonceEtape("Début du chargement du dictionnaire DAWG.");
            var assembly = Assembly.GetExecutingAssembly();
            string resourceName = assembly.GetManifestResourceNames().Single(str => str.EndsWith("dico_dawg.txt"));
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream, true))
            {
                string content = reader.ReadToEnd();
                string[] lignes = content.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                //string[] lignes = File.ReadAllLines(FileName);
                //le nombre de mots est présent dans le fichier par soucis de compatibilité avec le tutoriel de CArlVB, mais il n'est pas utile 
                NombreNoeuds = Convert.ToInt32(lignes[1].Split(':')[1]);

                /*On connait à l'avance le nombre de noeuds car c'est écrit en entête par soucis de compatibilité avec le tutoriel de CArlVB
                 *Cependant, on aurait pu le déduire à partir de lignes.Length
                 *Utiliser un tableau permet de regarder au bon index si le noeud a déjà été créé par un arc entrant ou s'il faut le faire
                 */
                Noeud[] noeuds = new Noeud[NombreNoeuds];

                for (int i = 0; i < NombreNoeuds; i++)
                {

                    if (noeuds[i] == null)//on vérifie si le noeud à déserializé a pas déjà été créé par un arc
                        noeuds[i] = new Noeud(i + 1);//on l'initialise si nécessaire

                    Noeud n = noeuds[i];

                    string ligne = lignes[i + 2];//on lit la ligne correspondante, il faut penser à sauter les 2 lignes d'entête

                    n.IsTerminal = ligne.EndsWith("#");

                    if (ligne != "#")//on exclu le cas particulier du noeud terminal sans enfant
                    {
                        //on désérialize les arcs sortants
                        string[] arcs = ligne.Replace("#", "").Split('-');

                        n.Sortants = (from a in arcs
                                      select new Arc(n, a, noeuds)
                                        ).ToList();
                    }
                }

                DAWG = noeuds[0];
                dawg = noeuds.ToList();

                AnnonceEtape("Fin du chargement du dictionnaire DAWG.");

                chrono.Stop();

                //==================Ici la lecture du fichier est finie

                //Cette étape n'est utile que pour la démo et le débug
                //ComparerListeMotsEtDAWG();

                TravailEnCours = Dawg.TravailEnCours.Aucun;

                return dawg[0];
            }
        }
        /// <summary>
        /// Vérifie la présence d'un mot dans le dictionnaire.
        /// </summary>
        /// <param name="mot">Mot à vérifier</param>
        /// <returns></returns>
        public List<string> AllWordsStartingWith(string mot)
        {
            List<string> ret = new List<string>();
            Noeud enCours = DAWG;
            List<Arc> arcs = new List<Arc>();

            for (int i = 0; i < mot.Length; i++)
            {
                List<Arc> sortants = enCours.Sortants.Where(a => a.Lettre == mot[i]).ToList();
                switch (sortants.Count)
                {
                    case 0:
                        return null;

                    case 1:
                        enCours = sortants[0].Destination;
                        arcs.Add(sortants[0]);
                        break;

                    default:
                        //il ne devrait jamais y avoir plus d'un arc pour une lettre sortant d'un noeud, si ça passe ici il y a un problème
                        AnnonceEtape(string.Format("Problème lors de la lecture du DAWG, le noeud n°{0} possède plusieurs arcs sortants avec la lettre {1}.", enCours.Numero, mot[i]));
                        break;

                }
            }
           
                    ParcoursDAWG(arcs, ret);
            return ret;
            
        }
        /// <summary>
        /// Vérifie la présence d'un mot dans le dictionnaire.
        /// </summary>
        /// <param name="Mot">Mot à vérifier</param>
        /// <returns></returns>
        public bool MotAdmis(string Mot)
        {

            Noeud enCours = DAWG;
            List<Arc> arcs = new List<Arc>();

            for (int i = 0; i < Mot.Length; i++)
            {
                List<Arc> sortants = enCours.Sortants.Where(a => a.Lettre == Mot[i]).ToList();
                switch (sortants.Count)
                {
                    case 0:
                        return false;

                    case 1:
                        enCours = sortants[0].Destination;
                        arcs.Add(sortants[0]);
                        break;

                    default:
                        //il ne devrait jamais y avoir plus d'un arc pour une lettre sortant d'un noeud, si ça passe ici il y a un problème
                        AnnonceEtape(string.Format("Problème lors de la lecture du DAWG, le noeud n°{0} possède plusieurs arcs sortants avec la lettre {1}.", enCours.Numero, Mot[i]));
                        break;

                }
            }


            return enCours.IsTerminal;
        }

        /// <summary>
        /// Ajoute un mot au dictionnaire
        /// </summary>
        /// <param name="Mot">Mot à ajouté</param>
        /// <remarks>On vérifie d'abord que le mot n'est pas déjà présent</remarks>
        public void AjouterUnMot(string Mot)
        {
            chrono.Restart();
            TravailEnCours = Dawg.TravailEnCours.AjoutMot;

            Mot = Mot.ToUpper();//au cas ou il soit en minuscule

            if (!MotAdmis(Mot))
            {
                Noeud aRejoinde = DAWG;
                string suffixe = Mot;

                List<Arc> prefixe = ExtrairePrefixeCommun(Mot, DAWG);
                int longueur = prefixe.Count;

                if (longueur > 0)//si un suffixe existe, on continue à partir de là
                {
                    AnnonceEtape(string.Format("Préfixe trouvé \"{0}\".", Arc.RetourneMot(prefixe)));
                    aRejoinde = prefixe.Last().Destination;
                    suffixe = Mot.Substring(longueur);
                }
                else
                    AnnonceEtape("Pas de préfixe trouvé");//théoriquement ça n'est pas possible, en effet, il n'existe aucune lettre qui ne commence aucun mot dans le dictionnaire ASCII

                List<Arc> arcsSuffixe = new List<Arc>();

                //On part du noeud terminal sans enfants et on remonte le graphe pour trouver le suffixe commun
                Noeud enCours = dawg.Single(n => n.Sortants.Count == 0 && n.IsTerminal);
                int i;
                for (i = suffixe.Length - 1; i > -1; i--)
                {
                    char lettre = suffixe[i];

                    Arc arc = enCours.Entrants.SingleOrDefault(a => a.Lettre == lettre && !a.Origine.IsTerminal && a.Origine.Sortants.Count == 1);

                    if (arc == null)//il n'y a plus de chemin possible
                        break;


                    arcsSuffixe.Insert(0, arc);
                    enCours = arc.Origine;
                }

                AnnonceEtape(string.Format("Suffixe trouvé \"{0}\".", Arc.RetourneMot(arcsSuffixe)));
                JoindreLesChemins(aRejoinde, enCours, suffixe.Take(i + 1).ToArray());

                Mots.Add(Mot);

                AnnonceEtape("Ecriture du nouveau fichier DAWG.");
                Noeud.Serialize(dawg, Mots.Count, "DawgEn2Temps.txt");
                AnnonceEtape("Mot ajouté et fichier enregistré.");
            }
            else
                AnnonceEtape(string.Format("Le mot \"{0}\" existe déja.", Mot));


            chrono.Stop();



            //==================Ici l'ajout du mot est fini

            //Cette étape n'est utile que pour la démo et le débug
            ComparerListeMotsEtDAWG();


            TravailEnCours = Dawg.TravailEnCours.Aucun;

        }

        /// <summary>
        /// Crée les arcs et les noeuds nécessaires à joindre le dernier noeud du préfixe commun au premier noeud du suffixe commun
        /// </summary>
        /// <param name="Prefixe">Dernier noeud du préfixe commum</param>
        /// <param name="Suffixe">Premier noeud du suffixe commun</param>
        /// <param name="ACreer">Lettres restant à créer</param>
        private void JoindreLesChemins(Noeud Prefixe, Noeud Suffixe, char[] ACreer)
        {
            Noeud enCours = Prefixe;

            for (int i = 0; i < ACreer.Length; i++)
            {
                char lettre = ACreer[i];
                Noeud destination;
                if (i == ACreer.Length - 1)
                    destination = Suffixe;
                else
                {
                    destination = new Noeud(NombreNoeuds++);
                    dawg.Add(destination);
                }
                enCours.Sortants.Add(new Arc(lettre, enCours, destination));
                enCours = destination;
            }
        }

        #endregion
    }

}
