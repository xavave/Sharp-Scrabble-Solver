using System;
using System.Windows.Forms;

using DawgResolver.Model;

namespace Dawg.Solver.Winform.Test
{
    public partial class FrmDebug : Form
    {
        public Game Game { get; set; }
        public FrmDebug()
        {
            InitializeComponent();
            Game =  Game.DefaultInstance;
            textBox3.Text = Game.Bag.GetBagContent();
            var t = Game.Grid[7, 5];
            t.SetWord("famille", MovementDirection.Across, true);
            t.SetWord( "foin", MovementDirection.Down, true);
            textBox2.Text = Game.GenerateTextGrid(Game.Grid, false);
            txtGrid2.Text = Game.GenerateTextGrid(Game.Grid, null);
        }


        private void button1_Click_1(object sender, EventArgs e)
        {
            Game.Bag.GetLetters(Game.Player1, textBox1.Text);
            lsb.DisplayMember = "DisplayText";
            var ret = Solver.DefaultInstance.FindMoves();
            lsb.Items.Clear();
            foreach (var r in ret)
                lsb.Items.Add(r);


        }

        private void lsb_SelectedIndexChanged(object sender, EventArgs e)
        {
            var word = lsb.SelectedItem as Word;
            if (word == null) return;
            //ClearTilesInPlay(Game.Player1);
            word.SetText("",false);
            textBox1.Text = Game.Player1.Rack.ToString();
            textBox2.Text = Game.GenerateTextGrid(Game.Grid, false);
            txtGrid2.Text = Game.GenerateTextGrid(Game.Grid, null);
            textBox3.Text = Game.Bag.GetBagContent();
        }
    }
}
