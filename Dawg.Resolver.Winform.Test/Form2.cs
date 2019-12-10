using DawgResolver;
using DawgResolver.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
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
            groupBox1.Controls.Clear();
            lsbInfos.Items.Clear();
            lblPlayer1Score.Text = lblPlayer2Score.Text = "";
            txtRackP1.Text = txtRackP2.Text = "";
           
            Cursor.Current = Cursors.WaitCursor;
            Game = new Game();
            Game.InitBoard();
            textBox3.Text = Game.Bag.GetBagContent();
            txtGrid2.Text = Game.GenerateTextGrid(Game.Grid, true);
            CustomGroupBox.SuspendDrawing(groupBox1.Parent);
            for (int i = 0; i < 15; i++)
            {
                groupBox1.Controls.Add(new FormTile(Game, new Tile(Game, 0, i), $"header_col{i}", Color.Gold) { Text = $"{i + 1}" });
                groupBox1.Controls.Add(new FormTile(Game, new Tile(Game, i, 0), $"header_ligne{i}", Color.Gold) { Text = $"{Game.Alphabet[i].Char}" });
            }

            foreach (var tile in Game.Grid)
            {
                groupBox1.Controls.Add(new FormTile(Game, tile));
            }
            CustomGroupBox.ResumeDrawing(groupBox1.Parent);
            Cursor.Current = Cursors.Default;

        }

        private int PreviewWord(Player p, Word word, bool validateWord = false)
        {
            Game.ClearTilesInPlay(p);
            int points = word.SetWord(p, validateWord);
            if (validateWord || points > 0)
            {
                //word.Validate();
                foreach (var t in word.GetTiles())
                {
                    var frmTile = groupBox1.Controls.Find($"t{t.Ligne}_{t.Col}", false).First() as FormTile;
                    if (frmTile.ReadOnly)
                        continue;
                    frmTile.ReadOnly = true;
                    frmTile.BackColor = p == Game.Player1 ? Color.LightYellow : Color.LightGreen;

                    if (t.FromJoker)
                        Game.Bag.RemoveLetterFromBag(Game.Joker);
                    else
                        Game.Bag.RemoveLetterFromBag(t.Letter.Char);
                }
                p.Moves.Add(word);
                Game.Resolver.PlayedWords.Add(word);
                Game.IsPlayer1 = !Game.IsPlayer1;

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
            txtRackP2.Text = Game.Player2.Rack.String();
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
            var word = lsb.SelectedItem as Word;
            if (word == null) return;
            if (Game.IsPlayer1)
                PreviewWord(Game.Player1, word, true);
            else
                PreviewWord(Game.Player2, word, true);

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
            lsbInfos.Items.Add(Game.IsTransposed ? "Transposed" : "Not Transposed");
            lsbInfos.Items.Add($"NbPossibleMoves={Game.Resolver.NbPossibleMoves}");
            lsbInfos.Items.Add($"NbAcceptedMoves={Game.Resolver.NbAcceptedMoves}");

            lsb.DataSource = ret;
            Cursor.Current = Cursors.Default;
        }

        private void btnDemo_Click(object sender, EventArgs e)
        {
            PlayDemo();
        }
      
        int NoMoreMovesCount { get; set; } = 0;
        private void PlayDemo(int wait = 0)
        {

            if (NoMoreMovesCount >= 2)
            {
                Game.EndGame = true;
                return;
            }
            System.Windows.Forms.Application.DoEvents();
            Thread.Sleep(wait);
            this.BeginInvoke((Action)(() =>
            {
                try
                {
                    Cursor.Current = Cursors.WaitCursor;
                    var rack = Game.Bag.GetLetters(Game.IsPlayer1 ? Game.Player1 : Game.Player2);

                    if (Game.IsPlayer1)
                    {
                        txtRackP1.Text = rack.String();
                    }
                    else
                    {
                        txtRackP2.Text = rack.String();
                    }
                    lblCurrentRack.Text = rack.String();

                    var ret = Game.Resolver.FindMoves(Game.IsPlayer1 ? Game.Player1 : Game.Player2, 50);
                    lsb.DataSource = ret;
                    if (ret.Any())
                    {
                        NoMoreMovesCount = 0;
                        var word = ret.Where(w => !Game.Resolver.PlayedWords.Any(pw => pw.Equals(w))).OrderByDescending(r => r.Points).First() as Word;
                        if (CurrentWord != null && CurrentWord.Equals(word))
                        {
                            Game.EndGame = true;
                            return;
                        }
                        CurrentWord = word;
                        lsbInfos.Items.Add($"{(Game.IsPlayer1 ? $"Player 1:{Game.Player1.Rack.String()}" : $"Player 2:{Game.Player2.Rack.String()}")} --> {word.DisplayText}");
                        int points = PreviewWord(Game.IsPlayer1 ? Game.Player1 : Game.Player2, word, true);
                        if (Game.IsPlayer1)
                            Game.Player1.Points += points;
                        else
                            Game.Player2.Points += points;

                        DisplayScores();

                        Cursor.Current = Cursors.Default;
                    }
                    else
                    {
                        NoMoreMovesCount++;
                        lsbInfos.Items.Add($"{(Game.IsPlayer1 ? $"Player 1:{Game.Player1.Rack.String()}" : $"Player 2:{Game.Player2.Rack.String()}")} --> No words found !");
                        Game.IsPlayer1 = !Game.IsPlayer1;
                        return;
                    }
                }
                catch (ArgumentException ex)
                {
                    MessageBox.Show(ex.Message);

                    ShowWinner(true);
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
            if (!ckKeepExistingBoard.Checked)
                NewGame();

            while (Game.Bag.LeftLettersCount > 0 && !Game.EndGame)
            {
                PlayDemo(500);
            }
            ShowWinner(Game.EndGame);
        }

        private void ShowWinner(bool endGame = false)
        {
            if (endGame)
            {
                lsbInfos.Items.Add("Game Ended");
                bool player1Wins = Game.Player1.Points > Game.Player2.Points;
                lsbInfos.Items.Add($"{(player1Wins ? "Player 1" : "Player 2")} wins with a score of {(player1Wins ? Game.Player1.Points : Game.Player2.Points)}");
            }
        }

        private void lsb_Click(object sender, EventArgs e)
        {
            var word = lsb.SelectedItem as Word;
            if (word == null) return;
            if (Game.IsPlayer1)
                PreviewWord(Game.Player1, word);
            else
                PreviewWord(Game.Player2, word);
        }

        private void btnSaveGame_Click(object sender, EventArgs e)
        {
            var sfd = saveFileDialog1.ShowDialog();
            if (sfd == DialogResult.OK)
            {
                var ret = Game.Serialise();
                File.WriteAllText(saveFileDialog1.FileName, ret);
                MessageBox.Show($"Game saved as {saveFileDialog1.FileName}");
            }
        }

        private void btnLoadGame_Click(object sender, EventArgs e)
        {
            var ofd = openFileDialog1.ShowDialog();
            if (ofd == DialogResult.OK)
            {
                var txt = File.ReadAllText(openFileDialog1.FileName);
                Game.Deserialize(txt);
                
            }
        }
    }
}
