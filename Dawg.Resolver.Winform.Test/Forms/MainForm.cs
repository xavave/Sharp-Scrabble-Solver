using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

using DawgSolver.Model;

namespace Dawg.Solver.Winform
{
    public partial class MainForm : Form
    {


        // Dictionnaire par défaut : FR ODS 9.
        public string CurrentDicoName { get; set; } = Dictionnaire.NomDicoDawgODS9;
        public Game game => Game.DefaultInstance;
        public FormTile LastPlayedTile { get; set; }
        public HashSet<IExtendedTile> BoardTiles { get; }

        // Empêche le handler rbDico_CheckedChanged de s'exécuter pendant InitializeComponent
        // (quand rbODS9.Checked=true déclenche l'événement avant que les contrôles soient prêts).
        private bool _initialized = false;

        // Réglages persistants (%AppData%\ScrabbleSolver\settings.json)
        private readonly AppSettings _settings = AppSettings.Load();

        public MainForm()
        {
            InitializeComponent();
            BoardTiles = gbBoard.Controls.OfType<IExtendedTile>().ToHashSet();

            L.SetLanguage(_settings.Language);
            TileFonts.EnsureLoaded();
            SetupWordsListView();
            CreateLanguageSelector();
            CreateBoardFontSelector();
            LayoutSettingsBar();
            ApplyTileStyle();
            ApplyLanguage();

            // Dictionnaire persisté : on coche le bon bouton AVANT _initialized=true
            // (le handler rbDico_CheckedChanged est alors ignoré), puis on le charge explicitement.
            CurrentDicoName = string.IsNullOrWhiteSpace(_settings.Dico) ? Dictionnaire.NomDicoDawgODS9 : _settings.Dico;
            var rbDico = RadioForDico(CurrentDicoName);
            if (rbDico != null) rbDico.Checked = true;

            _initialized = true;
            // Le singleton Dictionnaire est initialisé en anglais : on charge le dico choisi.
            LoadDictionary(CurrentDicoName);
            NewGame(CurrentDicoName, true);
            SetupRackLabel();
            SetupResizableBoard();
        }

        private RadioButton RadioForDico(string dico)
        {
            switch (dico)
            {
                case Dictionnaire.NomDicoDawgODS6: return rbODS6;
                case Dictionnaire.NomDicoDawgODS7: return rbODS7;
                case Dictionnaire.NomDicoDawgODS8: return rbODS8;
                case Dictionnaire.NomDicoDawgODS9: return rbODS9;
                case Dictionnaire.NomDicoDawgEN_Collins: return rbEnCollins2019;
                default: return null;
            }
        }

        private void LoadDictionary(string nomDico)
        {
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                Logger.Log($"Chargement du dictionnaire -> {nomDico}");
                Dictionnaire.DefaultInstance.ChargerFichierDAWG(nomDico);
                CurrentDicoName = nomDico;
                _settings.Dico = nomDico;   // mémorisé pour le prochain lancement
                _settings.Save();
                lsbInfos.Items.Insert(0, L.F("DicoLoaded", nomDico, Dictionnaire.DefaultInstance.NombreNoeuds));
            }
            catch (Exception ex)
            {
                Logger.LogException("Chargement du dictionnaire", ex);
                lsbInfos.Items.Insert(0, L.F("DicoLoadError", nomDico, ex.Message));
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private void NewGame(string nomDico, bool initUI)
        {
            Cursor.Current = Cursors.WaitCursor;

            if (initUI)
            {
                // Réinitialise réellement la partie (sac, coups, scores) et vide le plateau.
                game.ResetGame();

                txtGrid2.Visible = ckShowGrid.Checked;
                lsbInfos.Items.Clear();
                lblPlayer1Score.Text = lblPlayer2Score.Text = "";
                lblP1BestPlay.Text = L.F("PlayerBestPlay", 1);
                lblP2BestPlay.Text = L.F("PlayerBestPlay", 2);
                txtRackP1.Text = txtRackP2.Text = "";
                // Historique des coups (ListView 2 colonnes Player 1 / Player 2)
                lvWords.Items.Clear();
                _selectedHistoryWord = null;
                lsbHintWords.DataSource = null;
                lsbHintWords.Items.Clear();
                txtBag.Text = game.Bag.GetBagContent();

                if (ckShowGrid.Checked) txtGrid2.Text = game.GenerateTextGrid(game.Grid, true);

                RefreshGridTiles();

                // Remet l'affichage des cases à zéro (texte vide + couleur selon le type de case).
                foreach (var ft in BoardTiles.OfType<FormTile>().Where(t => !t.Name.StartsWith("header_")))
                {
                    ft.Text = string.Empty;
                    ft.SetBackColorFromInnerTile();
                }
            }
            Cursor.Current = Cursors.Default;

        }

        private void RefreshGridTiles()
        {
            //ClearBoardTilesExceptHeaders();

            //construct header tiles
            ConstructHeaderBoardTiles();
            //construct game tiles with letter/word multipliers
            ConstructBoardTiles();
            //Display tiles
            SetGbBoardFromBoardTiles();
            LayoutBoardTiles();
        }

        private void SetGbBoardFromBoardTiles()
        {
            gbBoard.SuspendLayout();
            gbBoard.Controls.Clear();
            gbBoard.Controls.AddRange(BoardTiles.Cast<FormTile>().ToArray());
            gbBoard.ResumeLayout();
        }

        private void ConstructBoardTiles()
        {

            foreach (var vtile in game.Grid)
            {
                BoardTiles.Add(vtile);
            }
        }


        //private void ClearBoardTilesExceptHeaders()
        //{
        //    foreach (var tile in BoardTiles.Where(t => !t.Name.StartsWith("header_")))
        //    {
        //        tile.Clear();
        //    }
        //}
        private void ConstructHeaderBoardTiles()
        {
            for (int i = 0; i < game.BoardSize; i++)
            {
                var frmTile = BoardTiles.FirstOrDefault(f => f.Name == $"header_col{i}");
                if (frmTile == null)
                {
                    frmTile = new FormTile(0, i, $"header_col{i}", FormTile.HeaderTilesBackColor) { Text = $"{i + 1}" };
                    BoardTiles.Add(frmTile);
                }

                frmTile = BoardTiles.FirstOrDefault(f => f.Name == $"header_ligne{i}");
                if (frmTile == null)
                {
                    frmTile = new FormTile(i, 0, $"header_ligne{i}", FormTile.HeaderTilesBackColor) { Text = $"{Solver.DefaultInstance.Alphabet.ElementAt(i).Char}" };
                    BoardTiles.Add(frmTile);
                }
            }
        }

        private void RefreshBoard()
        {
            //gbBoard.Invoke((MethodInvoker)(() => gbBoard.SuspendLayout()));
            //for (int ligne = 0; ligne < grid.GetLength(0); ligne++)
            //{
            //    for (int col = 0; col < grid.GetLength(1); col++)
            //    {

            //        //var formTile = FindFormTile(grid[ligne, col]);
            //        var formTile = grid[ligne, col] as FormTile;
            //        formTile.Tile = grid[ligne, col];
            //        if (formTile.InvokeRequired)
            //            formTile.Invoke((MethodInvoker)(() => formTile.Text = formTile.Tile?.Letter?.Char.ToString()));
            //        else
            //        {
            //            formTile.Text = formTile.Tile?.Letter?.Char.ToString();
            //        }
            //        if (grid[ligne, col].IsEmpty)
            //            if (formTile.InvokeRequired)
            //                formTile.Invoke((MethodInvoker)(() => formTile.GetBackColor(grid[ligne, col])));
            //            else
            //                formTile.GetBackColor(grid[ligne, col]);
            //    }
            //}

            //lsbInfos.Items.Add(Game.IsTransposed ? "Transposed" : "Not Transposed");
            if (ckShowGrid.Checked)
                txtGrid2.Invoke((MethodInvoker)(() => txtGrid2.Text = game.GenerateTextGrid(game.Grid, true)));
            //txtRackP1.Text = Game.Player1.Rack.String();
            //txtRackP2.Text = Game.Player2.Rack.String();
            txtBag.Invoke((MethodInvoker)(() => txtBag.Text = game.Bag.GetBagContent()));
            DisplayScores();
            //gbBoard.Invoke((MethodInvoker)(() => gbBoard.ResumeLayout()));

        }

        private void btnTranspose_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            game.TransposeGameGrid();
            //using (new SuspendDrawingUpdate(sender as Control))
            RefreshBoard();
            Cursor.Current = Cursors.Default;
        }
        //private List<VTile> ClearTilesInPlay(Player p)
        //{
        //    var ret = game.Grid.OfType<VTile>().Where(t => !t.IsValidated).ToList();
        //    foreach (var tile in ret)
        //    {
        //        var frmTile = FindTile(tile);
        //        frmTile.Invoke((MethodInvoker)(() =>
        //        {
        //            //frmTile.Text = "";
        //            //frmTile.Letter = new Letter();
        //            //frmTile.BackColor = frmTile.GetBackColor(tile);
        //        }));
        //    }
        //    //for (int i = 0; i < Grid.LongLength; i++)
        //    //{
        //    //    var tile = Game.Grid.OfType<VTile>().ElementAt(i);
        //    //    if (!tile.IsValidated)
        //    //    {
        //    //        if (!tile.IsEmpty)
        //    //        {
        //    //            if (tile.FromJoker)
        //    //            {
        //    //                p.Rack.Add(GameStyle == 'S' ? AlphabetScrabbleAvecJoker[26] : Game.AlphabetWWFAvecJoker[26]);
        //    //            }
        //    //            else
        //    //            {
        //    //                p.Rack.Add(tile.Letter);
        //    //            }
        //    //            tile.IsValidated = true;
        //    //        }
        //    //        else
        //    //            Grid[tile.Ligne, tile.Col].Letter = new Letter();

