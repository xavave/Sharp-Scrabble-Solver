using DawgResolver;
using DawgResolver.Model;
using System;
using System.Linq;
using System.Windows.Forms;

namespace Dawg.Resolver.Winform.Test
{
    public partial class Form3 : Form
    {
        public Game Game { get; set; }
        public Form3()
        {
            InitializeComponent();
            Cursor.Current = Cursors.WaitCursor;
            Game = new Game();
            Game.InitBoard();
            textBox3.Text = Game.Bag.GetBagContent();
            var t = Game.Grid[4, 7];
            //t.SetWord(Game.Player1, "famille", MovementDirection.Down, true);
            //t.SetWord(Game.Player1, "foin", MovementDirection.Across, true);
            txtGrid2.Text = Game.GenerateTextGrid(Game.Grid, true);

            //for (int i = 0; i < 15; i++)
            //{
            //    groupBox1.Controls.Add(new FormTile(Game, new Tile(Game, 0, i)) { Text = $"{i + 1}", Name = $"col{i}" });
            //    groupBox1.Controls.Add(new FormTile(Game, new Tile(Game, i, 0)) { Text = $"{i + 1}", Name = $"ligne{i}" });
            //}

            foreach (var tile in Game.Grid)
            {
                groupBox1.Controls.Add(new FormTile(Game, tile));
            }
            Cursor.Current = Cursors.Default;
        }


        private void button1_Click_1(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            Game.Bag.GetNewRack(Game.Player1, textBox1.Text);
            lsb.DisplayMember = "DisplayText";
            var ret = Game.Resolver.FindMoves(Game.Player1, 500);
            lsb.Items.Clear();
            foreach (var r in ret)
                lsb.Items.Add(r);
            Cursor.Current = Cursors.Default;
        }

        private void lsb_SelectedIndexChanged(object sender, EventArgs e)
        {
            var word = lsb.SelectedItem as Word;
            if (word == null) return;
            Game.ClearTilesInPlay(Game.Player1);
            word.SetWord(Game.Player1, false);

            RefreshBoard(Game.Grid);
            textBox1.Text = new string(Game.Player1.Rack.Select(r => r.Char).ToArray());

            textBox3.Text = Game.Bag.GetBagContent();
        }

        private VTile[,] RefreshBoard(VTile[,] grid)
        {
            for (int ligne = 0; ligne <= grid.GetUpperBound(0); ligne++)
            {
                for (int col = 0; col <= grid.GetUpperBound(1); col++)
                {
                    var formTile = groupBox1.Controls.Find($"t{ligne}_{col}", false).First() as FormTile;
                    formTile.Tile = grid[ligne, col];
                    formTile.Text = formTile.Tile.Letter.Char.ToString();
                }
            }
            txtGrid2.Text = Game.GenerateTextGrid(Game.Grid, true);
            return grid;
        }

        private void btnTranspose_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            var toto = new VTile[15, 15];
            toto = Game.Grid.Transpose(Game);
            Game.Grid = RefreshBoard(toto);
            Cursor.Current = Cursors.Default;
        }

        private void btnBackToRack_Click(object sender, EventArgs e)
        {
            Game.ClearTilesInPlay(Game.Player1);
        }
    }
}
