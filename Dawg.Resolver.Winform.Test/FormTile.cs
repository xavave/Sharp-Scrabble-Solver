using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

using DawgResolver.Model;

namespace Dawg.Resolver.Winform.Test
{

    public partial class FormTile : TranspTextBox, IExtendedTile
    {


        private MainForm Form { get; }
        public string TxtInfos { get; set; }
        public IExtendedTile Tile { get; set; }
        public Game game { get; set; }
        public FormTile(MainForm frm, Game g, int ligne, int col, string tileName = "", Color? color = null) : this(frm, g, new BaseVirtualTile(g.Resolver, ligne, col), tileName, color) { }

        public FormTile(MainForm frm, Game g, IExtendedTile t, string tileName = "", Color? color = null)
        {
            game = g;
            Tile = t;
            this.Width = 30;
            //Enabled = true;
            this.Height = 28;
            this.MaxLength = 1;
            Form = frm;
            this.Font = new Font("Verdana", 14, FontStyle.Bold);
            this.CharacterCasing = CharacterCasing.Upper;
            if (!color.HasValue)
            {
                if (!Tile.IsPlayedByPlayer1.HasValue)
                    GetBackColor(t);
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
            if (!string.IsNullOrWhiteSpace(Text)) this.BorderStyle = BorderStyle.Fixed3D;
            else this.BorderStyle = BorderStyle.FixedSingle;

            if (tileName == "")
                Name = $"t{t.Ligne}_{t.Col}";
            else
                Name = tileName;
            Click += FormTile_Click;
            KeyUp += FormTile_KeyUp;
            DoubleClick += FormTile_DoubleClick;

            if (Name.StartsWith("header_"))
            {
                this.BorderStyle = BorderStyle.Fixed3D;
                if (Name.Contains($"_col"))
                {
                    Location = new Point(15 + this.Width + t.Col * this.Width, 15 + t.Ligne * this.Height);
                    Enabled = false;
                }
                else if (Name.Contains($"_ligne"))
                {
                    Location = new Point(15 + t.Col * this.Width, 15 + this.Height + t.Ligne * this.Height);
                    Enabled = false;
                }
            }
            else
                Location = new Point(15 + this.Width + t.Col * this.Width, 15 + this.Height + t.Ligne * this.Height);


        }

        private void FormTile_DoubleClick(object sender, EventArgs e)
        {
            var frmTile = sender as FormTile;
            var word = frmTile.GetWordFromTile(game, MovementDirection.Across);
            if (word.Text.Trim().Length <= 1)
                word = frmTile.GetWordFromTile(game, MovementDirection.Down);
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
                this.Letter = game.Resolver.Find(e.KeyData.ToString().First());
                this.Text = this.Letter.Char.ToString();
                if (!game.Grid[Ligne, Col].IsValidated)
                {
                    //Game.Grid[Ligne, Col].Letter = this.Letter;
                    //this.Tile.Letter = this.Letter;
                    game.Bag.RemoveLetterFromBag(this.Letter.Char);
                    Form.txtBag.Text = game.Bag.GetBagContent();
                    //Game.Grid[Ligne, Col].IsValidated = true;
                    IsValidated = true;
                    Form.LastPlayedTile = this;

                }

                frmTile.BackColor = game.IsPlayer1 ? Form.Player1MoveColor : Form.Player2MoveColor;
                if (Col < game.BoardSize - 1 && Game.CurrentWordDirection == MovementDirection.Across)
                {
                    GetNextTile(Keys.Right, frmTile);

                }
                else
                    GetNextTile(Keys.Down, frmTile);
            }
            else if (e.KeyCode == Keys.Back)
            {
                frmTile.GetBackColor(frmTile.Tile);
                if (Game.CurrentWordDirection == MovementDirection.Across)
                    GetNextTile(Keys.Left, frmTile, false);
                else
                    GetNextTile(Keys.Up, frmTile, false);

                game.Grid[Ligne, Col].Letter = new Letter(game.Resolver);
                game.Grid[Ligne, Col].IsValidated = false;
                this.Text = Game.EmptyChar.ToString();
                frmTile.Tile = game.Grid[Ligne, Col];

            }
            else if (e.KeyCode == Keys.Enter)
            {
                Word word = null;
                if (Game.CurrentWordDirection == MovementDirection.Across)
                    word = Tile.LeftTile.GetWordFromTile(game, Game.CurrentWordDirection);
                else
                    word = Tile.UpTile.GetWordFromTile(game, Game.CurrentWordDirection);

                if (game.IsPlayer1)
                    Form.PreviewWord(game.Player1, word, true);
                else
                    Form.PreviewWord(game.Player2, word, true);

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
                while (nextTile != null && !nextTile.IsEmpty && nextTile.Col < (game.BoardSize - 1) && nextTile.Ligne < (game.BoardSize - 1))
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
            TxtInfos += $"WordIndex={t.WordIndex}";
            TxtInfos += Environment.NewLine;
            TxtInfos += $"IsPlayedByPlayer1={t.IsPlayedByPlayer1}";
            TxtInfos += Environment.NewLine;
            TxtInfos += $"LetterMultiplier={t.LetterMultiplier}";
            TxtInfos += Environment.NewLine;
            TxtInfos += $"WordMultiplier={t.WordMultiplier}";
            TxtInfos += Environment.NewLine;
            TxtInfos += $"IsValidated={t.IsValidated}";
            TxtInfos += Environment.NewLine;
            TxtInfos += $"FromJoker={t.Letter.LetterType == LetterType.Joker}";
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
                TxtInfos += $"{game.Resolver.Alphabet.ElementAt(c.Key).Char}:{c.Value}{Environment.NewLine}";

            Form.lsbInfos.Items.Clear();
            foreach (var l in TxtInfos.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries))
            {
                Form.lsbInfos.Items.Add(l);
            }
        }

