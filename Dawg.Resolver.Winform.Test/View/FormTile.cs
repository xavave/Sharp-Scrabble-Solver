using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.DesignerServices;
using System.Windows.Forms;

using DawgResolver.Model;

namespace Dawg.Solver.Winform
{

    public partial class FormTile : TranspTextBox, IExtendedTile, IFormattable
    {
        public static Color HeaderTilesBackColor { get; } = Color.Black;
        public static Color HeaderTilesForeColor { get; } = Color.White;
        public static Color Player1MoveColor { get; } = Color.LightYellow;
        public static Color Player2MoveColor { get; } = Color.LightGreen;
        public static Color BothPlayersMoveColor { get; } = Color.GreenYellow;

        public FormTile(int ligne, int col, string tileName = null, Color? color = null)
        {
            InitializeComponent();
            this.Ligne = ligne;
            this.Col = col;
            this.Letter = new Letter();
            this.Controllers = new Dictionary<int, int>();


            Text = $"{this.Letter.Char}";

            if (!string.IsNullOrWhiteSpace(Text))
                this.BorderStyle = BorderStyle.Fixed3D;
            else
                this.BorderStyle = BorderStyle.FixedSingle;

            if (string.IsNullOrEmpty(tileName))
                Name = $"t{this.Ligne}_{this.Col}";
            else
                Name = tileName;

            Click += FormTile_Click;
            KeyUp += FormTile_KeyUp;
            DoubleClick += FormTile_DoubleClick;

            var spacing = 15;
            if (Name.StartsWith("header_"))
            {
                this.BorderStyle = BorderStyle.Fixed3D;
                if (Name.Contains($"_col"))
                {
                    Location = new Point(spacing + this.Width + this.Col * this.Width, spacing + this.Ligne * this.Height);
                    Enabled = false;
                }
                else if (Name.Contains($"_ligne"))
                {
                    Location = new Point(spacing + this.Col * this.Width, spacing + this.Height + this.Ligne * this.Height);
                    Enabled = false;
                }
            }
            else
                Location = new Point(spacing + this.Width + this.Col * this.Width, spacing + this.Height + this.Ligne * this.Height);
        }

        public void SetTileBackColor(Color? color = null)
        {
            if (!color.HasValue)
            {
                if (!IsPlayer1.HasValue)
                    SetBackColorFrom(this);
                else
                    if (IsPlayer1.Value)
                    this.BackColor = Player1MoveColor;
                else
                    this.BackColor = Player2MoveColor;

            }
            else
            {
                this.BackColor = color.Value;
                this.ForeColor = HeaderTilesForeColor;
            }
        }

