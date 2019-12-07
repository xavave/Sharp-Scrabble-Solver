using DawgResolver;
using DawgResolver.Model;
using System;
using System.Linq;
using System.Windows.Forms;

namespace Dawg.Resolver.Winform.Test
{
    public partial class Form3: Form
    {
        public Game Game { get; set; }
        public Form3()
        {
            InitializeComponent();
            Game = new Game();
            textBox3.Text = Game.Bag.GetBagContent();
            var t = Game.Grid[7, 5];
            t.SetWord(Game.Player1, "famille", MovementDirection.Across,true);
            t.SetWord(Game.Player1, "foin", MovementDirection.Down,true);
            textBox2.Text = Game.GenerateTextGrid(Game.Grid, false);
            txtGrid2.Text = Game.GenerateTextGrid(Game.Grid, null);
        }

       
        private void button1_Click_1(object sender, EventArgs e)
        {
            Game.Bag.GetNewRack(Game.Player1, textBox1.Text);
            lsb.DisplayMember = "DisplayText";
            var ret = Game.Resolver.FindMoves(Game.Player1);
            lsb.Items.Clear();
            foreach (var r in ret)
                lsb.Items.Add(r);
        }

        private void lsb_SelectedIndexChanged(object sender, EventArgs e)
        {
            var word = lsb.SelectedItem as Word;
            if (word == null) return;
            Game.ClearTilesInPlay(Game.Player1);
            word.SetWord(Game.Player1,false);
            textBox1.Text = new string(Game.Player1.Rack.Select(r => r.Char).ToArray());
            textBox2.Text = Game.GenerateTextGrid(Game.Grid, false);
            txtGrid2.Text = Game.GenerateTextGrid(Game.Grid, null);
            textBox3.Text = Game.Bag.GetBagContent();
        }
    }
}
