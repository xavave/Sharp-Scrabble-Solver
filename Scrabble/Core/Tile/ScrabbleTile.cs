using Dawg;
using Scrabble.Core.Tile;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Scrabble.Core
{
    public interface ITile
    {
        void ClearHighlight();
        void OnHighlight(bool valid);
        ScrabbleForm ScrabbleForm { get; set; }
        int XLoc { get; set; }
        int YLoc { get; set; }
        TileType TileType { get; set; }
        string Text { get; set; }
        bool TileInPlay { get; set; }
    }
    //public class VirtualTile : ITile
    //{
    //    public ScrabbleForm ScrabbleForm { get; set; }
    //    public VirtualTile(ScrabbleForm scrabbleForm)
    //    {
    //        ScrabbleForm = scrabbleForm;
    //    }
    //    public int XLoc { get; set; }
    //    public int YLoc { get; set; }
    //    public TileType TileType { get; set; }
    //    public string Text { get; set; }
    //    public bool TileInPlay { get; set; }

    //    public void ClearHighlight()
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public void OnHighlight(bool valid)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}

    public class ScrabbleTile : TextBox, ITile
    {
        public ScrabbleTile(ScrabbleForm scrabbleForm)
        {
            InitializeComponent();
            ScrabbleForm = scrabbleForm;
            this.CharacterCasing = CharacterCasing.Upper;

            this.TextChanged += (s, e) =>
            {
                var tile = (ScrabbleTile)s;

                tile.OnLetterPlaced(this.Text);
                var rackTile = ScrabbleForm.PlayerManager.CurrentPlayer.Tiles.Find(r => r.Text == this.Text);
                if (rackTile != null)
                    rackTile.Text = "";
            };
        }
        public ScrabbleForm ScrabbleForm { get; set; }
        public int XLoc { get; set; }
        public int YLoc { get; set; }

        public bool TileInPlay { get; set; }
        public TileType TileType { get; set; }

        public void OnLetterPlaced(string letter)
        {
            this.Text = letter;
            this.TileInPlay = true;

            SetRegularBackgroundColour();
        }

        public void OnLetterRemoved()
        {
            this.Text = string.Empty;
            this.TileInPlay = false;
            SetRegularBackgroundColour();
        }

        public void OnHighlight(bool valid)
        {
            this.BorderStyle = BorderStyle.Fixed3D;
            this.BackColor = valid ? Color.LimeGreen : Color.DarkRed;
            //this.FlatAppearance.BorderSize = 5;
        }

        public void ClearHighlight()
        {
            this.BorderStyle = BorderStyle.FixedSingle;
            SetRegularBackgroundColour();
        }

        public void SetRegularBackgroundColour()
        {
            switch (this.TileType)
            {
                case TileType.Regular:
                    this.BackColor = SystemColors.ButtonFace;
                    break;
                case TileType.Center:
                    this.BackColor = Color.Purple;
                    break;
                case TileType.TripleWord:
                    this.BackColor = Color.Orange;
                    break;
                case TileType.TripleLetter:
                    this.BackColor = Color.ForestGreen;
                    break;
                case TileType.DoubleWord:
                    this.BackColor = Color.OrangeRed;
                    break;
                case TileType.DoubleLetter:
                    this.BackColor = Color.RoyalBlue;
                    break;
                default:
                    this.BackColor = SystemColors.ButtonFace;
                    break;
            }

            if (!string.IsNullOrEmpty(this.Text))
                this.BackColor = Color.Goldenrod;

            if (this.TileInPlay)
                this.BackColor = Color.Yellow;
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // ScrabbleTile
            // 
            this.AllowDrop = true;
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.ScrabbleTile_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.ScrabbleTile_DragEnter);


            this.ResumeLayout(false);

        }


        private void ScrabbleTile_DragDrop(object sender, DragEventArgs e)
        {
            this.OnLetterPlaced(e.Data.GetData(DataFormats.Text).ToString());

        }

        private void ScrabbleTile_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.Text))
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;
        }
    }
}
