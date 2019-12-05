using Dawg;
using DawgResolver;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Cefsharp;
namespace dawgResolverTestGui
{
    public partial class Form1 : Form
    {
        Game Game { get; set; }
        public Form1()
        {
            InitializeComponent();
            Game = new Game();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            //Pour mes tests 
            var t = Game.Grid[7, 5];
            t.SetWord(Game, "famille", MovementDirection.Across);
            t.SetWord(Game, "foin", MovementDirection.Down);

            Game.Resolver.NewDraught(Game.Player1, textBox1.Text.ToUpper());

            var sw = Stopwatch.StartNew();
            var ret = Game.Resolver.FindMoves(Game.Player1);
            sw.Stop();
            lblElapsed.Text = sw.ElapsedMilliseconds.ToString();
            listBox1.BeginUpdate();
            listBox1.DataSource = ret;
            listBox1.EndUpdate();

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var ret = listBox1.SelectedItem as Word;
            if (ret == null) return;
            var t = ret.StartTile;
            t.SetWord(Game, ret.Text, ret.Direction);
           webView1.Ite = Game.GenerateHtml(Game.Grid);
            
            
          

        }
    }
}
