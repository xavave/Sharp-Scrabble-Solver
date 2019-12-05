using DawgResolver;
using Scrabble.Core;
using Scrabble.Core.Log;
using Scrabble.Core.Players;
using Scrabble.Core.Stats;
using Scrabble.Core.Tile;
using Scrabble.Core.Words;
using Scrabble.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Scrabble
{
    public partial class ScrabbleForm : Form
    {
        public const int BOARD_WIDTH = 15;
        public const int BOARD_HEIGHT = 15;
        public const int TILE_SIZE = 48;
        public const int RACK_TILES = 7;
        public bool GamePlaying { get; set; }

        public WordValidator WordValidator { get; set; }
        public WordSolver WordSolver { get; set; }
        public StatManager StatManager { get; set; }
        public RackManager RackManager { get; set; }
        public TileManager TileManager { get; set; }
        public PlayerManager PlayerManager { get; set; }
        //public WordScorer WordScorer { get; set; }
        public GameLog Logger { get; set; }
        public WordHint WordHint { get; set; }


        public Game Game { get; set; }
        public ScrabbleForm()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.None;
            Game = new Game();


            //this.WordScorer = new WordScorer(this);
            this.TileManager = new TileManager(this);
            TileManager.SetupTiles();

           this.WordValidator = new WordValidator { ScrabbleForm = this };
            this.WordSolver = new WordSolver { ScrabbleForm = this };
            this.StatManager = new StatManager();
            this.RackManager = new RackManager(this);

            this.Logger = new GameLog(this);
            this.WordHint = new WordHint(this);

            this.PlayerManager = new PlayerManager { ScrabbleForm = this };
            PlayerManager.SetupPlayers();

            this.GamePlaying = true;
            btnLetters.Text = $"{TileManager.TileBag.LetterCountRemaining()} Letters Remaining";
        }

        private const int WM_NCHITTEST = 0x84;
        private const int HTCLIENT = 0x1;
        private const int HTCAPTION = 0x2;

        /// <summary>
        /// Handling the window messages, still allowing the form to be dragged without the title bar.
        /// </summary>
        /// <param name="message"></param>
        protected override void WndProc(ref Message message)
        {
            base.WndProc(ref message);

            if (message.Msg == WM_NCHITTEST && (int)message.Result == HTCLIENT)
                message.Result = (IntPtr)HTCAPTION;
        }

        /// <summary>
        /// Triggers the game to end.
        /// </summary>
        public void GameOver()
        {
            GamePlaying = false;

            Logger.LogMessage("Game Over! The scores were: ");
            Logger.LogMessage($"{PlayerManager.PlayerOne.Name}: {PlayerManager.PlayerOne.Score}");
            Logger.LogMessage($"{PlayerManager.PlayerTwo.Name}: {PlayerManager.PlayerTwo.Score}");

            MessageBox.Show($"Game Over! The scores were: \n\n{PlayerManager.PlayerOne.Name}: {PlayerManager.PlayerOne.Score}\n{PlayerManager.PlayerTwo.Name}: {PlayerManager.PlayerTwo.Score}", "Game Over", MessageBoxButtons.OK);
        }

        /// <summary>
        /// Handles playing a turn.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPlay_Click(object sender, EventArgs e)
        {
            if (!GamePlaying) return;

            var tilesPlayed = PlayerManager.CurrentPlayer.Tiles.Where(r => string.IsNullOrEmpty(r.Text)).Count();
            if (tilesPlayed == 0)
            {
                MessageBox.Show("You must place some tiles on the board and make a word to play. You can pass or swap your letters if cannot make a word. You can use the hint to see available words in your rack.");
                return;
            }

            var checkTilePositions = TileManager.ValidateTilePositions();
            //var moveResult = WordValidator.ValidateWordsInPlay();

            if (!checkTilePositions)
                MessageBox.Show("The placement of your tiles is invalid.");
            //else if (!moveResult.Valid)
            //    moveResult.Words.Where(w => !w.IsAllowed).ToList().ForEach(w => MessageBox.Show($"{w.Text} is not a valid word"));
            else
            {
                foreach (var t in this.TileManager.TilesInPlay)
                {
                    Game.Grid[t.Ligne, t.Col].Letter = Game.Alphabet.Find(a => a.Char == t.Text[0]);
                }

                TileManager.ResetTilesInPlay();
                RackManager.FillRack(PlayerManager.CurrentPlayer.Tiles);

                StatManager.Moves += 1;
                StatManager.ConsecutivePasses = 0;

                //moveResult.Words.ForEach(w => Logger.LogMessage($"{PlayerManager.CurrentPlayer.Name} played {w.Text} for {w.Points} points"));
                //Logger.LogMessage($"Turn ended - total score: {moveResult.TotalScore}");

                btnLetters.Text = $"{TileManager.TileBag.LetterCountRemaining()} Letters Remaining";

                // A player has finished all their tiles so the game is over
                if (PlayerManager.PlayerOne.Tiles.Count == 0 || PlayerManager.PlayerTwo.Tiles.Count == 0)
                    GameOver();

                //PlayerManager.CurrentPlayer.Score += moveResult.TotalScore;
                PlayerManager.SwapCurrentPlayer();
            }
        }

        /// <summary>
        /// Handles the event when the user wants to pass their turn.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPass_Click(object sender, EventArgs e)
        {
            if (!GamePlaying) return;

            var verification = MessageBox.Show("Do you really want to pass your turn?", "Pass your turn", MessageBoxButtons.YesNo);
            if (verification == DialogResult.Yes)
            {
                TileManager.ResetTilesOnBoardFromTurn();

                // 4 passes in a row will trigger the game to end (e.g. both players feel they have no where left to move)
                StatManager.Passes += 1;
                StatManager.ConsecutivePasses += 1;
                if (StatManager.ConsecutivePasses >= 4)
                    GameOver();

                Logger.LogMessage($"Turned ended - {PlayerManager.CurrentPlayer.Name} passed their turn.");
                PlayerManager.SwapCurrentPlayer();
            }
        }

        /// <summary>
        /// Handles allowing the user to swap their tiles.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSwap_Click(object sender, EventArgs e)
        {
            if (!GamePlaying) return;

            TileManager.ResetTilesOnBoardFromTurn();

            var verification = MessageBox.Show("Do you really want to swap the tiles you have selected?", "Swap your letters", MessageBoxButtons.YesNo);
            if (verification == DialogResult.Yes)
            {

                var tiles = PlayerManager.CurrentPlayer.Tiles.Where(c => c.LetterSelected).ToList();

                // Trying to swap no letters at all.
                if (tiles.Count == 0)
                {
                    MessageBox.Show("You must select at least one letter from your rack to swap.");
                    return;
                }

                // Trying to swap more letters than are left in the bag
                if (tiles.Count > TileManager.TileBag.LetterCountRemaining())
                {
                    MessageBox.Show($"You can only swap {TileManager.TileBag.LetterCountRemaining()} letter(s) or less.");
                    return;
                }

                tiles.ForEach(t =>
                {
                    TileManager.TileBag.GiveLetter(t.Text[0]);
                    t.ClearDisplay();
                });

                RackManager.FillRack(PlayerManager.CurrentPlayer.Tiles);

                StatManager.Swaps += 1;
                StatManager.ConsecutivePasses = 0;
                Logger.LogMessage($"Turn ended - {PlayerManager.CurrentPlayer.Name} swapped {tiles.Count} tile(s).");

                PlayerManager.SwapCurrentPlayer();
            }
        }

        /// <summary>
        /// Handles showing how many letters remain.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLetters_Click(object sender, EventArgs e)
        {
            var letterForm = new LettersForm { ScrabbleForm = this };
            letterForm.BindLetters();
            letterForm.Show();
        }

        /// <summary>
        /// Provides a hint to the player, will show all available anagrams for the letters
        /// in their rack.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnHint_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
           
            WordHint.ClearText();
            
            WordHint.BeginUpdate();
           
            foreach (var w in Game.Resolver.FindMoves(PlayerManager.CurrentPlayer.Name== PlayerManager.PlayerOne.Name ?  Game.Player1: Game.Player2))
                WordHint.AddWord(w);
           
            WordHint.EndUpdate();
            Cursor.Current = Cursors.Default;
        }

        /// <summary>
        /// Handles closing the game.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExit_Click(object sender, EventArgs e)
        {
            var verification = MessageBox.Show("Do you really want to exit? You will lose any game progress.", "Exit", MessageBoxButtons.YesNo);

            if (verification == DialogResult.Yes)
                Application.Exit();
        }

        private void ScrabbleForm_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }

        private void ScrabbleForm_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.Text))
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;
            //MessageBox.Show(e.Data.GetData(DataFormats.Text).ToString());
            var ctl = FindControlAtCursor(this) as ScrabbleTile;
            //var ctl = FindControlAtPoint(this, new Point(e.X, e.Y));
            if (ctl != null)
                ctl.OnLetterPlaced(e.Data.GetData(DataFormats.Text).ToString());
            else
                e.Effect = DragDropEffects.None;
        }
        public static Control FindControlAtPoint(Control container, Point pos)
        {
            Control child;
            foreach (Control c in container.Controls)
            {
                if (c.Visible && c.Bounds.Contains(pos))
                {
                    child = FindControlAtPoint(c, new Point(pos.X - c.Left, pos.Y - c.Top));
                    if (child == null) return c;
                    else return child;
                }
            }
            return null;
        }

        public static Control FindControlAtCursor(Form form)
        {
            Point pos = Cursor.Position;
            if (form.Bounds.Contains(pos))
                return FindControlAtPoint(form, form.PointToClient(pos));
            return null;
        }

        private void ScrabbleForm_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Clicks == 1 && e.Button == MouseButtons.Right)
            {
                var ctl = FindControlAtCursor(this) as ScrabbleTile;
                if (ctl != null)
                    ctl.OnLetterRemoved();
            }

        }
        private bool DraggingFromGrid = false;
        private System.Drawing.Point DraggingStartPoint = new System.Drawing.Point();
        private void ScrabbleForm_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                DraggingFromGrid = true;
                DraggingStartPoint = new System.Drawing.Point(e.X, e.Y);
            }
        }

        private void ScrabbleForm_MouseUp(object sender, MouseEventArgs e)
        {

            if (DraggingFromGrid)
            {
                DraggingFromGrid = false;
            }
        }

        private void ScrabbleForm_MouseMove(object sender, MouseEventArgs e)
        {
            if (DraggingFromGrid)
            {
                if (System.Math.Abs(e.X - DraggingStartPoint.X) > 10 ||
                    System.Math.Abs(e.Y - DraggingStartPoint.Y) > 10)
                {
                    StartDragging();
                }
            }
        }
        private void StartDragging()
        {
            DraggingFromGrid = false;

            
        }
        //private void ImplementComputerLogicHere()
        //{
        //    var makeableWords = new List<Word>();

        //    // Generate the possible plays
        //    for (int x = 0; x < ScrabbleForm.BOARD_WIDTH; x++)
        //    {
        //        for (int y = 0; y < ScrabbleForm.BOARD_HEIGHT; y++)
        //        {
        //            var t = TileManager.Tiles[x, y];

        //            if (!string.IsNullOrEmpty(t.Text))
        //            {
        //                //MessageBox.Show($"Finding words for {t.Text}");
        //                var w = WordSolver.Anagrams(PlayerManager.CurrentPlayer.Tiles, t);
        //                makeableWords.AddRange(w);
        //            }
        //        }
        //    }

        //    // Sort them by score so that it attempts to play words with the most value first
        //    makeableWords = makeableWords.OrderByDescending(w => w.Score).ThenByDescending(w => w.Text.Length).ToList();

        //    Console.WriteLine("About to play the word!");

        //    foreach (var w in makeableWords)
        //    {
        //        Logger.LogMessage($"Attempting to play {w.Text}");
        //        //MessageBox.Show($"Trying to play {w}");

        //        // If any of the tiles have already been played on, we can't make a word there
        //        //if (w.Tiles.Any(t => !string.IsNullOrEmpty(t.Text)))
        //        //    continue;

        //        // Select the tiles in the rack and put them on the board
        //        for (var x = 0; x < w.Tiles.Count; x++)
        //        {
        //            var t = w.Tiles[x];

        //            var rackTile = PlayerManager.CurrentPlayer.Tiles.FirstOrDefault(rt => rt.Letter == w.Text[x]);
        //            if (rackTile != null)
        //            {
        //                rackTile.PerformClick();
        //                t.PerformClick();
        //            }
        //        }

        //        var validation = WordValidator.ValidateAllWordsInPlay();
        //        if (validation.Valid)
        //        {
        //            // Once we have played a word, we are done!
        //            btnPlay.PerformClick();
        //            return;
        //        }
        //        else
        //        {
        //            Logger.LogMessage($"{w.Text} is not a valid word");

        //            // Word wasn't valid or has caused a combination of other invalid words (so return it to the rack)
        //            TileManager.ResetTilesOnBoardFromTurn();
        //            Thread.Sleep(1000);
        //            TileManager.ResetTilesInPlay();
        //            Thread.Sleep(1000);
        //            PlayerManager.CurrentPlayer.Tiles.ForEach(t =>
        //            {
        //                t.ClearDisplay();
        //                t.LetterSelected = false;
        //            });
        //            Thread.Sleep(1000);
        //            //foreach (var t in w.Tiles)
        //            //{
        //            //    t.PerformClick();
        //            //    t.TileInPlay = false;
        //            //}
        //        }
        //    }

        //    Console.WriteLine("Could not find a valid word!");
        //}
    }
}

