namespace Dawg.Resolver.Winform.Test
{
    partial class Form2
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
            this.lsb = new System.Windows.Forms.ListBox();
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
            this.groupBox1 = new Dawg.Resolver.Winform.Test.CustomGroupBox();
            this.SuspendLayout();
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(463, 3);
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
            // lsb
            // 
            this.lsb.FormattingEnabled = true;
            this.lsb.ItemHeight = 20;
            this.lsb.Location = new System.Drawing.Point(12, 43);
            this.lsb.Name = "lsb";
            this.lsb.Size = new System.Drawing.Size(270, 504);
            this.lsb.TabIndex = 8;
            this.lsb.Click += new System.EventHandler(this.lsb_Click);
            // 
            // txtBag
            // 
            this.txtBag.BackColor = System.Drawing.SystemColors.InfoText;
            this.txtBag.ForeColor = System.Drawing.SystemColors.InactiveBorder;
            this.txtBag.Location = new System.Drawing.Point(50, 555);
            this.txtBag.Multiline = true;
            this.txtBag.Name = "txtBag";
            this.txtBag.Size = new System.Drawing.Size(488, 153);
            this.txtBag.TabIndex = 9;
            // 
            // txtGrid2
            // 
            this.txtGrid2.BackColor = System.Drawing.SystemColors.InfoText;
            this.txtGrid2.ForeColor = System.Drawing.SystemColors.InactiveBorder;
            this.txtGrid2.Location = new System.Drawing.Point(289, 175);
            this.txtGrid2.Multiline = true;
            this.txtGrid2.Name = "txtGrid2";
            this.txtGrid2.Size = new System.Drawing.Size(250, 372);
            this.txtGrid2.TabIndex = 10;
            // 
            // btnTranspose
            // 
            this.btnTranspose.Location = new System.Drawing.Point(289, 136);
            this.btnTranspose.Name = "btnTranspose";
            this.btnTranspose.Size = new System.Drawing.Size(112, 38);
            this.btnTranspose.TabIndex = 12;
            this.btnTranspose.Text = "Transpose";
            this.btnTranspose.UseVisualStyleBackColor = true;
            this.btnTranspose.Click += new System.EventHandler(this.btnTranspose_Click);
            // 
            // btnBackToRack
            // 
            this.btnBackToRack.Location = new System.Drawing.Point(420, 136);
            this.btnBackToRack.Name = "btnBackToRack";
            this.btnBackToRack.Size = new System.Drawing.Size(118, 38);
            this.btnBackToRack.TabIndex = 13;
            this.btnBackToRack.Text = "BackToRack";
            this.btnBackToRack.UseVisualStyleBackColor = true;
            this.btnBackToRack.Click += new System.EventHandler(this.btnBackToRack_Click);
            // 
            // btnValidate
            // 
            this.btnValidate.Location = new System.Drawing.Point(441, 715);
            this.btnValidate.Name = "btnValidate";
            this.btnValidate.Size = new System.Drawing.Size(98, 58);
            this.btnValidate.TabIndex = 15;
            this.btnValidate.Text = "Validate Word";
            this.btnValidate.UseVisualStyleBackColor = true;
            this.btnValidate.Click += new System.EventHandler(this.btnValidate_Click);
            // 
            // btnDemo
            // 
            this.btnDemo.Location = new System.Drawing.Point(50, 714);
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
            this.lblPlayer1Score.Location = new System.Drawing.Point(1294, 12);
            this.lblPlayer1Score.Name = "lblPlayer1Score";
            this.lblPlayer1Score.Size = new System.Drawing.Size(0, 20);
            this.lblPlayer1Score.TabIndex = 17;
            // 
            // lblPlayer2Score
            // 
            this.lblPlayer2Score.AutoSize = true;
            this.lblPlayer2Score.BackColor = System.Drawing.Color.LightGreen;
            this.lblPlayer2Score.Location = new System.Drawing.Point(1294, 54);
            this.lblPlayer2Score.Name = "lblPlayer2Score";
            this.lblPlayer2Score.Size = new System.Drawing.Size(0, 20);
            this.lblPlayer2Score.TabIndex = 18;
            // 
            // btnDemoAll
            // 
            this.btnDemoAll.Location = new System.Drawing.Point(171, 714);
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
            this.txtRackP2.Location = new System.Drawing.Point(321, 54);
            this.txtRackP2.Name = "txtRackP2";
            this.txtRackP2.Size = new System.Drawing.Size(134, 26);
            this.txtRackP2.TabIndex = 20;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(289, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(28, 20);
            this.label1.TabIndex = 21;
            this.label1.Text = "P1";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(289, 57);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(28, 20);
            this.label2.TabIndex = 22;
            this.label2.Text = "P2";
            // 
            // lblCurrentRack
            // 
            this.lblCurrentRack.AutoSize = true;
            this.lblCurrentRack.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCurrentRack.Location = new System.Drawing.Point(75, 8);
            this.lblCurrentRack.Name = "lblCurrentRack";
            this.lblCurrentRack.Size = new System.Drawing.Size(0, 29);
            this.lblCurrentRack.TabIndex = 23;
            // 
            // ckKeepExistingBoard
            // 
            this.ckKeepExistingBoard.AutoSize = true;
            this.ckKeepExistingBoard.Checked = true;
            this.ckKeepExistingBoard.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ckKeepExistingBoard.Location = new System.Drawing.Point(293, 718);
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
            this.lsbInfos.Location = new System.Drawing.Point(1325, 78);
            this.lsbInfos.Name = "lsbInfos";
            this.lsbInfos.Size = new System.Drawing.Size(330, 704);
            this.lsbInfos.TabIndex = 25;
            this.lsbInfos.SelectedIndexChanged += new System.EventHandler(this.lsbInfos_SelectedIndexChanged);
            // 
            // btnLoadGame
            // 
            this.btnLoadGame.Location = new System.Drawing.Point(289, 86);
            this.btnLoadGame.Name = "btnLoadGame";
            this.btnLoadGame.Size = new System.Drawing.Size(112, 46);
            this.btnLoadGame.TabIndex = 26;
            this.btnLoadGame.Text = "Load Game";
            this.btnLoadGame.UseVisualStyleBackColor = true;
            this.btnLoadGame.Click += new System.EventHandler(this.btnLoadGame_Click);
            // 
            // btnSaveGame
            // 
            this.btnSaveGame.Location = new System.Drawing.Point(421, 86);
            this.btnSaveGame.Name = "btnSaveGame";
            this.btnSaveGame.Size = new System.Drawing.Size(118, 46);
            this.btnSaveGame.TabIndex = 27;
            this.btnSaveGame.Text = "Save Game";
            this.btnSaveGame.UseVisualStyleBackColor = true;
            this.btnSaveGame.Click += new System.EventHandler(this.btnSaveGame_Click);
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.CreatePrompt = true;
            this.saveFileDialog1.FileName = "Game";
            this.saveFileDialog1.Filter = "Scrabble Game|*.gam";
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
            this.btnNewGame.Location = new System.Drawing.Point(50, 752);
            this.btnNewGame.Name = "btnNewGame";
            this.btnNewGame.Size = new System.Drawing.Size(116, 32);
            this.btnNewGame.TabIndex = 28;
            this.btnNewGame.Text = "New game";
            this.btnNewGame.UseVisualStyleBackColor = true;
            this.btnNewGame.Click += new System.EventHandler(this.btnNewGame_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Location = new System.Drawing.Point(547, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(772, 780);
            this.groupBox1.TabIndex = 11;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Board";
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1668, 789);
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
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.txtGrid2);
            this.Controls.Add(this.txtBag);
            this.Controls.Add(this.lsb);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.txtRackP1);
            this.Name = "Form2";
            this.Text = "Scrabble";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.TextBox txtRackP1;
        private System.Windows.Forms.ListBox lsb;
        private System.Windows.Forms.TextBox txtBag;
        private System.Windows.Forms.TextBox txtGrid2;
        private CustomGroupBox groupBox1;
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
    }
}