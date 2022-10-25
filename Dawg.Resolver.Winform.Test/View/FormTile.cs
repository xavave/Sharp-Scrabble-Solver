using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

using Dawg.Scrabble.Model.Models;

namespace Dawg.Resolver.Winform.Test
{

    public partial class FormTile : TranspTextBox, IExtendedTile, IFormattable
    {
        private MainForm Form { get; }
        public string TxtInfos { get; set; }
        public Game game { get; set; }
        public int Ligne { get; }
        public int Col { get; }
        public FormTile(MainForm frm, Game g, string tileName = "", Color? color = null, int? ligne = null, int? col = null)
        {
            game = g;
            this.Width = 30;
            //Enabled = true;
            this.Height = 28;
            this.MaxLength = 1;
            Form = frm;
            Ligne = ligne ?? default(int);
            Col = col ?? default(int);
            this.Font = new Font("Verdana", 14, FontStyle.Bold);
            this.CharacterCasing = CharacterCasing.Upper;
            if (!color.HasValue)
            {
                //if (!IsPlayedByPlayer1.HasValue)
                //    SetBackColorFrom(t);
                //else
                if (IsPlayedByPlayer1.Value)
                    this.BackColor = Form.Player1MoveColor;
                else
                    this.BackColor = Form.Player2MoveColor;

            }
            else
            {
                this.BackColor = color.Value;
                this.ForeColor = frm.HeaderTilesForeColor;
            }
            Text = Letter?.Char.ToString();
            if (!string.IsNullOrWhiteSpace(Text)) this.BorderStyle = BorderStyle.Fixed3D;
            else this.BorderStyle = BorderStyle.FixedSingle;

            if (tileName == "")
                Name = $"t{Ligne}_{Col}";
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
                    Location = new Point(15 + this.Width + Col * this.Width, 15 + Ligne * this.Height);
                    Enabled = false;
                }
                else if (Name.Contains($"_ligne"))
                {
                    Location = new Point(15 + Col * this.Width, 15 + this.Height + Ligne * this.Height);
                    Enabled = false;
                }
            }
            else
                Location = new Point(15 + this.Width + Col * this.Width, 15 + this.Height + Ligne * this.Height);


        }

        private void FormTile_DoubleClick(object sender, EventArgs e)
        {
            var frmTile = sender as FormTile;
            var word = frmTile.GetWordFromTile(game, MovementDirection.Across);
            if (word == null || word.Text.Trim().Length <= 1)
                word = frmTile.GetWordFromTile(game, MovementDirection.Down);
            if (word == null) return;
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
                //frmTile.SetBackColorFrom(Tile); //TODO
                if (Game.CurrentWordDirection == MovementDirection.Across)
                    GetNextTile(Keys.Left, frmTile, false);
                else
                    GetNextTile(Keys.Up, frmTile, false);

                game.Grid[Ligne, Col].Letter = new Letter(game.Resolver);
                game.Grid[Ligne, Col].IsValidated = false;
                this.Text = Game.EmptyChar.ToString();
                //TODO frmTile.Tile = game.Grid[Ligne, Col];

            }
            else if (e.KeyCode == Keys.Enter)
            {
                Word word = null;
                if (Game.CurrentWordDirection == MovementDirection.Across)
                    word = LeftTile.GetWordFromTile(game, Game.CurrentWordDirection);
                else
                    word = UpTile.GetWordFromTile(game, Game.CurrentWordDirection);

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
            IExtendedTile t = sender as FormTile;

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
                TxtInfos += $"{game.Resolver.Alphabet[c.Key]}:{c.Value}{Environment.NewLine}";

            Form.lsbInfos.Items.Clear();
            foreach (var l in TxtInfos.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries))
            {
                Form.lsbInfos.Items.Add(l);
            }
        }

        public void SetBackColorFrom(IExtendedTile t)
        {
            this.BackColor = GetBackColorFrom(t);
        }
        public Color GetBackColorFrom(IExtendedTile t)
        {
            return Color.FromName(new TileColor(t.TileType).Name);
        }
        public Color GetBackColorInnerTile()
        {
            return GetBackColorFrom(this);
        }
        public void SetBackColorFromLetterType(IExtendedTile t)
        {
            this.BackColor = Color.FromName(new TileColor(t.Letter.LetterType).Name);
        }
        public void SetBackColorFromInnerLetterType()
        {
            SetBackColorFromLetterType(this);
        }
        public void SetBackColorFromInnerTile()
        {
            SetBackColorFrom(this);
        }

        public Word GetWordFromTile(Game g, MovementDirection direction)
        {
            return GetWordFromTile(g, direction);
        }

        public void SetWord(Game game, string text, MovementDirection direction, bool validate)
        {
            SetWord(game, text, direction, validate);
        }

        public void CopyControllers(IDictionary<int, int> controlers)
        {
            CopyControllers(controlers);
        }

        public IExtendedTile Copy(DawgSolver.Resolver r, bool transpose = false)
        {
            IExtendedTile newT = this;
            if (transpose) newT = r.game.Grid[this.Col, this.Ligne];

            IExtendedTile nTile = new FormTile(this.Form, game, newT.Name, null, newT.Ligne, newT.Col)
            {
                Letter = this.Letter,
                LetterMultiplier = this.LetterMultiplier,
                WordMultiplier = this.WordMultiplier,
                AnchorLeftMinLimit = this.AnchorLeftMinLimit,
                AnchorLeftMaxLimit = this.AnchorLeftMaxLimit,
                IsValidated = this.IsValidated,
            };
            nTile.CopyControllers(this.Controlers);
            return nTile;
        }

        public string ToString(string format, IFormatProvider formatProvider) => Serialize;
        public override string ToString() => ToString(null, System.Globalization.CultureInfo.CurrentCulture);

        public void SetTilePos(int ligne, int col)
        {

        }

        public IExtendedTile Copy(Scrabble.Model.Models.Resolver r, bool transpose = false)
        {
            throw new NotImplementedException();
        }

        public Color Background { get; set; }
        public bool IsValidated { get => IsValidated; set => IsValidated = value; }
        //public bool FromJoker { get => Tile.FromJoker; set => Tile.FromJoker = value; }
        public IDictionary<int, int> Controlers { get => Controlers; set => CopyControllers(value); }

        public int LetterMultiplier { get => LetterMultiplier; set => LetterMultiplier = value; }
        public int WordMultiplier { get => WordMultiplier; set => WordMultiplier = value; }
        public Letter Letter
        {
            get => Letter;
            set
            {
                if (Letter == null || Letter.Serialize != value.Serialize)
                {
                    Letter = value;
                    Text = Letter?.Char.ToString();
                }
            }
        }
        public int AnchorLeftMaxLimit { get => AnchorLeftMaxLimit; set => AnchorLeftMaxLimit = value; }
        public int AnchorLeftMinLimit { get => AnchorLeftMinLimit; set => AnchorLeftMinLimit = value; }
        public bool IsAnchor { get => IsAnchor; }
        public TileType TileType { get => TileType; }
        public bool IsEmpty { get => IsEmpty; }
        public IExtendedTile LeftTile { get => LeftTile; }
        public IExtendedTile RightTile { get => RightTile; }
        public IExtendedTile DownTile { get => DownTile; }
        public IExtendedTile UpTile { get => UpTile; }
        public string Serialize => Serialize;
        public bool? IsPlayedByPlayer1 => IsPlayedByPlayer1;
        public IExtendedTile WordMostRightTile => WordMostRightTile;
        public IExtendedTile WordMostLeftTile => WordMostLeftTile;
        public IExtendedTile WordLowerTile => WordLowerTile;
        public IExtendedTile WordUpperTile => WordUpperTile;
        public int WordIndex { get => WordIndex; set => WordIndex = value; }
    }
}