        //private MainForm Form { get; }
        public string TxtInfos { get; set; }
        public string ToString(string format, IFormatProvider formatProvider) => Serialize;
        public override string ToString() => ToString(null, System.Globalization.CultureInfo.CurrentCulture);
        public Color Background { get; set; }
        public bool IsValidated { get; set; }
        public bool? IsPlayer1 { get; }
        public IDictionary<int, int> Controllers { get; }
        public int Ligne { get; set; }
        public int Col { get; set; }
        public int LetterMultiplier { get; set; }
        public int WordMultiplier { get; set; }
        public Letter Letter { get; }
        public int AnchorLeftMaxLimit { get; set; }
        public int AnchorLeftMinLimit { get; set; }
        public bool IsAnchor => (IsEmpty && TileType == TileType.Center)
                || (IsEmpty && (
                    (UpTile != null && !UpTile.IsEmpty) ||
                    (DownTile != null && !DownTile.IsEmpty) ||
                    (RightTile != null && !RightTile.IsEmpty) ||
                    (LeftTile != null && !LeftTile.IsEmpty))
                );
        public TileType TileType
        {
            get
            {

                if (Ligne == Game.DefaultInstance.BoardCenter && Col == Game.DefaultInstance.BoardCenter)
                    return TileType.Center;
                else if (WordMultiplier == 2) return TileType.DoubleWord;
                else if (LetterMultiplier == 2) return TileType.DoubleLetter;
                else if (LetterMultiplier == 3) return TileType.TripleLetter;
                else if (WordMultiplier == 3) return TileType.TripleWord;
                return TileType.Regular;
            }
        }
        public bool IsEmpty => this == null || Letter == null || !Letter.HasValue() || string.IsNullOrWhiteSpace($"{Letter.Char}");
        public IExtendedTile LeftTile => Col > 0 ? Game.DefaultInstance.Grid[this.Ligne, this.Col - 1] : null;
        public IExtendedTile RightTile => Col < Game.DefaultInstance.BoardSize - 1 ? Game.DefaultInstance.Grid[this.Ligne, this.Col + 1] : null;
        public IExtendedTile DownTile => Ligne < Game.DefaultInstance.BoardSize - 1 ? Game.DefaultInstance.Grid[this.Ligne + 1, this.Col] : null;
        public IExtendedTile UpTile => Ligne > 0 ? Game.DefaultInstance.Grid[this.Ligne - 1, this.Col] : null;
        public string Serialize => $"T{Ligne};{Col};{LetterMultiplier};{WordMultiplier};{(Letter.LetterType == LetterType.Joker ? "true" : "false")};{IsValidated};{Letter?.Char};{IsPlayer1}";
        public char EmptyChar => ' ';
        public IExtendedTile WordMostRightTile
        {
            get
            {
                IExtendedTile t = this;

                if (IsEmpty || t.RightTile == null || t.RightTile.IsEmpty)
                    return t;
                else
                    while (t.RightTile != null && !t.RightTile.IsEmpty)
                    {
                        t = t.RightTile;
                    }
                return t;
            }
        }
        public IExtendedTile WordMostLeftTile
        {
            get
            {
                IExtendedTile t = this;
                if (IsEmpty || t.LeftTile == null || t.LeftTile.IsEmpty)
                    return t;
                else
                    while (t.LeftTile != null && !t.LeftTile.IsEmpty)
                    {
                        t = t.LeftTile;
                    }
                return t;
            }
        }
        public IExtendedTile WordLowerTile
        {
            get
            {
                IExtendedTile t = this;
                if (IsEmpty || t.DownTile == null || t.DownTile.IsEmpty)
                    return t;
                else
                    while (t.DownTile != null && !t.DownTile.IsEmpty)
                    {
                        t = t.DownTile;
                    }
                return t;
            }
        }
        public IExtendedTile WordUpperTile
        {
            get
            {
                IExtendedTile t = this;
                if (IsEmpty || t.UpTile == null || t.UpTile.IsEmpty)
                    return t;
                else
                    while (t.UpTile != null && !t.UpTile.IsEmpty)
                    {
                        t = t.UpTile;
                    }
                return t;
            }
        }
        public int WordIndex { get; set; } = 0;
        Keys PreviousKey { get; set; }
        Keys CurrentKey { get; set; }

        private void FormTile_DoubleClick(object sender, EventArgs e)
        {
            var frmTile = sender as FormTile;
            var word = frmTile.GetWordFromTile(MovementDirection.Across);
            if (word == null || word.Text.Trim().Length <= 1)
                word = frmTile.GetWordFromTile(MovementDirection.Down);
            if (word == null) return;
            word.ShowDefinition();
        }


