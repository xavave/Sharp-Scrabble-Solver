using DawgResolver;

using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Test_DAWG
{
    public partial class Form1 : Form
    {

        Dictionnaire dico;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            dico = new Dictionnaire();
            dico.EtapeAtteinte += dico_EtapeAtteinte;
            dico.Progression += dico_Progression;
            dico.ChargerDictionnaireAscii(Dictionnaire.NomDicoReel);
            grpLectureDAWG.Enabled = File.Exists(Dictionnaire.NomDicoDawgODS7);
            grpAjouterMot.Enabled = grpLectureDAWG.Enabled;
        }

        void dico_EtapeAtteinte(string Etape, long TempsExecution)
        {
            string texte = Etape + TempsExecution/1000.0 + " secondes.";

            ListBox lst;

            switch(dico.TravailEnCours)
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

        private void button1_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            dico.ConstruireDawgEnDeuxTemps();
            Cursor.Current = Cursors.Default;

        }

        void dico_Progression(int Pourcent)
        {
            switch (dico.TravailEnCours)
            {
                case TravailEnCours.ChargementFichierASCII:
                    break;

                case TravailEnCours.CreationDawgEn2Etapes:
                    pgb2etapes.Value = Pourcent;
                    grpLectureDAWG.Enabled = (Pourcent == 100);
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

        private void butLectureDAWG_Click(object sender, EventArgs e)
        {
            dico.ChargerFichierDAWG();
        }

        private void butAjouterUnMot_Click(object sender, EventArgs e)
        {
            dico.AjouterUnMot(txtNvMot.Text.ToLower());
        }

        private void button2_Click(object sender, EventArgs e)
        {
           var words = File.ReadAllLines(Dictionnaire.NomDicoReel).OrderBy(m => m).ToList();
            File.WriteAllLines(Dictionnaire.NomDicoReel, words);
        }
    }
}
