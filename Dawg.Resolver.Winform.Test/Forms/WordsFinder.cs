using System;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;

using Dawg;
using DawgSolver.Model;

namespace Dawg.Solver.Winform
{
    /// <summary>
    /// Chercheur de mots indépendant du plateau : anagrammes d'un tirage (avec jokers '?')
    /// et/ou recherche par motif (lettres imposées, '?' = une case, '*' = plusieurs cases).
    /// Double-clic sur un mot = définition.
    /// </summary>
    public partial class WordsFinder : Form
    {
        Game game { get; }
        public WordsFinder(Game g)
        {
            InitializeComponent();
            game = g;
            ApplyTexts();
        }

        private void ApplyTexts()
        {
            Text = L.T("WF_Title");
            lblLetters.Text = L.T("WF_Letters");
            lblPattern.Text = L.T("WF_Pattern");
            btnSearch.Text = L.T("WF_Search");
            lblLength.Text = L.T("WF_Length");
            lblLengthHint.Text = L.T("WF_LengthAll");
            ckAllLetters.Text = L.T("WF_AllLetters");
            lblHelp.Text = L.T("WF_Help");
            var tips = new ToolTip { AutoPopDelay = 15000 };
            tips.SetToolTip(txtLetters, L.T("WF_TipLetters"));
            tips.SetToolTip(txtPattern, L.T("WF_TipPattern"));
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            var rack = txtLetters.Text.Trim();
            var pattern = txtPattern.Text.Trim();
            if (string.IsNullOrEmpty(rack) && string.IsNullOrEmpty(pattern))
            {
                lblNbWords.Text = L.T("WF_NeedInput");
                return;
            }
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                var alphabet = Solver.DefaultInstance.Alphabet;
                Func<char, int> value = c => alphabet.FirstOrDefault(a => a.Char == c)?.Value ?? 0;
                var sw = Stopwatch.StartNew();
                var found = WordSearch.Search(Dictionnaire.DefaultInstance.Legacy, rack, pattern,
                                              (int)nudWordLength.Value, ckAllLetters.Checked, value);
                sw.Stop();
                lsbWords.BeginUpdate();
                lsbWords.DataSource = null;
                lsbWords.DataSource = found;
                lsbWords.EndUpdate();
                lblNbWords.Text = L.F("WF_Found", found.Count, sw.ElapsedMilliseconds)
                                  + (found.Count >= WordSearch.MaxResults ? " " + L.T("WF_Truncated") : "");
                Logger.Log($"WordsFinder : tirage='{rack}' motif='{pattern}' long={nudWordLength.Value} tout={ckAllLetters.Checked} -> {found.Count} mots.");
            }
            catch (Exception ex)
            {
                Logger.LogException("WordsFinder.Search", ex);
                lblNbWords.Text = L.F("GenericError", ex.GetType().Name, ex.Message);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private void lsbWords_DoubleClick(object sender, EventArgs e)
        {
            if (lsbWords.SelectedItem is FoundWord w)
                Process.Start(new ProcessStartInfo($"https://1mot.net/{w.Text.ToLowerInvariant()}") { UseShellExecute = true });
        }
    }
}
