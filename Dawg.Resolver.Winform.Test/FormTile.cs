using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DawgResolver.Model;
using System.Runtime.InteropServices;

namespace Dawg.Resolver.Winform.Test
{
    
    public partial class FormTile : TextBox, VTile
    {
       
        public VTile Tile { get; set; }
        public Game Game { get; set; }
        public FormTile(Game g, VTile t, string tileName = "")
        {
           
            Game = g;
            Tile = t;
            this.Ligne = t.Ligne;
            this.Col = t.Col;
            this.AnchorLeftMinLimit = t.AnchorLeftMinLimit;
            this.AnchorLeftMaxLimit = t.AnchorLeftMaxLimit;
            this.Controlers = t.Controlers;
            this.Width = 30;
            this.Height = 28;
            this.MaxLength = 1;
            this.Font = new Font("Verdana", 14);
            this.CharacterCasing = CharacterCasing.Upper;
            this.BackColor = GetBackColor(t);
            Text = t.Letter.Char.ToString();
            if (tileName == "")
                Name = $"t{t.Ligne}_{t.Col}";
            else
                Name = tileName;
            Click += FormTile_Click;
            KeyUp += FormTile_KeyUp;

            if (Name.StartsWith($"header_col"))
                Location = new Point(15 + this.Width + t.Col * this.Width, 15 + t.Ligne * this.Height);
            else if (Name.StartsWith($"header_ligne"))
                Location = new Point(15 + t.Col * this.Width, 15 + this.Height + t.Ligne * this.Height);
            else
                Location = new Point(15 + this.Width + t.Col * this.Width, 15 + this.Height + t.Ligne * this.Height);
           
        }

        private void FormTile_KeyUp(object sender, KeyEventArgs e)
        {
            var frmTile = sender as FormTile;
            if (e.KeyCode >= Keys.A && e.KeyCode <= Keys.Z)
            {
                this.Letter = Game.Alphabet.Find(a => a.Char == e.KeyData.ToString().First());
                this.Text = this.Letter.Char.ToString();
                Game.Grid[Ligne, Col].Letter = this.Letter;
                GetNextTile(Keys.Right, frmTile);
            }
            else if (e.KeyCode == Keys.Back)
            {
                GetNextTile(Keys.Left, frmTile);
                this.Letter = new Letter();
                this.Text = char.MinValue.ToString();
                Game.Grid[Ligne, Col].Letter = this.Letter;
            }
            else
            {
                GetNextTile(e.KeyCode, frmTile);
            }
        }

        private void GetNextTile(Keys key, FormTile frmTile)
        {
            Control nextTile = null;
            Control parent = frmTile.Parent;
            if (key == Keys.Right)
                nextTile = parent.Controls.Find($"t{Ligne}_{Col + 1}", false).FirstOrDefault();
            else if (key == Keys.Left)
                nextTile = parent.Controls.Find($"t{Ligne}_{Col - 1}", false).FirstOrDefault();
            else if (key == Keys.Up)
                nextTile = parent.Controls.Find($"t{Ligne - 1}_{Col}", false).FirstOrDefault();
            else if (key == Keys.Down)
                nextTile = parent.Controls.Find($"t{Ligne + 1}_{Col}", false).FirstOrDefault();

            if (nextTile != null) nextTile.Focus();
        }

        private void FormTile_Click(object sender, EventArgs e)
        {
            var txt = sender as FormTile;
            var t = txt.Tile;
            var frm = this.Parent.Parent as Form2;
            var txtProps = frm.txtTileProps;
            txtProps.Text = $"[{t.Ligne},{t.Col}] => IsAnchor:{t.IsAnchor} IsEmpty :{t.IsEmpty} => {t}";
            txtProps.Text += Environment.NewLine;
            txtProps.Text += $"LetterMultiplier={t.LetterMultiplier}";
            txtProps.Text += Environment.NewLine;
            txtProps.Text += $"WordMultiplier={t.WordMultiplier}";
            txtProps.Text += Environment.NewLine;
            txtProps.Text += $"AnchorLeftMinLimit = {t.AnchorLeftMinLimit}";
            txtProps.Text += Environment.NewLine;
            txtProps.Text += $"AnchorLeftMaxLimit = {t.AnchorLeftMaxLimit}";
            txtProps.Text += Environment.NewLine;
            txtProps.Text += $"UpTile = {t.UpTile}";
            txtProps.Text += Environment.NewLine;
            txtProps.Text += $"DownTile = {t.DownTile}";
            txtProps.Text += Environment.NewLine;
            txtProps.Text += $"RightTile = {t.RightTile}";
            txtProps.Text += Environment.NewLine;
            txtProps.Text += $"LeftTile = {t.LeftTile}";
            txtProps.Text += Environment.NewLine;

            txtProps.Text += "Controlers:" + Environment.NewLine;
            foreach (var c in t.Controlers)
                txtProps.Text += $"{c.Key}:{c.Value}{Environment.NewLine}";
        }

        private Color GetBackColor(VTile t)
        {
            switch (t.TileType)
            {
                case TileType.TripleWord:
                    return Color.OrangeRed;
                case TileType.DoubleWord:
                    return Color.Coral;
                case TileType.DoubleLetter:
                    return Color.LightSkyBlue;
                case TileType.TripleLetter:
                    return Color.MediumBlue;
                case TileType.Center:
                    return Color.Gold;
                default:
                    return Color.Bisque;
            }
        }

        public Color Background { get; set; }
        public bool IsValidated { get; set; } = true;
        public bool FromJoker { get; set; } = false;
        public Dictionary<int, int> Controlers { get; set; } = new Dictionary<int, int>(27);
        public int Ligne { get; set; }
        public int Col { get; set; }
        public int LetterMultiplier { get; set; }
        public int WordMultiplier { get; set; }
        public Letter Letter { get; set; }
        public int AnchorLeftMaxLimit { get; set; }
        public int AnchorLeftMinLimit { get; set; }
        public bool IsAnchor
        {
            get
            {
                return Tile.IsAnchor;
            }
        }

        public TileType TileType
        {
            get
            {
                return Tile.TileType;

            }
        }

        public bool IsEmpty
        {
            get
            {
                return Tile.IsEmpty;
            }
        }

        public VTile LeftTile
        {
            get
            {
                return Tile.LeftTile;
            }
        }
        public VTile RightTile
        {
            get
            {
                return Tile.RightTile;
            }
        }
        public VTile DownTile
        {
            get
            {
                return Tile.DownTile;
            }
        }
        public VTile UpTile
        {
            get
            {
                return Tile.UpTile;
            }
        }

    }
}
