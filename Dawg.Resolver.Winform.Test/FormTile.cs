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
        const int WM_NCPAINT = 0x85;
        const uint RDW_INVALIDATE = 0x1;
        const uint RDW_IUPDATENOW = 0x100;
        const uint RDW_FRAME = 0x400;
        [DllImport("user32.dll")]
        static extern IntPtr GetWindowDC(IntPtr hWnd);
        [DllImport("user32.dll")]
        static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);
        [DllImport("user32.dll")]
        static extern bool RedrawWindow(IntPtr hWnd, IntPtr lprc, IntPtr hrgn, uint flags);
        Color borderColor = Color.DarkGray;
        public Color BorderColor
        {
            get { return borderColor; }
            set
            {
                borderColor = value;
                RedrawWindow(Handle, IntPtr.Zero, IntPtr.Zero, RDW_FRAME | RDW_IUPDATENOW | RDW_INVALIDATE);
            }
        }
        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            if (m.Msg == WM_NCPAINT && BorderColor != Color.Transparent &&
                BorderStyle == System.Windows.Forms.BorderStyle.Fixed3D)
            {
                var hdc = GetWindowDC(this.Handle);
                using (var g = Graphics.FromHdcInternal(hdc))
                using (var p = new Pen(BorderColor))
                    g.DrawRectangle(p, new Rectangle(0, 0, Width - 1, Height - 1));
                ReleaseDC(this.Handle, hdc);
            }
        }
        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            RedrawWindow(Handle, IntPtr.Zero, IntPtr.Zero,
                   RDW_FRAME | RDW_IUPDATENOW | RDW_INVALIDATE);
        }

        private bool isValidated;
        public string TxtInfos { get; set; }
        public VTile Tile { get; set; }
        public Game Game { get; set; }
        public FormTile(Game g, VTile t, string tileName = "", Color? color = null)
        {
            BorderStyle = BorderStyle.Fixed3D;
            Game = g;
            Tile = t;
            this.Ligne = t.Ligne;
            this.Col = t.Col;
            this.AnchorLeftMinLimit = t.AnchorLeftMinLimit;
            this.AnchorLeftMaxLimit = t.AnchorLeftMaxLimit;
            this.Controlers = t.Controlers;
            this.Width = 30;
            ReadOnly = false;
            this.Height = 28;
            this.MaxLength = 1;
            this.Font = new Font("Verdana", 14);
            this.CharacterCasing = CharacterCasing.Upper;
            if (!color.HasValue)
                this.BackColor = GetBackColor(t);
            else
                this.BackColor = color.Value;
            Text = t.Letter.Char.ToString();
            if (tileName == "")
                Name = $"t{t.Ligne}_{t.Col}";
            else
                Name = tileName;
            //Click += FormTile_Click;
            KeyUp += FormTile_KeyUp;

            if (Name.StartsWith($"header_col"))
                Location = new Point(15 + this.Width + t.Col * this.Width, 15 + t.Ligne * this.Height);
            else if (Name.StartsWith($"header_ligne"))
                Location = new Point(15 + t.Col * this.Width, 15 + this.Height + t.Ligne * this.Height);
            else
                Location = new Point(15 + this.Width + t.Col * this.Width, 15 + this.Height + t.Ligne * this.Height);

            if (Tile.FromJoker) this.BorderStyle = BorderStyle.Fixed3D;
        }

        private void FormTile_KeyUp(object sender, KeyEventArgs e)
        {
            var frmTile = sender as FormTile;
            var frm = frmTile.Parent.Parent as Form2;
            if (e.KeyCode >= Keys.A && e.KeyCode <= Keys.Z)
            {
                this.Letter = Game.Alphabet.Find(a => a.Char == e.KeyData.ToString().First());
                this.Text = this.Letter.Char.ToString();
                Game.Grid[Ligne, Col].Letter = this.Letter;
                Game.Grid[Ligne, Col].IsValidated = true;
                frmTile.BackColor = Game.IsPlayer1 ? frm.Player1MoveColor : frm.Player2MoveColor;
                GetNextTile(Keys.Right, frmTile);
            }
            else if (e.KeyCode == Keys.Back)
            {
                frmTile.BackColor = GetBackColor(frmTile.Tile);
                GetNextTile(Keys.Left, frmTile);
                Game.Grid[Ligne, Col].Letter = new Letter();
                Game.Grid[Ligne, Col].IsValidated = false;
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
            TxtInfos = string.Empty;
            TxtInfos = $"[{t.Ligne},{t.Col}] => IsAnchor:{t.IsAnchor} IsEmpty :{t.IsEmpty} => {t}";
            TxtInfos += Environment.NewLine;
            TxtInfos += $"LetterMultiplier={t.LetterMultiplier}";
            TxtInfos += Environment.NewLine;
            TxtInfos += $"WordMultiplier={t.WordMultiplier}";
            TxtInfos += Environment.NewLine;
            TxtInfos += $"IsValidated={t.IsValidated}";
            TxtInfos += Environment.NewLine;
            TxtInfos += $"FromJoker={t.FromJoker}";
            TxtInfos += Environment.NewLine;
            TxtInfos += $"AnchorLeftMinLimit = {t.AnchorLeftMinLimit}";
            TxtInfos += Environment.NewLine;
            TxtInfos += $"AnchorLeftMaxLimit = {t.AnchorLeftMaxLimit}";
            TxtInfos += Environment.NewLine;
            TxtInfos += $"UpTile = {t.UpTile}";
            TxtInfos += Environment.NewLine;
            TxtInfos += $"DownTile = {t.DownTile}";
            TxtInfos += Environment.NewLine;
            TxtInfos += $"RightTile = {t.RightTile}";
            TxtInfos += Environment.NewLine;
            TxtInfos += $"LeftTile = {t.LeftTile}";
            TxtInfos += Environment.NewLine;

            TxtInfos += "Controlers:" + Environment.NewLine;
            foreach (var c in t.Controlers)
                TxtInfos += $"{Game.AlphabetAvecJoker[c.Key].Char}:{c.Value}{Environment.NewLine}";

            frm.lsbInfos.Items.Clear();
            foreach (var l in TxtInfos.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries))
            {
                frm.lsbInfos.Items.Add(l);
            }
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
        public bool IsValidated
        {
            get => isValidated; set => isValidated = Tile.IsValidated;
        }

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

        public string Serialize => throw new NotImplementedException();
    }
}
