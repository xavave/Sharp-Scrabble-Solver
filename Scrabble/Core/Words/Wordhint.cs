using DawgResolver;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Scrabble.Core.Words
{
    public class WordHint : ListBox
    {
        private ToolTip toolTip1;
        private System.ComponentModel.IContainer components;

        public ScrabbleForm ScrabbleForm { get; set; }
        public List<RackTile> RackTiles = new List<RackTile>();
        public WordHint(ScrabbleForm _scrabbleForm)
        {
            InitializeComponent();
            this.Enabled = true;
            this.Location = new Point(836, 500);
            this.Name = "wordHintTxtBox";
            //this.ScrollBars = ScrollBars.Both;
            this.Font = new Font("Verdana", 8F, FontStyle.Regular);
            this.Size = new Size(327, 335);
            this.TabIndex = 3;
            this.ScrabbleForm = _scrabbleForm;
            this.SelectionMode = SelectionMode.One;
            this.SelectedIndexChanged += WordHint_SelectedIndexChanged;
            this.MouseMove += listBox_MouseMove;
            ScrabbleForm.Controls.Add(this);
        }
        private void listBox_MouseMove(object sender, MouseEventArgs e)
        {
            ListBox lb = (ListBox)sender;
            int index = lb.IndexFromPoint(e.Location);

            if (index >= 0 && index < lb.Items.Count)
            {
                string toolTipString = lb.Items[index].ToString();

                // check if tooltip text coincides with the current one,
                // if so, do nothing
                if (toolTip1.GetToolTip(lb) != toolTipString)
                    toolTip1.SetToolTip(lb, toolTipString);
            }
            else
                toolTip1.Hide(lb);
        }
        private void WordHint_SelectedIndexChanged(object sender, EventArgs e)
        {

            var currentTiles = ScrabbleForm.TileManager.Tiles.OfType<ScrabbleTile>().Where(t => t.TileInPlay).ToList();
            ScrabbleForm.TileManager.ResetTilesOnBoardFromTurn();
            //currentTiles.ForEach(ti =>
            //{
            //    if (ti.TileInPlay)
            //    {

            //        ti.Text = "";
            //    }
            //    ti.TileInPlay = false;
            //    ti.ClearHighlight();
            //});
            //if (this.SelectedIndex > -1)
            //    SetHintWord(this.Items[this.SelectedIndex] as Word, ScrabbleForm.TileManager.Tiles);

        }

        public void PrintGridConsole(ScrabbleTile[,] Tiles)
        {

            for (int x = 0; x < Game.BoardSize; x++)
            {
                for (int y = 0; y < Game.BoardSize; y++)
                {
                    var tile = ScrabbleForm.Game.Grid[x, y];

                    var txt = tile.IsEmpty ? "#" : tile.Letter.ToString();
                    Debug.Write(txt);
                }
                Debug.WriteLine("|");
            }
            Debug.WriteLine("___________________________");
        }
        //public bool CheckHintWord(Word word, ITile[,] scrabbleTile = null)
        //{
        //    if (word.Direction == MovementDirection.None)
        //    {
        //        word.StartX = 7;
        //        word.StartY = 7;
        //        word.EndX = 7 + word.Text.Length;
        //        word.EndY = 7+1;
        //        word.Direction = MovementDirection.Across;
        //    }
        //    for (int i = 0; i < word.Text.Length; i++)
        //    {
        //        var tile = scrabbleTile[word.StartX + (word.Direction == MovementDirection.Across ? i : 0), word.StartY + (word.Direction == MovementDirection.Down ? i : 0)];

        //        if (tile.Text == "")
        //        {
        //            tile.Text = word.Text[i].ToString();
        //            tile.TileInPlay = true;
        //        }

        //    }
        //    //PrintGridConsole(scrabbleTile);
        //    var valid = ScrabbleForm.WordValidator.ValidateWordsInPlay(scrabbleTile).Valid;


        //    return valid;

        //}
        //public bool SetHintWord(Word word, ITile[,] scrabbleTile = null)
        //{

        //    //if (word.Direction == MovementDirection.None)
        //    //{
        //    //    word.StartX = 7;
        //    //    word.StartY = 7;
        //    //    word.Direction = MovementDirection.Across;
        //    //}

        //    //for (int i = 0; i < word.Text.Length; i++)
        //    //{
        //    //    var tile = scrabbleTile[word.StartX + (word.Direction == MovementDirection.Across ? i : 0), word.StartY + (word.Direction == MovementDirection.Down ? i : 0)];
        //    //    if (tile.Text == "")// || tile.Text == word.Text[i].ToString() && ScrabbleForm.WordValidator.GetSurroundingWords(tile.XLoc, tile.YLoc).All(w => w.Valid))
        //    //                        //{
        //    //        tile.Text = word.Text[i].ToString();


        //    //}
        //    //return ScrabbleForm.WordValidator.ValidateWordsInPlay(scrabbleTile).Valid;
        //}

        public void ClearText()
        {
            this.Items.Clear();
        }
        public void AddWord(Word word)
        {
            var idx = this.Items.Add(word);

        }
        public void AddString(string word)
        {
            var idx = this.Items.Add(word);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            this.ResumeLayout(false);

        }
    }
}
