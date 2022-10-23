using System;
using System.Linq;
using System.Windows.Forms;

using DawgResolver;
using DawgResolver.Model;

namespace Dawg.Resolver.Winform.Test
{
    public partial class FrmDebug : Form
    {
        public Game Game { get; set; }
        public FrmDebug()
        {
            InitializeComponent();
            Game = new Game(Dictionnaire.NomDicoDawgEN_Collins);
            textBox3.Text = Game.Bag.GetBagContent();
            var t = Game.Grid[7, 5];
            t.SetWord(Game, "famille", MovementDirection.Across, true);
            t.SetWord(Game, "foin", MovementDirection.Down, true);
            textBox2.Text = Game.GenerateTextGrid(Game.Grid, false);
            txtGrid2.Text = Game.GenerateTextGrid(Game.Grid, null);
        }


        private void button1_Click_1(object sender, EventArgs e)
        {
            Game.Bag.GetLetters(Game.Player1, textBox1.Text);
            lsb.DisplayMember = "DisplayText";
            var ret = Game.Resolver.FindMoves(Game);
            lsb.Items.Clear();
            foreach (var r in ret)
                lsb.Items.Add(r);


        }

        private void lsb_SelectedIndexChanged(object sender, EventArgs e)
        {
            var word = lsb.SelectedItem as Word;
            if (word == null) return;
            //ClearTilesInPlay(Game.Player1);
            word.SetWord(false);
            textBox1.Text = new string(Game.Player1.Rack.Select(r => r.Char).ToArray());
            textBox2.Text = Game.GenerateTextGrid(Game.Grid, false);
            txtGrid2.Text = Game.GenerateTextGrid(Game.Grid, null);
            textBox3.Text = Game.Bag.GetBagContent();
        }
    }
}
