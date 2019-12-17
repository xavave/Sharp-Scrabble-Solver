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
                //RedrawWindow(Handle, IntPtr.Zero, IntPtr.Zero, RDW_FRAME | RDW_IUPDATENOW | RDW_INVALIDATE);
            }
        }
        //protected override void WndProc(ref Message m)
        //{
        //    base.WndProc(ref m);
        //    if (m.Msg == WM_NCPAINT && BorderColor != Color.Transparent &&
        //        BorderStyle == System.Windows.Forms.BorderStyle.Fixed3D)
        //    {
        //        var hdc = GetWindowDC(this.Handle);
        //        using (var g = Graphics.FromHdcInternal(hdc))
        //        using (var p = new Pen(BorderColor))
        //            g.DrawRectangle(p, new Rectangle(0, 0, Width - 1, Height - 1));
        //        ReleaseDC(this.Handle, hdc);
        //    }
        //}
        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            RedrawWindow(Handle, IntPtr.Zero, IntPtr.Zero,
                   RDW_FRAME | RDW_IUPDATENOW | RDW_INVALIDATE);
        }
        private MainForm Form { get; }
        public string TxtInfos { get; set; }
        public VTile Tile { get; set; }
        public Game Game { get; set; }
        public FormTile(MainForm frm, Game g, VTile t, string tileName = "", Color? color = null)
        {

            BorderStyle = BorderStyle.Fixed3D;
            Game = g;
            Tile = t;
            this.Width = 30;
            //Enabled = true;
            this.Height = 28;
            this.MaxLength = 1;
            Form = frm;
            this.Font = new Font("Verdana", 14);
            this.CharacterCasing = CharacterCasing.Upper;
            if (!color.HasValue)
            {
                if (!Tile.IsPlayedByPlayer1.HasValue)
                    this.BackColor = GetBackColor(t);
                else
                    if (Tile.IsPlayedByPlayer1.Value)
                    this.BackColor = Form.Player1MoveColor;
                else
                    this.BackColor = Form.Player2MoveColor;

            }
            else
            {
                this.BackColor = color.Value;
                this.ForeColor = frm.HeaderTilesForeColor;
            }
            Text = t.Letter?.Char.ToString();
            if (tileName == "")
                Name = $"t{t.Ligne}_{t.Col}";
            else
                Name = tileName;
            Click += FormTile_Click;
            KeyUp += FormTile_KeyUp;
            DoubleClick += FormTile_DoubleClick;

            if (Name.StartsWith($"header_col"))
            {
                Location = new Point(15 + this.Width + t.Col * this.Width, 15 + t.Ligne * this.Height);
                Enabled = false;
            }
            else if (Name.StartsWith($"header_ligne"))
            {
                Location = new Point(15 + t.Col * this.Width, 15 + this.Height + t.Ligne * this.Height);
                Enabled = false;
            }
            else
                Location = new Point(15 + this.Width + t.Col * this.Width, 15 + this.Height + t.Ligne * this.Height);

            if (Tile.FromJoker) this.BorderStyle = BorderStyle.Fixed3D;
        }

        private void FormTile_DoubleClick(object sender, EventArgs e)
        {
            var frmTile = sender as FormTile;
            var word = frmTile.GetWordFromTile(MovementDirection.Across);
            if (word.Text.Trim().Length <= 1)
                word = frmTile.GetWordFromTile(MovementDirection.Down);
            Form.ShowDefinition(word);
        }

        Keys PreviousKey { get; set; }
        Keys CurrentKey { get; set; }

        private void FormTile_KeyUp(object sender, KeyEventArgs e)
        {

            if (e.Control && e.KeyCode == Keys.S)
            {
                Form.SaveGame();
                return;
            }
            else if (e.Control && e.KeyCode == Keys.L)
            {
                Form.LoadGame();
                return;
            }
            var frmTile = sender as FormTile;
            if (CurrentKey != e.KeyCode)
            {
                PreviousKey = CurrentKey;
                CurrentKey = e.KeyCode;
            }

            if (e.KeyCode >= Keys.A && e.KeyCode <= Keys.Z)
            {
                this.Letter = Game.Alphabet.Find(a => a.Char == e.KeyData.ToString().First());
                this.Text = this.Letter.Char.ToString();
                if (!Game.Grid[Ligne, Col].IsValidated)
                {
                    //Game.Grid[Ligne, Col].Letter = this.Letter;
                    //this.Tile.Letter = this.Letter;
                    Game.Bag.RemoveLetterFromBag(this.Letter.Char);
                    Form.txtBag.Text = Game.Bag.GetBagContent();
                    //Game.Grid[Ligne, Col].IsValidated = true;
                    IsValidated = true;
                    Form.LastPlayedTile = this;

                }

                frmTile.BackColor = Game.IsPlayer1 ? Form.Player1MoveColor : Form.Player2MoveColor;
                if (Col < Game.BoardSize - 1 && Game.CurrentWordDirection == MovementDirection.Across)
                {
                    GetNextTile(Keys.Right, frmTile);

                }
                else
                    GetNextTile(Keys.Down, frmTile);
            }
            else if (e.KeyCode == Keys.Back)
            {
                frmTile.BackColor = GetBackColor(frmTile.Tile);
                if (Game.CurrentWordDirection == MovementDirection.Across)
                    GetNextTile(Keys.Left, frmTile, false);
                else
                    GetNextTile(Keys.Up, frmTile, false);

                Game.Grid[Ligne, Col].Letter = new Letter();
                Game.Grid[Ligne, Col].IsValidated = false;
                this.Text = Game.EmptyChar.ToString();
                frmTile.Tile = Game.Grid[Ligne, Col];

            }
            else if (e.KeyCode == Keys.Enter)
            {
                Word word = null;
                if (Game.CurrentWordDirection == MovementDirection.Across)
                    word = Tile.LeftTile.GetWordFromTile(Game.CurrentWordDirection);
                else
                    word = Tile.UpTile.GetWordFromTile(Game.CurrentWordDirection);

                if (Game.IsPlayer1)
                    Form.PreviewWord(Game.Player1, word, true);
                else
                    Form.PreviewWord(Game.Player2, word, true);

            }
            else
            {
                GetNextTile(e.KeyCode, frmTile);
            }
        }

        private FormTile GetNextTile(Keys key, FormTile frmTile, bool skipNotEmpty = true)
        {
            FormTile nextTile = frmTile;

            Control parent = frmTile.Parent;
            if (key == Keys.Right)
                nextTile = parent.Controls.Find($"t{nextTile.Ligne}_{nextTile.Col + 1}", false).FirstOrDefault() as FormTile;
            else if (key == Keys.Left)
                nextTile = parent.Controls.Find($"t{nextTile.Ligne}_{nextTile.Col - 1}", false).FirstOrDefault() as FormTile;
            else if (key == Keys.Up)
                nextTile = parent.Controls.Find($"t{nextTile.Ligne - 1}_{nextTile.Col}", false).FirstOrDefault() as FormTile;
            else if (key == Keys.Down)
                nextTile = parent.Controls.Find($"t{nextTile.Ligne + 1}_{nextTile.Col}", false).FirstOrDefault() as FormTile;
            if (skipNotEmpty)
                while (nextTile != null && !nextTile.IsEmpty && nextTile.Col < (Game.BoardSize - 1) && nextTile.Ligne < (Game.BoardSize - 1))
                    nextTile = GetNextTile(key, nextTile);
            if (nextTile != null) nextTile.Focus();
            return nextTile;
        }

        private void FormTile_Click(object sender, EventArgs e)
        {
            var txt = sender as FormTile;
            var t = txt.Tile;

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
                TxtInfos += $"{Game.AlphabetWWFAvecJoker[c.Key].Char}:{c.Value}{Environment.NewLine}";

            Form.lsbInfos.Items.Clear();
            foreach (var l in TxtInfos.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries))
            {
                Form.lsbInfos.Items.Add(l);
            }
        }

        public Color GetBackColor(VTile t)
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

        public Word GetWordFromTile(MovementDirection direction)
        {
            return Tile.GetWordFromTile(direction);
        }

        public void Initialize()
        {
            Tile.Initialize();
        }

        public Color Background { get; set; }
        public bool IsValidated { get => Tile.IsValidated; set => Tile.IsValidated = value; }
        public bool FromJoker { get => Tile.FromJoker; set => Tile.FromJoker = value; }
        public Dictionary<int, int> Controlers { get => Tile.Controlers; set => Tile.Controlers = value; }
        public int Ligne { get => Tile.Ligne; set => Tile.Ligne = value; }
        public int Col { get => Tile.Col; set => Tile.Col = value; }
        public int LetterMultiplier { get => Tile.LetterMultiplier; set => Tile.LetterMultiplier = value; }
        public int WordMultiplier { get => Tile.WordMultiplier; set => Tile.WordMultiplier = value; }
        public Letter Letter
        {
            get => Tile.Letter;
            set
            {
                if (Tile.Letter.Serialize != value.Serialize)
                {
                    Tile.Letter = value;
                    Text = Tile.Letter?.Char.ToString();
                }
            }
        }
        public int AnchorLeftMaxLimit { get => Tile.AnchorLeftMaxLimit; set => Tile.AnchorLeftMaxLimit = value; }
        public int AnchorLeftMinLimit { get => Tile.AnchorLeftMinLimit; set => Tile.AnchorLeftMinLimit = value; }
        public bool IsAnchor { get => Tile.IsAnchor; }
        public TileType TileType { get => Tile.TileType; }
        public bool IsEmpty { get => Tile.IsEmpty; }
        public VTile LeftTile { get => Tile.LeftTile; }
        public VTile RightTile { get => Tile.RightTile; }
        public VTile DownTile { get => Tile.DownTile; }
        public VTile UpTile { get => Tile.UpTile; }
        public string Serialize => Tile.Serialize;

        public bool? IsPlayedByPlayer1 { get => Tile.IsPlayedByPlayer1; set => Tile.IsPlayedByPlayer1 = value; }

        public VTile WordMostRightTile => Tile.WordMostRightTile;

        public VTile WordMostLeftTile => Tile.WordMostLeftTile;

        public VTile WordLowerTile => Tile.WordLowerTile;

        public VTile WordUpperTile => Tile.WordUpperTile;
    }
}