        public void GetBackColor(IExtendedTile t)
        {
            this.BackColor = Color.FromName(new TileColor(t.TileType).Name);
        }
        public void GetBackColorFromLetterType(IExtendedTile t)
        {
            this.BackColor = Color.FromName(new TileColor(t.Letter.LetterType).Name);
        }
        public void GetBackColorFromInnerLetterType()
        {
            GetBackColorFromLetterType(this.Tile);
        }
        public void GetBackColorFromInnerTile()
        {
            GetBackColor(this.Tile);
        }

        public Word GetWordFromTile(Game g, MovementDirection direction)
        {
            return Tile.GetWordFromTile(g, direction);
        }

        //public void Initialize()
        //{
        //    Tile.Initialize();
        //}

        public void CopyControllers(Dictionary<int, int> source)
        {
            throw new NotImplementedException();
        }

        public void SetWord(Game game, string text, MovementDirection direction, bool validate)
        {
            Tile.SetWord(game, text, direction, validate);
        }

        public void CopyControllers(IDictionary<int, int> controlers)
        {
            Tile.CopyControllers(controlers);
        }

        public IExtendedTile Copy(DawgResolver.Resolver r, bool transpose = false)
        {
            return Tile.Copy(r, transpose);
        }

        public Color Background { get; set; }
        public bool IsValidated { get => Tile.IsValidated; set => Tile.IsValidated = value; }
        //public bool FromJoker { get => Tile.FromJoker; set => Tile.FromJoker = value; }
        public IDictionary<int, int> Controlers { get => Tile.Controlers; set => Tile.CopyControllers(value); }
        public int Ligne { get => Tile.Ligne; set => Tile.Ligne = value; }
        public int Col { get => Tile.Col; set => Tile.Col = value; }
        public int LetterMultiplier { get => Tile.LetterMultiplier; set => Tile.LetterMultiplier = value; }
        public int WordMultiplier { get => Tile.WordMultiplier; set => Tile.WordMultiplier = value; }
        public Letter Letter
        {
            get => Tile.Letter;
            set
            {
                if (Tile.Letter == null || Tile.Letter.Serialize != value.Serialize)
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
        public IExtendedTile LeftTile { get => Tile.LeftTile; }
        public IExtendedTile RightTile { get => Tile.RightTile; }
        public IExtendedTile DownTile { get => Tile.DownTile; }
        public IExtendedTile UpTile { get => Tile.UpTile; }
        public string Serialize => Tile.Serialize;
        public bool? IsPlayedByPlayer1 => Tile.IsPlayedByPlayer1;
        public IExtendedTile WordMostRightTile => Tile.WordMostRightTile;
        public IExtendedTile WordMostLeftTile => Tile.WordMostLeftTile;
        public IExtendedTile WordLowerTile => Tile.WordLowerTile;
        public IExtendedTile WordUpperTile => Tile.WordUpperTile;
        public int WordIndex { get => Tile.WordIndex; set => Tile.WordIndex = value; }
    }
}
