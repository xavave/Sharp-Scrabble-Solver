using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

using DawgSolver.Model;

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
        private int _wordIndex;
        /// <summary>Index du coup qui a posé la lettre (0 = aucun). Détermine la couleur de la lettre.</summary>
        public int WordIndex
        {
            get => _wordIndex;
            set { if (_wordIndex == value) return; _wordIndex = value; if (IsHandleCreated) Invalidate(); }
        }

        #region Rendu des lettres : contour noir + remplissage couleur du joueur

        private const int WM_PAINT = 0x000F;

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            // Le TextBox natif vient de dessiner sa lettre : on repeint la case par-dessus.
            if (m.Msg == WM_PAINT) PaintStyledLetter();
        }

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool HideCaret(IntPtr hWnd);

        // Pendant la saisie (case active), la lettre garde aussi le rendu stylé (contour + couleur) ;
        // le curseur natif est masqué et remplacé par un cadre + un trait « en cours d'édition ».
        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);
            if (TileStyle.Family != null && !Name.StartsWith("header_"))
            {
                HideCaret(Handle);
                SelectAll();        // la frappe suivante remplace la lettre (MaxLength = 1)
            }
            Invalidate();
        }
        protected override void OnLostFocus(EventArgs e) { base.OnLostFocus(e); Invalidate(); }
        // Un clic dans la case active fait redessiner le texte natif (sélection) sans WM_PAINT : on repeint.
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            if (Focused && TileStyle.Family != null) { HideCaret(Handle); Invalidate(); }
        }
        protected override void OnTextChanged(EventArgs e) { base.OnTextChanged(e); Invalidate(); }

        /// <summary>Couleur du joueur qui a posé la lettre, ou null si non attribuée.</summary>
        private Color? OwnerFillColor()
        {
            if (WordIndex <= 0) return null;
            var g = Game.DefaultInstance;
            if (g.Player1.Moves.Any(w => w.Index == WordIndex)) return TileStyle.Player1Fill;
            if (g.Player2.Moves.Any(w => w.Index == WordIndex)) return TileStyle.Player2Fill;
            return null;
        }

        private void PaintStyledLetter()
        {
            var family = TileStyle.Family;
            if (family == null || !IsHandleCreated || Name.StartsWith("header_")) return;
            var text = Text?.Trim();
            bool editing = Focused;
            if (string.IsNullOrEmpty(text) && !editing) return;
            try
            {
                using (var g = CreateGraphics())
                {
                    var rect = ClientRectangle;
                    var bg = BackColor.A == 0 ? (Parent?.BackColor ?? SystemColors.Window) : Color.FromArgb(255, BackColor);
                    using (var b = new SolidBrush(bg)) g.FillRectangle(b, rect);
                    if (!string.IsNullOrEmpty(text))
                        TileFonts.DrawOutlinedCentered(g, text, family, rect, OwnerFillColor() ?? TileStyle.NeutralFill, TileStyle.OutlineWidth);
                    if (editing)
                    {
                        // Indicateur d'édition : cadre + trait en bas (remplace le curseur clignotant).
                        int pw = Math.Max(2, rect.Height / 14);
                        using (var pen = new Pen(SystemColors.Highlight, pw))
                            g.DrawRectangle(pen, rect.X + pw / 2, rect.Y + pw / 2, rect.Width - pw, rect.Height - pw);
                        int barW = rect.Width / 2, barH = Math.Max(2, rect.Height / 12);
                        using (var bar = new SolidBrush(SystemColors.Highlight))
                            g.FillRectangle(bar, rect.X + (rect.Width - barW) / 2, rect.Bottom - pw - barH - 1, barW, barH);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogException($"PaintStyledLetter {Name}", ex);
            }
        }

        #endregion
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
                TxtInfos += $"{ControllerLabel(c.Key)}:{c.Value}{Environment.NewLine}";
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
            // Remonte au début du mot (gauche pour un mot horizontal, haut pour un mot vertical),
            // puis lit les lettres contiguës jusqu'à la première case vide.
            var start = direction == MovementDirection.Across ? this.WordMostLeftTile : this.WordUpperTile;
            if (start == null || start.IsEmpty) return null;

            var wordText = string.Empty;
            IExtendedTile tile = start;
            while (tile != null && !tile.IsEmpty)
            {
                wordText += tile.Letter.Char;
                tile = direction == MovementDirection.Across ? tile.RightTile : tile.DownTile;
            }

            if (wordText.Length <= 1) return null;
            return new Word { StartTile = start, Direction = direction, Text = wordText };
        }

        public IExtendedTile GetNextTile(MovementDirection direction, ref string wordText)
        {
            // Avance d'une case dans la direction donnée tant que la case suivante est occupée,
            // en accumulant les lettres. Protégé contre le dépassement de plateau (null).
            var tileToCheck = direction == MovementDirection.Across ? this.RightTile : this.DownTile;
            if (tileToCheck == null || tileToCheck.IsEmpty)
                return tileToCheck;

            wordText += $"{tileToCheck.Letter}";
            return tileToCheck.GetNextTile(direction, ref wordText);
        }

        public void SetWord(string text, MovementDirection direction, bool validate = false)
        {
            if (string.IsNullOrEmpty(text)) return;

            // On parcourt le plateau case par case à partir de la case de départ (this),
            // dans la direction du mot, en posant une lettre par case.
            // NB : les accesseurs RightTile/DownTile résolvent toujours la grille courante
            // via les coordonnées Ligne/Col, donc même si 'this' provient d'une grille
            // transposée (recherche verticale), le parcours reste correct sur la grille vivante.
            var p = Game.DefaultInstance.CurrentPlayer;
            IExtendedTile tile = this;
            bool first = true;
            foreach (var c in text)
            {
                if (!first)
                    tile = direction == MovementDirection.Down ? tile.DownTile : tile.RightTile;
                if (tile == null) break; // sécurité : on sort du plateau

                SetLetterOnTile(tile, c, p, validate);
                first = false;
            }
        }

        /// <summary>
        /// Pose une lettre sur une case donnée et synchronise la grille vivante.
        /// Seules les lettres réellement posées (case précédemment vide) sont retirées du chevalet :
        /// les lettres déjà présentes sur le plateau (préfixes/suffixes croisés) ne doivent pas être consommées.
        /// </summary>
        private void SetLetterOnTile(IExtendedTile tile, char c, Player p, bool validate)
        {
            bool wasEmpty = tile.IsEmpty;
            if (wasEmpty) tile.IsValidated = validate;

            tile.Letter.CopyFromOtherLetter(Solver.DefaultInstance.Find(char.ToUpper(c)));
            if (char.IsLower(c)) tile.Letter.LetterType = LetterType.Joker;

            // Synchronise la case correspondante de la grille vivante si 'tile' est une référence
            // issue d'une grille de travail (transposée) différente de la grille affichée.
            var liveTile = Game.DefaultInstance.Grid[tile.Ligne, tile.Col];
            if (!ReferenceEquals(liveTile, tile))
            {
                liveTile.Letter.CopyFromOtherLetter(tile.Letter);
                liveTile.IsValidated = tile.IsValidated;
            }

            if (!wasEmpty) return; // lettre déjà sur le plateau : on ne touche pas au chevalet

            if (tile.Letter.LetterType == LetterType.Joker)
                p.Rack.Remove(Solver.DefaultInstance.Alphabet.ElementAt(26));
            else
                p.Rack.Remove(Solver.DefaultInstance.Alphabet.First(a => a.Char == char.ToUpper(c)));
        }
        /// <summary>
        /// Libellé d'une clé de Controllers. Les clés sont (code ASCII de la lettre - AscShift) :
        /// 1..26 = A..Z ; Game.AnyLetterKey = marqueur de case (« * »).
        /// L'ancien code indexait Alphabet[c.Key] -> ArgumentOutOfRangeException pour la clé -1 (joker).
        /// </summary>
        private static string ControllerLabel(int key)
        {
            if (key == Game.AnyLetterKey) return "*";
            int code = key + global::Dawg.Dictionnaire.AscShift;
            return code >= 32 && code < 127 ? ((char)code).ToString() : $"#{key}";
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
