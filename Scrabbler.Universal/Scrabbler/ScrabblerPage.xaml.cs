#pragma warning disable 4014        // for non-await'ed async call

using DawgResolver.Model;
using DawgResolver;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Scrabbler
{
    public partial class ScrabblerPage : ContentPage
    {
        static Color Player1MoveColor = Color.LightYellow;
        static Color Player2MoveColor = Color.LightGreen;
        const string timeFormat = @"%m\:ss";
        Game Game { get; }
        bool isGameInProgress;
        DateTime gameStartTime;

        public ScrabblerPage()
        {
            InitializeComponent();

            board.GameStarted += (sender, args) =>
                {
                    isGameInProgress = true;
                    gameStartTime = DateTime.Now;

                    Device.StartTimer(TimeSpan.FromSeconds(1), () =>
                    {
                        timeLabel.Text = (DateTime.Now - gameStartTime).ToString(timeFormat);
                        return isGameInProgress;
                    });
                };

            board.GameEnded += (sender, hasWon) =>
                {
                    isGameInProgress = false;

                    if (hasWon)
                    {
                        DisplayWonAnimation();
                    }
                    else
                    {
                        DisplayLostAnimation();
                    }
                };

            PrepareForNewGame();
            Game = board.Game;
        }

        void PrepareForNewGame()
        {
            board.NewGameInitialize();

            congratulationsText.IsVisible = false;
            consolationText.IsVisible = false;
            playAgainButton.IsVisible = false;
            playAgainButton.IsEnabled = false;

            timeLabel.Text = new TimeSpan().ToString(timeFormat);
            isGameInProgress = false;
        }

        void OnMainContentViewSizeChanged(object sender, EventArgs args)
        {
            ContentView contentView = (ContentView)sender;
            double width = contentView.Width;
            double height = contentView.Height;

            bool isLandscape = width > height;

            if (isLandscape)
            {
                mainGrid.RowDefinitions[0].Height = 0;
                mainGrid.RowDefinitions[1].Height = new GridLength(1, GridUnitType.Star);

                mainGrid.ColumnDefinitions[0].Width = new GridLength(1, GridUnitType.Star);
                mainGrid.ColumnDefinitions[1].Width = new GridLength(1, GridUnitType.Star);

                Grid.SetRow(textStack, 1);
                Grid.SetColumn(textStack, 0);
            }
            else // portrait
            {
                mainGrid.RowDefinitions[0].Height = new GridLength(3, GridUnitType.Star);
                mainGrid.RowDefinitions[1].Height = new GridLength(5, GridUnitType.Star);

                mainGrid.ColumnDefinitions[0].Width = 0;
                mainGrid.ColumnDefinitions[1].Width = new GridLength(1, GridUnitType.Star);

                Grid.SetRow(textStack, 0);
                Grid.SetColumn(textStack, 1);
            }
        }

        // Maintains a square aspect ratio for the board.
        void OnBoardContentViewSizeChanged(object sender, EventArgs args)
        {
            ContentView contentView = (ContentView)sender;
            double width = contentView.Width;
            double height = contentView.Height;
            double dimension = Math.Min(width, height);
            double horzPadding = (width - dimension) / 2;
            double vertPadding = (height - dimension) / 2;
            contentView.Padding = new Thickness(horzPadding, vertPadding);
        }

        async void DisplayWonAnimation()
        {
            congratulationsText.Scale = 0;
            congratulationsText.IsVisible = true;

            // Because IsVisible has been false, the text might not have a size yet,
            //  in which case Measure will return a size.
            double congratulationsTextWidth = congratulationsText.Measure(Double.PositiveInfinity, Double.PositiveInfinity).Request.Width;

            congratulationsText.Rotation = 0;
            congratulationsText.RotateTo(3 * 360, 1000, Easing.CubicOut);

            double maxScale = 0.9 * board.Width / congratulationsTextWidth;
            await congratulationsText.ScaleTo(maxScale, 1000);

            foreach (View view in congratulationsText.Children)
            {
                view.Rotation = 0;
                view.RotateTo(180);
                await view.ScaleTo(3, 100);
                view.RotateTo(360);
                await view.ScaleTo(1, 100);
            }

            await DisplayPlayAgainButton();
        }

        async void DisplayLostAnimation()
        {
            consolationText.Scale = 0;
            consolationText.IsVisible = true;

            // (See above for rationale)
            double consolationTextWidth = consolationText.Measure(Double.PositiveInfinity, Double.PositiveInfinity).Request.Width;

            double maxScale = 0.9 * board.Width / consolationTextWidth;
            await consolationText.ScaleTo(maxScale, 1000);
            await Task.Delay(1000);
            await DisplayPlayAgainButton();
        }

        async Task DisplayPlayAgainButton()
        {
            playAgainButton.Scale = 0;
            playAgainButton.IsVisible = true;
            playAgainButton.IsEnabled = true;

            // (See above for rationale)
            double playAgainButtonWidth = playAgainButton.Measure(Double.PositiveInfinity, Double.PositiveInfinity).Request.Width;

            double maxScale = board.Width / playAgainButtonWidth;
            await playAgainButton.ScaleTo(maxScale, 1000, Easing.SpringOut);
        }

        void OnplayAgainButtonClicked(object sender, object EventArgs)
        {
            PrepareForNewGame();
        }

        private void btnAutoplay_Clicked(object sender, EventArgs e)
        {
            while (!Game.EndGame)
            {
                PlayDemo(500);
            }
        }
        Word CurrentWord { get; set; }
        private void PlayDemo(int wait = 0)
        {

            if (Game.NoMoreMovesCount >= 2)
            {
                Game.EndGame = true;
                return;
            }


            try
            {
                var rack = Game.Bag.GetLetters(Game.IsPlayer1 ? Game.Player1 : Game.Player2);
                //if (!rack.Any())
                //    lsbInfos.Items.Insert(0, "Le sac est vide !");
                if (Game.IsPlayer1)
                {
                    txtRackP1.Text = Game.Player1.Rack.String();
                }
                else
                {
                    txtRackP2.Text = Game.Player2.Rack.String();
                }
                lblCurrentRack.Text = $"{(Game.IsPlayer1 ? "Player 1 :" : "Player 2 :")} " + rack.String();
                if (!rack.Any())
                {
                    Game.EndGame = true;
                    return;
                }
                var ret = Game.Resolver.FindMoves(Game.IsPlayer1 ? Game.Player1 : Game.Player2, 30);
                //lsbHintWords.DataSource = ret;
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
                    //DisplayPlayerWord(word);
                    int points = PreviewWord(Game.IsPlayer1 ? Game.Player1 : Game.Player2, word, true);
                    board.BagContent = Game.Bag.GetBagContent();
                    if (Game.IsPlayer1)
                        Game.Player1.Points += points;
                    else
                        Game.Player2.Points += points;
                    board.Player1Points = Game.Player1.Points;
                    board.Player2Points = Game.Player2.Points;

                }
                else
                {
                    Game.NoMoreMovesCount++;
                    //if (Game.NoMoreMovesCount < 2)
                    //    lsbInfos.Items.Insert(0, $"{(Game.IsPlayer1 ? $"Player 1:{Game.Player1.Rack.String()}" : $"Player 2:{Game.Player2.Rack.String()}")} --> No words found !");
                    Game.IsPlayer1 = !Game.IsPlayer1;
                    return;
                }

                //DisplayScores();
            }
            catch (ArgumentException ex)
            {
                lblInfos.Text = ex.Message;
            }
            finally
            {
                //Thread.Sleep(wait);
            }

        }

        public int PreviewWord(Player p, Word word, bool validateWord = false, bool addMove = true)
        {

            int points = word.SetWord(p, validateWord);
            if (validateWord || points > 0)
            {
                //word.Validate();
                foreach (var t in word.GetTiles())
                {
                    var gfxTile = board.Children.First(ti => ti.ClassId == $"t{t.Ligne}_{t.Col}") as Tile;
                    if (gfxTile.IsEmpty)
                        gfxTile.BackgroundColor = gfxTile.GetTileColor(t);
                    else
                    {
                        gfxTile.IsPlayedByPlayer1 = p == Game.Player1;
                        gfxTile.BackgroundColor = p == Game.Player1 ? Player1MoveColor : Player2MoveColor;
                    }

                    if (!gfxTile.IsEnabled)
                        continue;

                    Device.BeginInvokeOnMainThread(() =>
                   {
                       gfxTile.label.Text = Game.Grid[t.Ligne, t.Col].Letter?.Char.ToString();
                       gfxTile.label.BackgroundColor = Game.IsPlayer1 ? Player1MoveColor : Player2MoveColor;
                   });
                    if (t.FromJoker)
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            gfxTile.BorderColor = Color.Gold;
                        });
                        if (Game.IsPlayer1)
                            Game.Player1.Rack = Game.Player1.Rack.RemoveFromRack(Game.GameStyle == 'S' ? Game.AlphabetScrabbleAvecJoker[26] : Game.AlphabetWWFAvecJoker[26]);
                        else
                            Game.Player2.Rack = Game.Player2.Rack.RemoveFromRack(Game.GameStyle == 'S' ? Game.AlphabetScrabbleAvecJoker[26] : Game.AlphabetWWFAvecJoker[26]);
                    }
                    else
                    {
                        if (Game.IsPlayer1)

                            Game.Player1.Rack = Game.Player1.Rack.RemoveFromRack(Game.Grid[t.Ligne, t.Col].Letter);
                        else
                            Game.Player2.Rack = Game.Player2.Rack.RemoveFromRack(Game.Grid[t.Ligne, t.Col].Letter);
                    }
                    Game.Bag.RemoveLetterFromBag(Game.Grid[t.Ligne, t.Col].Letter.Char);
                    gfxTile.Status = TileStatus.Exposed;
                    gfxTile.IsEnabled = false;
                }
                //if (addMove)
                //{
                //    p.Moves.Add(word);
                //    Game.Resolver.PlayedWords.Add(word);
                //    DisplayPlayerWord(word);
                //}
                if (validateWord)
                    Game.IsPlayer1 = !Game.IsPlayer1;


            }
            else return 0;

            //RefreshBoard(Game.Grid);
            return points;
        }

        private void btnAutoplay1_Clicked(object sender, EventArgs e)
        {
            PlayDemo(500);
        }
    }
}