        public IExtendedTile FindFormTile(HashSet<IExtendedTile> boardTiles)
        {
            return boardTiles.FirstOrDefault(f => f.Name == $"t{this.Ligne}_{this.Col}");
        }
        private void FormTile_KeyUp(object sender, KeyEventArgs e)
        {

            if (e.Control && e.KeyCode == Keys.S)
            {
                Game.DefaultInstance.SaveGame();
                return;
            }
            else if (e.Control && e.KeyCode == Keys.L)
            {
                //TODO Game.DefaultInstance.Load(boardTiles);
                return;
            }
            IExtendedTile frmTile = sender as FormTile;
            if (CurrentKey != e.KeyCode)
            {
                PreviousKey = CurrentKey;
                CurrentKey = e.KeyCode;
            }

            if (e.KeyCode >= Keys.A && e.KeyCode <= Keys.Z)
            {
                this.Letter.CopyFromOtherLetter(Solver.DefaultInstance.Find(e.KeyData.ToString().First()));
                this.Text = this.Letter.Char.ToString();
                if (!Game.DefaultInstance.Grid[Ligne, Col].IsValidated)
                {
                    //Game.Grid[Ligne, Col].Letter = this.Letter;
                    //this.Tile.Letter = this.Letter;
                    Game.DefaultInstance.Bag.RemoveLetterFromBag(this.Letter.Char);
                    //TODO Form.txtBag.Text = Game.DefaultInstance.Bag.GetBagContent();
                    //Game.Grid[Ligne, Col].IsValidated = true;
                    IsValidated = true;
                    Game.DefaultInstance.LastPlayedTile = this;

                }

                frmTile.BackColor = Game.DefaultInstance.IsPlayer1 ? Player1MoveColor : Player2MoveColor;
                if (Col < Game.DefaultInstance.BoardSize - 1 && Game.CurrentWordDirection == MovementDirection.Across)
                {
                    GetNextTile(Keys.Right, frmTile);

                }
                else
                    GetNextTile(Keys.Down, frmTile);
            }
            else if (e.KeyCode == Keys.Back)
            {
                frmTile.SetBackColorFromInnerTile();
                if (Game.CurrentWordDirection == MovementDirection.Across)
                    GetNextTile(Keys.Left, frmTile, false);
                else
                    GetNextTile(Keys.Up, frmTile, false);

                //TODO check Game.DefaultInstance.Grid[Ligne, Col].Letter = new Letter();
                Game.DefaultInstance.Grid[Ligne, Col].IsValidated = false;
                this.Text = Game.EmptyChar.ToString();
                frmTile = Game.DefaultInstance.Grid[Ligne, Col];

            }
            else if (e.KeyCode == Keys.Enter)
            {
                Word word = null;
                if (Game.CurrentWordDirection == MovementDirection.Across)
                    word = this.LeftTile.GetWordFromTile(Game.CurrentWordDirection);
                else
                    word = this.UpTile.GetWordFromTile(Game.CurrentWordDirection);

                //TODO
                //if (Game.DefaultInstance.IsPlayer1)
                //    Game.DefaultInstance.PreviewWord(Game.DefaultInstance.Player1,(w) => addw word, true);
                //else
                //    Game.DefaultInstance.PreviewWord(Game.DefaultInstance.Player2, word, true);

            }
            else
            {
                GetNextTile(e.KeyCode, frmTile);
            }
        }

        private FormTile GetNextTile(Keys key, IExtendedTile tile, bool skipNotEmpty = true)
        {
            FormTile nextTile = (tile as FormTile);

            Control parent = (tile as FormTile).Parent;
            if (key == Keys.Right)
                nextTile = parent.Controls.Find($"t{nextTile.Ligne}_{nextTile.Col + 1}", false).FirstOrDefault() as FormTile;
            else if (key == Keys.Left)
                nextTile = parent.Controls.Find($"t{nextTile.Ligne}_{nextTile.Col - 1}", false).FirstOrDefault() as FormTile;
            else if (key == Keys.Up)
                nextTile = parent.Controls.Find($"t{nextTile.Ligne - 1}_{nextTile.Col}", false).FirstOrDefault() as FormTile;
            else if (key == Keys.Down)
                nextTile = parent.Controls.Find($"t{nextTile.Ligne + 1}_{nextTile.Col}", false).FirstOrDefault() as FormTile;
            if (skipNotEmpty)
                while (nextTile != null && !nextTile.IsEmpty && nextTile.Col < (Game.DefaultInstance.BoardSize - 1) && nextTile.Ligne < (Game.DefaultInstance.BoardSize - 1))
                    nextTile = GetNextTile(key, nextTile);
            if (nextTile != null) nextTile.Focus();
            return nextTile;
        }

        private void FormTile_Click(object sender, EventArgs e)
        {
            var t = sender as FormTile;

            TxtInfos = string.Empty;
            TxtInfos = $"[{t.Ligne},{t.Col}] => IsAnchor:{t.IsAnchor} IsEmpty :{t.IsEmpty} => {t}";
            TxtInfos += Environment.NewLine;
            TxtInfos += $"WordIndex={t.WordIndex}";
            TxtInfos += Environment.NewLine;
            TxtInfos += $"IsPlayedByPlayer1={t.IsPlayer1}";
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
            foreach (var c in t.Controllers)
                TxtInfos += $"{Solver.DefaultInstance.Alphabet[c.Key]}:{c.Value}{Environment.NewLine}";
            //TODO
            //Form.lsbInfos.Items.Clear();
            //foreach (var l in TxtInfos.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries))
            //{
            //    Form.lsbInfos.Items.Add(l);
            //}
        }


        public void SetBackColorFrom(IExtendedTile t)
        {
            this.BackColor = GetBackColorFromtileType(t);
        }
        public Color GetBackColorFromtileType(IExtendedTile t)
        {
            return Color.FromName(new TileColor(t.TileType).Name);
        }

