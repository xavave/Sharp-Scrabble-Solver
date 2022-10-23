using Dawg;

using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Test_DAWG
{
    public partial class Form1 : Form
    {

        Dictionnaire dico = new Dictionnaire();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            dico.EtapeAtteinte += dico_EtapeAtteinte;
            dico.Progression += dico_Progression;
        }

        void dico_EtapeAtteinte(string Etape, long TempsExecution)
        {
            string texte = Etape + TempsExecution / 1000.0 + " secondes.";

            ListBox lst;

            switch (dico.TravailEnCours)
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
            switch (dico.TravailEnCours)
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
                  
                    dico.ChargerFichierDAWG(ofd.FileName);
                }
            }
        }

        private void butAjouterUnMot_Click(object sender, EventArgs e)
        {
            dico.AjouterUnMot(txtNvMot.Text.ToLower());
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
                    dicoLoaded = dico.ChargerDictionnaireAscii(ofd.FileName);
                }
            }
            btnCreationDawg.Enabled = dicoLoaded;
        }

        private void btnCreationDawg_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            dico.ConstruireDawgEnDeuxTemps();

            Cursor.Current = Cursors.Default;

        }
    }
}
