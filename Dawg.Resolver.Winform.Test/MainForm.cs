using DawgResolver;
using DawgResolver.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Dawg.Resolver.Winform.Test
{
    public partial class MainForm : Form
    {
        public Color Player1MoveColor { get; } = Color.LightYellow;
        public Color Player2MoveColor { get; } = Color.LightGreen;
        public Color HeaderTilesBackColor { get; } = Color.Black;
        public Color HeaderTilesForeColor { get; } = Color.WhiteSmoke;
        public Game Game { get; set; }
        public FormTile LastPlayedTile { get; set; }

        public MainForm()
        {
            InitializeComponent();
            NewGame();
        }

        private void NewGame()
        {
            DawgResolver.Model.Game.IsTransposed = false;
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
            RefreshGridTiles(false);
            Cursor.Current = Cursors.Default;

        }

        private void RefreshGridTiles(bool initBoard = true)
        {
            if (initBoard) Game.InitBoard();
            //CustomGroupBox.SuspendDrawing(gbBoard.Parent);
            bool hasHeaderTile = gbBoard.Controls.Count > 0;
            FormTile frmTile = null;
            for (int i = 0; i < Game.BoardSize; i++)
            {
                if (hasHeaderTile)
                {
                    frmTile = gbBoard.Controls.Find($"header_col{i}", false).First() as FormTile;
                }
                if (frmTile == null)
                    gbBoard.Controls.Add(new FormTile(this, Game, new Tile(Game, 0, i), $"header_col{i}", HeaderTilesBackColor) { Text = $"{i + 1}" });
                else
                    frmTile = new FormTile(this, Game, new Tile(Game, 0, i), $"header_col{i}", HeaderTilesBackColor) { Text = $"{i + 1}" };

                if (hasHeaderTile)
                {
                    frmTile = gbBoard.Controls.Find($"header_ligne{i}", false).First() as FormTile;
                }
                if (frmTile == null)
                    gbBoard.Controls.Add(new FormTile(this, Game, new Tile(Game, i, 0), $"header_ligne{i}", HeaderTilesBackColor) { Text = $"{Game.Alphabet[i].Char}" });
                else
                    frmTile = new FormTile(this, Game, new Tile(Game, i, 0), $"header_ligne{i}", HeaderTilesBackColor) { Text = $"{Game.Alphabet[i].Char}" };
            }

            foreach (var tile in Game.Grid)
            {
                frmTile = new FormTile(this, Game, tile);
                frmTile.Text = tile.Letter?.Char.ToString();
                var existingTile = hasHeaderTile ? gbBoard.Controls.Find($"t{frmTile.Ligne}_{frmTile.Col}", false).First() as FormTile : null;
                if (existingTile == null)
                    gbBoard.Controls.Add(frmTile);
                else
                {
                    gbBoard.Controls.RemoveByKey(frmTile.Name);
                    gbBoard.Controls.Add(frmTile);

                }
            }
            //CustomGroupBox.ResumeDrawing(gbBoard.Parent);
        }

        public int PreviewWord(Player p, Word word, bool validateWord = false, bool addMove = true)
        {
            ClearTilesInPlay(p);
            int points = word.SetWord(p, validateWord);
            if (validateWord || points > 0)
            {
                //word.Validate();
                foreach (var t in word.GetTiles())
                {
                    var frmTile = gbBoard.Controls.Find($"t{t.Ligne}_{t.Col}", false).First() as FormTile;
                    if (frmTile.IsEmpty)
                        frmTile.BackColor = frmTile.GetBackColor(t);
                    else
                    {
                        frmTile.Tile.IsPlayedByPlayer1 = p == Game.Player1;
                        this.BeginInvoke((Action)(() =>
                        {
                            frmTile.BackColor = p == Game.Player1 ? Player1MoveColor : Player2MoveColor;
                        }));
                    }

                    if (!frmTile.Enabled)
                        continue;

                    this.BeginInvoke((Action)(() =>
                    {
                        frmTile.Text = Game.Grid[t.Ligne, t.Col].Letter?.Char.ToString();
                    }));
                    if (t.FromJoker)
                    {
                        frmTile.BorderColor = Color.Gold;
                        if (Game.IsPlayer1)
                            Game.Player1.Rack.Remove(Game.GameStyle == 'S' ? Game.AlphabetScrabbleAvecJoker[26] : Game.AlphabetWWFAvecJoker[26]);
                        else
                            Game.Player2.Rack.Remove(Game.GameStyle == 'S' ? Game.AlphabetScrabbleAvecJoker[26] : Game.AlphabetWWFAvecJoker[26]);

                    }

                    else
                    {
                        if (Game.IsPlayer1)
                            Game.Player1.Rack.Remove(Game.Grid[t.Ligne, t.Col].Letter);
                        else
                            Game.Player2.Rack.Remove(Game.Grid[t.Ligne, t.Col].Letter);
                    }

                    this.BeginInvoke((Action)(() =>
                    {
                        frmTile.Enabled = false;
                    }));
                }
                if (addMove)
                {
                    p.Moves.Add(word);
                    Game.Resolver.PlayedWords.Add(word);
                    DisplayPlayerWord(word);
                }
                if (validateWord)
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
                    formTile.Text = formTile.Tile?.Letter?.Char.ToString();
                    formTile.Enabled = !formTile.Tile.IsValidated;

                }
            }
            //CustomGroupBox.ResumeDrawing(groupBox1.Parent);
            //lsbInfos.Items.Add(Game.IsTransposed ? "Transposed" : "Not Transposed");
            txtGrid2.Text = Game.GenerateTextGrid(Game.Grid, true);
            //txtRackP1.Text = Game.Player1.Rack.String();
            //txtRackP2.Text = Game.Player2.Rack.String();
            this.BeginInvoke((Action)(() =>
            {
                txtBag.Text = Game.Bag.GetBagContent();
            }));
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
        private List<VTile> ClearTilesInPlay(Player p)
        {
            var ret = Game.Grid.OfType<VTile>().Where(t => !t.IsValidated && !t.IsEmpty).ToList();
            foreach (var tile in ret)
            {
                var frmTile = gbBoard.Controls.Find($"t{tile.Ligne}_{tile.Col}", false).First() as FormTile;
                this.BeginInvoke((Action)(() =>
                {
                    frmTile.Text = "";
                    frmTile.Tile.Letter = new Letter();
                    frmTile.BackColor = frmTile.GetBackColor(tile);
                }));
            }
            //for (int i = 0; i < Grid.LongLength; i++)
            //{
            //    var tile = Game.Grid.OfType<VTile>().ElementAt(i);
            //    if (!tile.IsValidated)
            //    {
            //        if (!tile.IsEmpty)
            //        {
            //            if (tile.FromJoker)
            //            {
            //                p.Rack.Add(GameStyle == 'S' ? AlphabetScrabbleAvecJoker[26] : Game.AlphabetWWFAvecJoker[26]);
            //            }
            //            else
            //            {
            //                p.Rack.Add(tile.Letter);
            //            }
            //            tile.IsValidated = true;
            //        }
            //        else
            //            Grid[tile.Ligne, tile.Col].Letter = new Letter();

            //    }
            //}
            return ret;
        }
        private void btnBackToRack_Click(object sender, EventArgs e)
        {
            //var ret = ClearTilesInPlay(Game.Player1);
            //if (ret.Any()) txtRackP1.Text = ret.String();
            //Game.Grid = RefreshBoard(Game.Grid);
        }

        private void btnValidate_Click(object sender, EventArgs e)
        {

            if (LastPlayedTile == null) return;
            var word = LastPlayedTile.Tile.GetWordFromTile(Game.CurrentWordDirection);
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
            if (string.IsNullOrWhiteSpace(txtRackP1.Text)) return;// || txtRackP1.Text.Any(c => !Game.AlphabetAvecJoker.Any(ch => c == ch.Char))) return;
            Cursor.Current = Cursors.WaitCursor;
            Game.Bag.GetLetters(Game.Player1, txtRackP1.Text.Trim());
            lsbHintWords.DisplayMember = "DisplayText";
            var ret = Game.Resolver.FindMoves(Game.Player1, 100, Game.HintSortByBestScore);
            lsbInfos.Items.Insert(0, Game.IsTransposed ? "Transposed" : "Not Transposed");
            lsbInfos.Items.Insert(0, $"NbPossibleMoves={Game.Resolver.NbPossibleMoves}");
            lsbInfos.Items.Insert(0, $"NbAcceptedMoves={Game.Resolver.NbAcceptedMoves}");

            lsbHintWords.DataSource = ret;
            Cursor.Current = Cursors.Default;
        }

        private async void btnDemo_Click(object sender, EventArgs e)
        {
            await PlayDemo();
        }


        private async Task PlayDemo(int wait = 0)
        {

            if (Game.NoMoreMovesCount >= 2)
            {
                Game.EndGame = true;
                return;
            }
            System.Windows.Forms.Application.DoEvents();
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                var rack = Game.Bag.GetLetters(Game.IsPlayer1 ? Game.Player1 : Game.Player2);
                if (!rack.Any()) this.BeginInvoke((Action)(() =>
                {
                    lsbInfos.Items.Insert(0, "Le sac est vide !");
                }));
                if (Game.IsPlayer1)
                {
                    this.BeginInvoke((Action)(() =>
                    {
                        txtRackP1.Text = Game.Player1.Rack.String();
                    }));
                }
                else
                {
                    this.BeginInvoke((Action)(() =>
                    {
                        txtRackP2.Text = Game.Player2.Rack.String();
                    }));
                }
                this.BeginInvoke((Action)(() =>
                {
                    lblCurrentRack.Text = $"{(Game.IsPlayer1 ? "Player 1 :" : "Player 2 :")} " + rack.String();
                }));
                if (!rack.Any())
                {
                    Game.EndGame = true;
                    return;
                }
                var ret = Game.Resolver.FindMoves(Game.IsPlayer1 ? Game.Player1 : Game.Player2, 30);
                lsbHintWords.DataSource = ret;
                if (ret.Any())
                {
                    Game.NoMoreMovesCount = 0;
                    var word = ret.Where(w => !Game.Resolver.PlayedWords.Any(pw => pw.Serialize == w.Serialize)).OrderByDescending(r => r.Points).First() as Word;
                    if (CurrentWord != null && CurrentWord.Serialize == word.Serialize)
                    {
                        Game.EndGame = true;
                        return;
                    }
                    CurrentWord = word;
                    DisplayPlayerWord(word);
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
                //DisplayScores();
            }
            catch (ArgumentException ex)
            {
                lsbInfos.Items.Insert(0, ex.Message);
            }
            finally
            {
                await Task.Delay(wait);
                Cursor.Current = Cursors.Default;
            }



        }

        private void DisplayPlayerWord(Word word)
        {
            this.BeginInvoke((Action)(() =>
            {
                lsbWords.Items.Add(word);
            }));
        }

        private void DisplayScores()
        {
            this.BeginInvoke((Action)(() =>
            {
                lblPlayer1Score.Text = $"Player 1 Score :{Game.Player1.Points}";
                lblPlayer2Score.Text = $"Player 2 Score :{Game.Player2.Points}";
                var bestP1Move = Game.Player1.Moves.OrderByDescending(m => m.Points).FirstOrDefault();
                if (bestP1Move != null) lblP1BestPlay.Text = $"{bestP1Move}";
                var bestP2Move = Game.Player2.Moves.OrderByDescending(m => m.Points).FirstOrDefault();
                if (bestP2Move != null) lblP2BestPlay.Text = $"{bestP2Move}";
            }));
        }

        Word CurrentWord { get; set; }
        private void btnDemoAll_Click(object sender, EventArgs e)
        {

            if (!ckKeepExistingBoard.Checked)
                NewGame();
            Game.CancelToken = new CancellationTokenSource();
            Task.Factory.StartNew(async () =>
            {
                await LoopDemo(Game.CancelToken);
                // call web API
            }, Game.CancelToken.Token);

        }

        private async Task LoopDemo(CancellationTokenSource cancelToken)
        {
            while (!Game.EndGame)
            {
                if (cancelToken.IsCancellationRequested)
                {
                    return;
                }
                await PlayDemo(500);
            }
            if (Game.EndGame)
                ShowWinner(true);

        }

        private void ShowWinner(bool endGame = false)
        {
            if (endGame)
            {
                this.BeginInvoke((Action)(() =>
                {
                    lsbInfos.Items.Insert(0, "Game Ended");
                    bool player1Wins = Game.Player1.Points > Game.Player2.Points;
                    lsbInfos.Items.Insert(0, $"{(player1Wins ? "Player 1" : "Player 2")} wins with a score of {(player1Wins ? Game.Player1.Points : Game.Player2.Points)}");
                }));
            }
        }

        private void lsb_Click(object sender, EventArgs e)
        {
            var word = lsbHintWords.SelectedItem as Word;
            if (word == null) return;
            if (Game.IsPlayer1)
                PreviewWord(Game.Player1, word, false, false);
            else
                PreviewWord(Game.Player2, word, false, false);
        }

        private void btnSaveGame_Click(object sender, EventArgs e)
        {
            SaveGame();
        }

        public void SaveGame()
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
            LoadGame();
        }

        public void LoadGame()
        {
            DawgResolver.Model.Game.IsTransposed = false;
            var ofd = openFileDialog1.ShowDialog();
            if (ofd == DialogResult.OK)
            {
                saveFileDialog1.FileName = openFileDialog1.FileName;
                this.Text = $"Scrabble ({openFileDialog1.FileName})";
                var txt = File.ReadAllText(openFileDialog1.FileName);
                Game.Deserialize(txt);
                Game.IsPlayer1 = true;
                foreach (var t in Game.Grid)
                {
                    var frmTile = gbBoard.Controls.Find($"t{t.Ligne}_{t.Col}", false).First() as FormTile;
                    if (t.IsValidated)
                    {

                        //frmTile.IsValidated = t.IsValidated;
                        frmTile.BackColor = t.IsPlayedByPlayer1.HasValue && t.IsPlayedByPlayer1.Value ? Player1MoveColor : Player2MoveColor;
                        //frmTile.Enabled = false;
                    }
                    else
                    {
                        frmTile.BackColor = frmTile.GetBackColor(t);
                        //frmTile.Enabled = true;
                    }
                }
                //var wordsCount = Math.Max(Game.Player1.Moves.Count, Game.Player2.Moves.Count);
                //for (int i = 0; i < wordsCount; i++)
                //{
                //    if (i < Game.Player1.Moves.Count)
                //    {
                //        PreviewWord(Game.Player1, Game.Player1.Moves[i], true, false);
                //        DisplayPlayerWord(Game.Player1.Moves[i]);
                //    }
                //    if (i < Game.Player2.Moves.Count)
                //    {
                //        PreviewWord(Game.Player2, Game.Player2.Moves[i], true, false);
                //        DisplayPlayerWord(Game.Player2.Moves[i]);
                //    }
                //}
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

        private void rbWordDirDown_CheckedChanged(object sender, EventArgs e)
        {
            Game.CurrentWordDirection = rbWordDirDown.Checked ? MovementDirection.Down : MovementDirection.Across;
        }



        private void lsbWords_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            int index = this.lsbWords.IndexFromPoint(e.Location);
            if (index != System.Windows.Forms.ListBox.NoMatches)
            {
                var word = lsbWords.Items[index] as Word;
                ShowDefinition(word);
            }
        }

        public void ShowDefinition(Word word)
        {

            if (word != null && !string.IsNullOrWhiteSpace(word.Text))
                Process.Start($"https://1mot.net/{word.Text.ToLower()}");
        }

        private void rbMaxLength_CheckedChanged(object sender, EventArgs e)
        {
            Game.HintSortByBestScore = !rbMaxLength.Checked;
        }

        private void MainForm_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.S)
            {
                SaveGame();

            }
            else if (e.Control && e.KeyCode == Keys.L)
            {
                LoadGame();

            }

        }
        private void rbGameStyleScrabble_CheckedChanged(object sender, EventArgs e)
        {
            //Game.GameStyle = rbGameStyleScrabble.Checked ? 'S' : 'W';
            //RefreshGridTiles();
        }
        private void rbWordsWithFriends_CheckedChanged(object sender, EventArgs e)
        {
            Game.GameStyle = rbGameStyleScrabble.Checked ? 'S' : 'W';
            Game.Bag.Letters = new List<Letter>(Game.GameStyle == 'S' ? Game.AlphabetScrabbleAvecJoker : Game.AlphabetWWFAvecJoker);
            txtBag.Text = Game.Bag.GetBagContent();
            RefreshGridTiles();
        }

        private void txtMotExiste_TextChanged(object sender, EventArgs e)
        {
            lblMotExiste.ForeColor = Game.Dico.MotAdmis(txtMotExiste.Text.Trim()) ? Color.Green : Color.Red;
        }

        private void txtMotExiste_DoubleClick(object sender, EventArgs e)
        {
            if (Game.Dico.MotAdmis(txtMotExiste.Text.Trim()))
                Process.Start($"https://1mot.net/{txtMotExiste.Text.ToLower()}");

        }

        private void MainForm_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void MainForm_Click(object sender, EventArgs e)
        {
            Game.CancelToken.Cancel();
        }
    }
}