        public void SetBackColorFromLetterType(IExtendedTile t)
        {
            var colorName = new TileColor(t.Letter.LetterType).Name;
            if (string.IsNullOrEmpty(colorName)) colorName = GetBackColorFromtileType(t).Name;
            this.BackColor = Color.FromName(colorName);
        }
        public void SetBackColorFromInnerLetterType()
        {
            SetBackColorFromLetterType(this);
        }
        public void SetBackColorFromInnerTile()
        {
            SetBackColorFromLetterType(this);
        }

        public Word GetWordFromTile(MovementDirection direction)
        {
            var word = new Word();
            var wordText = "";
            this.GetNextTile( direction, ref wordText);

            word.SetText(wordText, false);
            return word;
        }
        public IExtendedTile GetNextTile(MovementDirection direction, ref string wordText)
        {
            var tileToCheck = direction == MovementDirection.Across ? this.RightTile : this.DownTile;
            while (tileToCheck != null && !tileToCheck.IsEmpty || !tileToCheck.IsAnchor || !tileToCheck.IsValidated)
            {
                wordText += $"{tileToCheck.Letter}";
                return tileToCheck.GetNextTile(direction, ref wordText);
            }
            return tileToCheck;
        }

        public void SetWord(string text, MovementDirection direction, bool validate = false)
        {
            if (text == "") return;

            this.SetFirstLetter(text.First(), Game.DefaultInstance.CurrentPlayer, validate);

            foreach (var c in text.Skip(1))
            {
                this.SetRightOrDownLetter(c, Game.DefaultInstance.CurrentPlayer, validate, direction);

            }
        }
        private IExtendedTile SetRightOrDownLetter(char c, Player p, bool validate, MovementDirection direction)
        {
            var nextTile = direction == MovementDirection.Down ? this.DownTile : this.RightTile;

            if (nextTile != null)
            {
                if (nextTile.IsEmpty) nextTile.IsValidated = validate;
                nextTile.Letter.CopyFromOtherLetter(Solver.DefaultInstance.Find(char.ToUpper(c)));

                if (char.IsLower(c)) nextTile.Letter.LetterType = LetterType.Joker;

                if (nextTile.Letter.LetterType == LetterType.Joker)
                    p.Rack.Remove(Solver.DefaultInstance.Alphabet.ElementAt(26));
                else
                    p.Rack.Remove(Solver.DefaultInstance.Alphabet.First(a => a.Char == char.ToUpper(c)));
                return nextTile;
            }
            return nextTile;
        }
        public IExtendedTile SetFirstLetter(char c, Player p, bool validate)
        {
            //IExtendedTile nextTile = null;
            //if (this.Col == r.game.BoardSize - 1)
            //    nextTile = this.LeftTile.RightTile;
            //else nextTile = this.RightTile.LeftTile;

            if (this.IsEmpty) this.IsValidated = validate;

            this.Letter.CopyFromOtherLetter(Solver.DefaultInstance.Find(char.ToUpper(c)));
            if (char.IsLower(c))
                this.Letter.LetterType = LetterType.Joker;
            if (this.Letter.LetterType == LetterType.Joker)
                p.Rack.Remove(Solver.DefaultInstance.Alphabet.ElementAt(26));
            else
                p.Rack.Remove(this.Letter);
            return this;

        }
        public void CopyControllers(IDictionary<int, int> source)
        {
            this.Controllers.Clear();
            foreach (var k in source.Keys)
            {
                this.Controllers[k] = source[k];
            }
        }
        public IExtendedTile Copy(bool transpose = false)
        {
            IExtendedTile newT = this;
            if (transpose) newT = Game.DefaultInstance.Grid[this.Col, this.Ligne];

            IExtendedTile nTile = new FormTile(newT.Ligne, newT.Col)
            {

                LetterMultiplier = this.LetterMultiplier,
                WordMultiplier = this.WordMultiplier,
                AnchorLeftMinLimit = this.AnchorLeftMinLimit,
                AnchorLeftMaxLimit = this.AnchorLeftMaxLimit,
                IsValidated = this.IsValidated,
            };
            nTile.Letter.CopyFromOtherLetter(this.Letter);
            nTile.SetTileBackColor(this.BackColor);
            nTile.CopyControllers(this.Controllers);
            return nTile;
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // FormTile
            // 
            this.BorderColor = System.Drawing.Color.Gray;
            this.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            //this.Font = new Font("Verdana", 14, FontStyle.Bold);
            this.ForeColor = System.Drawing.Color.Black;
            this.MaxLength = 1;
            this.Opacity = 60;
            this.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Width = 30;
            //Enabled = true;
            this.Height = 28;

            this.ResumeLayout(false);

        }
    }
}
