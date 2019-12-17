namespace Dawg.Resolver.Winform.Test
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
            this.gbBoard = new Dawg.Resolver.Winform.Test.CustomGroupBox();
            this.groupBox1.SuspendLayout();
            this.gbWordDirection.SuspendLayout();
            this.gbGameStyle.SuspendLayout();
            this.gbSortBy.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(309, 2);
            this.btnSearch.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(50, 23);
            this.btnSearch.TabIndex = 7;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // txtRackP1
            // 
            this.txtRackP1.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtRackP1.Location = new System.Drawing.Point(214, 2);
            this.txtRackP1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtRackP1.Name = "txtRackP1";
            this.txtRackP1.Size = new System.Drawing.Size(91, 20);
            this.txtRackP1.TabIndex = 5;
            // 
            // lsbHintWords
            // 
            this.lsbHintWords.FormattingEnabled = true;
            this.lsbHintWords.Location = new System.Drawing.Point(8, 41);
            this.lsbHintWords.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.lsbHintWords.Name = "lsbHintWords";
            this.lsbHintWords.Size = new System.Drawing.Size(174, 316);
            this.lsbHintWords.TabIndex = 8;
            this.lsbHintWords.Click += new System.EventHandler(this.lsb_Click);
            // 
            // txtBag
            // 
            this.txtBag.BackColor = System.Drawing.SystemColors.InfoText;
            this.txtBag.ForeColor = System.Drawing.SystemColors.InactiveBorder;
            this.txtBag.Location = new System.Drawing.Point(8, 365);
            this.txtBag.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtBag.Multiline = true;
            this.txtBag.Name = "txtBag";
            this.txtBag.Size = new System.Drawing.Size(252, 97);
            this.txtBag.TabIndex = 9;
            // 
            // txtGrid2
            // 
            this.txtGrid2.BackColor = System.Drawing.SystemColors.InfoText;
            this.txtGrid2.ForeColor = System.Drawing.SystemColors.InactiveBorder;
            this.txtGrid2.Location = new System.Drawing.Point(185, 123);
            this.txtGrid2.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtGrid2.Multiline = true;
            this.txtGrid2.Name = "txtGrid2";
            this.txtGrid2.Size = new System.Drawing.Size(177, 239);
            this.txtGrid2.TabIndex = 10;
            // 
            // btnTranspose
            // 
            this.btnTranspose.Location = new System.Drawing.Point(193, 79);
            this.btnTranspose.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnTranspose.Name = "btnTranspose";
            this.btnTranspose.Size = new System.Drawing.Size(75, 25);
            this.btnTranspose.TabIndex = 12;
            this.btnTranspose.Text = "Transpose";
            this.btnTranspose.UseVisualStyleBackColor = true;
            this.btnTranspose.Click += new System.EventHandler(this.btnTranspose_Click);
            // 
            // btnBackToRack
            // 
            this.btnBackToRack.Location = new System.Drawing.Point(280, 79);
            this.btnBackToRack.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnBackToRack.Name = "btnBackToRack";
            this.btnBackToRack.Size = new System.Drawing.Size(79, 25);
            this.btnBackToRack.TabIndex = 13;
            this.btnBackToRack.Text = "BackToRack";
            this.btnBackToRack.UseVisualStyleBackColor = true;
            this.btnBackToRack.Click += new System.EventHandler(this.btnBackToRack_Click);
            // 
            // btnValidate
            // 
            this.btnValidate.Location = new System.Drawing.Point(265, 365);
            this.btnValidate.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnValidate.Name = "btnValidate";
            this.btnValidate.Size = new System.Drawing.Size(94, 33);
            this.btnValidate.TabIndex = 15;
            this.btnValidate.Text = "Validate Word";
            this.btnValidate.UseVisualStyleBackColor = true;
            this.btnValidate.Click += new System.EventHandler(this.btnValidate_Click);
            // 
            // btnDemo
            // 
            this.btnDemo.Location = new System.Drawing.Point(11, 466);
            this.btnDemo.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnDemo.Name = "btnDemo";
            this.btnDemo.Size = new System.Drawing.Size(77, 21);
            this.btnDemo.TabIndex = 16;
            this.btnDemo.Text = "AutoPlay 1";
            this.btnDemo.UseVisualStyleBackColor = true;
            this.btnDemo.Click += new System.EventHandler(this.btnDemo_Click);
            // 
            // lblPlayer1Score
            // 
            this.lblPlayer1Score.AutoSize = true;
            this.lblPlayer1Score.BackColor = System.Drawing.Color.LightYellow;
            this.lblPlayer1Score.Location = new System.Drawing.Point(889, 8);
            this.lblPlayer1Score.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblPlayer1Score.Name = "lblPlayer1Score";
            this.lblPlayer1Score.Size = new System.Drawing.Size(51, 13);
            this.lblPlayer1Score.TabIndex = 17;
            this.lblPlayer1Score.Text = "P1 Score";
            // 
            // lblPlayer2Score
            // 
            this.lblPlayer2Score.AutoSize = true;
            this.lblPlayer2Score.BackColor = System.Drawing.Color.LightGreen;
            this.lblPlayer2Score.Location = new System.Drawing.Point(889, 28);
            this.lblPlayer2Score.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblPlayer2Score.Name = "lblPlayer2Score";
            this.lblPlayer2Score.Size = new System.Drawing.Size(51, 13);
            this.lblPlayer2Score.TabIndex = 18;
            this.lblPlayer2Score.Text = "P2 Score";
            // 
            // btnDemoAll
            // 
            this.btnDemoAll.Location = new System.Drawing.Point(92, 466);
            this.btnDemoAll.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnDemoAll.Name = "btnDemoAll";
            this.btnDemoAll.Size = new System.Drawing.Size(77, 21);
            this.btnDemoAll.TabIndex = 19;
            this.btnDemoAll.Text = "AutoPlay All";
            this.btnDemoAll.UseVisualStyleBackColor = true;
            this.btnDemoAll.Click += new System.EventHandler(this.btnDemoAll_Click);
            // 
            // txtRackP2
            // 
            this.txtRackP2.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtRackP2.Location = new System.Drawing.Point(214, 24);
            this.txtRackP2.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtRackP2.Name = "txtRackP2";
            this.txtRackP2.Size = new System.Drawing.Size(91, 20);
            this.txtRackP2.TabIndex = 20;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(193, 4);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(20, 13);
            this.label1.TabIndex = 21;
            this.label1.Text = "P1";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(193, 26);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(20, 13);
            this.label2.TabIndex = 22;
            this.label2.Text = "P2";
            // 
            // lblCurrentRack
            // 
            this.lblCurrentRack.AutoSize = true;
            this.lblCurrentRack.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCurrentRack.Location = new System.Drawing.Point(494, 536);
            this.lblCurrentRack.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblCurrentRack.Name = "lblCurrentRack";
            this.lblCurrentRack.Size = new System.Drawing.Size(0, 26);
            this.lblCurrentRack.TabIndex = 23;
            // 
            // ckKeepExistingBoard
            // 
            this.ckKeepExistingBoard.AutoSize = true;
            this.ckKeepExistingBoard.Checked = true;
            this.ckKeepExistingBoard.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ckKeepExistingBoard.Location = new System.Drawing.Point(173, 469);
            this.ckKeepExistingBoard.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.ckKeepExistingBoard.Name = "ckKeepExistingBoard";
            this.ckKeepExistingBoard.Size = new System.Drawing.Size(76, 17);
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
            this.lsbInfos.Location = new System.Drawing.Point(883, 51);
            this.lsbInfos.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.lsbInfos.Name = "lsbInfos";
            this.lsbInfos.Size = new System.Drawing.Size(221, 186);
            this.lsbInfos.TabIndex = 25;
            // 
            // btnLoadGame
            // 
            this.btnLoadGame.Location = new System.Drawing.Point(193, 45);
            this.btnLoadGame.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnLoadGame.Name = "btnLoadGame";
            this.btnLoadGame.Size = new System.Drawing.Size(75, 30);
            this.btnLoadGame.TabIndex = 26;
            this.btnLoadGame.Text = "Load Game";
            this.btnLoadGame.UseVisualStyleBackColor = true;
            this.btnLoadGame.Click += new System.EventHandler(this.btnLoadGame_Click);
            // 
            // btnSaveGame
            // 
            this.btnSaveGame.Location = new System.Drawing.Point(280, 45);
            this.btnSaveGame.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnSaveGame.Name = "btnSaveGame";
            this.btnSaveGame.Size = new System.Drawing.Size(79, 30);
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
            this.btnNewGame.Location = new System.Drawing.Point(11, 491);
            this.btnNewGame.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnNewGame.Name = "btnNewGame";
            this.btnNewGame.Size = new System.Drawing.Size(77, 21);
            this.btnNewGame.TabIndex = 28;
            this.btnNewGame.Text = "New game";
            this.btnNewGame.UseVisualStyleBackColor = true;
            this.btnNewGame.Click += new System.EventHandler(this.btnNewGame_Click);
            // 
            // lblP1BestPlay
            // 
            this.lblP1BestPlay.AutoSize = true;
            this.lblP1BestPlay.BackColor = System.Drawing.Color.LightYellow;
            this.lblP1BestPlay.Location = new System.Drawing.Point(1003, 8);
            this.lblP1BestPlay.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblP1BestPlay.Name = "lblP1BestPlay";
            this.lblP1BestPlay.Size = new System.Drawing.Size(49, 13);
            this.lblP1BestPlay.TabIndex = 29;
            this.lblP1BestPlay.Text = "best play";
            // 
            // lblP2BestPlay
            // 
            this.lblP2BestPlay.AutoSize = true;
            this.lblP2BestPlay.BackColor = System.Drawing.Color.LightGreen;
            this.lblP2BestPlay.Location = new System.Drawing.Point(1001, 28);
            this.lblP2BestPlay.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblP2BestPlay.Name = "lblP2BestPlay";
            this.lblP2BestPlay.Size = new System.Drawing.Size(49, 13);
            this.lblP2BestPlay.TabIndex = 30;
            this.lblP2BestPlay.Text = "best play";
            // 
            // lsbWords
            // 
            this.lsbWords.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lsbWords.FormattingEnabled = true;
            this.lsbWords.Location = new System.Drawing.Point(883, 239);
            this.lsbWords.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.lsbWords.Name = "lsbWords";
            this.lsbWords.Size = new System.Drawing.Size(221, 329);
            this.lsbWords.TabIndex = 31;
            this.lsbWords.SelectedIndexChanged += new System.EventHandler(this.lsbWords_SelectedIndexChanged);
            this.lsbWords.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lsbWords_MouseDoubleClick);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rbSize15);
            this.groupBox1.Controls.Add(this.rbSize11);
            this.groupBox1.Location = new System.Drawing.Point(12, 517);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(85, 68);
            this.groupBox1.TabIndex = 32;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Board Size";
            // 
            // rbSize15
            // 
            this.rbSize15.AutoSize = true;
            this.rbSize15.Checked = true;
            this.rbSize15.Location = new System.Drawing.Point(15, 40);
            this.rbSize15.Name = "rbSize15";
            this.rbSize15.Size = new System.Drawing.Size(37, 17);
            this.rbSize15.TabIndex = 1;
            this.rbSize15.TabStop = true;
            this.rbSize15.Text = "15";
            this.rbSize15.UseVisualStyleBackColor = true;
            this.rbSize15.CheckedChanged += new System.EventHandler(this.rbSize15_CheckedChanged);
            // 
            // rbSize11
            // 
            this.rbSize11.AutoSize = true;
            this.rbSize11.Location = new System.Drawing.Point(15, 19);
            this.rbSize11.Name = "rbSize11";
            this.rbSize11.Size = new System.Drawing.Size(37, 17);
            this.rbSize11.TabIndex = 0;
            this.rbSize11.Text = "11";
            this.rbSize11.UseVisualStyleBackColor = true;
            // 
            // ckShowGrid
            // 
            this.ckShowGrid.AutoSize = true;
            this.ckShowGrid.Location = new System.Drawing.Point(193, 108);
            this.ckShowGrid.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.ckShowGrid.Name = "ckShowGrid";
            this.ckShowGrid.Size = new System.Drawing.Size(94, 17);
            this.ckShowGrid.TabIndex = 33;
            this.ckShowGrid.Text = "Show dev grid";
            this.ckShowGrid.UseVisualStyleBackColor = true;
            this.ckShowGrid.CheckedChanged += new System.EventHandler(this.ckShowGrid_CheckedChanged);
            // 
            // gbWordDirection
            // 
            this.gbWordDirection.Controls.Add(this.rbWordDirDown);
            this.gbWordDirection.Controls.Add(this.rbWordDirRight);
            this.gbWordDirection.Location = new System.Drawing.Point(265, 403);
            this.gbWordDirection.Name = "gbWordDirection";
            this.gbWordDirection.Size = new System.Drawing.Size(94, 57);
            this.gbWordDirection.TabIndex = 33;
            this.gbWordDirection.TabStop = false;
            this.gbWordDirection.Text = "word direction";
            // 
            // rbWordDirDown
            // 
            this.rbWordDirDown.AutoSize = true;
            this.rbWordDirDown.Location = new System.Drawing.Point(8, 34);
            this.rbWordDirDown.Name = "rbWordDirDown";
            this.rbWordDirDown.Size = new System.Drawing.Size(53, 17);
            this.rbWordDirDown.TabIndex = 1;
            this.rbWordDirDown.Text = "Down";
            this.rbWordDirDown.UseVisualStyleBackColor = true;
            this.rbWordDirDown.CheckedChanged += new System.EventHandler(this.rbWordDirDown_CheckedChanged);
            // 
            // rbWordDirRight
            // 
            this.rbWordDirRight.AutoSize = true;
            this.rbWordDirRight.Checked = true;
            this.rbWordDirRight.Location = new System.Drawing.Point(8, 19);
            this.rbWordDirRight.Name = "rbWordDirRight";
            this.rbWordDirRight.Size = new System.Drawing.Size(57, 17);
            this.rbWordDirRight.TabIndex = 0;
            this.rbWordDirRight.TabStop = true;
            this.rbWordDirRight.Text = "Across";
            this.rbWordDirRight.UseVisualStyleBackColor = true;
            // 
            // gbGameStyle
            // 
            this.gbGameStyle.Controls.Add(this.rbGameStyleScrabble);
            this.gbGameStyle.Controls.Add(this.rbWordsWithFriends);
            this.gbGameStyle.Location = new System.Drawing.Point(103, 517);
            this.gbGameStyle.Name = "gbGameStyle";
            this.gbGameStyle.Size = new System.Drawing.Size(131, 68);
            this.gbGameStyle.TabIndex = 33;
            this.gbGameStyle.TabStop = false;
            this.gbGameStyle.Text = "Game Style";
            // 
            // rbGameStyleScrabble
            // 
            this.rbGameStyleScrabble.AutoSize = true;
            this.rbGameStyleScrabble.Checked = true;
            this.rbGameStyleScrabble.Location = new System.Drawing.Point(15, 40);
            this.rbGameStyleScrabble.Name = "rbGameStyleScrabble";
            this.rbGameStyleScrabble.Size = new System.Drawing.Size(67, 17);
            this.rbGameStyleScrabble.TabIndex = 1;
            this.rbGameStyleScrabble.TabStop = true;
            this.rbGameStyleScrabble.Text = "Scrabble";
            this.rbGameStyleScrabble.UseVisualStyleBackColor = true;
            this.rbGameStyleScrabble.CheckedChanged += new System.EventHandler(this.rbGameStyleScrabble_CheckedChanged);
            // 
            // rbWordsWithFriends
            // 
            this.rbWordsWithFriends.AutoSize = true;
            this.rbWordsWithFriends.Location = new System.Drawing.Point(15, 19);
            this.rbWordsWithFriends.Name = "rbWordsWithFriends";
            this.rbWordsWithFriends.Size = new System.Drawing.Size(112, 17);
            this.rbWordsWithFriends.TabIndex = 0;
            this.rbWordsWithFriends.Text = "Words with friends";
            this.rbWordsWithFriends.UseVisualStyleBackColor = true;
            this.rbWordsWithFriends.CheckedChanged += new System.EventHandler(this.rbWordsWithFriends_CheckedChanged);
            // 
            // gbSortBy
            // 
            this.gbSortBy.Controls.Add(this.rbMaxLength);
            this.gbSortBy.Controls.Add(this.rbBestScore);
            this.gbSortBy.Location = new System.Drawing.Point(8, 3);
            this.gbSortBy.Name = "gbSortBy";
            this.gbSortBy.Size = new System.Drawing.Size(177, 38);
            this.gbSortBy.TabIndex = 33;
            this.gbSortBy.TabStop = false;
            this.gbSortBy.Text = "Sort by";
            // 
            // rbMaxLength
            // 
            this.rbMaxLength.AutoSize = true;
            this.rbMaxLength.Location = new System.Drawing.Point(91, 14);
            this.rbMaxLength.Name = "rbMaxLength";
            this.rbMaxLength.Size = new System.Drawing.Size(77, 17);
            this.rbMaxLength.TabIndex = 1;
            this.rbMaxLength.Text = "Max length";
            this.rbMaxLength.UseVisualStyleBackColor = true;
            this.rbMaxLength.CheckedChanged += new System.EventHandler(this.rbMaxLength_CheckedChanged);
            // 
            // rbBestScore
            // 
            this.rbBestScore.AutoSize = true;
            this.rbBestScore.Checked = true;
            this.rbBestScore.Location = new System.Drawing.Point(10, 14);
            this.rbBestScore.Name = "rbBestScore";
            this.rbBestScore.Size = new System.Drawing.Size(75, 17);
            this.rbBestScore.TabIndex = 0;
            this.rbBestScore.TabStop = true;
            this.rbBestScore.Text = "Best score";
            this.rbBestScore.UseVisualStyleBackColor = true;
            // 
            // txtMotExiste
            // 
            this.txtMotExiste.Location = new System.Drawing.Point(239, 534);
            this.txtMotExiste.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtMotExiste.Name = "txtMotExiste";
            this.txtMotExiste.Size = new System.Drawing.Size(107, 20);
            this.txtMotExiste.TabIndex = 34;
            this.txtMotExiste.TextChanged += new System.EventHandler(this.txtMotExiste_TextChanged);
            this.txtMotExiste.DoubleClick += new System.EventHandler(this.txtMotExiste_DoubleClick);
            // 
            // lblMotExiste
            // 
            this.lblMotExiste.AutoSize = true;
            this.lblMotExiste.Location = new System.Drawing.Point(266, 519);
            this.lblMotExiste.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblMotExiste.Name = "lblMotExiste";
            this.lblMotExiste.Size = new System.Drawing.Size(64, 13);
            this.lblMotExiste.TabIndex = 35;
            this.lblMotExiste.Text = "Mot existe ?";
            // 
            // gbBoard
            // 
            this.gbBoard.Location = new System.Drawing.Point(365, 3);
            this.gbBoard.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.gbBoard.Name = "gbBoard";
            this.gbBoard.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.gbBoard.Size = new System.Drawing.Size(515, 507);
            this.gbBoard.TabIndex = 11;
            this.gbBoard.TabStop = false;
            this.gbBoard.Text = "Board";
            // 
            // MainForm
            // 
            this.AcceptButton = this.btnSearch;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1112, 586);
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
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
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
    }
}