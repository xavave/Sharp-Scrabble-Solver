using System;
using System.Windows.Forms;

namespace Dawg.Solver.Winform
{
    public partial class WordsFinder : Form
    {
        Game game { get; }
        public WordsFinder(Game g)
        {
            InitializeComponent();
            game = g;

        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtLetters.Text)) return;
            Cursor.Current = Cursors.WaitCursor;

            var foundWords = Solver.DefaultInstance.FindMoves( 500, false, txtLetters.Text);

            lsbWords.DataSource = foundWords;
            lblNbWords.Text = $"{foundWords.Count} words found";
            Cursor.Current = Cursors.Default;
        }
    }
}
