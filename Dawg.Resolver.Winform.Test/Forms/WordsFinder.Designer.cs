namespace Dawg.Solver.Winform
{
    partial class WordsFinder
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
            this.lblLetters = new System.Windows.Forms.Label();
            this.txtLetters = new System.Windows.Forms.TextBox();
            this.lblPattern = new System.Windows.Forms.Label();
            this.txtPattern = new System.Windows.Forms.TextBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.lblLength = new System.Windows.Forms.Label();
            this.nudWordLength = new System.Windows.Forms.NumericUpDown();
            this.lblLengthHint = new System.Windows.Forms.Label();
            this.ckAllLetters = new System.Windows.Forms.CheckBox();
            this.lblHelp = new System.Windows.Forms.Label();
            this.lblNbWords = new System.Windows.Forms.Label();
            this.lsbWords = new System.Windows.Forms.ListBox();
            ((System.ComponentModel.ISupportInitialize)(this.nudWordLength)).BeginInit();
            this.SuspendLayout();
            // 
            // lblLetters
            // 
            this.lblLetters.AutoSize = true;
            this.lblLetters.Location = new System.Drawing.Point(12, 15);
            this.lblLetters.Name = "lblLetters";
            this.lblLetters.Size = new System.Drawing.Size(59, 20);
            this.lblLetters.TabIndex = 0;
            this.lblLetters.Text = "Letters";
            // 
            // txtLetters
            // 
            this.txtLetters.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtLetters.Location = new System.Drawing.Point(110, 12);
            this.txtLetters.MaxLength = 15;
            this.txtLetters.Name = "txtLetters";
            this.txtLetters.Size = new System.Drawing.Size(150, 26);
            this.txtLetters.TabIndex = 1;
            // 
            // lblPattern
            // 
            this.lblPattern.AutoSize = true;
            this.lblPattern.Location = new System.Drawing.Point(280, 15);
            this.lblPattern.Name = "lblPattern";
            this.lblPattern.Size = new System.Drawing.Size(61, 20);
            this.lblPattern.TabIndex = 2;
            this.lblPattern.Text = "Pattern";
            // 
            // txtPattern
            // 
            this.txtPattern.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtPattern.Location = new System.Drawing.Point(360, 12);
            this.txtPattern.MaxLength = 15;
            this.txtPattern.Name = "txtPattern";
            this.txtPattern.Size = new System.Drawing.Size(170, 26);
            this.txtPattern.TabIndex = 3;
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(545, 8);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(110, 34);
            this.btnSearch.TabIndex = 4;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // lblLength
            // 
            this.lblLength.AutoSize = true;
            this.lblLength.Location = new System.Drawing.Point(12, 53);
            this.lblLength.Name = "lblLength";
            this.lblLength.Size = new System.Drawing.Size(59, 20);
            this.lblLength.TabIndex = 5;
            this.lblLength.Text = "Length";
            // 
            // nudWordLength
            // 
            this.nudWordLength.Location = new System.Drawing.Point(110, 50);
            this.nudWordLength.Maximum = new decimal(new int[] {
            15,
            0,
            0,
            0});
            this.nudWordLength.Name = "nudWordLength";
            this.nudWordLength.Size = new System.Drawing.Size(60, 26);
            this.nudWordLength.TabIndex = 6;
            // 
            // lblLengthHint
            // 
            this.lblLengthHint.AutoSize = true;
            this.lblLengthHint.Location = new System.Drawing.Point(176, 53);
            this.lblLengthHint.Name = "lblLengthHint";
            this.lblLengthHint.Size = new System.Drawing.Size(70, 20);
            this.lblLengthHint.TabIndex = 7;
            this.lblLengthHint.Text = "(0 = all)";
            // 
            // ckAllLetters
            // 
            this.ckAllLetters.AutoSize = true;
            this.ckAllLetters.Location = new System.Drawing.Point(360, 52);
            this.ckAllLetters.Name = "ckAllLetters";
            this.ckAllLetters.Size = new System.Drawing.Size(140, 24);
            this.ckAllLetters.TabIndex = 8;
            this.ckAllLetters.Text = "Use all letters";
            this.ckAllLetters.UseVisualStyleBackColor = true;
            // 
            // lblHelp
            // 
            this.lblHelp.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblHelp.ForeColor = System.Drawing.SystemColors.GrayText;
            this.lblHelp.Location = new System.Drawing.Point(12, 84);
            this.lblHelp.Name = "lblHelp";
            this.lblHelp.Size = new System.Drawing.Size(776, 64);
            this.lblHelp.TabIndex = 9;
            this.lblHelp.Text = "help";
            // 
            // lblNbWords
            // 
            this.lblNbWords.AutoSize = true;
            this.lblNbWords.Location = new System.Drawing.Point(12, 152);
            this.lblNbWords.Name = "lblNbWords";
            this.lblNbWords.Size = new System.Drawing.Size(0, 20);
            this.lblNbWords.TabIndex = 10;
            // 
            // lsbWords
            // 
            this.lsbWords.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lsbWords.Font = new System.Drawing.Font("Consolas", 11F);
            this.lsbWords.FormattingEnabled = true;
            this.lsbWords.IntegralHeight = false;
            this.lsbWords.ItemHeight = 22;
            this.lsbWords.Location = new System.Drawing.Point(12, 178);
            this.lsbWords.MultiColumn = true;
            this.lsbWords.ColumnWidth = 250;
            this.lsbWords.Name = "lsbWords";
            this.lsbWords.Size = new System.Drawing.Size(776, 400);
            this.lsbWords.TabIndex = 11;
            this.lsbWords.DoubleClick += new System.EventHandler(this.lsbWords_DoubleClick);
            // 
            // WordsFinder
            // 
            this.AcceptButton = this.btnSearch;
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 590);
            this.Controls.Add(this.lsbWords);
            this.Controls.Add(this.lblNbWords);
            this.Controls.Add(this.lblHelp);
            this.Controls.Add(this.ckAllLetters);
            this.Controls.Add(this.lblLengthHint);
            this.Controls.Add(this.nudWordLength);
            this.Controls.Add(this.lblLength);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.txtPattern);
            this.Controls.Add(this.lblPattern);
            this.Controls.Add(this.txtLetters);
            this.Controls.Add(this.lblLetters);
            this.MinimumSize = new System.Drawing.Size(700, 400);
            this.Name = "WordsFinder";
            this.Text = "WordsFinder";
            ((System.ComponentModel.ISupportInitialize)(this.nudWordLength)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblLetters;
        private System.Windows.Forms.TextBox txtLetters;
        private System.Windows.Forms.Label lblPattern;
        private System.Windows.Forms.TextBox txtPattern;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Label lblLength;
        private System.Windows.Forms.NumericUpDown nudWordLength;
        private System.Windows.Forms.Label lblLengthHint;
        private System.Windows.Forms.CheckBox ckAllLetters;
        private System.Windows.Forms.Label lblHelp;
        private System.Windows.Forms.Label lblNbWords;
        private System.Windows.Forms.ListBox lsbWords;
    }
}
