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
            this.txtLetters = new System.Windows.Forms.TextBox();
            this.lblLetters = new System.Windows.Forms.Label();
            this.btnSearch = new System.Windows.Forms.Button();
            this.lsbWords = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lblNbWords = new System.Windows.Forms.Label();
            this.nudWordLength = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.nudWordLength)).BeginInit();
            this.SuspendLayout();
            // 
            // txtLetters
            // 
            this.txtLetters.Location = new System.Drawing.Point(322, 12);
            this.txtLetters.Name = "txtLetters";
            this.txtLetters.Size = new System.Drawing.Size(134, 26);
            this.txtLetters.TabIndex = 6;
            // 
            // lblLetters
            // 
            this.lblLetters.AutoSize = true;
            this.lblLetters.Location = new System.Drawing.Point(257, 15);
            this.lblLetters.Name = "lblLetters";
            this.lblLetters.Size = new System.Drawing.Size(59, 20);
            this.lblLetters.TabIndex = 22;
            this.lblLetters.Text = "Letters";
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(462, 8);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 35);
            this.btnSearch.TabIndex = 23;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // lsbWords
            // 
            this.lsbWords.FormattingEnabled = true;
            this.lsbWords.ItemHeight = 20;
            this.lsbWords.Location = new System.Drawing.Point(23, 108);
            this.lsbWords.Name = "lsbWords";
            this.lsbWords.Size = new System.Drawing.Size(765, 464);
            this.lsbWords.TabIndex = 24;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(543, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(102, 20);
            this.label1.TabIndex = 25;
            this.label1.Text = "? for wildcard";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(219, 41);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(237, 20);
            this.label2.TabIndex = 26;
            this.label2.Text = "Uppercase : Letter is well placed";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(219, 71);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(297, 20);
            this.label3.TabIndex = 27;
            this.label3.Text = "Lowercase : Letter is good but misplaced";
            // 
            // lblNbWords
            // 
            this.lblNbWords.AutoSize = true;
            this.lblNbWords.Location = new System.Drawing.Point(19, 85);
            this.lblNbWords.Name = "lblNbWords";
            this.lblNbWords.Size = new System.Drawing.Size(0, 20);
            this.lblNbWords.TabIndex = 28;
            // 
            // nudWordLength
            // 
            this.nudWordLength.Location = new System.Drawing.Point(126, 13);
            this.nudWordLength.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nudWordLength.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudWordLength.Name = "nudWordLength";
            this.nudWordLength.Size = new System.Drawing.Size(57, 26);
            this.nudWordLength.TabIndex = 29;
            this.nudWordLength.Value = new decimal(new int[] {
            7,
            0,
            0,
            0});
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(19, 15);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(101, 20);
            this.label4.TabIndex = 30;
            this.label4.Text = "Word Length";
            // 
            // WordsFinder
            // 
            this.AcceptButton = this.btnSearch;
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 584);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.nudWordLength);
            this.Controls.Add(this.lblNbWords);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lsbWords);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.lblLetters);
            this.Controls.Add(this.txtLetters);
            this.Name = "WordsFinder";
            this.Text = "WordsFinder";
            ((System.ComponentModel.ISupportInitialize)(this.nudWordLength)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtLetters;
        private System.Windows.Forms.Label lblLetters;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.ListBox lsbWords;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblNbWords;
        private System.Windows.Forms.NumericUpDown nudWordLength;
        private System.Windows.Forms.Label label4;
    }
}