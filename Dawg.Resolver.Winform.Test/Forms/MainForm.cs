using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

using DawgResolver.Model;

namespace Dawg.Solver.Winform
{
    public partial class MainForm : Form
    {


        public string CurrentDicoName { get; set; } = Dictionnaire.NomDicoDawgEN_Collins;
        public Game game => Game.DefaultInstance;
        public FormTile LastPlayedTile { get; set; }
        public HashSet<IExtendedTile> BoardTiles { get; }

        public MainForm()
        {
            InitializeComponent();
            BoardTiles = gbBoard.Controls.OfType<IExtendedTile>().ToHashSet();
            NewGame(CurrentDicoName, true);
        }

        private void NewGame(string nomDico, bool initUI)
        {
            Cursor.Current = Cursors.WaitCursor;

            if (initUI)
            {
                txtGrid2.Visible = ckShowGrid.Checked;
                lsbInfos.Items.Clear();
                lblPlayer1Score.Text = lblPlayer2Score.Text = "";
                lblP1BestPlay.Text = "Player 1 best play";
                lblP2BestPlay.Text = "Player 2 best play";
                txtRackP1.Text = txtRackP2.Text = "";
                lsbWords.DisplayMember = "DisplayInList";
                lsbWords.Items.Clear();
                txtBag.Text = game.Bag.GetBagContent();

                if (ckShowGrid.Checked) txtGrid2.Text = game.GenerateTextGrid(game.Grid, true);

                RefreshGridTiles();
            }
            Cursor.Current = Cursors.Default;

        }

        private void RefreshGridTiles()
        {
            //ClearBoardTilesExceptHeaders();

            //construct tiles with letter/word multipliers
            ConstructBoardTiles();
            //construct headers
            ConstructHeaderBoardTiles();

            SetGbBoardFromBoardTiles();
        }

        private void SetGbBoardFromBoardTiles()
        {
            gbBoard.SuspendLayout();
            gbBoard.Controls.Clear();
            gbBoard.Controls.AddRange(BoardTiles.Cast<FormTile>().ToArray());
            gbBoard.ResumeLayout();
        }

        private void ConstructBoardTiles()
        {

            foreach (var vtile in game.Grid)
            {
                //IExtendedTile frmTile = new FormTile(this, game, vtile)
                //{
                //    Text = vtile.Letter?.Char.ToString(),
                //    LetterMultiplier = vtile.LetterMultiplier,
                //    WordMultiplier = vtile.WordMultiplier,
                //};
                //frmTile.SetBackColorFrom(vtile);
                BoardTiles.Add(vtile);
            }
        }


        //private void ClearBoardTilesExceptHeaders()
        //{
        //    foreach (var tile in BoardTiles.Where(t => !t.Name.StartsWith("header_")))
        //    {
        //        tile.Clear();
        //    }
        //}
        private void ConstructHeaderBoardTiles()
        {
            for (int i = 0; i < game.BoardSize; i++)
            {
                var frmTile = BoardTiles.FirstOrDefault(f => f.Name == $"header_col{i}");
                if (frmTile == null)
                {
                    frmTile = new FormTile(0, i, $"header_col{i}", FormTile.HeaderTilesBackColor) { Text = $"{i + 1}" };
                    BoardTiles.Add(frmTile);
                }

                frmTile = BoardTiles.FirstOrDefault(f => f.Name == $"header_ligne{i}");
                if (frmTile == null)
                {
                    frmTile = new FormTile(i, 0, $"header_ligne{i}", FormTile.HeaderTilesBackColor) { Text = $"{Solver.DefaultInstance.Alphabet.ElementAt(i).Char}" };
                    BoardTiles.Add(frmTile);
                }
            }
        }

