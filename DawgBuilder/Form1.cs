using Dawg;

using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Test_DAWG
{
    public partial class Form1 : Form
    {


        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Dictionnaire.DefaultInstance.EtapeAtteinte += dico_EtapeAtteinte;
            Dictionnaire.DefaultInstance.Progression += dico_Progression;
        }

        void dico_EtapeAtteinte(string Etape, long TempsExecution)
        {
            string texte = Etape + TempsExecution / 1000.0 + " secondes.";

            ListBox lst;

            switch (Dictionnaire.DefaultInstance.TravailEnCours)
            {
                case TravailEnCours.ChargementFichierASCII:
                    lst = lstChargementASCII;
                    break;

                case TravailEnCours.CreationDawgEn2Etapes:
                    lst = lstAvancement2Etapes;
                    break;

                case TravailEnCours.ChargementFichierDAWG:
                    lst = lstLectureDAWG;
                    break;

                case TravailEnCours.AjoutMot:
                    lst = lstAjoutMot;
                    break;

                default:
                    return;
            }

            lst.Items.Add(texte);
            lst.SelectedIndex = lst.Items.Count - 1;
        }

        void dico_Progression(int Pourcent)
        {
            switch (Dictionnaire.DefaultInstance.TravailEnCours)
            {
                case TravailEnCours.ChargementFichierASCII:
                    break;

                case TravailEnCours.CreationDawgEn2Etapes:
                    pgb2etapes.Value = Pourcent;
                    grpAjouterMot.Enabled = grpLectureDAWG.Enabled;
                    break;

                case TravailEnCours.ChargementFichierDAWG:
                    break;

                case TravailEnCours.AjoutMot:
                    break;

                default:
                    return;
            }
        }

        private void btnLectureDAWG_Click(object sender, EventArgs e)
        {
            using (var ofd = new OpenFileDialog() { CheckFileExists = true, RestoreDirectory = true })
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {

                    Dictionnaire.DefaultInstance.ChargerFichierDAWG(ofd.FileName);
                }
            }
        }

        private void butAjouterUnMot_Click(object sender, EventArgs e)
        {
            Dictionnaire.DefaultInstance.AjouterUnMot(txtNvMot.Text.ToLower());
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (!File.Exists(Dictionnaire.NomDicoReel)) return;
            var words = File.ReadAllLines(Dictionnaire.NomDicoReel).OrderBy(m => m).ToList();
            File.WriteAllLines(Dictionnaire.NomDicoReel, words);
        }

        private void btnReadDicoText_Click(object sender, EventArgs e)
        {
            bool dicoLoaded = false;
            using (var ofd = new OpenFileDialog() { CheckFileExists = true, RestoreDirectory = true })
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    Dictionnaire.NomDicoReel = ofd.FileName;
                    

                    grpLectureDAWG.Enabled = true;
                    grpAjouterMot.Enabled = grpLectureDAWG.Enabled;
                    dicoLoaded = Dictionnaire.DefaultInstance.ChargerDictionnaireAscii(ofd.FileName);
                }
            }
            btnCreationDawg.Enabled = dicoLoaded;
        }

        private void btnCreationDawg_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Dictionnaire.NomDicoReel) || !File.Exists(Dictionnaire.NomDicoReel))
            {
                MessageBox.Show("Chargez d'abord une liste de mots (bouton de lecture du dico texte).");
                return;
            }

            // Choix du fichier DAWG de sortie (nom par défaut dérivé de la liste chargée,
            // ou dico_dawgODS8.txt pour l'ODS8).
            using (var sfd = new SaveFileDialog()
            {
                Filter = "Fichier DAWG|*.txt",
                DefaultExt = "txt",
                RestoreDirectory = true,
                FileName = "dico_dawgODS8.txt",
                InitialDirectory = Path.GetDirectoryName(Dictionnaire.NomDicoReel)
            })
            {
                if (sfd.ShowDialog() != DialogResult.OK) return;
                Dictionnaire.NomDicoDawg = sfd.FileName; // chemin complet -> le DAWG sera écrit ici
            }

            Cursor.Current = Cursors.WaitCursor;
            // La construction tourne sur un thread dédié ; l'avancement s'affiche dans les listes,
            // et le chemin final est journalisé à la fin ("Fichier DAWG écrit : ...").
            Dictionnaire.DefaultInstance.ConstruireDawgEnDeuxTemps();
            Cursor.Current = Cursors.Default;
        }
    }
}
