using DawgResolver;
using DawgResolver.Model;
using System;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Dawg.Resolver.Winform.Test
{
    public partial class Form2 : Form
    {
        public Game Game { get; set; }
        public Form2()
        {
            InitializeComponent();
            Cursor.Current = Cursors.WaitCursor;
            Game = new Game();
            Game.InitBoard();
            textBox3.Text = Game.Bag.GetBagContent();
            var t = Game.Grid[4, 7];
            //t.SetWord(Game.Player1, "famille", MovementDirection.Down, true);
            //t.SetWord(Game.Player1, "foin", MovementDirection.Across, true);
            txtGrid2.Text = Game.GenerateTextGrid(Game.Grid, true);
            CustomGroupBox.SuspendDrawing(groupBox1.Parent);
            for (int i = 0; i < 15; i++)
            {
                groupBox1.Controls.Add(new FormTile(Game, new Tile(Game, 0, i), $"header_col{i}") { Text = $"{i + 1}" });
                groupBox1.Controls.Add(new FormTile(Game, new Tile(Game, i, 0), $"header_ligne{i}") { Text = $"{Game.Alphabet[i].Char}" });
            }

            foreach (var tile in Game.Grid)
            {
                groupBox1.Controls.Add(new FormTile(Game, tile));
            }
            CustomGroupBox.ResumeDrawing(groupBox1.Parent);
            Cursor.Current = Cursors.Default;
        }

        int player1Score = 0;
        int player2Score = 0;
        private void lsb_SelectedIndexChanged(object sender, EventArgs e)
        {
            var word = lsb.SelectedItem as Word;
            if (word == null) return;
            if (isPlayer1)
                player1Score = PlayWord(isPlayer1 ? Game.Player1 : Game.Player2, word);
            else
                player2Score = PlayWord(isPlayer1 ? Game.Player1 : Game.Player2, word);


        }

        private int PlayWord(Player p, Word word)
        {
            Game.ClearTilesInPlay(p);
            int points = word.SetWord(p, false);

            RefreshBoard(Game.Grid);
            return points;
        }

        private VTile[,] RefreshBoard(VTile[,] grid)
        {
            //CustomGroupBox.SuspendDrawing(groupBox1.Parent);
            for (int ligne = 0; ligne <= grid.GetUpperBound(0); ligne++)
            {
                for (int col = 0; col <= grid.GetUpperBound(1); col++)
                {
                    var formTile = groupBox1.Controls.Find($"t{ligne}_{col}", false).First() as FormTile;
                    formTile.Tile = grid[ligne, col];
                    formTile.Text = formTile.Tile.Letter.Char.ToString();
                }
            }
            //CustomGroupBox.ResumeDrawing(groupBox1.Parent);
            //txtTileProps.Text += Game.IsTransposed ? "Transposed" : "Not Transposed";
            //txtGrid2.Text = Game.GenerateTextGrid(Game.Grid, true);
            txtRack.Text = Game.Player1.Rack.String();
            textBox3.Text = Game.Bag.GetBagContent();
            return grid;
        }

        private void btnTranspose_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            Game.Grid = Game.Grid.Transpose(Game);

            Game.Grid = RefreshBoard(Game.Grid);
            Cursor.Current = Cursors.Default;
        }

        private void btnBackToRack_Click(object sender, EventArgs e)
        {
            var ret = Game.ClearTilesInPlay(Game.Player1);
            if (ret.Any()) txtRack.Text = ret.String();
            Game.Grid = RefreshBoard(Game.Grid);
        }

        private void btnValidate_Click(object sender, EventArgs e)
        {
            if (ValidateWord(isPlayer1 ? Game.Player1 : Game.Player2))
                DisplayScores();

        }

        private bool ValidateWord(Player p)
        {
            var validate = Game.ValidateWords();

            if (!validate.Any())
                return false;
            foreach (var t in validate)
            {
                var frmTile = groupBox1.Controls.Find($"t{t.Ligne}_{t.Col}", false).First() as FormTile;
                frmTile.ReadOnly = true;
                frmTile.BackColor = p == Game.Player1 ? Color.LightYellow : Color.LightGreen;
                Game.Bag.GetLetterInFlatList(t.Letter.Char);
            }


            var newRack = Game.Bag.GetNewRack(Game.Player1);
            if (newRack.Any())
                txtRack.Text = newRack.String();

            p.Points += isPlayer1 ? player1Score : player2Score;
            isPlayer1 = !isPlayer1;
            Game.Grid = RefreshBoard(Game.Grid);
            return true;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtRack.Text) || txtRack.Text.Any(c => !Game.AlphabetAvecJoker.Any(ch => c == ch.Char))) return;
            Cursor.Current = Cursors.WaitCursor;
            Game.Bag.GetNewRack(Game.Player1, txtRack.Text.Trim());
            lsb.DisplayMember = "DisplayText";
            var ret = Game.Resolver.FindMoves(Game.Player1, 100);
            txtTileProps.Text = Game.IsTransposed ? "Transposed" : "Not Transposed";
            txtTileProps.Text += Environment.NewLine;
            txtTileProps.Text += $"NbPossibleMoves={Game.Resolver.NbPossibleMoves}";
            txtTileProps.Text += Environment.NewLine;
            txtTileProps.Text += $"NbAcceptedMoves={Game.Resolver.NbAcceptedMoves}";
            txtTileProps.Text += Environment.NewLine;
            lsb.DataSource = ret;
            Cursor.Current = Cursors.Default;
        }

        private void btnDemo_Click(object sender, EventArgs e)
        {

            PlayDemo();
        }
        bool isPlayer1 = true;

        private void PlayDemo()
        {

            Cursor.Current = Cursors.WaitCursor;

            this.BeginInvoke((Action)(() =>
            {
                Cursor.Current = Cursors.WaitCursor;
                var rack = Game.Bag.GetNewRack(isPlayer1 ? Game.Player1 : Game.Player2);
                txtRack.Text = rack.String();
                var ret = Game.Resolver.FindMoves(isPlayer1 ? Game.Player1 : Game.Player2, 50);
                lsb.DataSource = ret;
                if (ret.Any())
                {

                    var word = ret.OrderByDescending(r => r.Points).First() as Word;

                    int score = PlayWord(isPlayer1 ? Game.Player1 : Game.Player2, word);

                    txtTileProps.Text += $"{word.Text} ({word.Points})" + Environment.NewLine;
                    if (ValidateWord(isPlayer1 ? Game.Player1 : Game.Player2))
                        if (isPlayer1)
                            Game.Player1.Points += score;
                        else
                            Game.Player2.Points += score;

                    DisplayScores();

                    Cursor.Current = Cursors.Default;
                }
                else return;
            }));

        }

        private void DisplayScores()
        {
            lblPlayer1Score.Text = $"Player 1 Score :{Game.Player1.Points}";
            lblPlayer2Score.Text = $"Player 2 Score :{Game.Player2.Points}";
        }
    }
}
