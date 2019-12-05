using DawgResolver;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DawgResolver.Resolver.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private Word selectedHintWord;

        public Game game { get; set; }
        public MainWindow()
        {
            game = new Game();
            InitializeComponent();
            game.InitBoard();

        }
        public Word SelectedHintWord
        {
            get => selectedHintWord; set
            {
                if (value!=null && selectedHintWord!=value)
                selectedHintWord = value;
                selectedHintWord.StartTile.SetWord(game, selectedHintWord.Text, selectedHintWord.Direction);
                OnPropertyChanged(nameof(Game.Grid));
            }
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
            game = new Game();
            game.InitBoard();
        }

        private void btnHint_Click(object sender, RoutedEventArgs e)
        {
            game.Resolver.NewDraught(game.Player1, txtRack.Text);
            lstHint.ItemsSource = game.Resolver.FindMoves(game.Player1);
        }
    }
}