        //    //    }
        //    //}
        //    return ret;
        //}
        private void btnBackToRack_Click(object sender, EventArgs e)
        {
            //var ret = ClearTilesInPlay(Game.Player1);
            //if (ret.Any()) txtRackP1.Text = ret.String();
            //Game.Grid = RefreshBoard(Game.Grid);
        }

        private void btnValidate_Click(object sender, EventArgs e)
        {

            var lastPlayed = game.LastPlayedTile ?? LastPlayedTile;
            if (lastPlayed == null) return;
            var word = lastPlayed.GetWordFromTile(Game.CurrentWordDirection);
            if (word == null) return;
            if (game.IsPlayer1)
                Game.DefaultInstance.PreviewWord(game.Player1, (w) => AddWord(w), word, true);
            else
                Game.DefaultInstance.PreviewWord(game.Player2, (w) => AddWord(w), word, true);
            RefreshBoard();
        }
        private Word AddWord(Word word)
        {
            lvWords.Invoke((MethodInvoker)(() => AddWordToHistory(word)));
            return word;
        }

        #region Plateau redimensionnable (suit la taille de la fenêtre)

        private readonly System.Windows.Forms.Timer _resizeTimer = new System.Windows.Forms.Timer { Interval = 80 };
        private List<Control> _rightColumn;
        private int _rightGap;        // écart horizontal plateau -> colonne de droite
        private int _bottomReserve;   // place réservée sous le plateau (barre de réglages + chevalet)
        private Font _tileFont;
        private bool _boardLayoutReady;

        /// <summary>
        /// Prépare le redimensionnement : la colonne de droite (scores, infos, historique) est ancrée
        /// à DROITE pour se décaler au lieu de s'étirer, ce qui libère la place pour le plateau.
        /// </summary>
        private void SetupResizableBoard()
        {
            _rightColumn = Controls.Cast<Control>()
                .Where(c => c != gbBoard && c.Left >= gbBoard.Right - 2)
                .ToList();
            foreach (var c in _rightColumn)
            {
                var keepBottom = (c.Anchor & AnchorStyles.Bottom) != 0;
                c.Anchor = AnchorStyles.Top | AnchorStyles.Right | (keepBottom ? AnchorStyles.Bottom : AnchorStyles.None);
            }
            _rightGap = _rightColumn.Any() ? Math.Max(4, _rightColumn.Min(c => c.Left) - gbBoard.Right) : 8;
            int below = Math.Max(lblCurrentRack.Bottom, pnlSettingsBar?.Bottom ?? 0);
            _bottomReserve = Math.Max(ClientSize.Height - gbBoard.Bottom, below - gbBoard.Bottom + 6);

            _resizeTimer.Tick += (s, e) => { _resizeTimer.Stop(); LayoutBoardArea(); };
            Resize += (s, e) => { if (WindowState != FormWindowState.Minimized) { _resizeTimer.Stop(); _resizeTimer.Start(); } };
            _boardLayoutReady = true;
            LayoutBoardArea();
        }