        private void RefreshBoard()
        {
            //gbBoard.Invoke((MethodInvoker)(() => gbBoard.SuspendLayout()));
            //for (int ligne = 0; ligne < grid.GetLength(0); ligne++)
            //{
            //    for (int col = 0; col < grid.GetLength(1); col++)
            //    {

            //        //var formTile = FindFormTile(grid[ligne, col]);
            //        var formTile = grid[ligne, col] as FormTile;
            //        formTile.Tile = grid[ligne, col];
            //        if (formTile.InvokeRequired)
            //            formTile.Invoke((MethodInvoker)(() => formTile.Text = formTile.Tile?.Letter?.Char.ToString()));
            //        else
            //        {
            //            formTile.Text = formTile.Tile?.Letter?.Char.ToString();
            //        }
            //        if (grid[ligne, col].IsEmpty)
            //            if (formTile.InvokeRequired)
            //                formTile.Invoke((MethodInvoker)(() => formTile.GetBackColor(grid[ligne, col])));
            //            else
            //                formTile.GetBackColor(grid[ligne, col]);
            //    }
            //}

            //lsbInfos.Items.Add(Game.IsTransposed ? "Transposed" : "Not Transposed");
            if (ckShowGrid.Checked)
                txtGrid2.Invoke((MethodInvoker)(() => txtGrid2.Text = game.GenerateTextGrid(game.Grid, true)));
            //txtRackP1.Text = Game.Player1.Rack.String();
            //txtRackP2.Text = Game.Player2.Rack.String();
            txtBag.Invoke((MethodInvoker)(() => txtBag.Text = game.Bag.GetBagContent()));
            DisplayScores();
            //gbBoard.Invoke((MethodInvoker)(() => gbBoard.ResumeLayout()));

        }

        private void btnTranspose_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            game.TransposeGameGrid();
            //using (new SuspendDrawingUpdate(sender as Control))
            RefreshBoard();
            Cursor.Current = Cursors.Default;
        }
        //private List<VTile> ClearTilesInPlay(Player p)
        //{
        //    var ret = game.Grid.OfType<VTile>().Where(t => !t.IsValidated).ToList();
        //    foreach (var tile in ret)
        //    {
        //        var frmTile = FindTile(tile);
        //        frmTile.Invoke((MethodInvoker)(() =>
        //        {
        //            //frmTile.Text = "";
        //            //frmTile.Letter = new Letter();
        //            //frmTile.BackColor = frmTile.GetBackColor(tile);
        //        }));
        //    }
        //    //for (int i = 0; i < Grid.LongLength; i++)
        //    //{
        //    //    var tile = Game.Grid.OfType<VTile>().ElementAt(i);
        //    //    if (!tile.IsValidated)
        //    //    {
        //    //        if (!tile.IsEmpty)
        //    //        {
        //    //            if (tile.FromJoker)
        //    //            {
        //    //                p.Rack.Add(GameStyle == 'S' ? AlphabetScrabbleAvecJoker[26] : Game.AlphabetWWFAvecJoker[26]);
        //    //            }
        //    //            else
        //    //            {
        //    //                p.Rack.Add(tile.Letter);
        //    //            }
        //    //            tile.IsValidated = true;
        //    //        }
        //    //        else
        //    //            Grid[tile.Ligne, tile.Col].Letter = new Letter();

        //    //    }
        //    //}
        //    return ret;
        //}
        private void btnBackToRack_Click(object sender, EventArgs e)
        {
            //var ret = ClearTilesInPlay(Game.Player1);
            //if (ret.Any()) txtRackP1.Text = ret.String();
            //Game.Grid = RefreshBoard(Game.Grid);
        }

