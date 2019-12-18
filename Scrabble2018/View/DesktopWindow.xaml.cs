using DawgResolver;
using DawgResolver.Model;

using Scrabble2018.Model;
using Scrabble2018.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Scrabble2018
{

    public partial class DesktopWindow : Window, IView, INotifyPropertyChanged
    {
        Controller.GameGUI game;
        public Game drGame { get; set; }
        List<TextBox> RackTileButtons;
        private int ThisPlayer;
        int PlayerNow;

        public ObservableCollection<TextBoxTile> BoardTiles { get => boardTiles; set { boardTiles = value; } }
        List<TextBox> ListSwapRackButton = new List<TextBox>();
        public DesktopWindow(int P, Controller.GameGUI g)
        {
            InitializeComponent();
            ThisPlayer = P;
            game = g;
            game.Subs(this);
            drGame = new Game(Dictionnaire.NomDicoDawgODS7);
            foreach (var t in drGame.InitBoard())
            {
                var b = new TextBoxTile(t);
                b.TextChanged += B_TextChanged;
                b.FontSize = 33;
                b.CharacterCasing = CharacterCasing.Upper;
                b.MaxLength = 1;
                b.DataContext = t;
                BoardTiles.Add(b);
            }

            GameState.GSInstance.OnStateChanged += OnStateChanged;
            this.Title = "Player " + (P + 1) + " - ScrabbleDesktop";
            ListSwapRackButton = new List<TextBox>();
            RackTileButtons = new List<TextBox>();

            // Adding rack buttons
            for (int i = 0; i < 7; ++i)
            {
                TextBox t = new TextBox();
                //t.Click += Poster;
                t.FontSize = 33;
                HandGrid.Children.Add(t);
                t.Background = Brushes.Chocolate;

                RackTileButtons.Add(t);
            }
            LogBoardWriter(Welcome.WelcomeText);
            LogBoardWriter("Game starts...");
            LogBoardWriter("This is a " + GameState.GSInstance.NumOfPlayers + " players game.");

            //foreach (KeyValuePair<int, Letter> kvp in GameStartDraw.Drawn)
            //{
            //    LogBoardWriter("Player " + (kvp.Key + 1) + " gets " + kvp.Value.Letter.Char + "!");
            //}
            LogBoardWriter("Player " + (GameState.GSInstance.PlayerNow + 1) + " first!");
            GetNewRackLetters();


            StorageLbl.Content = '\0';
        }

        private void GetNewRackLetters()
        {
            //TODO
            //try
            //{
            //    drGame.Bag.GetLetters(drGame.Player1);
            //    for (int i = 0; i < RackTileButtons.Count; ++i)
            //    {
            //        if (i < drGame.Bag.LeftLettersCount)
            //        {
            //            char c = rack[i].Char;
            //            RackTileButtons[i].Text = c.ToString();
            //            //if (GameState.GSInstance.PlayerNow == ThisPlayer) { RackTileButtons[i].IsEnabled = true; EnableAll(); }

            //            //else { RackTileButtons[i].IsEnabled = false; DisableAll(); }
            //        }
            //    }
            //    txtBagContent.Text = drGame.Bag.GetBagContent();
            //}
            //catch (ArgumentException)
            //{
            //    LogBoardWriter("Bag is empty!");
            //}
        }

        private void B_TextChanged(object sender, TextChangedEventArgs e)
        {
            var tile = (sender as TextBoxTile);
            if (!string.IsNullOrWhiteSpace(tile.Text) && char.IsLetter(tile.Text[0]))
            {
                tile.Letter = Game.Alphabet.Find(a => a.Char == tile.Text[0]);
                drGame.Grid[tile.Ligne, tile.Col] = (VTile)tile;

            }

        }

        Button LastButton;
        private void Poster(object sender, RoutedEventArgs e)
        {
            //Button clickedButton = sender as Button;
            //if( clickedButton == null ) // safety reason
            //    return;
            //if( LastButton == SwapButton )
            //{
            //    ListSwapRackButton.Add(clickedButton);
            //    clickedButton.IsEnabled = false;
            //}
            //else
            //{
            //    if( Convert.ToChar(StorageLbl.Content) == '\0' || LastButton == null )
            //    {
            //        StorageLbl.Content = clickedButton.Content;
            //        LastButton = clickedButton;
            //        clickedButton.IsEnabled = false;
            //    }
            //    else
            //    {
            //        LastButton.IsEnabled = true;
            //        StorageLbl.Content = clickedButton.Content;
            //        LastButton = clickedButton;
            //        clickedButton.IsEnabled = false;
            //    }
            //}


        }

        private void Copier(object sender, RoutedEventArgs e)
        {
            //Button clickedButton = sender as Button;
            //if( clickedButton == null ) // safety reason
            //    return;
            //if( Convert.ToChar(clickedButton.Content) != '\0' )
            //    return;
            //clickedButton.Content = StorageLbl.Content;
            //StorageLbl.Content = '\0';

            //for( int i = 0 ; i < BoardButtons.GetLength(0) && Convert.ToChar(clickedButton.Content) != '\0' ; ++i )
            //{
            //    for( int j = 0 ; j < BoardButtons.GetLength(1) ; ++j )
            //    {
            //        if( BoardButtons[i, j] == clickedButton )
            //        {
            //            if( (char) BoardButtons[i, j].Content == '-' )
            //            {
            //                BlankTileForm bf = new BlankTileForm();
            //                if( bf.ShowDialog() == true )
            //                {
            //                    BoardButtons[i, j].Content = bf.List.SelectedItem;
            //                }
            //            }
            //            BoardCharView[i, j] = (char) BoardButtons[i, j].Content;
            //            game.moveRecorder.Record(i, j);
            //        }
            //    }

            //}
        }



        private void UpdatePlayerInfoLbl(int p)
        {
            PlayerInfoLbl.Content = "Player " + (p + 1) + "'s turn.";
        }

        private void LoadBoardView()
        {
            //foreach (VTile t in drGame.Grid)
            //{
            //    BoardTiles[t.Ligne, t.Col] = t;
            //}

        }

        private void LogBoardWriter(string s)
        {
            LogBoard.Text = s + "\n" + LogBoard.Text;
        }

        //private void GetNewTiles()
        //{
        //    List<char> LoC = new List<char>();
        //    foreach (var b in RackTileButtons)
        //    {
        //        if (b.IsEnabled == false)
        //        {
        //            LoC.Add((char)b.Text[0]);
        //        }
        //    }
        //    game.GetNewTiles(LoC, ThisPlayer);
        //}

        private void Retry()
        {
            foreach (var b in RackTileButtons)
            {
                if (b.IsEnabled == false)
                {
                    b.IsEnabled = true;
                }
            }
            game.ClearMovement();
        }

        //private void LoadRackView()
        //{
        //    DisableAll();
        //    for (int i = 0; i < game.gs.ListOfPlayers[ThisPlayer].PlayingTiles.Count; ++i)
        //    {
        //        char c = game.gs.ListOfPlayers[ThisPlayer].PlayingTiles[i].Letter.Char;
        //        RackTileButtons[i].Text = c.ToString();
        //    }
        //    if (ThisPlayer == GameState.GSInstance.PlayerNow)
        //    {
        //        EnableAll(); LogBoardWriter("Your turn!");
        //    }
        //}


        //private void ListingPrevWords()
        //{
        //    string s = "Player " + (GameState.GSInstance.PrevPlayer + 1) + " made the words: ";
        //    foreach (KeyValuePair<string, int> kvp in GameState.GSInstance.CorrectWords)
        //    {
        //        s += kvp.Key + "(" + kvp.Value + " scores) ";
        //    }
        //    LogBoardWriter(s);
        //}

        private void ValidateButton_Click(object sender, RoutedEventArgs e)
        {
            // UpdateBoard();
            //if (game.Validate(BoardCharView))
            //{
            //    GetNewTiles();
            //    game.UpdateState(BoardCharView);
            //    PlayerNow = GameState.GSInstance.PlayerNow;
            //    UpdatePlayerInfoLbl(PlayerNow);

            //}
            //else
            //{
            //    LogBoardWriter("Game Judge: \"You didn't score. Please try again!\"");
            //    LoadBoardView();
            //    Retry();
            //}
        }

        private void PassButton_Click(object sender, RoutedEventArgs e)
        {
            GetNewRackLetters();
            //LogBoardWriter("Player " + (PlayerNow + 1) + " decides to pass the turn!");
            //GameState.GSInstance.GamePass();
            //UpdatePlayerInfoLbl(GameState.GSInstance.PlayerNow);
        }


        bool SwapMode = false;
        private ObservableCollection<TextBoxTile> boardTiles = new ObservableCollection<TextBoxTile>();

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string propertyName = "")
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private void SwapButton_Click(object sender, RoutedEventArgs e)
        {
            //LastButton = SwapButton;
            //if (!SwapMode)
            //{
            //    SwapMode = true;
            //    LoadBoardView();
            //    LoadRackView();
            //    SwapButton.Content = "FINISH";
            //    ValidateButton.IsEnabled = false;
            //    PassButton.IsEnabled = false;
            //    ReloadButton.IsEnabled = false;
            //    if (game.CanSwap())
            //    {
            //        LogBoardWriter("Select the tiles you don't want...Then press the FINISH button.");
            //    }
            //    else
            //    {
            //        LogBoardWriter("You can't swap tiles now becuase less than 7 tiles are left in the bag!");
            //        SwapButton.IsEnabled = false;
            //    }
            //}
            //else
            //{
            //    foreach (var b in ListSwapRackButton)
            //    {
            //        b.Text = game.SwapChar((char)b.Text[0]).ToString();
            //    }
            //    LastButton = null;
            //    SwapMode = false;
            //    ValidateButton.IsEnabled = true;
            //    ListSwapRackButton.Clear();
            //    PassButton.IsEnabled = true;
            //    SwapButton.Content = "SWAP";
            //    LogBoardWriter("Player " + (PlayerNow + 1) + " finishing swapping tiles!");
            //    game.UpdateState(null);
            //}
        }

        private void ReloadButton_Click(object sender, RoutedEventArgs e)
        {
            LoadBoardView();
            Retry();
        }

        private void EnableAll()
        {
            //this.Topmost = true;
            //foreach (var b in RackTileButtons)
            //{
            //    b.IsEnabled = true;
            //}
            //ValidateButton.IsEnabled = true;
            //PassButton.IsEnabled = true;
            //SwapButton.IsEnabled = true;
            //ReloadButton.IsEnabled = true;
            //HelpButton.IsEnabled = true;
        }

        private void DisableAll()
        {
            //this.Topmost = false;
            //foreach (var b in RackTileButtons)
            //{
            //    b.IsEnabled = false;
            //}
            //ValidateButton.IsEnabled = false;
            //PassButton.IsEnabled = false;
            //SwapButton.IsEnabled = false;
            //ReloadButton.IsEnabled = false;
            //HelpButton.IsEnabled = false;
        }

        private void DisableEverthing()
        {
            //DisableAll();
            //foreach (Button b in BoardGrid.Children)
            //{
            //    b.IsEnabled = false;
            //}
        }

        public void OnStateChanged()
        {
            //LogBoardWriter("Player " + (GameState.GSInstance.PrevPlayer + 1) + " finished his turn!");
            //if (game.GameEnd())
            //{
            //    foreach (Player p in game.gs.ListOfPlayers)
            //    {
            //        LogBoardWriter(game.gs.ListOfPlayers[PlayerNow].ToString());
            //    }
            //    game.gs.ListOfPlayers.Sort();
            //    LogBoardWriter("Game Winner is Player " + (game.gs.ListOfPlayers[0].Id + 1) + " with scores" + (game.gs.ListOfPlayers[0].Score) + "!!!");
            //    LogBoardWriter("Close this window to restart Scrabble!");
            //    DisableEverthing();
            //    return;
            //    }
            //        //Enable all buttons
            //        if (GameState.GSInstance.LastAction == "play")
            //        {
            //            ListingPrevWords();
            //    LogBoardWriter(game.gs.ListOfPlayers[GameState.GSInstance.PrevPlayer].ToString());
            //}
            //        else if (GameState.GSInstance.LastAction == "pass")
            //        {
            //            LogBoardWriter("Player " + (GameState.GSInstance.PrevPlayer + 1) + " passed!");
            //        }
            //        else if (GameState.GSInstance.LastAction == "swap")
            //        {
            //            LogBoardWriter("Player " + (GameState.GSInstance.PrevPlayer + 1) + " swapped his tiles!");
            //        }
            //        PlayerNow = GameState.GSInstance.PlayerNow;
            //        UpdatePlayerInfoLbl(GameState.GSInstance.PlayerNow);
            //LoadBoardView();
            //LoadRackView();
        }

        //private void Window_KeyDown(object sender, KeyEventArgs e)
        //{
        //    //if( ( e.Key >= Key.A ) && ( e.Key <= Key.Z ) && ThisPlayer == GameState.GSInstance.PlayerNow )
        //    //{
        //    //    foreach( var b in RackTileButtons )
        //    //    {
        //    //        KeyConverter kc = new KeyConverter();
        //    //        var str = kc.ConvertToString(e.Key);
        //    //        if( b.Text.ToString() == str && b.IsEnabled == true )
        //    //        {
        //    //            Poster(b, null);
        //    //        }
        //    //    }
        //    //    //do some stuff here
        //    //    return;
        //    //}
        //}

        private void HelpButton_Click(object sender, RoutedEventArgs e)
        {
            HelpWindow hw = new HelpWindow();
            hw.ShowDialog();
        }

        private void HintButton_Click(object sender, RoutedEventArgs e)
        {
            //drGame.Resolver.NewDraught(drGame.Player1, "EUDNA*A");

            var ret = drGame.Resolver.FindMoves(drGame.Player1);
            lstHint.ItemsSource = ret;
        }

        private void lstHint_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var addedItems = e.AddedItems;
            if (addedItems.Count > 0)
            {
                var word = addedItems[0] as Word;
                if (word != null)
                {
                    foreach (var tile in drGame.Grid.OfType<VTile>().Where(t => !t.IsValidated))
                    {
                        var bt = BoardTiles.First(t => t.Ligne == tile.Ligne && t.Col == tile.Col);
                        bt.Text = "";
                        tile.IsValidated = false;
                    }
                    //drGame.ClearTilesInPlay(drGame.Player1);
                    word.SetWord(drGame.Player1,false);
                    txtBagContent.Text = drGame.Bag.GetBagContent();
                    foreach (var tile in drGame.Grid.OfType<VTile>().Where(t => !t.IsEmpty))
                    {
                        var bt = BoardTiles.First(t => t.Ligne == tile.Ligne && t.Col == tile.Col);
                        bt.Text = tile.Letter.Char.ToString();
                    }
                }

            }
        }

        private void myMatrix_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            //if ((e.Key >= Key.A) && (e.Key <= Key.Z) && ThisPlayer == GameState.GSInstance.PlayerNow)
            //{
            //    foreach (var b in RackTileButtons)
            //    {
            //        KeyConverter kc = new KeyConverter();
            //        var str = kc.ConvertToString(e.Key);
            //        if (b.Text.ToString() == str && b.IsEnabled == true)
            //        {
            //            //Poster(b, null);
            //        }
            //    }
            //    //do some stuff here
            //    return;
            //}
        }
    }
}
