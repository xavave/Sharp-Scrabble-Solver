using System;
using System.Linq;
using System.Windows.Forms;

using DawgSolver;
using DawgSolver.Model;

namespace Dawg.Resolver.Winform.Test
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

            var foundWords = game.Resolver.FindMoves(game, 500, false, txtLetters.Text);

            lsbWords.DataSource = foundWords;
            lblNbWords.Text = $"{foundWords.Count} words found";
            Cursor.Current = Cursors.Default;
        }
    }
}
