namespace Dawg.Solver.Winform
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnSearch = new System.Windows.Forms.Button();
            this.txtRackP1 = new System.Windows.Forms.TextBox();
            this.lsbHintWords = new System.Windows.Forms.ListBox();
            this.txtBag = new System.Windows.Forms.TextBox();
            this.txtGrid2 = new System.Windows.Forms.TextBox();
            this.btnTranspose = new System.Windows.Forms.Button();
            this.btnBackToRack = new System.Windows.Forms.Button();
            this.btnValidate = new System.Windows.Forms.Button();
            this.btnDemo = new System.Windows.Forms.Button();
            this.lblPlayer1Score = new System.Windows.Forms.Label();
            this.lblPlayer2Score = new System.Windows.Forms.Label();
            this.btnDemoAll = new System.Windows.Forms.Button();
            this.txtRackP2 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblCurrentRack = new System.Windows.Forms.Label();
            this.ckKeepExistingBoard = new System.Windows.Forms.CheckBox();
            this.lsbInfos = new System.Windows.Forms.ListBox();
            this.btnLoadGame = new System.Windows.Forms.Button();
            this.btnSaveGame = new System.Windows.Forms.Button();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.btnNewGame = new System.Windows.Forms.Button();
            this.lblP1BestPlay = new System.Windows.Forms.Label();
            this.lblP2BestPlay = new System.Windows.Forms.Label();
            this.lsbWords = new System.Windows.Forms.ListBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rbSize15 = new System.Windows.Forms.RadioButton();
            this.rbSize11 = new System.Windows.Forms.RadioButton();
            this.ckShowGrid = new System.Windows.Forms.CheckBox();
            this.gbWordDirection = new System.Windows.Forms.GroupBox();
            this.rbWordDirDown = new System.Windows.Forms.RadioButton();
            this.rbWordDirRight = new System.Windows.Forms.RadioButton();
            this.gbGameStyle = new System.Windows.Forms.GroupBox();
            this.rbGameStyleScrabble = new System.Windows.Forms.RadioButton();
            this.rbWordsWithFriends = new System.Windows.Forms.RadioButton();
            this.gbSortBy = new System.Windows.Forms.GroupBox();
            this.rbMaxLength = new System.Windows.Forms.RadioButton();
            this.rbBestScore = new System.Windows.Forms.RadioButton();
            this.txtMotExiste = new System.Windows.Forms.TextBox();
            this.lblMotExiste = new System.Windows.Forms.Label();
            this.btnUndoLast = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.rbEnCollins2019 = new System.Windows.Forms.RadioButton();
            this.rbODS7 = new System.Windows.Forms.RadioButton();
            this.rbODS6 = new System.Windows.Forms.RadioButton();
            this.btnWordsFinder = new System.Windows.Forms.Button();
            this.gbBoard = new Dawg.Solver.Winform.CustomGroupBox();
            this.nudCustomBoardSize = new System.Windows.Forms.NumericUpDown();
            this.groupBox1.SuspendLayout();
            this.gbWordDirection.SuspendLayout();
            this.gbGameStyle.SuspendLayout();
            this.gbSortBy.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudCustomBoardSize)).BeginInit();
            this.SuspendLayout();
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(464, 3);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 35);
            this.btnSearch.TabIndex = 7;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // txtRackP1
            // 
            this.txtRackP1.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtRackP1.Location = new System.Drawing.Point(321, 3);
            this.txtRackP1.Name = "txtRackP1";
            this.txtRackP1.Size = new System.Drawing.Size(134, 26);
            this.txtRackP1.TabIndex = 5;
            // 
            // lsbHintWords
            // 
            this.lsbHintWords.FormattingEnabled = true;
            this.lsbHintWords.ItemHeight = 20;
            this.lsbHintWords.Location = new System.Drawing.Point(12, 63);
            this.lsbHintWords.Name = "lsbHintWords";
            this.lsbHintWords.Size = new System.Drawing.Size(259, 484);
            this.lsbHintWords.TabIndex = 8;
            this.lsbHintWords.Click += new System.EventHandler(this.lsb_Click);
            // 
            // txtBag
            // 
            this.txtBag.BackColor = System.Drawing.SystemColors.InfoText;
            this.txtBag.ForeColor = System.Drawing.SystemColors.InactiveBorder;
            this.txtBag.Location = new System.Drawing.Point(12, 562);
            this.txtBag.Multiline = true;
            this.txtBag.Name = "txtBag";
            this.txtBag.Size = new System.Drawing.Size(376, 147);
            this.txtBag.TabIndex = 9;
            // 
            // txtGrid2
            // 
            this.txtGrid2.BackColor = System.Drawing.SystemColors.InfoText;
            this.txtGrid2.ForeColor = System.Drawing.SystemColors.InactiveBorder;
            this.txtGrid2.Location = new System.Drawing.Point(278, 189);
            this.txtGrid2.Multiline = true;
            this.txtGrid2.Name = "txtGrid2";
            this.txtGrid2.Size = new System.Drawing.Size(264, 366);
            this.txtGrid2.TabIndex = 10;
            // 
            // btnTranspose
            // 
            this.btnTranspose.Location = new System.Drawing.Point(290, 122);
            this.btnTranspose.Name = "btnTranspose";
            this.btnTranspose.Size = new System.Drawing.Size(112, 38);
            this.btnTranspose.TabIndex = 12;
            this.btnTranspose.Text = "Transpose";
            this.btnTranspose.UseVisualStyleBackColor = true;
            this.btnTranspose.Click += new System.EventHandler(this.btnTranspose_Click);
            // 
            // btnBackToRack
            // 
            this.btnBackToRack.Location = new System.Drawing.Point(420, 122);
            this.btnBackToRack.Name = "btnBackToRack";
            this.btnBackToRack.Size = new System.Drawing.Size(118, 38);
            this.btnBackToRack.TabIndex = 13;
            this.btnBackToRack.Text = "BackToRack";
            this.btnBackToRack.UseVisualStyleBackColor = true;
            this.btnBackToRack.Click += new System.EventHandler(this.btnBackToRack_Click);
            // 
            // btnValidate
            // 
            this.btnValidate.Location = new System.Drawing.Point(398, 562);
            this.btnValidate.Name = "btnValidate";
            this.btnValidate.Size = new System.Drawing.Size(141, 51);
            this.btnValidate.TabIndex = 15;
            this.btnValidate.Text = "Validate Word";
            this.btnValidate.UseVisualStyleBackColor = true;
            this.btnValidate.Click += new System.EventHandler(this.btnValidate_Click);
            // 
            // btnDemo
            // 
            this.btnDemo.Location = new System.Drawing.Point(16, 717);
            this.btnDemo.Name = "btnDemo";
            this.btnDemo.Size = new System.Drawing.Size(116, 32);
            this.btnDemo.TabIndex = 16;
            this.btnDemo.Text = "AutoPlay 1";
            this.btnDemo.UseVisualStyleBackColor = true;
            this.btnDemo.Click += new System.EventHandler(this.btnDemo_Click);
            // 
            // lblPlayer1Score
            // 
            this.lblPlayer1Score.AutoSize = true;
            this.lblPlayer1Score.BackColor = System.Drawing.Color.LightYellow;
            this.lblPlayer1Score.Location = new System.Drawing.Point(1334, 12);
            this.lblPlayer1Score.Name = "lblPlayer1Score";
            this.lblPlayer1Score.Size = new System.Drawing.Size(74, 20);
            this.lblPlayer1Score.TabIndex = 17;
            this.lblPlayer1Score.Text = "P1 Score";
            // 
            // lblPlayer2Score
            // 
            this.lblPlayer2Score.AutoSize = true;
            this.lblPlayer2Score.BackColor = System.Drawing.Color.LightGreen;
            this.lblPlayer2Score.Location = new System.Drawing.Point(1334, 43);
            this.lblPlayer2Score.Name = "lblPlayer2Score";
            this.lblPlayer2Score.Size = new System.Drawing.Size(74, 20);
            this.lblPlayer2Score.TabIndex = 18;
            this.lblPlayer2Score.Text = "P2 Score";
            // 
            // btnDemoAll
            // 
            this.btnDemoAll.Location = new System.Drawing.Point(138, 717);
            this.btnDemoAll.Name = "btnDemoAll";
            this.btnDemoAll.Size = new System.Drawing.Size(116, 32);
            this.btnDemoAll.TabIndex = 19;
            this.btnDemoAll.Text = "AutoPlay All";
            this.btnDemoAll.UseVisualStyleBackColor = true;
            this.btnDemoAll.Click += new System.EventHandler(this.btnDemoAll_Click);
            // 
            // txtRackP2
            // 
            this.txtRackP2.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtRackP2.Location = new System.Drawing.Point(321, 37);
            this.txtRackP2.Name = "txtRackP2";
            this.txtRackP2.Size = new System.Drawing.Size(134, 26);
            this.txtRackP2.TabIndex = 20;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(290, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(28, 20);
            this.label1.TabIndex = 21;
            this.label1.Text = "P1";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(290, 40);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(28, 20);
            this.label2.TabIndex = 22;
            this.label2.Text = "P2";
            // 
            // lblCurrentRack
            // 
            this.lblCurrentRack.AutoSize = true;
            this.lblCurrentRack.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCurrentRack.Location = new System.Drawing.Point(741, 825);
            this.lblCurrentRack.Name = "lblCurrentRack";
            this.lblCurrentRack.Size = new System.Drawing.Size(0, 37);
            this.lblCurrentRack.TabIndex = 23;
            // 
            // ckKeepExistingBoard
            // 
            this.ckKeepExistingBoard.AutoSize = true;
            this.ckKeepExistingBoard.Location = new System.Drawing.Point(260, 722);
            this.ckKeepExistingBoard.Name = "ckKeepExistingBoard";
            this.ckKeepExistingBoard.Size = new System.Drawing.Size(108, 24);
            this.ckKeepExistingBoard.TabIndex = 24;
            this.ckKeepExistingBoard.Text = "Keep Tiles";
            this.ckKeepExistingBoard.UseVisualStyleBackColor = true;
            // 
            // lsbInfos
            // 
            this.lsbInfos.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lsbInfos.FormattingEnabled = true;
            this.lsbInfos.ItemHeight = 20;
            this.lsbInfos.Location = new System.Drawing.Point(1324, 78);
            this.lsbInfos.Name = "lsbInfos";
            this.lsbInfos.Size = new System.Drawing.Size(432, 284);
            this.lsbInfos.TabIndex = 25;
            // 
            // btnLoadGame
            // 
            this.btnLoadGame.Location = new System.Drawing.Point(290, 69);
            this.btnLoadGame.Name = "btnLoadGame";
            this.btnLoadGame.Size = new System.Drawing.Size(112, 46);
            this.btnLoadGame.TabIndex = 26;
            this.btnLoadGame.Text = "Load Game";
            this.btnLoadGame.UseVisualStyleBackColor = true;
            this.btnLoadGame.Click += new System.EventHandler(this.btnLoadGame_Click);
            // 
            // btnSaveGame
            // 
            this.btnSaveGame.Location = new System.Drawing.Point(420, 69);
            this.btnSaveGame.Name = "btnSaveGame";
            this.btnSaveGame.Size = new System.Drawing.Size(118, 46);
            this.btnSaveGame.TabIndex = 27;
            this.btnSaveGame.Text = "Save Game";
            this.btnSaveGame.UseVisualStyleBackColor = true;
            this.btnSaveGame.Click += new System.EventHandler(this.btnSaveGame_Click);
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.Filter = "Scrabble Game|*.gam";
            this.saveFileDialog1.OverwritePrompt = false;
            this.saveFileDialog1.RestoreDirectory = true;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.DefaultExt = "gam";
            this.openFileDialog1.FileName = "Game";
            this.openFileDialog1.Filter = "Scrabble Game|*.gam";
            this.openFileDialog1.RestoreDirectory = true;
            // 
            // btnNewGame
            // 
            this.btnNewGame.Location = new System.Drawing.Point(16, 755);
            this.btnNewGame.Name = "btnNewGame";
            this.btnNewGame.Size = new System.Drawing.Size(116, 32);
            this.btnNewGame.TabIndex = 28;
            this.btnNewGame.Text = "New game";
            this.btnNewGame.UseVisualStyleBackColor = true;
            this.btnNewGame.Click += new System.EventHandler(this.btnNewGame_Click);
            // 
            // lblP1BestPlay
            // 
            this.lblP1BestPlay.AutoSize = true;
            this.lblP1BestPlay.BackColor = System.Drawing.Color.LightYellow;
            this.lblP1BestPlay.Location = new System.Drawing.Point(1502, 12);
            this.lblP1BestPlay.Name = "lblP1BestPlay";
            this.lblP1BestPlay.Size = new System.Drawing.Size(72, 20);
            this.lblP1BestPlay.TabIndex = 29;
            this.lblP1BestPlay.Text = "best play";
            // 
            // lblP2BestPlay
            // 
            this.lblP2BestPlay.AutoSize = true;
            this.lblP2BestPlay.BackColor = System.Drawing.Color.LightGreen;
            this.lblP2BestPlay.Location = new System.Drawing.Point(1502, 43);
            this.lblP2BestPlay.Name = "lblP2BestPlay";
            this.lblP2BestPlay.Size = new System.Drawing.Size(72, 20);
            this.lblP2BestPlay.TabIndex = 30;
            this.lblP2BestPlay.Text = "best play";
            // 
            // lsbWords
            // 
            this.lsbWords.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lsbWords.FormattingEnabled = true;
            this.lsbWords.ItemHeight = 20;
            this.lsbWords.Location = new System.Drawing.Point(1324, 368);
            this.lsbWords.Name = "lsbWords";
            this.lsbWords.Size = new System.Drawing.Size(432, 504);
            this.lsbWords.TabIndex = 31;
            this.lsbWords.SelectedIndexChanged += new System.EventHandler(this.lsbWords_SelectedIndexChanged);
            this.lsbWords.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lsbWords_MouseDoubleClick);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.nudCustomBoardSize);
            this.groupBox1.Controls.Add(this.rbSize15);
            this.groupBox1.Controls.Add(this.rbSize11);
            this.groupBox1.Location = new System.Drawing.Point(18, 795);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox1.Size = new System.Drawing.Size(114, 105);
            this.groupBox1.TabIndex = 32;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Board Size";
            // 
            // rbSize15
            // 
            this.rbSize15.AutoSize = true;
            this.rbSize15.Checked = true;
            this.rbSize15.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbSize15.Location = new System.Drawing.Point(8, 48);
            this.rbSize15.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.rbSize15.Name = "rbSize15";
            this.rbSize15.Size = new System.Drawing.Size(49, 21);
            this.rbSize15.TabIndex = 1;
            this.rbSize15.TabStop = true;
            this.rbSize15.Text = "15";
            this.rbSize15.UseVisualStyleBackColor = true;
            this.rbSize15.CheckedChanged += new System.EventHandler(this.rbSize15_CheckedChanged);
            // 
            // rbSize11
            // 
            this.rbSize11.AutoSize = true;
            this.rbSize11.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbSize11.Location = new System.Drawing.Point(8, 22);
            this.rbSize11.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.rbSize11.Name = "rbSize11";
            this.rbSize11.Size = new System.Drawing.Size(49, 21);
            this.rbSize11.TabIndex = 0;
            this.rbSize11.Text = "11";
            this.rbSize11.UseVisualStyleBackColor = true;
            // 
            // ckShowGrid
            // 
            this.ckShowGrid.AutoSize = true;
            this.ckShowGrid.Location = new System.Drawing.Point(290, 166);
            this.ckShowGrid.Name = "ckShowGrid";
            this.ckShowGrid.Size = new System.Drawing.Size(134, 24);
            this.ckShowGrid.TabIndex = 33;
            this.ckShowGrid.Text = "Show dev grid";
            this.ckShowGrid.UseVisualStyleBackColor = true;
            this.ckShowGrid.CheckedChanged += new System.EventHandler(this.ckShowGrid_CheckedChanged);
            // 
            // gbWordDirection
            // 
            this.gbWordDirection.Controls.Add(this.rbWordDirDown);
            this.gbWordDirection.Controls.Add(this.rbWordDirRight);
            this.gbWordDirection.Location = new System.Drawing.Point(398, 620);
            this.gbWordDirection.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.gbWordDirection.Name = "gbWordDirection";
            this.gbWordDirection.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.gbWordDirection.Size = new System.Drawing.Size(141, 88);
            this.gbWordDirection.TabIndex = 33;
            this.gbWordDirection.TabStop = false;
            this.gbWordDirection.Text = "word direction";
            // 
            // rbWordDirDown
            // 
            this.rbWordDirDown.AutoSize = true;
            this.rbWordDirDown.Location = new System.Drawing.Point(12, 52);
            this.rbWordDirDown.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.rbWordDirDown.Name = "rbWordDirDown";
            this.rbWordDirDown.Size = new System.Drawing.Size(75, 24);
            this.rbWordDirDown.TabIndex = 1;
            this.rbWordDirDown.Text = "Down";
            this.rbWordDirDown.UseVisualStyleBackColor = true;
            this.rbWordDirDown.CheckedChanged += new System.EventHandler(this.rbWordDirDown_CheckedChanged);
            // 
            // rbWordDirRight
            // 
            this.rbWordDirRight.AutoSize = true;
            this.rbWordDirRight.Checked = true;
            this.rbWordDirRight.Location = new System.Drawing.Point(12, 29);
            this.rbWordDirRight.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.rbWordDirRight.Name = "rbWordDirRight";
            this.rbWordDirRight.Size = new System.Drawing.Size(83, 24);
            this.rbWordDirRight.TabIndex = 0;
            this.rbWordDirRight.TabStop = true;
            this.rbWordDirRight.Text = "Across";
            this.rbWordDirRight.UseVisualStyleBackColor = true;
            // 
            // gbGameStyle
            // 
            this.gbGameStyle.Controls.Add(this.rbGameStyleScrabble);
            this.gbGameStyle.Controls.Add(this.rbWordsWithFriends);
            this.gbGameStyle.Location = new System.Drawing.Point(138, 812);
            this.gbGameStyle.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.gbGameStyle.Name = "gbGameStyle";
            this.gbGameStyle.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.gbGameStyle.Size = new System.Drawing.Size(213, 88);
            this.gbGameStyle.TabIndex = 33;
            this.gbGameStyle.TabStop = false;
            this.gbGameStyle.Text = "Game Style";
            // 
            // rbGameStyleScrabble
            // 
            this.rbGameStyleScrabble.AutoSize = true;
            this.rbGameStyleScrabble.Checked = true;
            this.rbGameStyleScrabble.Location = new System.Drawing.Point(20, 52);
            this.rbGameStyleScrabble.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.rbGameStyleScrabble.Name = "rbGameStyleScrabble";
            this.rbGameStyleScrabble.Size = new System.Drawing.Size(97, 24);
            this.rbGameStyleScrabble.TabIndex = 1;
            this.rbGameStyleScrabble.TabStop = true;
            this.rbGameStyleScrabble.Text = "Scrabble";
            this.rbGameStyleScrabble.UseVisualStyleBackColor = true;
            this.rbGameStyleScrabble.CheckedChanged += new System.EventHandler(this.rbGameStyleScrabble_CheckedChanged);
            // 
            // rbWordsWithFriends
            // 
            this.rbWordsWithFriends.AutoSize = true;
            this.rbWordsWithFriends.Location = new System.Drawing.Point(20, 26);
            this.rbWordsWithFriends.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.rbWordsWithFriends.Name = "rbWordsWithFriends";
            this.rbWordsWithFriends.Size = new System.Drawing.Size(164, 24);
            this.rbWordsWithFriends.TabIndex = 0;
            this.rbWordsWithFriends.Text = "Words with friends";
            this.rbWordsWithFriends.UseVisualStyleBackColor = true;
            this.rbWordsWithFriends.CheckedChanged += new System.EventHandler(this.rbWordsWithFriends_CheckedChanged);
            // 
            // gbSortBy
            // 
            this.gbSortBy.Controls.Add(this.rbMaxLength);
            this.gbSortBy.Controls.Add(this.rbBestScore);
            this.gbSortBy.Location = new System.Drawing.Point(12, 5);
            this.gbSortBy.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.gbSortBy.Name = "gbSortBy";
            this.gbSortBy.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.gbSortBy.Size = new System.Drawing.Size(266, 58);
            this.gbSortBy.TabIndex = 33;
            this.gbSortBy.TabStop = false;
            this.gbSortBy.Text = "Sort by";
            // 
            // rbMaxLength
            // 
            this.rbMaxLength.AutoSize = true;
            this.rbMaxLength.Location = new System.Drawing.Point(136, 22);
            this.rbMaxLength.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.rbMaxLength.Name = "rbMaxLength";
            this.rbMaxLength.Size = new System.Drawing.Size(111, 24);
            this.rbMaxLength.TabIndex = 1;
            this.rbMaxLength.Text = "Max length";
            this.rbMaxLength.UseVisualStyleBackColor = true;
            this.rbMaxLength.CheckedChanged += new System.EventHandler(this.rbMaxLength_CheckedChanged);
            // 
            // rbBestScore
            // 
            this.rbBestScore.AutoSize = true;
            this.rbBestScore.Checked = true;
            this.rbBestScore.Location = new System.Drawing.Point(15, 22);
            this.rbBestScore.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.rbBestScore.Name = "rbBestScore";
            this.rbBestScore.Size = new System.Drawing.Size(110, 24);
            this.rbBestScore.TabIndex = 0;
            this.rbBestScore.TabStop = true;
            this.rbBestScore.Text = "Best score";
            this.rbBestScore.UseVisualStyleBackColor = true;
            // 
            // txtMotExiste
            // 
            this.txtMotExiste.Location = new System.Drawing.Point(358, 837);
            this.txtMotExiste.Name = "txtMotExiste";
            this.txtMotExiste.Size = new System.Drawing.Size(158, 26);
            this.txtMotExiste.TabIndex = 34;
            this.txtMotExiste.TextChanged += new System.EventHandler(this.txtMotExiste_TextChanged);
            this.txtMotExiste.DoubleClick += new System.EventHandler(this.txtMotExiste_DoubleClick);
            // 
            // lblMotExiste
            // 
            this.lblMotExiste.AutoSize = true;
            this.lblMotExiste.Location = new System.Drawing.Point(392, 815);
            this.lblMotExiste.Name = "lblMotExiste";
            this.lblMotExiste.Size = new System.Drawing.Size(108, 20);
            this.lblMotExiste.TabIndex = 35;
            this.lblMotExiste.Text = "Words exists?";
            // 
            // btnUndoLast
            // 
            this.btnUndoLast.Location = new System.Drawing.Point(398, 715);
            this.btnUndoLast.Name = "btnUndoLast";
            this.btnUndoLast.Size = new System.Drawing.Size(141, 46);
            this.btnUndoLast.TabIndex = 36;
            this.btnUndoLast.Text = "Undo Last";
            this.btnUndoLast.UseVisualStyleBackColor = true;
            this.btnUndoLast.Click += new System.EventHandler(this.btnUndoLast_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.rbEnCollins2019);
            this.groupBox2.Controls.Add(this.rbODS7);
            this.groupBox2.Controls.Add(this.rbODS6);
            this.groupBox2.Location = new System.Drawing.Point(138, 754);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox2.Size = new System.Drawing.Size(404, 53);
            this.groupBox2.TabIndex = 34;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Dictionary";
            // 
            // rbEnCollins2019
            // 
            this.rbEnCollins2019.AutoSize = true;
            this.rbEnCollins2019.Checked = true;
            this.rbEnCollins2019.Location = new System.Drawing.Point(234, 22);
            this.rbEnCollins2019.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.rbEnCollins2019.Name = "rbEnCollins2019";
            this.rbEnCollins2019.Size = new System.Drawing.Size(146, 24);
            this.rbEnCollins2019.TabIndex = 2;
            this.rbEnCollins2019.TabStop = true;
            this.rbEnCollins2019.Text = "EN Collins 2019";
            this.rbEnCollins2019.UseVisualStyleBackColor = true;
            this.rbEnCollins2019.CheckedChanged += new System.EventHandler(this.rbDico_CheckedChanged);
            // 
            // rbODS7
            // 
            this.rbODS7.AutoSize = true;
            this.rbODS7.Location = new System.Drawing.Point(117, 22);
            this.rbODS7.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.rbODS7.Name = "rbODS7";
            this.rbODS7.Size = new System.Drawing.Size(108, 24);
            this.rbODS7.TabIndex = 1;
            this.rbODS7.Text = "FR ODS 7";
            this.rbODS7.UseVisualStyleBackColor = true;
            this.rbODS7.CheckedChanged += new System.EventHandler(this.rbDico_CheckedChanged);
            // 
            // rbODS6
            // 
            this.rbODS6.AutoSize = true;
            this.rbODS6.Location = new System.Drawing.Point(9, 22);
            this.rbODS6.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.rbODS6.Name = "rbODS6";
            this.rbODS6.Size = new System.Drawing.Size(108, 24);
            this.rbODS6.TabIndex = 0;
            this.rbODS6.Text = "FR ODS 6";
            this.rbODS6.UseVisualStyleBackColor = true;
            this.rbODS6.CheckedChanged += new System.EventHandler(this.rbDico_CheckedChanged);
            // 
            // btnWordsFinder
            // 
            this.btnWordsFinder.Location = new System.Drawing.Point(376, 869);
            this.btnWordsFinder.Name = "btnWordsFinder";
            this.btnWordsFinder.Size = new System.Drawing.Size(116, 32);
            this.btnWordsFinder.TabIndex = 37;
            this.btnWordsFinder.Text = "Words finder";
            this.btnWordsFinder.UseVisualStyleBackColor = true;
            this.btnWordsFinder.Click += new System.EventHandler(this.btnWordsFinder_Click);
            // 
            // gbBoard
            // 
            this.gbBoard.Location = new System.Drawing.Point(548, 5);
            this.gbBoard.Name = "gbBoard";
            this.gbBoard.Size = new System.Drawing.Size(772, 780);
            this.gbBoard.TabIndex = 11;
            this.gbBoard.TabStop = false;
            this.gbBoard.Text = "Board";
            // 
            // nudCustomBoardSize
            // 
            this.nudCustomBoardSize.Location = new System.Drawing.Point(9, 74);
            this.nudCustomBoardSize.Maximum = new decimal(new int[] {
            18,
            0,
            0,
            0});
            this.nudCustomBoardSize.Minimum = new decimal(new int[] {
            7,
            0,
            0,
            0});
            this.nudCustomBoardSize.Name = "nudCustomBoardSize";
            this.nudCustomBoardSize.Size = new System.Drawing.Size(50, 26);
            this.nudCustomBoardSize.TabIndex = 0;
            this.nudCustomBoardSize.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nudCustomBoardSize.ValueChanged += new System.EventHandler(this.nudCustomBoardSize_ValueChanged);
            // 
            // MainForm
            // 
            this.AcceptButton = this.btnSearch;
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1770, 902);
            this.Controls.Add(this.btnWordsFinder);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.btnUndoLast);
            this.Controls.Add(this.lblMotExiste);
            this.Controls.Add(this.txtMotExiste);
            this.Controls.Add(this.gbSortBy);
            this.Controls.Add(this.gbGameStyle);
            this.Controls.Add(this.gbWordDirection);
            this.Controls.Add(this.ckShowGrid);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.lsbWords);
            this.Controls.Add(this.lblP2BestPlay);
            this.Controls.Add(this.lblP1BestPlay);
            this.Controls.Add(this.btnNewGame);
            this.Controls.Add(this.btnSaveGame);
            this.Controls.Add(this.btnLoadGame);
            this.Controls.Add(this.lsbInfos);
            this.Controls.Add(this.ckKeepExistingBoard);
            this.Controls.Add(this.lblCurrentRack);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtRackP2);
            this.Controls.Add(this.btnDemoAll);
            this.Controls.Add(this.lblPlayer2Score);
            this.Controls.Add(this.lblPlayer1Score);
            this.Controls.Add(this.btnDemo);
            this.Controls.Add(this.btnValidate);
            this.Controls.Add(this.btnBackToRack);
            this.Controls.Add(this.btnTranspose);
            this.Controls.Add(this.gbBoard);
            this.Controls.Add(this.txtGrid2);
            this.Controls.Add(this.txtBag);
            this.Controls.Add(this.lsbHintWords);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.txtRackP1);
            this.Name = "MainForm";
            this.Text = "Scrabble";
            this.Click += new System.EventHandler(this.MainForm_Click);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.MainForm_KeyPress);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.MainForm_KeyUp);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.gbWordDirection.ResumeLayout(false);
            this.gbWordDirection.PerformLayout();
            this.gbGameStyle.ResumeLayout(false);
            this.gbGameStyle.PerformLayout();
            this.gbSortBy.ResumeLayout(false);
            this.gbSortBy.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudCustomBoardSize)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.TextBox txtRackP1;
        private System.Windows.Forms.ListBox lsbHintWords;
        private System.Windows.Forms.TextBox txtGrid2;
        private CustomGroupBox gbBoard;
        private System.Windows.Forms.Button btnTranspose;
        private System.Windows.Forms.Button btnBackToRack;
        private System.Windows.Forms.Button btnValidate;
        private System.Windows.Forms.Button btnDemo;
        private System.Windows.Forms.Label lblPlayer1Score;
        private System.Windows.Forms.Label lblPlayer2Score;
        private System.Windows.Forms.Button btnDemoAll;
        private System.Windows.Forms.TextBox txtRackP2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblCurrentRack;
        private System.Windows.Forms.CheckBox ckKeepExistingBoard;
        public System.Windows.Forms.ListBox lsbInfos;
        private System.Windows.Forms.Button btnLoadGame;
        private System.Windows.Forms.Button btnSaveGame;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button btnNewGame;
        private System.Windows.Forms.Label lblP1BestPlay;
        private System.Windows.Forms.Label lblP2BestPlay;
        public System.Windows.Forms.ListBox lsbWords;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rbSize15;
        private System.Windows.Forms.RadioButton rbSize11;
        private System.Windows.Forms.CheckBox ckShowGrid;
        private System.Windows.Forms.GroupBox gbWordDirection;
        private System.Windows.Forms.RadioButton rbWordDirDown;
        private System.Windows.Forms.RadioButton rbWordDirRight;
        private System.Windows.Forms.GroupBox gbGameStyle;
        private System.Windows.Forms.RadioButton rbGameStyleScrabble;
        private System.Windows.Forms.RadioButton rbWordsWithFriends;
        public System.Windows.Forms.TextBox txtBag;
        private System.Windows.Forms.GroupBox gbSortBy;
        private System.Windows.Forms.RadioButton rbMaxLength;
        private System.Windows.Forms.RadioButton rbBestScore;
        private System.Windows.Forms.TextBox txtMotExiste;
        private System.Windows.Forms.Label lblMotExiste;
        private System.Windows.Forms.Button btnUndoLast;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton rbODS7;
        private System.Windows.Forms.RadioButton rbODS6;
        private System.Windows.Forms.RadioButton rbEnCollins2019;
        private System.Windows.Forms.Button btnWordsFinder;
        private System.Windows.Forms.NumericUpDown nudCustomBoardSize;
    }
}