        /// <summary>Taille le cadre du plateau au plus grand carré disponible, puis replace les cases.</summary>
        private void LayoutBoardArea()
        {
            if (!_boardLayoutReady) return;
            int n = game.BoardSize + 1;                                // +1 : en-têtes (1..15 / A..O)
            int rightLimit = _rightColumn.Any() ? _rightColumn.Min(c => c.Left) - _rightGap : ClientSize.Width - 8;
            int availW = rightLimit - gbBoard.Left;
            int availH = ClientSize.Height - _bottomReserve - gbBoard.Top;
            int borderW = gbBoard.Width - gbBoard.DisplayRectangle.Width + 4;
            int borderH = gbBoard.Height - gbBoard.DisplayRectangle.Height + 4;
            int tile = Math.Max(14, Math.Min((availW - borderW) / n, (availH - borderH) / n));

            SuspendLayout();
            gbBoard.Size = new Size(tile * n + borderW, tile * n + borderH);
            LayoutBoardTiles();
            if (pnlSettingsBar != null)
            {
                pnlSettingsBar.Location = new Point(gbBoard.Left, gbBoard.Bottom + 2);
                lblCurrentRack.Top = pnlSettingsBar.Bottom + 2;
            }
            else lblCurrentRack.Top = gbBoard.Bottom + 4;
            ResumeLayout(true);
        }

        /// <summary>Place et dimensionne toutes les cases (et en-têtes) dans la zone utile du cadre.</summary>
        private void LayoutBoardTiles()
        {
            var tiles = gbBoard.Controls.OfType<FormTile>().ToList();
            if (tiles.Count == 0) return;
            int n = game.BoardSize + 1;
            var r = gbBoard.DisplayRectangle;
            int tile = Math.Max(14, Math.Min((r.Width - 4) / n, (r.Height - 4) / n));
            int ox = r.Left + (r.Width - tile * n) / 2;
            int oy = r.Top + 2;

            // Police du texte natif (en-têtes, saisie) proportionnelle à la case ; une seule instance partagée.
            var oldFont = _tileFont;
            _tileFont = new Font("Microsoft Sans Serif", Math.Max(7f, tile * 0.55f), FontStyle.Bold, GraphicsUnit.Pixel);

            gbBoard.SuspendLayout();
            foreach (var t in tiles)
            {
                int col, row;
                if (t.Name.StartsWith("header_col")) { col = t.Col + 1; row = 0; }
                else if (t.Name.StartsWith("header_ligne")) { col = 0; row = t.Ligne + 1; }
                else { col = t.Col + 1; row = t.Ligne + 1; }
                t.AutoSize = false;     // sinon la hauteur d'un TextBox est imposée par sa police
                t.Font = _tileFont;
                t.SetBounds(ox + col * tile, oy + row * tile, tile, tile);
            }
            gbBoard.ResumeLayout(true);
            oldFont?.Dispose();
        }

        #endregion

        #region Langue de l'interface (FR / EN)

        private ComboBox cbLanguage;
        private Label lblLanguage;
        private readonly ToolTip _tips = new ToolTip();

        /// <summary>Sélecteur de langue créé en code (évite de modifier le Designer).</summary>
        private void CreateLanguageSelector()
        {
            lblLanguage = new Label { AutoSize = true, Name = "lblLanguage" };
            cbLanguage = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Name = "cbLanguage"
            };
            cbLanguage.Items.AddRange(new object[] { "Français", "English" });
            cbLanguage.SelectedIndex = L.Language == "en" ? 1 : 0;
            cbLanguage.SelectedIndexChanged += (s, e) =>
            {
                var lang = cbLanguage.SelectedIndex == 1 ? "en" : "fr";
                if (lang == L.Language) return;
                L.SetLanguage(lang);
                _settings.Language = lang;
                _settings.Save();
                ApplyLanguage();
            };
        }

        private ComboBox cbBoardFont;
        private Label lblBoardFont;

