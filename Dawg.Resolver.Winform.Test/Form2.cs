using DawgResolver;
using DawgResolver.Model;
using System;
using System.Collections.Generic;
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
            NewGame();
        }

        private void NewGame()
        {
            EndGame = false;
            Cursor.Current = Cursors.WaitCursor;
            Game = new Game();
            Game.InitBoard();
            textBox3.Text = Game.Bag.GetBagContent();
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


        private int PreviewWord(Player p, Word word, bool validateWord = false)
        {
            Game.ClearTilesInPlay(p);
            int points = word.SetWord(p, validateWord);
            if (validateWord)
            {
                word.Validate();
                foreach (var t in word.GetTiles())
                {
                    var frmTile = groupBox1.Controls.Find($"t{t.Ligne}_{t.Col}", false).First() as FormTile;
                    if (frmTile.ReadOnly)
                        continue;
                    frmTile.ReadOnly = true;
                    frmTile.BackColor = p == Game.Player1 ? Color.LightYellow : Color.LightGreen;
                    Game.Bag.RemoveLetterFromBag(t.Letter);
                }
                isPlayer1 = !isPlayer1;

            }
            else return 0;
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
            txtRackP1.Text = Game.Player1.Rack.String();
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
            if (ret.Any()) txtRackP1.Text = ret.String();
            Game.Grid = RefreshBoard(Game.Grid);
        }

        private void btnValidate_Click(object sender, EventArgs e)
        {
            //if (ValidateWord(isPlayer1 ? Game.Player1 : Game.Player2))
            //    DisplayScores();

        }

        //private bool ValidateWord(Player p)
        //{
        //    var validate = new List<VTile>();
        //    foreach (var t in Game.Grid)
        //        if (!t.IsValidated)
        //        {
        //            validate.Add(t);
        //            t.IsValidated = true;
        //        }

        //    if (!validate.Any())
        //        return false;
        //    foreach (var t in validate)
        //    {
        //        var frmTile = groupBox1.Controls.Find($"t{t.Ligne}_{t.Col}", false).First() as FormTile;
        //        frmTile.ReadOnly = true;
        //        frmTile.BackColor = p == Game.Player1 ? Color.LightYellow : Color.LightGreen;
        //        Game.Bag.RemoveLetterFromBag(t.Letter);
        //    }

        //    p.Points += isPlayer1 ? player1Score : player2Score;
        //    isPlayer1 = !isPlayer1;
        //    Game.Grid = RefreshBoard(Game.Grid);
        //    return true;
        //}

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtRackP1.Text) || txtRackP1.Text.Any(c => !Game.AlphabetAvecJoker.Any(ch => c == ch.Char))) return;
            Cursor.Current = Cursors.WaitCursor;
            Game.Bag.GetLetters(Game.Player1, txtRackP1.Text.Trim());
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
        bool EndGame { get; set; } = false;
        private void PlayDemo(int wait = 0)
        {

            System.Windows.Forms.Application.DoEvents();
            Thread.Sleep(wait);
            this.BeginInvoke((Action)(() =>
            {

                Cursor.Current = Cursors.WaitCursor;
                var rack = Game.Bag.GetLetters(isPlayer1 ? Game.Player1 : Game.Player2, isPlayer1 ? txtRackP1.Text : txtRackP2.Text);
                if (isPlayer1)
                    txtRackP1.Text = rack.String();
                else
                    txtRackP2.Text = rack.String();
                var ret = Game.Resolver.FindMoves(isPlayer1 ? Game.Player1 : Game.Player2, 50);
                lsb.DataSource = ret;
                if (ret.Any())
                {
                    var word = ret.OrderByDescending(r => r.Points).First() as Word;
                    if (CurrentWord != null && CurrentWord.Equals(word))
                    {
                        EndGame = true;
                        return;
                    }
                    CurrentWord = word;
                    txtTileProps.Text += $"{(isPlayer1 ? $"Player 1:{Game.Player1.Rack.String()}" : $"Player 2:{Game.Player2.Rack.String()}")} --> {word.Text} ({word.Points})" + Environment.NewLine;
                    int points = PreviewWord(isPlayer1 ? Game.Player1 : Game.Player2, word, true);
                    if (isPlayer1) Game.Player1.Points += points;
                    else
                        Game.Player2.Points += points;

                    DisplayScores();

                    Cursor.Current = Cursors.Default;
                }
                else
                {
                    isPlayer1 = !isPlayer1;
                    return;
                }
            }));

        }

        private void DisplayScores()
        {
            lblPlayer1Score.Text = $"Player 1 Score :{Game.Player1.Points}";
            lblPlayer2Score.Text = $"Player 2 Score :{Game.Player2.Points}";
        }

        Word CurrentWord { get; set; }
        private void btnDemoAll_Click(object sender, EventArgs e)
        {
           

            NewGame();

            while (Game.Bag.LeftLettersCount > 0 && !EndGame)
                PlayDemo(500);
        }

        private void lsb_Click(object sender, EventArgs e)
        {
            var word = lsb.SelectedItem as Word;
            if (word == null) return;
            if (isPlayer1)
                player1Score = PreviewWord(isPlayer1 ? Game.Player1 : Game.Player2, word);
            else
                player2Score = PreviewWord(isPlayer1 ? Game.Player1 : Game.Player2, word);
        }
    }
}
