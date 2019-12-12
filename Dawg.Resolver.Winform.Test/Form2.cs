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
        public Color Player1MoveColor { get; } = Color.LightYellow;
        public Color Player2MoveColor { get; } = Color.LightGreen;
        public Color HeaderTilesBackColor { get; } = Color.Black;
        public Color HeaderTilesForeColor { get; } = Color.WhiteSmoke;
        public Game Game { get; set; }
        public Form2()
        {
            InitializeComponent();
            NewGame();
        }

        private void NewGame()
        {
            txtGrid2.Visible = false;
            gbBoard.Controls.Clear();
            lsbInfos.Items.Clear();
            lblPlayer1Score.Text = lblPlayer2Score.Text = "";
            txtRackP1.Text = txtRackP2.Text = "";
            lsbWords.DisplayMember = "DisplayInList";
            Cursor.Current = Cursors.WaitCursor;
            Game = new Game();
            lsbWords.Items.Clear();
            txtBag.Text = Game.Bag.GetBagContent();
            txtGrid2.Text = Game.GenerateTextGrid(Game.Grid, true);
            CustomGroupBox.SuspendDrawing(gbBoard.Parent);
            for (int i = 0; i < Game.BoardSize; i++)
            {
                gbBoard.Controls.Add(new FormTile(this, Game, new Tile(Game, 0, i), $"header_col{i}", HeaderTilesBackColor) { Text = $"{i + 1}" });
                gbBoard.Controls.Add(new FormTile(this, Game, new Tile(Game, i, 0), $"header_ligne{i}", HeaderTilesBackColor) { Text = $"{Game.Alphabet[i].Char}" });
            }

            foreach (var tile in Game.Grid)
            {
                gbBoard.Controls.Add(new FormTile(this, Game, tile));
            }
            CustomGroupBox.ResumeDrawing(gbBoard.Parent);
            Cursor.Current = Cursors.Default;

        }

        private int PreviewWord(Player p, Word word, bool validateWord = false, bool addMove = true)
        {
            Game.ClearTilesInPlay(p);
            int points = word.SetWord(p, validateWord);
            if (validateWord || points > 0)
            {
                //word.Validate();
                foreach (var t in word.GetTiles())
                {
                    var frmTile = gbBoard.Controls.Find($"t{t.Ligne}_{t.Col}", false).First() as FormTile;
                    if (frmTile.ReadOnly)
                        continue;
                    frmTile.ReadOnly = true;
                    frmTile.BackColor = p == Game.Player1 ? Player1MoveColor : Player2MoveColor;

                    if (t.FromJoker)
                    {
                        frmTile.BorderColor = Color.Gold;
                        //Game.Bag.RemoveLetterFromBag(Game.Joker);
                    }

                    else
                    {
                        //Game.Bag.RemoveLetterFromBag(t.Letter.Char);
                    }
                }
                if (addMove)
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
                    var formTile = gbBoard.Controls.Find($"t{ligne}_{col}", false).First() as FormTile;
                    formTile.Tile = grid[ligne, col];
                    formTile.Text = formTile.Tile.Letter.Char.ToString();
                }
            }
            //CustomGroupBox.ResumeDrawing(groupBox1.Parent);
            //lsbInfos.Items.Add(Game.IsTransposed ? "Transposed" : "Not Transposed");
            txtGrid2.Text = Game.GenerateTextGrid(Game.Grid, true);
            txtRackP1.Text = Game.Player1.Rack.String();
            txtRackP2.Text = Game.Player2.Rack.String();
            txtBag.Text = Game.Bag.GetBagContent();
            DisplayScores();
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
            lsbInfos.Items.Insert(0, Game.IsTransposed ? "Transposed" : "Not Transposed");
            lsbInfos.Items.Insert(0, $"NbPossibleMoves={Game.Resolver.NbPossibleMoves}");
            lsbInfos.Items.Insert(0, $"NbAcceptedMoves={Game.Resolver.NbAcceptedMoves}");

            lsb.DataSource = ret;
            Cursor.Current = Cursors.Default;
        }

        private void btnDemo_Click(object sender, EventArgs e)
        {
            PlayDemo();
        }


        private void PlayDemo(int wait = 0)
        {

            if (Game.NoMoreMovesCount == 2)
            {
                Game.EndGame = true;
                return;
            }
            System.Windows.Forms.Application.DoEvents();

            this.BeginInvoke((Action)(() =>
            {
                try
                {
                    Cursor.Current = Cursors.WaitCursor;
                    var rack = Game.Bag.GetLetters(Game.IsPlayer1 ? Game.Player1 : Game.Player2);
                    if (!rack.Any())
                        lsbInfos.Items.Insert(0, "Le sac est vide !");
                    if (Game.IsPlayer1)
                    {
                        txtRackP1.Text = Game.Player1.Rack.String();
                    }
                    else
                    {
                        txtRackP2.Text = Game.Player2.Rack.String();
                    }
                    lblCurrentRack.Text = rack.String();

                    var ret = Game.Resolver.FindMoves(Game.IsPlayer1 ? Game.Player1 : Game.Player2, 30);
                    lsb.DataSource = ret;
                    if (ret.Any())
                    {
                        Game.NoMoreMovesCount = 0;
                        var word = ret.Where(w => !Game.Resolver.PlayedWords.Any(pw => pw.Equals(w))).OrderByDescending(r => r.Points).First() as Word;
                        if (CurrentWord != null && CurrentWord.Equals(word))
                        {
                            Game.EndGame = true;
                            return;
                        }
                        CurrentWord = word;
                        DisplayPlayerWords(word);
                        int points = PreviewWord(Game.IsPlayer1 ? Game.Player1 : Game.Player2, word, true);
                        if (Game.IsPlayer1)
                            Game.Player1.Points += points;
                        else
                            Game.Player2.Points += points;


                    }
                    else
                    {
                        Game.NoMoreMovesCount++;
                        if (Game.NoMoreMovesCount < 2)
                            lsbInfos.Items.Insert(0, $"{(Game.IsPlayer1 ? $"Player 1:{Game.Player1.Rack.String()}" : $"Player 2:{Game.Player2.Rack.String()}")} --> No words found !");
                        Game.IsPlayer1 = !Game.IsPlayer1;
                        return;
                    }
                    DisplayScores();
                }
                catch (ArgumentException ex)
                {
                    lsbInfos.Items.Insert(0, ex.Message);
                }
                finally
                {
                    Thread.Sleep(wait);
                    Cursor.Current = Cursors.Default;
                }
            }));


        }

        private void DisplayPlayerWords(Word word)
        {
            lsbWords.Items.Add(word);
        }

        private void DisplayScores()
        {
            lblPlayer1Score.Text = $"Player 1 Score :{Game.Player1.Points}";
            lblPlayer2Score.Text = $"Player 2 Score :{Game.Player2.Points}";
            var bestP1Move = Game.Player1.Moves.OrderByDescending(m => m.Points).FirstOrDefault();
            if (bestP1Move != null) lblP1BestPlay.Text = $"{bestP1Move}";
            var bestP2Move = Game.Player2.Moves.OrderByDescending(m => m.Points).FirstOrDefault();
            if (bestP2Move != null) lblP2BestPlay.Text = $"{bestP2Move}";
        }

        Word CurrentWord { get; set; }
        private void btnDemoAll_Click(object sender, EventArgs e)
        {
            if (!ckKeepExistingBoard.Checked)
                NewGame();

            while (!Game.EndGame)
            {
                PlayDemo(500);
            }
            ShowWinner(true);
        }

        private void ShowWinner(bool endGame = false)
        {
            if (endGame)
            {
                lsbInfos.Items.Insert(0, "Game Ended");
                bool player1Wins = Game.Player1.Points > Game.Player2.Points;
                lsbInfos.Items.Insert(0, $"{(player1Wins ? "Player 1" : "Player 2")} wins with a score of {(player1Wins ? Game.Player1.Points : Game.Player2.Points)}");
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
                var wordsCount = Math.Max(Game.Player1.Moves.Count, Game.Player2.Moves.Count);
                for (int i = 0; i < wordsCount; i++)
                {
                    if (i < Game.Player1.Moves.Count)
                    {
                        PreviewWord(Game.Player1, Game.Player1.Moves[i], true, false);
                        DisplayPlayerWords(Game.Player1.Moves[i]);
                    }
                    if (i < Game.Player2.Moves.Count)
                    {
                        PreviewWord(Game.Player2, Game.Player2.Moves[i], true, false);
                        DisplayPlayerWords(Game.Player2.Moves[i]);
                    }
                }
                RefreshBoard(Game.Grid);
            }
        }

        private void btnNewGame_Click(object sender, EventArgs e)
        {
            NewGame();
        }


        private void lsbWords_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (var c in gbBoard.Controls.OfType<FormTile>().Where(ft => ft.BackColor == Color.LightBlue))
                c.BackColor = Player1MoveColor;
            foreach (var c in gbBoard.Controls.OfType<FormTile>().Where(ft => ft.BackColor == Color.LightCoral))
                c.BackColor = Player2MoveColor;
            var word = lsbWords.SelectedItem as Word;
            if (word == null) return;


            bool isPlayer1word = Game.Player1.Moves.Contains(word);
            foreach (var t in word.GetTiles())
            {
                var frmTile = gbBoard.Controls.Find($"t{t.Ligne}_{t.Col}", false).First() as FormTile;

                frmTile.BackColor = isPlayer1word ? Color.LightBlue : Color.LightCoral;
            }
        }

        private void rbSize15_CheckedChanged(object sender, EventArgs e)
        {
            if (rbSize15.Checked)
                Game.BoardSize = 15;
            else
                Game.BoardSize = 11;
            NewGame();
        }

        private void ckShowGrid_CheckedChanged(object sender, EventArgs e)
        {
            txtGrid2.Visible = ckShowGrid.Checked;
        }
    }
}