        /// <summary>Sélecteur de la police des lettres du plateau (créé en code, sous la langue).</summary>
        private void CreateBoardFontSelector()
        {
            lblBoardFont = new Label { AutoSize = true, Name = "lblBoardFont" };
            cbBoardFont = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Name = "cbBoardFont"
            };
            cbBoardFont.Items.Add(L.T("StandardFont"));          // index 0 = rendu Windows standard
            foreach (var n in TileFonts.Names) cbBoardFont.Items.Add(n);
            int idx = cbBoardFont.Items.IndexOf(_settings.BoardFontName ?? "");
            cbBoardFont.SelectedIndex = idx > 0 ? idx : 0;
            cbBoardFont.SelectedIndexChanged += (s, e) =>
            {
                _settings.BoardFontName = cbBoardFont.SelectedIndex <= 0 ? "" : cbBoardFont.SelectedItem.ToString();
                _settings.Save();
                ApplyTileStyle();
            };
        }

        private FlowLayoutPanel pnlSettingsBar;

        /// <summary>
        /// Barre « Langue / Police plateau » placée juste SOUS le plateau, calculée à partir de la
        /// position réelle du plateau (après mise à l'échelle DPI de la fenêtre), avec des largeurs
        /// mesurées sur le texte : elle reste entièrement visible quelle que soit l'échelle d'affichage.
        /// </summary>
        private void LayoutSettingsBar()
        {
            pnlSettingsBar = new FlowLayoutPanel
            {
                Name = "pnlSettingsBar",
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                WrapContents = false,
                FlowDirection = FlowDirection.LeftToRight,
                BackColor = Color.Transparent,
                Location = new Point(gbBoard.Left, gbBoard.Bottom + 2)
            };
            // Libellés : marge haute pour les centrer verticalement face aux listes déroulantes.
            int labelTop = Math.Max(3, (cbLanguage.PreferredHeight - Font.Height) / 2 + 3);
            lblLanguage.Margin = new Padding(6, labelTop, 2, 3);
            lblBoardFont.Margin = new Padding(18, labelTop, 2, 3);
            cbLanguage.Margin = cbBoardFont.Margin = new Padding(3);
            cbLanguage.Width = ComboWidth(cbLanguage);
            cbBoardFont.Width = ComboWidth(cbBoardFont);
            pnlSettingsBar.Controls.AddRange(new Control[] { lblLanguage, cbLanguage, lblBoardFont, cbBoardFont });
            Controls.Add(pnlSettingsBar);
            pnlSettingsBar.BringToFront();

            // Le grand libellé « Joueur X : chevalet » est juste sous le plateau : on le décale si besoin.
            if (lblCurrentRack.Top < pnlSettingsBar.Bottom + 2)
                lblCurrentRack.Top = pnlSettingsBar.Bottom + 2;
        }

        /// <summary>Largeur d'une liste déroulante ajustée à son élément le plus long.</summary>
        private static int ComboWidth(ComboBox cb)
        {
            int max = 60;
            foreach (var o in cb.Items)
                max = Math.Max(max, TextRenderer.MeasureText(o.ToString(), cb.Font).Width);
            return max + SystemInformation.VerticalScrollBarWidth + 12;
        }

        /// <summary>Applique police / couleurs / contour aux lettres du plateau et les redessine.</summary>
        private void ApplyTileStyle()
        {
            TileStyle.Family = TileFonts.Get(_settings.BoardFontName);
            TileStyle.Player1Fill = _settings.GetPlayerColor(true);
            TileStyle.Player2Fill = _settings.GetPlayerColor(false);
            TileStyle.OutlineWidth = Math.Max(0.5f, Math.Min(_settings.BoardOutlineWidth, 4f));
            foreach (var t in gbBoard.Controls.OfType<FormTile>())
                t.Invalidate();
            UpdateRackLabelLayout();
        }

        private bool _rackIsPlayer1 = true;
        private int _rackLabelHeight;

        /// <summary>
        /// Le libellé « Joueur X : chevalet » sous le plateau utilise le même rendu que les lettres
        /// du plateau (police choisie, contour noir, couleur du joueur dont c'est le tour).
        /// </summary>
        private void SetupRackLabel()
        {
            _rackLabelHeight = Math.Max(20, lblCurrentRack.Height);   // hauteur de la police d'origine
            lblCurrentRack.Paint += LblCurrentRack_Paint;
            lblCurrentRack.TextChanged += (s, e) => UpdateRackLabelLayout();
            UpdateRackLabelLayout();
        }

        private void UpdateRackLabelLayout()
        {
            if (_rackLabelHeight == 0) return;                // pas encore initialisé
            var family = TileStyle.Family;
            if (family == null)
            {
                lblCurrentRack.AutoSize = true;               // police « standard » : rendu natif
            }
            else
            {
                lblCurrentRack.AutoSize = false;
                int w = TileFonts.MeasureLineWidth(lblCurrentRack.Text, family, _rackLabelHeight, RackOutline);
                lblCurrentRack.Size = new Size(Math.Max(10, w), _rackLabelHeight);
            }
            lblCurrentRack.Invalidate();
        }

        private static float RackOutline => TileStyle.OutlineWidth * 1.4f;   // texte plus grand : contour un peu plus épais

        private void LblCurrentRack_Paint(object sender, PaintEventArgs e)
        {
            var family = TileStyle.Family;
            if (family == null || string.IsNullOrEmpty(lblCurrentRack.Text)) return;
            // Le Label a déjà dessiné son texte natif : on repeint son fond puis le texte stylé.
            var bg = lblCurrentRack.BackColor.A == 255 ? lblCurrentRack.BackColor : (lblCurrentRack.Parent?.BackColor ?? BackColor);
            using (var b = new SolidBrush(bg)) e.Graphics.FillRectangle(b, lblCurrentRack.ClientRectangle);
            TileFonts.DrawOutlinedLine(e.Graphics, lblCurrentRack.Text, family, lblCurrentRack.ClientRectangle,
                                       _rackIsPlayer1 ? TileStyle.Player1Fill : TileStyle.Player2Fill, RackOutline);
        }

        /// <summary>
        /// Applique la langue courante : chaque contrôle dont la clé "Ctl_" + Name existe dans
        /// Strings(.fr).resx reçoit le texte traduit, puis on rafraîchit les textes dynamiques.
        /// </summary>
        private void ApplyLanguage()
        {
            var title = L.TryT("Ctl_" + Name);
            if (title != null) Text = title;
            foreach (var c in AllControls(this))
            {
                var t = L.TryT("Ctl_" + c.Name);
                if (t != null) c.Text = t;
            }
            if (lblLanguage != null) lblLanguage.Text = L.T("Language");
            if (lblBoardFont != null) lblBoardFont.Text = L.T("BoardFont");
            if (cbBoardFont != null && cbBoardFont.Items.Count > 0)
            {
                int sel = cbBoardFont.SelectedIndex;
                cbBoardFont.Items[0] = L.T("StandardFont");
                cbBoardFont.SelectedIndex = sel;
            }

            // Textes dynamiques
            if (game.CancelToken != null) btnDemoAll.Text = L.T("StopAutoPlay");
            if (lvWords.Columns.Count >= 2)
            {
                lvWords.Columns[0].Text = L.F("PlayerName", 1);
                lvWords.Columns[1].Text = L.F("PlayerName", 2);
            }
            if (IsHandleCreated && (game.Player1.Moves.Any() || game.Player2.Moves.Any()))
                DisplayScores();
            else
            {
                lblP1BestPlay.Text = L.F("PlayerBestPlay", 1);
                lblP2BestPlay.Text = L.F("PlayerBestPlay", 2);
            }
            _tips.SetToolTip(lvWords, L.T("ColorHint"));
        }

        private static System.Collections.Generic.IEnumerable<Control> AllControls(Control root)
        {
            foreach (Control c in root.Controls)
            {
                yield return c;
                foreach (var child in AllControls(c)) yield return child;
            }
        }

        #endregion

        #region Historique des coups : ListView 2 colonnes (Player 1 | Player 2)

        private Word _selectedHistoryWord;

        /// <summary>
        /// Configuration du ListView (faite en code plutôt que dans le Designer).
        /// Une ligne = un tour : coup de P1 à gauche, coup de P2 à droite, le plus récent en haut.
        /// </summary>
        private void SetupWordsListView()
        {
            lvWords.View = View.Details;
            lvWords.OwnerDraw = true;
            lvWords.FullRowSelect = false;
            lvWords.MultiSelect = false;
            lvWords.HideSelection = false;
            lvWords.HeaderStyle = ColumnHeaderStyle.Clickable;
            lvWords.Columns.Clear();
            lvWords.Columns.Add(L.F("PlayerName", 1));
            lvWords.Columns.Add(L.F("PlayerName", 2));
            ResizeWordsColumns();

            // Hauteur de ligne : astuce classique, une ImageList "vide" de la hauteur voulue.
            int rowHeight = (int)Math.Ceiling(_settings.HistoryFontSizePt * 96f / 72f * 1.6f);
            lvWords.SmallImageList = new ImageList { ImageSize = new Size(1, Math.Max(16, Math.Min(rowHeight, 64))) };

            lvWords.DrawColumnHeader += (s, e) => e.DrawDefault = true;
            lvWords.DrawItem += (s, e) => { /* dessin réalisé par sous-élément (DrawSubItem) */ };
            lvWords.DrawSubItem += LvWords_DrawSubItem;
            lvWords.MouseClick += LvWords_MouseClick;
            lvWords.MouseDoubleClick += LvWords_MouseDoubleClick;
            lvWords.ColumnClick += LvWords_ColumnClick;
            lvWords.Resize += (s, e) => ResizeWordsColumns();
        }

        private void ResizeWordsColumns()
        {
            if (lvWords.Columns.Count < 2) return;
            int w = Math.Max(50, (lvWords.ClientSize.Width - 2) / 2);
            lvWords.Columns[0].Width = w;
            lvWords.Columns[1].Width = w;
        }

        private void AddWordToHistory(Word word)
        {
            // PreviewWord ajoute le coup dans p.Moves AVANT d'appeler ce callback :
            // on sait donc à quel joueur il appartient (plus fiable que word.IsPlayedByPlayer1).
            bool player1 = !game.Player2.Moves.Contains(word);
            var top = lvWords.Items.Count > 0 ? lvWords.Items[0] : null;

            if (player1)
            {
                var item = new ListViewItem(word.ToString());
                item.SubItems[0].Tag = word;
                item.SubItems.Add("");
                lvWords.Items.Insert(0, item);
            }
            else if (top != null && top.SubItems.Count > 1 && string.IsNullOrEmpty(top.SubItems[1].Text)
                     && !string.IsNullOrEmpty(top.SubItems[0].Text))
            {
                // P1 vient de jouer sur cette ligne : on complète la colonne P2 du même tour
                top.SubItems[1].Text = word.ToString();
                top.SubItems[1].Tag = word;
                lvWords.Invalidate(top.Bounds);
            }
            else
            {
                // P2 joue sans coup P1 en attente (P1 a passé, ou P2 commence) : nouvelle ligne
                var item = new ListViewItem("");
                item.SubItems.Add(word.ToString());
                item.SubItems[1].Tag = word;
                lvWords.Items.Insert(0, item);
            }
        }

        private void LvWords_DrawSubItem(object sender, DrawListViewSubItemEventArgs e)
        {
            // Particularité WinForms : pour la colonne 0, e.Bounds couvre toute la ligne.
            var bounds = e.ColumnIndex == 0
                ? new Rectangle(e.Bounds.X, e.Bounds.Y, lvWords.Columns[0].Width, e.Bounds.Height)
                : e.Bounds;

            var word = e.SubItem?.Tag as Word;
            bool selected = word != null && ReferenceEquals(word, _selectedHistoryWord);
            using (var bg = new SolidBrush(selected ? Color.FromArgb(215, 228, 255) : lvWords.BackColor))
                e.Graphics.FillRectangle(bg, bounds);

            var text = e.SubItem?.Text;
            if (string.IsNullOrEmpty(text)) return;

            bool player1 = e.ColumnIndex == 0;
            using (var sf = new StringFormat
            {
                LineAlignment = StringAlignment.Center,
                Trimming = StringTrimming.EllipsisCharacter,
                FormatFlags = StringFormatFlags.NoWrap
            })
            {
                var r = new RectangleF(bounds.X + 4, bounds.Y, bounds.Width - 6, bounds.Height);
                var c = _settings.GetPlayerColor(player1);
                // Couleur assombrie : un jaune vif sans contour serait illisible sur fond blanc.
                var dark = Color.FromArgb(c.R * 2 / 3, c.G * 2 / 3, c.B * 2 / 3);
                using (var f = new Font("Segoe UI", Math.Max(7f, Math.Min(_settings.HistoryFontSizePt, 16f)), FontStyle.Bold, GraphicsUnit.Point))
                using (var b = new SolidBrush(dark))
                    e.Graphics.DrawString(text, f, b, r, sf);
            }
        }

        /// <summary>Coup situé sous le curseur (la colonne est déduite de l'abscisse).</summary>
        private Word HistoryWordAt(Point p)
        {
            var hit = lvWords.HitTest(p);
            if (hit.Item == null) return null;
            int col = p.X < lvWords.Columns[0].Width ? 0 : 1;
            return hit.Item.SubItems.Count > col ? hit.Item.SubItems[col].Tag as Word : null;
        }

        private void LvWords_MouseClick(object sender, MouseEventArgs e)
        {
            _selectedHistoryWord = HistoryWordAt(e.Location);
            lvWords.Invalidate();
            HighlightWordOnBoard(_selectedHistoryWord);
        }

        private void LvWords_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            HistoryWordAt(e.Location)?.ShowDefinition();
        }

        /// <summary>Clic sur l'en-tête d'une colonne : choix de la couleur du joueur (persistée).</summary>
        private void LvWords_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            bool player1 = e.Column == 0;
            using (var dlg = new ColorDialog { Color = _settings.GetPlayerColor(player1), FullOpen = true, AnyColor = true })
            {
                if (dlg.ShowDialog(this) != DialogResult.OK) return;
                _settings.SetPlayerColor(player1, dlg.Color);
                _settings.Save();
                lvWords.Invalidate();
                ApplyTileStyle();   // la même couleur sert aux lettres du plateau
            }
        }

        /// <summary>Surligne sur le plateau les cases du coup sélectionné dans l'historique.</summary>
        private void HighlightWordOnBoard(Word word)
        {
            foreach (var c in gbBoard.Controls.OfType<FormTile>().Where(ft => ft.BackColor == Color.LightBlue))
                c.BackColor = FormTile.Player1MoveColor;
            foreach (var c in gbBoard.Controls.OfType<FormTile>().Where(ft => ft.BackColor == Color.LightCoral))
                c.BackColor = FormTile.Player2MoveColor;
            if (word == null) return;

            bool isPlayer1word = game.Player1.Moves.Contains(word);
            foreach (var t in word.GetTiles())
            {
                var frmTile = t.FindFormTile(BoardTiles);
                if (frmTile != null)
                    frmTile.BackColor = isPlayer1word ? Color.LightBlue : Color.LightCoral;
            }
        }

        #endregion
        //private bool ValidateWord(Player p)
        //{
        //    var validate = new List<VTile>();
        //    foreach (var t in Game.Grid)
        //        if (!t.IsValidated)
        //        {
        //            validate.Add(t);
        //            t.IsValidated = true;
        //        }

        //    if (!validate.Any())
        //        return false;
        //    foreach (var t in validate)
        //    {
        //        var frmTile = groupBox1.Controls.Find($"t{t.Ligne}_{t.Col}", false).First() as FormTile;
        //        frmTile.ReadOnly = true;
        //        frmTile.BackColor = p == Game.Player1 ? Color.LightYellow : Color.LightGreen;
        //        Game.Bag.RemoveLetterFromBag(t.Letter);
        //    }

        //    p.Points += isPlayer1 ? player1Score : player2Score;
        //    isPlayer1 = !isPlayer1;
        //    Game.Grid = RefreshBoard(Game.Grid);
        //    return true;
        //}

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtRackP1.Text)) return;// || txtRackP1.Text.Any(c => !Game.AlphabetAvecJoker.Any(ch => c == ch.Char))) return;
            Cursor.Current = Cursors.WaitCursor;
            game.Bag.GetLetters(game.Player1, txtRackP1.Text.Trim());
            if (string.IsNullOrEmpty(lsbHintWords.DisplayMember)) lsbHintWords.DisplayMember = "DisplayText";
            var ret = Solver.DefaultInstance.FindMoves(150, Game.HintSortByBestScore);
            lsbInfos.Items.Insert(0, game.IsTransposed ? L.T("Transposed") : L.T("NotTransposed"));
            lsbInfos.Items.Insert(0, $"NbPossibleMoves={Solver.DefaultInstance.NbPossibleMoves}");
            lsbInfos.Items.Insert(0, $"NbAcceptedMoves={Solver.DefaultInstance.NbAcceptedMoves}");

            lsbHintWords.DataSource = ret;
            Cursor.Current = Cursors.Default;
        }

        private async void btnDemo_Click(object sender, EventArgs e)
        {
            await PlayDemo();
        }


        private async Task PlayDemo(int wait = 0)
        {

            if (game.NoMoreMovesCount > 1)
            {
                game.EndGame = true;
                return;
            }
            var playerNumber = game.IsPlayer1 ? "1" : "2";
            try
            {
                Logger.Log($"-- Tour Player {playerNumber} (IsPlayer1={game.IsPlayer1}, NoMoreMovesCount={game.NoMoreMovesCount}) --");
                Cursor.Current = Cursors.WaitCursor;
                game.Bag.GetLetters(game.CurrentPlayer);
                Logger.Log($"Player {playerNumber}: tirage OK (rack={game.CurrentPlayer.Rack}).");
                // Sac vide : simple information. Ce n'est PAS une fin de tour : le joueur peut
                // encore jouer avec les lettres de son chevalet. On n'incrémente donc plus
                // NoMoreMovesCount ici (c'est la branche "aucun coup -> passe" qui le fait).
                if (game.Bag.LeftLettersCount == 0)
                    lsbInfos.Invoke((MethodInvoker)(() => lsbInfos.Items.Insert(0, L.T("BagEmpty"))));

                lblCurrentRack.Invoke((MethodInvoker)(() =>
                {
                    _rackIsPlayer1 = playerNumber == "1";   // couleur du joueur dont c'est le tour
                    lblCurrentRack.Text = L.F("CurrentRack", playerNumber, game.CurrentPlayer.Rack);
                    lblCurrentRack.Invalidate();
                }));

                Logger.Log($"Player {playerNumber}: avant FindMoves...");
                // IMPORTANT : FindMoves transpose la grille en créant des contrôles FormTile.
                // Il DOIT donc tourner sur le thread UI (sinon ces contrôles sont créés sur un
                // thread de fond qui meurt -> InvalidAsynchronousStateException lors du Invoke,
                // et fuite de handles). La boucle AutoPlay tournant déjà sur le thread UI, l'appel
                // est synchrone ; l'UI se fige brièvement pendant la recherche, ce qui est acceptable.
                var ret = Solver.DefaultInstance.FindMoves(60);
                Logger.Log($"Player {playerNumber} ({game.CurrentPlayer.Rack}) : {ret.Count} coups trouvés.");
                // On est sur le thread UI : affectation directe (plus de Invoke superflu).
                // Encadré pour qu'un éventuel souci de data binding n'interrompe pas le coup.
                try
                {
                    Logger.Log($"Player {playerNumber}: binding lsbHintWords...");
                    using (var susp = new SuspendDrawingUpdate(lsbHintWords))
                        lsbHintWords.DataSource = ret;
                    Logger.Log($"Player {playerNumber}: binding lsbHintWords OK.");
                }
                catch (Exception bindEx)
                {
                    Logger.LogException($"binding lsbHintWords (Player {playerNumber})", bindEx);
                }
                Logger.Log($"Player {playerNumber}: sélection du coup...");
                // Dédoublonnage par POSITION + TEXTE + DIRECTION (Word.Equals), et NON par Serialize :
                // Serialize inclut l'Index, donc un candidat (Index=0) n'égalait jamais un coup déjà
                // joué (Index>0) -> le même mot (ex. KEUF) pouvait être rejoué et recompté.
                var word = ret
                    .Where(w => !Solver.DefaultInstance.PlayedWords.Any(pw => pw.Equals(w)))
                    .OrderByDescending(r => r.Points)
                    .FirstOrDefault() as Word;
                Logger.Log($"Player {playerNumber}: coup choisi = {(word == null ? "aucun" : word.ToString())}.");

                if (word != null)
                {
                    game.NoMoreMovesCount = 0; // un coup a été joué : on réarme le compteur de passes
                    CurrentWord = word;

                    // On mémorise le joueur courant AVANT PreviewWord : celui-ci bascule IsPlayer1
                    // quand validateWord=true, donc game.CurrentPlayer pointerait ensuite sur l'adversaire.
                    var playingPlayer = game.CurrentPlayer;
                    Logger.Log($"Player {playerNumber}: PreviewWord {word.Text} ({word.Direction})...");
                    int points = Game.DefaultInstance.PreviewWord(playingPlayer, (w) => AddWord(w), word, true);
                    Logger.Log($"Player {playerNumber}: PreviewWord terminé.");
                    playingPlayer.Points += points;
                    Logger.Log($"Player {playerNumber} joue {word} (+{points}) -> score {playingPlayer.Points}.");
                    RefreshBoard();
                    Logger.Log($"Player {playerNumber}: plateau rafraîchi.");
                }
                else
                {
                    // Aucun NOUVEAU coup jouable : le joueur passe. La partie se termine seulement
                    // quand les deux joueurs passent consécutivement (NoMoreMovesCount > 1, testé en
                    // haut de PlayDemo).
                    game.NoMoreMovesCount++;
                    Logger.Log($"Player {playerNumber} : aucun nouveau coup -> passe (NoMoreMovesCount={game.NoMoreMovesCount}).");
                    if (game.NoMoreMovesCount < 2)
                        InsertLsbText(lsbInfos, L.F("NoWordsFound", playerNumber, game.CurrentPlayer.Rack));
                    game.IsPlayer1 = !game.IsPlayer1; // le joueur passe son tour
                }
                game.Bag.GetLetters(game.CurrentPlayer);
                SetPlayerRackText(game.IsPlayer1 ? txtRackP1 : txtRackP2);
                Logger.Log($"Player {playerNumber}: fin de tour.");

            }
            catch (Exception ex)
            {
                // IMPORTANT : on attrape TOUTES les exceptions (avant, seules les ArgumentException
                // l'étaient, et le reste faisait planter AutoPlay en silence -> "figé").
                Logger.LogException($"PlayDemo (Player {playerNumber})", ex);
                InsertLsbText(lsbInfos, L.F("GenericError", ex.GetType().Name, ex.Message));
                game.EndGame = true; // on arrête proprement la boucle AutoPlay plutôt que de la laisser figée
            }
            finally
            {
                Logger.Log($"Player {playerNumber}: finally - avant Task.Delay({wait}).");
                await Task.Delay(wait);
                Logger.Log($"Player {playerNumber}: finally - après Task.Delay (thread={System.Threading.Thread.CurrentThread.ManagedThreadId}).");
                Cursor.Current = Cursors.Default;
            }

        }

        private void SetPlayerRackText(TextBox txt)
        {
            txt.Invoke((MethodInvoker)(() => txt.Text = $"{game.CurrentPlayer.Rack}"));
        }
        private void InsertLsbText(ListBox lsb, string txt)
        {
            lsb.Invoke((MethodInvoker)(() => lsb.Items.Insert(0, txt)));
        }

        private void DisplayScores()
        {
            this.Invoke((MethodInvoker)(() =>
            {
                lblPlayer1Score.Text = L.F("PlayerScore", 1, game.Player1.Points);
                lblPlayer2Score.Text = L.F("PlayerScore", 2, game.Player2.Points);
                var bestP1Move = game.Player1.Moves.OrderByDescending(m => m.Points).FirstOrDefault();
                if (bestP1Move != null) lblP1BestPlay.Text = L.F("PlayerBestPlayValue", 1, bestP1Move);
                var bestP2Move = game.Player2.Moves.OrderByDescending(m => m.Points).FirstOrDefault();
                if (bestP2Move != null) lblP2BestPlay.Text = L.F("PlayerBestPlayValue", 2, bestP2Move);
            }));
        }

        Word CurrentWord { get; set; }
        private async void btnDemoAll_Click(object sender, EventArgs e)
        {

            if (game.CancelToken == null)
            {
                NewGame(CurrentDicoName, !ckKeepExistingBoard.Checked);
                game.CancelToken = new CancellationTokenSource();
                var btn = sender as Button;
                btn.Text = L.T("StopAutoPlay");
                Logger.Log("AutoPlay All : démarrage.");
                // La boucle tourne désormais sur le THREAD UI (via async/await), et non plus
                // sur un thread d'arrière-plan. Les reprises d'await se font sur le thread UI
                // de façon déterministe (plus de deadlock/continuation perdue entre deux tours),
                // et la seule partie coûteuse (FindMoves) est déportée en tâche de fond.
                try
                {
                    await LoopDemo(game.CancelToken);
                }
                catch (Exception ex)
                {
                    Logger.LogException("btnDemoAll_Click", ex);
                }
                finally
                {
                    CancelAction();
                }
            }
            else
            {
                Logger.Log("AutoPlay All : demande d'arrêt.");
                game.CancelToken.Cancel();
            }
        }

        private void CancelAction()
        {
            btnDemoAll.Invoke((MethodInvoker)(() => btnDemoAll.Text = L.T("Ctl_btnDemoAll")));
            game.EndGame = true;
            game.CancelToken = null;
        }

        private async Task LoopDemo(CancellationTokenSource cancelToken)
        {
            try
            {
                while (!game.EndGame)
                {
                    if (cancelToken != null && cancelToken.IsCancellationRequested && cancelToken.Token.CanBeCanceled)
                    {
                        cancelToken.Token.ThrowIfCancellationRequested();
                        return;
                    }
                    Logger.Log($"LoopDemo: nouvelle itération (EndGame={game.EndGame}).");
                    await PlayDemo(1000);
                    Logger.Log($"LoopDemo: retour de PlayDemo (EndGame={game.EndGame}).");
                }
                Logger.Log($"LoopDemo: sortie de boucle (EndGame={game.EndGame}).");
                if (game.EndGame)
                    ShowWinner(true);
            }


            catch (OperationCanceledException)
            {
                Logger.Log("AutoPlay All : annulé par l'utilisateur.");
                CancelAction();
            }
            catch (Exception ex)
            {
                Logger.LogException("LoopDemo", ex);
                CancelAction();
            }
            finally
            {
                Logger.Log("AutoPlay All : boucle terminée.");
            }

        }

        private void ShowWinner(bool endGame = false)
        {
            if (endGame)
            {
                lsbInfos.Invoke((MethodInvoker)(() =>
                {
                    lsbInfos.Items.Insert(0, L.T("GameEnded"));
                    if (game.Player1.Points == game.Player2.Points)
                        lsbInfos.Items.Insert(0, L.F("Draw", game.Player1.Points));
                    else
                    {
                        bool player1Wins = game.Player1.Points > game.Player2.Points;
                        lsbInfos.Items.Insert(0, L.F("PlayerWins", L.F("PlayerName", player1Wins ? 1 : 2),
                                                     player1Wins ? game.Player1.Points : game.Player2.Points));
                    }
                }));
                game.CancelToken?.Cancel();

            }
        }

        private void lsb_Click(object sender, EventArgs e)
        {
            var word = lsbHintWords.SelectedItem as Word;
            if (word == null) return;
            Game.DefaultInstance.PreviewWord(game.CurrentPlayer, (w) => AddWord(w), word, false, false);
            RefreshBoard();

        }

        private void btnSaveGame_Click(object sender, EventArgs e)
        {
            Game.DefaultInstance.SaveGame();
        }



        private void btnLoadGame_Click(object sender, EventArgs e)
        {
            Game.DefaultInstance.Load(BoardTiles, (w) => AddWord(w));

        }



        private void btnNewGame_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(CurrentDicoName)) return;
            NewGame(CurrentDicoName, true);
        }


        private void rbSize15_CheckedChanged(object sender, EventArgs e)
        {
            if (rbSize15.Checked) game.BoardSize = 15;
            if (rbSize11.Checked) game.BoardSize = 11;

            nudCustomBoardSize.Value = game.BoardSize;
            NewGame(CurrentDicoName, !ckKeepExistingBoard.Checked);
        }

        private void ckShowGrid_CheckedChanged(object sender, EventArgs e)
        {
            txtGrid2.Visible = ckShowGrid.Checked;
        }

        private void rbWordDirDown_CheckedChanged(object sender, EventArgs e)
        {
            Game.CurrentWordDirection = rbWordDirDown.Checked ? MovementDirection.Down : MovementDirection.Across;
        }



        private void rbMaxLength_CheckedChanged(object sender, EventArgs e)
        {
            Game.HintSortByBestScore = !rbMaxLength.Checked;
        }

        private void MainForm_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.S)
            {
                Game.DefaultInstance.SaveGame();

            }
            else if (e.Control && e.KeyCode == Keys.L)
            {
                Game.DefaultInstance.Load(BoardTiles, (w) => AddWord(w));

            }

        }
        private void rbGameStyleScrabble_CheckedChanged(object sender, EventArgs e)
        {

        }
        private void rbWordsWithFriends_CheckedChanged(object sender, EventArgs e)
        {
            Solver.DefaultInstance.Mode = rbGameStyleScrabble.Checked ? 'S' : 'W';
            NewGame(CurrentDicoName, !ckKeepExistingBoard.Checked);
        }

        private void txtMotExiste_TextChanged(object sender, EventArgs e)
        {
            lblMotExiste.ForeColor = Dictionnaire.DefaultInstance.MotAdmis(txtMotExiste.Text.Trim()) ? Color.Green : Color.Red;
        }

        private void txtMotExiste_DoubleClick(object sender, EventArgs e)
        {
            if (Dictionnaire.DefaultInstance.MotAdmis(txtMotExiste.Text.Trim()))
                Process.Start(new ProcessStartInfo($"https://1mot.net/{txtMotExiste.Text.ToLower()}") { UseShellExecute = true });

        }

        private void MainForm_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void MainForm_Click(object sender, EventArgs e)
        {

        }

        private void btnUndoLast_Click(object sender, EventArgs e)
        {
            var allWords = game.Player1.Moves.Union(game.Player2.Moves).OrderByDescending(a => a.Index);
            var lastWord = allWords.FirstOrDefault();
            if (lastWord == null) return;
            var tiles = lastWord.GetTiles();
            if (lastWord.IsPlayedByPlayer1)
                game.Player1.Moves.Remove(lastWord);
            else
                game.Player2.Moves.Remove(lastWord);

            foreach (var t in tiles.Where(tt => tt.Letter.Char != Game.EmptyChar))
            {
                game.Bag.PutBackLetterInBag(t.Letter);
            }
            foreach (var tile in tiles.Where(t => t.WordIndex == lastWord.Index))
            {

                game.Grid[tile.Ligne, tile.Col] = new FormTile(tile.Ligne, tile.Col);
                game.Grid[tile.Ligne, tile.Col].Letter.Char = Game.EmptyChar;
            }
            //using (new SuspendDrawingUpdate(sender as Control))
            RefreshBoard();
        }

        private void rbDico_CheckedChanged(object sender, EventArgs e)
        {
            var rb = sender as RadioButton;
            // On ignore l'événement pendant l'initialisation du formulaire, et pour le bouton
            // qui vient de se décocher (CheckedChanged se déclenche aussi dans ce cas).
            if (!_initialized || rb == null || !rb.Checked) return;

            var currentDico = Dictionnaire.NomDicoDawgEN_Collins;
            switch (rb.Name)
            {
                case "rbODS6": currentDico = Dictionnaire.NomDicoDawgODS6; break;
                case "rbODS7": currentDico = Dictionnaire.NomDicoDawgODS7; break;   // (le nom du contrôle est bien rbODS7)
                case "rbODS8": currentDico = Dictionnaire.NomDicoDawgODS8; break;
                case "rbODS9": currentDico = Dictionnaire.NomDicoDawgODS9; break;
                case "rbEnCollins2019": currentDico = Dictionnaire.NomDicoDawgEN_Collins; break;
                default: break;
            }

            // Le dictionnaire est un singleton chargé une seule fois : il faut explicitement
            // recharger le DAWG pour que le changement prenne effet.
            LoadDictionary(currentDico);
            NewGame(CurrentDicoName, !ckKeepExistingBoard.Checked);
        }

        private void btnWordsFinder_Click(object sender, EventArgs e)
        {
            //FrmDebug frm = new FrmDebug();
            //frm.Show();
            WordsFinder wf = new WordsFinder(game);
            wf.Show();
        }

        private void nudCustomBoardSize_ValueChanged(object sender, EventArgs e)
        {
            var newVal = (int)nudCustomBoardSize.Value;
            if (newVal != game.BoardSize)
            {
                game.BoardSize = newVal;
                NewGame(CurrentDicoName, !ckKeepExistingBoard.Checked);
            }
        }
    }
}
