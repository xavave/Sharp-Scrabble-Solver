using DawgResolver;
using DawgResolver.Model;
using System;
using System.Linq;
using System.Windows.Forms;

namespace Dawg.Resolver.Winform.Test
{
    public partial class Form2 : Form
    {
        public Game Game { get; set; }
        public Form2()
        {
            InitializeComponent();
            Cursor.Current = Cursors.WaitCursor;
            Game = new Game();
            Game.InitBoard();
            textBox3.Text = Game.Bag.GetBagContent();
            var t = Game.Grid[4, 7];
            t.SetWord(Game.Player1, "famille", MovementDirection.Down, true);
            t.SetWord(Game.Player1, "foin", MovementDirection.Across, true);
            txtGrid2.Text = Game.GenerateTextGrid(Game.Grid, true);

            for (int i = 0; i < 15; i++)
            {
                groupBox1.Controls.Add(new FormTile(Game, new Tile(Game, 0, i)) { Text = $"{i+1}",Name=$"col{i}" });
                groupBox1.Controls.Add(new FormTile(Game, new Tile(Game, i, 0)) { Text = $"{i + 1}" , Name = $"ligne{i}" });
            }

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
            var ret = Game.Resolver.FindMoves(Game.Player1,500);
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
          
            RefreshBoard();
            textBox1.Text = new string(Game.Player1.Rack.Select(r => r.Char).ToArray());
           
            textBox3.Text = Game.Bag.GetBagContent();
        }

        private void RefreshBoard()
        {
            foreach (var tile in Game.Grid.OfType<VTile>())
            {
                var formTile = groupBox1.Controls.Find($"t{tile.Ligne}_{tile.Col}", false).First() as FormTile;
                formTile.Text = tile.Letter.Char.ToString();
            }
            txtGrid2.Text = Game.GenerateTextGrid(Game.Grid, true);
        }

        private void btnTranspose_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            Game.Grid = Game.Grid.Transpose();
            RefreshBoard();
            Cursor.Current = Cursors.Default;
        }

        private void btnBackToRack_Click(object sender, EventArgs e)
        {
            Game.ClearTilesInPlay(Game.Player1);
        }
    }
}
