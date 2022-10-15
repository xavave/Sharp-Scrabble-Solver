using DawgResolver;
using DawgResolver.Model;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace DawgResolver.Resolver.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private Word selectedHintWord;
        private VTile[,] data;
        public Game game { get; set; }
        public MainWindow()
        {
            game = new Game(Dictionnaire.NomDicoDawgODS7);
            InitializeComponent();
            game.InitBoard();

        }
        public VTile[,] Data
        {
            get => this.data;

            private set
            {
                this.data = value;
                this.OnPropertyChanged();
            }
        }

        public Word SelectedHintWord
        {
            get => selectedHintWord; set
            {
                if (value != null && selectedHintWord != value)
                {
                    selectedHintWord = value;
                    selectedHintWord.StartTile.SetWord(new Player(game, "player 1"), selectedHintWord.Text, selectedHintWord.Direction);
                    UpdateData();
                }
            }
        }
        
        private void UpdateData()
        {
            this.Data = this.game.Grid;
            OnPropertyChanged();
        }
        public string[] RowHeaders
        {
            get => new[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O" };
        }
        public string[] ColumnHeaders
        {
            get => new[] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15" };
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string propertyName = "")
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private void OnNewGame(object sender, RoutedEventArgs e)
        {
            game = new Game(Dictionnaire.NomDicoDawgODS7);
            game.InitBoard();
        }

        private void btnHint_Click(object sender, RoutedEventArgs e)
        {
            //game.Resolver.NewDraught(game.Player1, txtRack.Text);
            lstHint.ItemsSource = game.Resolver.FindMoves(game.Player1);
        }
    }
}