        private void btnValidate_Click(object sender, EventArgs e)
        {

            if (LastPlayedTile == null) return;
            var word = LastPlayedTile.GetWordFromTile(Game.CurrentWordDirection);
            if (word == null) return;
            if (game.IsPlayer1)
                Game.DefaultInstance.PreviewWord(game.Player1, (w) => AddWord(w), word, true);
            else
                Game.DefaultInstance.PreviewWord(game.Player2, (w) => AddWord(w), word, true);
            RefreshBoard();
        }
        private Word AddWord(Word word)
        {
            lsbWords.Invoke((MethodInvoker)(() => lsbWords.Text = word.Text));
            return word;
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
            game.Bag.GetLetters(game.Player1, txtRackP1.Text.Trim());
            lsbHintWords.DisplayMember = "DisplayText";
            var ret = Solver.DefaultInstance.FindMoves(150, Game.HintSortByBestScore);
            lsbInfos.Items.Insert(0, game.IsTransposed ? "Transposed" : "Not Transposed");
            lsbInfos.Items.Insert(0, $"NbPossibleMoves={Solver.DefaultInstance.NbPossibleMoves}");
            lsbInfos.Items.Insert(0, $"NbAcceptedMoves={Solver.DefaultInstance.NbAcceptedMoves}");

            lsbHintWords.DataSource = ret;
            Cursor.Current = Cursors.Default;
        }

        private async void btnDemo_Click(object sender, EventArgs e)
        {
            await PlayDemo();
        }


