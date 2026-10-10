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
            btnSearch = new System.Windows.Forms.Button();
            txtRackP1 = new System.Windows.Forms.TextBox();
            lsbHintWords = new System.Windows.Forms.ListBox();
            txtBag = new System.Windows.Forms.TextBox();
            txtGrid2 = new System.Windows.Forms.TextBox();
            btnTranspose = new System.Windows.Forms.Button();
            btnBackToRack = new System.Windows.Forms.Button();
            btnValidate = new System.Windows.Forms.Button();
            btnDemo = new System.Windows.Forms.Button();
            lblPlayer1Score = new System.Windows.Forms.Label();
            lblPlayer2Score = new System.Windows.Forms.Label();
            btnDemoAll = new System.Windows.Forms.Button();
            txtRackP2 = new System.Windows.Forms.TextBox();
            label1 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            lblCurrentRack = new System.Windows.Forms.Label();
            ckKeepExistingBoard = new System.Windows.Forms.CheckBox();
            lsbInfos = new System.Windows.Forms.ListBox();
            btnLoadGame = new System.Windows.Forms.Button();
            btnSaveGame = new System.Windows.Forms.Button();
            saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            btnNewGame = new System.Windows.Forms.Button();
            lblP1BestPlay = new System.Windows.Forms.Label();
            lblP2BestPlay = new System.Windows.Forms.Label();
            lvWords = new System.Windows.Forms.ListView();
            groupBox1 = new System.Windows.Forms.GroupBox();
            nudCustomBoardSize = new System.Windows.Forms.NumericUpDown();
            rbSize15 = new System.Windows.Forms.RadioButton();
            rbSize11 = new System.Windows.Forms.RadioButton();
            ckShowGrid = new System.Windows.Forms.CheckBox();
            gbWordDirection = new System.Windows.Forms.GroupBox();
            rbWordDirDown = new System.Windows.Forms.RadioButton();
            rbWordDirRight = new System.Windows.Forms.RadioButton();
            gbGameStyle = new System.Windows.Forms.GroupBox();
            rbGameStyleScrabble = new System.Windows.Forms.RadioButton();
            rbWordsWithFriends = new System.Windows.Forms.RadioButton();
            gbSortBy = new System.Windows.Forms.GroupBox();
            rbMaxLength = new System.Windows.Forms.RadioButton();
            rbBestScore = new System.Windows.Forms.RadioButton();
            txtMotExiste = new System.Windows.Forms.TextBox();
            lblMotExiste = new System.Windows.Forms.Label();
            btnUndoLast = new System.Windows.Forms.Button();
            groupBox2 = new System.Windows.Forms.GroupBox();
            rbEnCollins2019 = new System.Windows.Forms.RadioButton();
            rbODS9 = new System.Windows.Forms.RadioButton();
            rbODS8 = new System.Windows.Forms.RadioButton();
            rbODS7 = new System.Windows.Forms.RadioButton();
            rbODS6 = new System.Windows.Forms.RadioButton();
            btnWordsFinder = new System.Windows.Forms.Button();
            gbBoard = new CustomGroupBox();
            groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nudCustomBoardSize).BeginInit();
            gbWordDirection.SuspendLayout();
            gbGameStyle.SuspendLayout();
            gbSortBy.SuspendLayout();
            groupBox2.SuspendLayout();
            SuspendLayout();
            // 
            // btnSearch
            // 
            btnSearch.Location = new System.Drawing.Point(516, 4);
            btnSearch.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            btnSearch.Name = "btnSearch";
            btnSearch.Size = new System.Drawing.Size(83, 44);
            btnSearch.TabIndex = 7;
            btnSearch.Text = "Search";
            btnSearch.UseVisualStyleBackColor = true;
            btnSearch.Click += btnSearch_Click;
            // 
            // txtRackP1
            // 
            txtRackP1.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            txtRackP1.Location = new System.Drawing.Point(357, 4);
            txtRackP1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            txtRackP1.Name = "txtRackP1";
            txtRackP1.Size = new System.Drawing.Size(148, 31);
            txtRackP1.TabIndex = 5;
            // 
            // lsbHintWords
            // 
            lsbHintWords.FormattingEnabled = true;
            lsbHintWords.Location = new System.Drawing.Point(-6, 98);
            lsbHintWords.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            lsbHintWords.Name = "lsbHintWords";
            lsbHintWords.Size = new System.Drawing.Size(287, 604);
            lsbHintWords.TabIndex = 8;
            lsbHintWords.Click += lsb_Click;
            // 
            // txtBag
            // 
            txtBag.BackColor = System.Drawing.SystemColors.InfoText;
            txtBag.Font = new System.Drawing.Font("Courier New", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            txtBag.ForeColor = System.Drawing.SystemColors.InactiveBorder;
            txtBag.Location = new System.Drawing.Point(13, 702);
            txtBag.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            txtBag.Multiline = true;
            txtBag.Name = "txtBag";
            txtBag.Size = new System.Drawing.Size(417, 183);
            txtBag.TabIndex = 9;
            // 
            // txtGrid2
            // 
            txtGrid2.BackColor = System.Drawing.SystemColors.InfoText;
            txtGrid2.Font = new System.Drawing.Font("Courier New", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, 0);
            txtGrid2.ForeColor = System.Drawing.SystemColors.InactiveBorder;
            txtGrid2.Location = new System.Drawing.Point(309, 236);
            txtGrid2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            txtGrid2.Multiline = true;
            txtGrid2.Name = "txtGrid2";
            txtGrid2.Size = new System.Drawing.Size(293, 456);
            txtGrid2.TabIndex = 10;
            // 
            // btnTranspose
            // 
            btnTranspose.Location = new System.Drawing.Point(322, 152);
            btnTranspose.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            btnTranspose.Name = "btnTranspose";
            btnTranspose.Size = new System.Drawing.Size(124, 48);
            btnTranspose.TabIndex = 12;
            btnTranspose.Text = "Transpose";
            btnTranspose.UseVisualStyleBackColor = true;
            btnTranspose.Click += btnTranspose_Click;
            // 
            // btnBackToRack
            // 
            btnBackToRack.Location = new System.Drawing.Point(467, 152);
            btnBackToRack.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            btnBackToRack.Name = "btnBackToRack";
            btnBackToRack.Size = new System.Drawing.Size(131, 48);
            btnBackToRack.TabIndex = 13;
            btnBackToRack.Text = "BackToRack";
            btnBackToRack.UseVisualStyleBackColor = true;
            btnBackToRack.Click += btnBackToRack_Click;
            // 
            // btnValidate
            // 
            btnValidate.Location = new System.Drawing.Point(442, 702);
            btnValidate.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            btnValidate.Name = "btnValidate";
            btnValidate.Size = new System.Drawing.Size(157, 64);
            btnValidate.TabIndex = 15;
            btnValidate.Text = "Validate Word";
            btnValidate.UseVisualStyleBackColor = true;
            btnValidate.Click += btnValidate_Click;
            // 
            // btnDemo
            // 
            btnDemo.Location = new System.Drawing.Point(18, 896);
            btnDemo.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            btnDemo.Name = "btnDemo";
            btnDemo.Size = new System.Drawing.Size(129, 40);
            btnDemo.TabIndex = 16;
            btnDemo.Text = "AutoPlay 1";
            btnDemo.UseVisualStyleBackColor = true;
            btnDemo.Click += btnDemo_Click;
            // 
            // lblPlayer1Score
            // 
            lblPlayer1Score.AutoSize = true;
            lblPlayer1Score.BackColor = System.Drawing.Color.LightYellow;
            lblPlayer1Score.Location = new System.Drawing.Point(1482, 15);
            lblPlayer1Score.Name = "lblPlayer1Score";
            lblPlayer1Score.Size = new System.Drawing.Size(81, 25);
            lblPlayer1Score.TabIndex = 17;
            lblPlayer1Score.Text = "P1 Score";
            // 
            // lblPlayer2Score
            // 
            lblPlayer2Score.AutoSize = true;
            lblPlayer2Score.BackColor = System.Drawing.Color.LightGreen;
            lblPlayer2Score.Location = new System.Drawing.Point(1482, 54);
            lblPlayer2Score.Name = "lblPlayer2Score";
            lblPlayer2Score.Size = new System.Drawing.Size(81, 25);
            lblPlayer2Score.TabIndex = 18;
            lblPlayer2Score.Text = "P2 Score";
            // 
            // btnDemoAll
            // 
            btnDemoAll.Location = new System.Drawing.Point(153, 896);
            btnDemoAll.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            btnDemoAll.Name = "btnDemoAll";
            btnDemoAll.Size = new System.Drawing.Size(129, 40);
            btnDemoAll.TabIndex = 19;
            btnDemoAll.Text = "AutoPlay All";
            btnDemoAll.UseVisualStyleBackColor = true;
            btnDemoAll.Click += btnDemoAll_Click;
            // 
            // txtRackP2
            // 
            txtRackP2.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            txtRackP2.Location = new System.Drawing.Point(357, 46);
            txtRackP2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            txtRackP2.Name = "txtRackP2";
            txtRackP2.Size = new System.Drawing.Size(148, 31);
            txtRackP2.TabIndex = 20;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(322, 8);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(32, 25);
            label1.TabIndex = 21;
            label1.Text = "P1";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(322, 50);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(32, 25);
            label2.TabIndex = 22;
            label2.Text = "P2";
            // 
            // lblCurrentRack
            // 
            lblCurrentRack.AutoSize = true;
            lblCurrentRack.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            lblCurrentRack.Location = new System.Drawing.Point(823, 1031);
            lblCurrentRack.Name = "lblCurrentRack";
            lblCurrentRack.Size = new System.Drawing.Size(0, 37);
            lblCurrentRack.TabIndex = 23;
            // 
            // ckKeepExistingBoard
            // 
            ckKeepExistingBoard.AutoSize = true;
            ckKeepExistingBoard.Location = new System.Drawing.Point(289, 902);
            ckKeepExistingBoard.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            ckKeepExistingBoard.Name = "ckKeepExistingBoard";
            ckKeepExistingBoard.Size = new System.Drawing.Size(116, 29);
            ckKeepExistingBoard.TabIndex = 24;
            ckKeepExistingBoard.Text = "Keep Tiles";
            ckKeepExistingBoard.UseVisualStyleBackColor = true;
            // 
            // lsbInfos
            // 
            lsbInfos.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            lsbInfos.FormattingEnabled = true;
            lsbInfos.Location = new System.Drawing.Point(1471, 98);
            lsbInfos.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            lsbInfos.Name = "lsbInfos";
            lsbInfos.Size = new System.Drawing.Size(480, 354);
            lsbInfos.TabIndex = 25;
            // 
            // btnLoadGame
            // 
            btnLoadGame.Location = new System.Drawing.Point(322, 86);
            btnLoadGame.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            btnLoadGame.Name = "btnLoadGame";
            btnLoadGame.Size = new System.Drawing.Size(124, 58);
            btnLoadGame.TabIndex = 26;
            btnLoadGame.Text = "Load Game";
            btnLoadGame.UseVisualStyleBackColor = true;
            btnLoadGame.Click += btnLoadGame_Click;
            // 
            // btnSaveGame
            // 
            btnSaveGame.Location = new System.Drawing.Point(467, 86);
            btnSaveGame.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            btnSaveGame.Name = "btnSaveGame";
            btnSaveGame.Size = new System.Drawing.Size(131, 58);
            btnSaveGame.TabIndex = 27;
            btnSaveGame.Text = "Save Game";
            btnSaveGame.UseVisualStyleBackColor = true;
            btnSaveGame.Click += btnSaveGame_Click;
            // 
            // saveFileDialog1
            // 
            saveFileDialog1.Filter = "Scrabble Game|*.gam";
            saveFileDialog1.OverwritePrompt = false;
            saveFileDialog1.RestoreDirectory = true;
            // 
            // openFileDialog1
            // 
            openFileDialog1.DefaultExt = "gam";
            openFileDialog1.FileName = "Game";
            openFileDialog1.Filter = "Scrabble Game|*.gam";
            openFileDialog1.RestoreDirectory = true;
            // 
            // btnNewGame
            // 
            btnNewGame.Location = new System.Drawing.Point(18, 944);
            btnNewGame.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            btnNewGame.Name = "btnNewGame";
            btnNewGame.Size = new System.Drawing.Size(129, 40);
            btnNewGame.TabIndex = 28;
            btnNewGame.Text = "New game";
            btnNewGame.UseVisualStyleBackColor = true;
            btnNewGame.Click += btnNewGame_Click;
            // 
            // lblP1BestPlay
            // 
            lblP1BestPlay.AutoSize = true;
            lblP1BestPlay.BackColor = System.Drawing.Color.LightYellow;
            lblP1BestPlay.Location = new System.Drawing.Point(1669, 15);
            lblP1BestPlay.Name = "lblP1BestPlay";
            lblP1BestPlay.Size = new System.Drawing.Size(84, 25);
            lblP1BestPlay.TabIndex = 29;
            lblP1BestPlay.Text = "best play";
            // 
            // lblP2BestPlay
            // 
            lblP2BestPlay.AutoSize = true;
            lblP2BestPlay.BackColor = System.Drawing.Color.LightGreen;
            lblP2BestPlay.Location = new System.Drawing.Point(1669, 54);
            lblP2BestPlay.Name = "lblP2BestPlay";
            lblP2BestPlay.Size = new System.Drawing.Size(84, 25);
            lblP2BestPlay.TabIndex = 30;
            lblP2BestPlay.Text = "best play";
            // 
            // lvWords
            // 
            lvWords.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            lvWords.Location = new System.Drawing.Point(1471, 460);
            lvWords.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            lvWords.Name = "lvWords";
            lvWords.Size = new System.Drawing.Size(480, 629);
            lvWords.TabIndex = 31;
            lvWords.UseCompatibleStateImageBehavior = false;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(nudCustomBoardSize);
            groupBox1.Controls.Add(rbSize15);
            groupBox1.Controls.Add(rbSize11);
            groupBox1.Location = new System.Drawing.Point(20, 994);
            groupBox1.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            groupBox1.Name = "groupBox1";
            groupBox1.Padding = new System.Windows.Forms.Padding(4, 6, 4, 6);
            groupBox1.Size = new System.Drawing.Size(127, 131);
            groupBox1.TabIndex = 32;
            groupBox1.TabStop = false;
            groupBox1.Text = "Board Size";
            // 
            // nudCustomBoardSize
            // 
            nudCustomBoardSize.Location = new System.Drawing.Point(10, 92);
            nudCustomBoardSize.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            nudCustomBoardSize.Maximum = new decimal(new int[] { 18, 0, 0, 0 });
            nudCustomBoardSize.Minimum = new decimal(new int[] { 7, 0, 0, 0 });
            nudCustomBoardSize.Name = "nudCustomBoardSize";
            nudCustomBoardSize.Size = new System.Drawing.Size(56, 31);
            nudCustomBoardSize.TabIndex = 0;
            nudCustomBoardSize.Value = new decimal(new int[] { 10, 0, 0, 0 });
            nudCustomBoardSize.ValueChanged += nudCustomBoardSize_ValueChanged;
            // 
            // rbSize15
            // 
            rbSize15.AutoSize = true;
            rbSize15.Checked = true;
            rbSize15.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            rbSize15.Location = new System.Drawing.Point(9, 60);
            rbSize15.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            rbSize15.Name = "rbSize15";
            rbSize15.Size = new System.Drawing.Size(49, 21);
            rbSize15.TabIndex = 1;
            rbSize15.TabStop = true;
            rbSize15.Text = "15";
            rbSize15.UseVisualStyleBackColor = true;
            rbSize15.CheckedChanged += rbSize15_CheckedChanged;
            // 
            // rbSize11
            // 
            rbSize11.AutoSize = true;
            rbSize11.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            rbSize11.Location = new System.Drawing.Point(9, 28);
            rbSize11.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            rbSize11.Name = "rbSize11";
            rbSize11.Size = new System.Drawing.Size(49, 21);
            rbSize11.TabIndex = 0;
            rbSize11.Text = "11";
            rbSize11.UseVisualStyleBackColor = true;
            // 
            // ckShowGrid
            // 
            ckShowGrid.AutoSize = true;
            ckShowGrid.Location = new System.Drawing.Point(322, 208);
            ckShowGrid.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            ckShowGrid.Name = "ckShowGrid";
            ckShowGrid.Size = new System.Drawing.Size(153, 29);
            ckShowGrid.TabIndex = 33;
            ckShowGrid.Text = "Show dev grid";
            ckShowGrid.UseVisualStyleBackColor = true;
            ckShowGrid.CheckedChanged += ckShowGrid_CheckedChanged;
            // 
            // gbWordDirection
            // 
            gbWordDirection.Controls.Add(rbWordDirDown);
            gbWordDirection.Controls.Add(rbWordDirRight);
            gbWordDirection.Location = new System.Drawing.Point(442, 775);
            gbWordDirection.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            gbWordDirection.Name = "gbWordDirection";
            gbWordDirection.Padding = new System.Windows.Forms.Padding(4, 6, 4, 6);
            gbWordDirection.Size = new System.Drawing.Size(157, 110);
            gbWordDirection.TabIndex = 33;
            gbWordDirection.TabStop = false;
            gbWordDirection.Text = "word direction";
            // 
            // rbWordDirDown
            // 
            rbWordDirDown.AutoSize = true;
            rbWordDirDown.Location = new System.Drawing.Point(13, 65);
            rbWordDirDown.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            rbWordDirDown.Name = "rbWordDirDown";
            rbWordDirDown.Size = new System.Drawing.Size(84, 29);
            rbWordDirDown.TabIndex = 1;
            rbWordDirDown.Text = "Down";
            rbWordDirDown.UseVisualStyleBackColor = true;
            rbWordDirDown.CheckedChanged += rbWordDirDown_CheckedChanged;
            // 
            // rbWordDirRight
            // 
            rbWordDirRight.AutoSize = true;
            rbWordDirRight.Checked = true;
            rbWordDirRight.Location = new System.Drawing.Point(13, 36);
            rbWordDirRight.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            rbWordDirRight.Name = "rbWordDirRight";
            rbWordDirRight.Size = new System.Drawing.Size(90, 29);
            rbWordDirRight.TabIndex = 0;
            rbWordDirRight.TabStop = true;
            rbWordDirRight.Text = "Across";
            rbWordDirRight.UseVisualStyleBackColor = true;
            // 
            // gbGameStyle
            // 
            gbGameStyle.Controls.Add(rbGameStyleScrabble);
            gbGameStyle.Controls.Add(rbWordsWithFriends);
            gbGameStyle.Location = new System.Drawing.Point(153, 1015);
            gbGameStyle.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            gbGameStyle.Name = "gbGameStyle";
            gbGameStyle.Padding = new System.Windows.Forms.Padding(4, 6, 4, 6);
            gbGameStyle.Size = new System.Drawing.Size(237, 110);
            gbGameStyle.TabIndex = 33;
            gbGameStyle.TabStop = false;
            gbGameStyle.Text = "Game Style";
            // 
            // rbGameStyleScrabble
            // 
            rbGameStyleScrabble.AutoSize = true;
            rbGameStyleScrabble.Checked = true;
            rbGameStyleScrabble.Location = new System.Drawing.Point(22, 65);
            rbGameStyleScrabble.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            rbGameStyleScrabble.Name = "rbGameStyleScrabble";
            rbGameStyleScrabble.Size = new System.Drawing.Size(105, 29);
            rbGameStyleScrabble.TabIndex = 1;
            rbGameStyleScrabble.TabStop = true;
            rbGameStyleScrabble.Text = "Scrabble";
            rbGameStyleScrabble.UseVisualStyleBackColor = true;
            rbGameStyleScrabble.CheckedChanged += rbGameStyleScrabble_CheckedChanged;
            // 
            // rbWordsWithFriends
            // 
            rbWordsWithFriends.AutoSize = true;
            rbWordsWithFriends.Location = new System.Drawing.Point(22, 32);
            rbWordsWithFriends.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            rbWordsWithFriends.Name = "rbWordsWithFriends";
            rbWordsWithFriends.Size = new System.Drawing.Size(186, 29);
            rbWordsWithFriends.TabIndex = 0;
            rbWordsWithFriends.Text = "Words with friends";
            rbWordsWithFriends.UseVisualStyleBackColor = true;
            rbWordsWithFriends.CheckedChanged += rbWordsWithFriends_CheckedChanged;
            // 
            // gbSortBy
            // 
            gbSortBy.Controls.Add(rbMaxLength);
            gbSortBy.Controls.Add(rbBestScore);
            gbSortBy.Location = new System.Drawing.Point(13, 6);
            gbSortBy.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            gbSortBy.Name = "gbSortBy";
            gbSortBy.Padding = new System.Windows.Forms.Padding(4, 6, 4, 6);
            gbSortBy.Size = new System.Drawing.Size(296, 72);
            gbSortBy.TabIndex = 33;
            gbSortBy.TabStop = false;
            gbSortBy.Text = "Sort by";
            // 
            // rbMaxLength
            // 
            rbMaxLength.AutoSize = true;
            rbMaxLength.Location = new System.Drawing.Point(151, 28);
            rbMaxLength.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            rbMaxLength.Name = "rbMaxLength";
            rbMaxLength.Size = new System.Drawing.Size(125, 29);
            rbMaxLength.TabIndex = 1;
            rbMaxLength.Text = "Max length";
            rbMaxLength.UseVisualStyleBackColor = true;
            rbMaxLength.CheckedChanged += rbMaxLength_CheckedChanged;
            // 
            // rbBestScore
            // 
            rbBestScore.AutoSize = true;
            rbBestScore.Checked = true;
            rbBestScore.Location = new System.Drawing.Point(17, 28);
            rbBestScore.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            rbBestScore.Name = "rbBestScore";
            rbBestScore.Size = new System.Drawing.Size(117, 29);
            rbBestScore.TabIndex = 0;
            rbBestScore.TabStop = true;
            rbBestScore.Text = "Best score";
            rbBestScore.UseVisualStyleBackColor = true;
            // 
            // txtMotExiste
            // 
            txtMotExiste.Location = new System.Drawing.Point(398, 1046);
            txtMotExiste.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            txtMotExiste.Name = "txtMotExiste";
            txtMotExiste.Size = new System.Drawing.Size(175, 31);
            txtMotExiste.TabIndex = 34;
            txtMotExiste.TextChanged += txtMotExiste_TextChanged;
            txtMotExiste.DoubleClick += txtMotExiste_DoubleClick;
            // 
            // lblMotExiste
            // 
            lblMotExiste.AutoSize = true;
            lblMotExiste.Location = new System.Drawing.Point(436, 1019);
            lblMotExiste.Name = "lblMotExiste";
            lblMotExiste.Size = new System.Drawing.Size(120, 25);
            lblMotExiste.TabIndex = 35;
            lblMotExiste.Text = "Words exists?";
            // 
            // btnUndoLast
            // 
            btnUndoLast.Location = new System.Drawing.Point(442, 894);
            btnUndoLast.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            btnUndoLast.Name = "btnUndoLast";
            btnUndoLast.Size = new System.Drawing.Size(157, 58);
            btnUndoLast.TabIndex = 36;
            btnUndoLast.Text = "Undo Last";
            btnUndoLast.UseVisualStyleBackColor = true;
            btnUndoLast.Click += btnUndoLast_Click;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(rbEnCollins2019);
            groupBox2.Controls.Add(rbODS9);
            groupBox2.Controls.Add(rbODS8);
            groupBox2.Controls.Add(rbODS7);
            groupBox2.Controls.Add(rbODS6);
            groupBox2.Location = new System.Drawing.Point(153, 942);
            groupBox2.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            groupBox2.Name = "groupBox2";
            groupBox2.Padding = new System.Windows.Forms.Padding(4, 6, 4, 6);
            groupBox2.Size = new System.Drawing.Size(449, 106);
            groupBox2.TabIndex = 34;
            groupBox2.TabStop = false;
            groupBox2.Text = "Dictionary";
            // 
            // rbEnCollins2019
            // 
            rbEnCollins2019.AutoSize = true;
            rbEnCollins2019.Location = new System.Drawing.Point(130, 62);
            rbEnCollins2019.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            rbEnCollins2019.Name = "rbEnCollins2019";
            rbEnCollins2019.Size = new System.Drawing.Size(161, 29);
            rbEnCollins2019.TabIndex = 2;
            rbEnCollins2019.TabStop = true;
            rbEnCollins2019.Text = "EN Collins 2019";
            rbEnCollins2019.UseVisualStyleBackColor = true;
            rbEnCollins2019.CheckedChanged += rbDico_CheckedChanged;
            // 
            // rbODS9
            // 
            rbODS9.AutoSize = true;
            rbODS9.Checked = true;
            rbODS9.Location = new System.Drawing.Point(10, 62);
            rbODS9.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            rbODS9.Name = "rbODS9";
            rbODS9.Size = new System.Drawing.Size(114, 29);
            rbODS9.TabIndex = 4;
            rbODS9.TabStop = true;
            rbODS9.Text = "FR ODS 9";
            rbODS9.UseVisualStyleBackColor = true;
            rbODS9.CheckedChanged += rbDico_CheckedChanged;
            // 
            // rbODS8
            // 
            rbODS8.AutoSize = true;
            rbODS8.Location = new System.Drawing.Point(250, 28);
            rbODS8.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            rbODS8.Name = "rbODS8";
            rbODS8.Size = new System.Drawing.Size(114, 29);
            rbODS8.TabIndex = 3;
            rbODS8.Text = "FR ODS 8";
            rbODS8.UseVisualStyleBackColor = true;
            rbODS8.CheckedChanged += rbDico_CheckedChanged;
            // 
            // rbODS7
            // 
            rbODS7.AutoSize = true;
            rbODS7.Location = new System.Drawing.Point(130, 28);
            rbODS7.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            rbODS7.Name = "rbODS7";
            rbODS7.Size = new System.Drawing.Size(114, 29);
            rbODS7.TabIndex = 1;
            rbODS7.Text = "FR ODS 7";
            rbODS7.UseVisualStyleBackColor = true;
            rbODS7.CheckedChanged += rbDico_CheckedChanged;
            // 
            // rbODS6
            // 
            rbODS6.AutoSize = true;
            rbODS6.Location = new System.Drawing.Point(10, 28);
            rbODS6.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            rbODS6.Name = "rbODS6";
            rbODS6.Size = new System.Drawing.Size(114, 29);
            rbODS6.TabIndex = 0;
            rbODS6.Text = "FR ODS 6";
            rbODS6.UseVisualStyleBackColor = true;
            rbODS6.CheckedChanged += rbDico_CheckedChanged;
            // 
            // btnWordsFinder
            // 
            btnWordsFinder.Location = new System.Drawing.Point(418, 1086);
            btnWordsFinder.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            btnWordsFinder.Name = "btnWordsFinder";
            btnWordsFinder.Size = new System.Drawing.Size(129, 40);
            btnWordsFinder.TabIndex = 37;
            btnWordsFinder.Text = "Words finder";
            btnWordsFinder.UseVisualStyleBackColor = true;
            btnWordsFinder.Click += btnWordsFinder_Click;
            // 
            // gbBoard
            // 
            gbBoard.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            gbBoard.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            gbBoard.Location = new System.Drawing.Point(611, 7);
            gbBoard.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            gbBoard.Name = "gbBoard";
            gbBoard.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            gbBoard.Size = new System.Drawing.Size(858, 975);
            gbBoard.TabIndex = 11;
            gbBoard.TabStop = false;
            gbBoard.Text = "Board";
            // 
            // MainForm
            // 
            AcceptButton = btnSearch;
            AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(1967, 1128);
            Controls.Add(btnWordsFinder);
            Controls.Add(groupBox2);
            Controls.Add(btnUndoLast);
            Controls.Add(lblMotExiste);
            Controls.Add(txtMotExiste);
            Controls.Add(gbSortBy);
            Controls.Add(gbGameStyle);
            Controls.Add(gbWordDirection);
            Controls.Add(ckShowGrid);
            Controls.Add(groupBox1);
            Controls.Add(lvWords);
            Controls.Add(lblP2BestPlay);
            Controls.Add(lblP1BestPlay);
            Controls.Add(btnNewGame);
            Controls.Add(btnSaveGame);
            Controls.Add(btnLoadGame);
            Controls.Add(lsbInfos);
            Controls.Add(ckKeepExistingBoard);
            Controls.Add(lblCurrentRack);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(txtRackP2);
            Controls.Add(btnDemoAll);
            Controls.Add(lblPlayer2Score);
            Controls.Add(lblPlayer1Score);
            Controls.Add(btnDemo);
            Controls.Add(btnValidate);
            Controls.Add(btnBackToRack);
            Controls.Add(btnTranspose);
            Controls.Add(gbBoard);
            Controls.Add(txtGrid2);
            Controls.Add(txtBag);
            Controls.Add(lsbHintWords);
            Controls.Add(btnSearch);
            Controls.Add(txtRackP1);
            Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            Name = "MainForm";
            Text = "Scrabble";
            Click += MainForm_Click;
            KeyPress += MainForm_KeyPress;
            KeyUp += MainForm_KeyUp;
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)nudCustomBoardSize).EndInit();
            gbWordDirection.ResumeLayout(false);
            gbWordDirection.PerformLayout();
            gbGameStyle.ResumeLayout(false);
            gbGameStyle.PerformLayout();
            gbSortBy.ResumeLayout(false);
            gbSortBy.PerformLayout();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            ResumeLayout(false);
            PerformLayout();

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
        public System.Windows.Forms.ListView lvWords;
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
        private System.Windows.Forms.RadioButton rbODS8;
        private System.Windows.Forms.RadioButton rbODS9;
        private System.Windows.Forms.RadioButton rbEnCollins2019;
        private System.Windows.Forms.Button btnWordsFinder;
        private System.Windows.Forms.NumericUpDown nudCustomBoardSize;
    }
}