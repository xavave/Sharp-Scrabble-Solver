using DawgResolver;
using DawgResolver.Model;
using System;
using System.Drawing;
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
            //t.SetWord(Game.Player1, "famille", MovementDirection.Down, true);
            //t.SetWord(Game.Player1, "foin", MovementDirection.Across, true);
            txtGrid2.Text = Game.GenerateTextGrid(Game.Grid, true);
            CustomGroupBox.SuspendDrawing(groupBox1.Parent);
            for (int i = 0; i < 15; i++)
            {
                groupBox1.Controls.Add(new FormTile(Game, new Tile(Game, 0, i), $"header_col{i}") { Text = $"{i + 1}" });
                groupBox1.Controls.Add(new FormTile(Game, new Tile(Game, i, 0), $"header_ligne{i}") { Text = $"{Game.Alphabet[i].Char}" });
            }

            foreach (var tile in Game.Grid)
            {
                groupBox1.Controls.Add(new FormTile(Game, tile));
            }
            CustomGroupBox.ResumeDrawing(groupBox1.Parent);
            Cursor.Current = Cursors.Default;
        }


        private void lsb_SelectedIndexChanged(object sender, EventArgs e)
        {
            var word = lsb.SelectedItem as Word;
            if (word == null) return;
            Game.ClearTilesInPlay(Game.Player1);
            word.SetWord(Game.Player1, false);

            RefreshBoard(Game.Grid);
          
        }

        private VTile[,] RefreshBoard(VTile[,] grid)
        {
            //CustomGroupBox.SuspendDrawing(groupBox1.Parent);
            for (int ligne = 0; ligne <= grid.GetUpperBound(0); ligne++)
            {
                for (int col = 0; col <= grid.GetUpperBound(1); col++)
                {
                    var formTile = groupBox1.Controls.Find($"t{ligne}_{col}", false).First() as FormTile;
                    formTile.Tile = grid[ligne, col];
                    formTile.Text = formTile.Tile.Letter.Char.ToString();
                }
            }
            //CustomGroupBox.ResumeDrawing(groupBox1.Parent);
            txtGrid2.Text = Game.GenerateTextGrid(Game.Grid, true);
            txtRack.Text = Game.Player1.Rack.String();
            textBox3.Text = Game.Bag.GetBagContent();
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
            if (ret.Any()) txtRack.Text = ret.String();
            Game.Grid = RefreshBoard(Game.Grid);
        }

        private void btnValidate_Click(object sender, EventArgs e)
        {
            foreach (var t in Game.ValidateWords())
            {
                var frmTile = groupBox1.Controls.Find($"t{t.Ligne}_{t.Col}", false).First() as FormTile;
                frmTile.ReadOnly = true;
                frmTile.BackColor = Color.LightYellow;
                Game.Bag.GetLetterInFlatList(t.Letter.Char);
            }


            var newRack = Game.Bag.GetNewRack(Game.Player1, 7 - txtRack.Text.Count());
            if (newRack.Any())
                txtRack.Text = newRack.String();

            Game.Grid= RefreshBoard(Game.Grid);

        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            Game.Bag.GetNewRack(Game.Player1, 7 - txtRack.Text.Count(), txtRack.Text);
            lsb.DisplayMember = "DisplayText";
            var ret = Game.Resolver.FindMoves(Game.Player1, 100);
            lsb.Items.Clear();
            foreach (var r in ret)
                lsb.Items.Add(r);
            Cursor.Current = Cursors.Default;
        }
    }
}