        private async Task PlayDemo(int wait = 0)
        {

            if (game.NoMoreMovesCount > 1)
            {
                game.EndGame = true;
                return;
            }
            System.Windows.Forms.Application.DoEvents();
            var playerNumber = game.IsPlayer1 ? "1" : "2";
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                game.Bag.GetLetters(game.CurrentPlayer);
                if (game.Bag.LeftLettersCount == 0)
                {
                    lsbInfos.Invoke((MethodInvoker)(() => lsbInfos.Items.Insert(0, "Le sac est vide !")));
                    game.NoMoreMovesCount++;
                }

                lblCurrentRack.Invoke((MethodInvoker)(() => lblCurrentRack.Text = $"Player {playerNumber} :{game.CurrentPlayer.Rack}"));

                if (!game.CurrentPlayer.Rack.Any())
                {
                    game.NoMoreMovesCount++;

                }
                var ret = Solver.DefaultInstance.FindMoves(60);
                lsbHintWords.Invoke((MethodInvoker)(() =>
                {
                    using (var susp = new SuspendDrawingUpdate(lsbHintWords))
                        lsbHintWords.DataSource = ret;
                }));
                if (ret.Any())
                {
                    var word = ret.Where(w => !Solver.DefaultInstance.PlayedWords.Any(pw => pw.Serialize == w.Serialize)).OrderByDescending(r => r.Points).FirstOrDefault() as Word;
                    if (word == null)//TODO CHECK || CurrentWord?.Text == word.Text)
                    {
                        game.EndGame = true;
                        return;
                    }
                    CurrentWord = word;

                    int points = Game.DefaultInstance.PreviewWord(game.CurrentPlayer, (w) => AddWord(w), word, true);
                    RefreshBoard();
                    game.CurrentPlayer.Points += points;

                }
                else
                {
                    game.NoMoreMovesCount++;
                    if (game.NoMoreMovesCount < 2)
                        InsertLsbText(lsbInfos, $"Player {playerNumber}:{game.CurrentPlayer.Rack} --> No words found !");
                    game.IsPlayer1 = !game.IsPlayer1;
                }
                game.Bag.GetLetters(game.CurrentPlayer);
                SetPlayerRackText(game.IsPlayer1 ? txtRackP1 : txtRackP2);

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

        private void SetPlayerRackText(TextBox txt)
        {
            txt.Invoke((MethodInvoker)(() => txt.Text = $"{game.CurrentPlayer.Rack}"));
        }
        private void InsertLsbText(ListBox lsb, string txt)
        {
            lsb.Invoke((MethodInvoker)(() => lsb.Items.Insert(0, txt)));
        }

        private void DisplayScores()
        {
            this.Invoke((MethodInvoker)(() =>
            {
                lblPlayer1Score.Text = $"Player 1 Score :{game.Player1.Points}";
                lblPlayer2Score.Text = $"Player 2 Score :{game.Player2.Points}";
                var bestP1Move = game.Player1.Moves.OrderByDescending(m => m.Points).FirstOrDefault();
                if (bestP1Move != null) lblP1BestPlay.Text = $"Player 1 best play:{bestP1Move}";
                var bestP2Move = game.Player2.Moves.OrderByDescending(m => m.Points).FirstOrDefault();
                if (bestP2Move != null) lblP2BestPlay.Text = $"Player 2 best play:{bestP2Move}";
            }));
        }

        Word CurrentWord { get; set; }
        private void btnDemoAll_Click(object sender, EventArgs e)
        {

            if (game.CancelToken == null)
            {
                NewGame(CurrentDicoName, !ckKeepExistingBoard.Checked);
                game.CancelToken = new CancellationTokenSource();
                game.CancelToken.Token.Register(() => CancelAction());
                var btn = sender as Button;
                btn.Text = "Stop AutoPlay";
                var t = Task.Factory.StartNew(async () =>
                {
                    await LoopDemo(game.CancelToken);
                }, game.CancelToken.Token);
            }
            else
            {
                game.CancelToken.Cancel();
            }
        }

        private void CancelAction()
        {
            btnDemoAll.Invoke((MethodInvoker)(() => btnDemoAll.Text = "AutoPlay All"));
            game.EndGame = true;
            game.CancelToken = null;
        }

        private async Task LoopDemo(CancellationTokenSource cancelToken)
        {
            try
            {
                while (!game.EndGame)
                {
                    if (cancelToken != null && cancelToken.IsCancellationRequested && cancelToken.Token.CanBeCanceled)
                    {
                        cancelToken.Token.ThrowIfCancellationRequested();
                        return;
                    }
                    await PlayDemo(1000);
                }
                if (game.EndGame)
                    ShowWinner(true);
            }


            catch (OperationCanceledException)
            {
                CancelAction();
            }

        }

        private void ShowWinner(bool endGame = false)
        {
            if (endGame)
            {
                lsbInfos.Invoke((MethodInvoker)(() =>
                {
                    lsbInfos.Items.Insert(0, "Game Ended");
                    bool player1Wins = game.Player1.Points > game.Player2.Points;
                    lsbInfos.Items.Insert(0, $"{(player1Wins ? game.Player1.Name : game.Player2.Name)} wins with a score of {(player1Wins ? game.Player1.Points : game.Player2.Points)}");
                }));
                game.CancelToken?.Cancel();

            }
        }

        private void lsb_Click(object sender, EventArgs e)
        {
            var word = lsbHintWords.SelectedItem as Word;
            if (word == null) return;
            Game.DefaultInstance.PreviewWord(game.CurrentPlayer, (w) => AddWord(w), word, false, false);
            RefreshBoard();

        }

        private void btnSaveGame_Click(object sender, EventArgs e)
        {
            Game.DefaultInstance.SaveGame();
        }



        private void btnLoadGame_Click(object sender, EventArgs e)
        {
            Game.DefaultInstance.Load(BoardTiles, (w) => AddWord(w));

        }



        private void btnNewGame_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(CurrentDicoName)) return;
            NewGame(CurrentDicoName, true);
        }


        private void lsbWords_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (var c in gbBoard.Controls.OfType<FormTile>().Where(ft => ft.BackColor == Color.LightBlue))
                c.BackColor = FormTile.Player1MoveColor;
            foreach (var c in gbBoard.Controls.OfType<FormTile>().Where(ft => ft.BackColor == Color.LightCoral))
                c.BackColor = FormTile.Player2MoveColor;
            var word = lsbWords.SelectedItem as Word;
            if (word == null) return;


            bool isPlayer1word = game.Player1.Moves.Contains(word);
            foreach (var t in word.GetTiles())
            {
                var frmTile = t.FindFormTile(BoardTiles);

                frmTile.BackColor = isPlayer1word ? Color.LightBlue : Color.LightCoral;
            }
        }

        private void rbSize15_CheckedChanged(object sender, EventArgs e)
        {
            if (rbSize15.Checked) game.BoardSize = 15;
            if (rbSize11.Checked) game.BoardSize = 11;

            nudCustomBoardSize.Value = game.BoardSize;
            NewGame(CurrentDicoName, !ckKeepExistingBoard.Checked);
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
                if (word == null) return;
                word.ShowDefinition();
            }
        }


        private void rbMaxLength_CheckedChanged(object sender, EventArgs e)
        {
            Game.HintSortByBestScore = !rbMaxLength.Checked;
        }

        private void MainForm_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.S)
            {
                Game.DefaultInstance.SaveGame();

            }
            else if (e.Control && e.KeyCode == Keys.L)
            {
                Game.DefaultInstance.Load(BoardTiles, (w) => AddWord(w));

            }

        }
        private void rbGameStyleScrabble_CheckedChanged(object sender, EventArgs e)
        {

        }
        private void rbWordsWithFriends_CheckedChanged(object sender, EventArgs e)
        {
            Solver.DefaultInstance.Mode = rbGameStyleScrabble.Checked ? 'S' : 'W';
            NewGame(CurrentDicoName, !ckKeepExistingBoard.Checked);
        }

        private void txtMotExiste_TextChanged(object sender, EventArgs e)
        {
            lblMotExiste.ForeColor = Dictionnaire.DefaultInstance.MotAdmis(txtMotExiste.Text.Trim()) ? Color.Green : Color.Red;
        }

        private void txtMotExiste_DoubleClick(object sender, EventArgs e)
        {
            if (Dictionnaire.DefaultInstance.MotAdmis(txtMotExiste.Text.Trim()))
                Process.Start($"https://1mot.net/{txtMotExiste.Text.ToLower()}");

        }

        private void MainForm_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void MainForm_Click(object sender, EventArgs e)
        {

        }

        private void btnUndoLast_Click(object sender, EventArgs e)
        {
            var allWords = game.Player1.Moves.Union(game.Player2.Moves).OrderByDescending(a => a.Index);
            var lastWord = allWords.FirstOrDefault();
            if (lastWord == null) return;
            var tiles = lastWord.GetTiles();
            if (lastWord.IsPlayedByPlayer1)
                game.Player1.Moves.Remove(lastWord);
            else
                game.Player2.Moves.Remove(lastWord);

            foreach (var t in tiles.Where(tt => tt.Letter.Char != Game.EmptyChar))
            {
                game.Bag.PutBackLetterInBag(t.Letter);
            }
            foreach (var tile in tiles.Where(t => t.WordIndex == lastWord.Index))
            {

                game.Grid[tile.Ligne, tile.Col] = new FormTile(tile.Ligne, tile.Col);
                game.Grid[tile.Ligne, tile.Col].Letter.Char = Game.EmptyChar;
            }
            //using (new SuspendDrawingUpdate(sender as Control))
            RefreshBoard();
        }

        private void rbDico_CheckedChanged(object sender, EventArgs e)
        {
            var rb = sender as RadioButton;
            var currentDico = Dictionnaire.NomDicoDawgEN_Collins;
            switch (rb.Name)
            {
                case "rbODS6": currentDico = Dictionnaire.NomDicoDawgODS6; break;
                case "rbrbODS7ODS6": currentDico = Dictionnaire.NomDicoDawgODS7; break;
                case "rbEnCollins2019": break;
                default: break;
            }
            NewGame(currentDico, !ckKeepExistingBoard.Checked);
        }

        private void btnWordsFinder_Click(object sender, EventArgs e)
        {
            //FrmDebug frm = new FrmDebug();
            //frm.Show();
            WordsFinder wf = new WordsFinder(game);
            wf.Show();
        }

        private void nudCustomBoardSize_ValueChanged(object sender, EventArgs e)
        {
            var newVal = (int)nudCustomBoardSize.Value;
            if (newVal != game.BoardSize)
            {
                game.BoardSize = newVal;
                NewGame(CurrentDicoName, !ckKeepExistingBoard.Checked);
            }
        }
    }
}
