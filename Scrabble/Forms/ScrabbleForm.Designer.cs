using System;

namespace Scrabble
{
    [Serializable]
    partial class ScrabbleForm
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
            this.btnPlay = new System.Windows.Forms.Button();
            this.btnPass = new System.Windows.Forms.Button();
            this.btnSwap = new System.Windows.Forms.Button();
            this.lblLog = new System.Windows.Forms.Label();
            this.groupBoxPlayers = new System.Windows.Forms.GroupBox();
            this.lblCurrentTurn = new System.Windows.Forms.Label();
            this.lblPlayerTwo = new System.Windows.Forms.Label();
            this.lblPlayerOne = new System.Windows.Forms.Label();
            this.btnLetters = new System.Windows.Forms.Button();
            this.groupTiles = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lblTripleWord = new System.Windows.Forms.Label();
            this.btnHint = new System.Windows.Forms.Button();
            this.lblScrabble = new System.Windows.Forms.Label();
            this.btnExit = new System.Windows.Forms.Button();
            this.groupBoxPlayers.SuspendLayout();
            this.groupTiles.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnPlay
            // 
            this.btnPlay.BackColor = System.Drawing.Color.Silver;
            this.btnPlay.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPlay.Location = new System.Drawing.Point(700, 830);
            this.btnPlay.Name = "btnPlay";
            this.btnPlay.Size = new System.Drawing.Size(98, 45);
            this.btnPlay.TabIndex = 0;
            this.btnPlay.Text = "Play";
            this.btnPlay.UseVisualStyleBackColor = false;
            this.btnPlay.Click += new System.EventHandler(this.btnPlay_Click);
            // 
            // btnPass
            // 
            this.btnPass.BackColor = System.Drawing.Color.Silver;
            this.btnPass.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPass.Location = new System.Drawing.Point(49, 830);
            this.btnPass.Name = "btnPass";
            this.btnPass.Size = new System.Drawing.Size(98, 45);
            this.btnPass.TabIndex = 1;
            this.btnPass.Text = "Pass";
            this.btnPass.UseVisualStyleBackColor = false;
            this.btnPass.Click += new System.EventHandler(this.btnPass_Click);
            // 
            // btnSwap
            // 
            this.btnSwap.BackColor = System.Drawing.Color.Silver;
            this.btnSwap.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSwap.Location = new System.Drawing.Point(49, 881);
            this.btnSwap.Name = "btnSwap";
            this.btnSwap.Size = new System.Drawing.Size(98, 45);
            this.btnSwap.TabIndex = 2;
            this.btnSwap.Text = "Swap";
            this.btnSwap.UseVisualStyleBackColor = false;
            this.btnSwap.Click += new System.EventHandler(this.btnSwap_Click);
            // 
            // lblLog
            // 
            this.lblLog.AutoSize = true;
            this.lblLog.Location = new System.Drawing.Point(837, 136);
            this.lblLog.Name = "lblLog";
            this.lblLog.Size = new System.Drawing.Size(56, 13);
            this.lblLog.TabIndex = 4;
            this.lblLog.Text = "Game Log";
            // 
            // groupBoxPlayers
            // 
            this.groupBoxPlayers.Controls.Add(this.lblCurrentTurn);
            this.groupBoxPlayers.Controls.Add(this.lblPlayerTwo);
            this.groupBoxPlayers.Controls.Add(this.lblPlayerOne);
            this.groupBoxPlayers.Location = new System.Drawing.Point(840, 13);
            this.groupBoxPlayers.Name = "groupBoxPlayers";
            this.groupBoxPlayers.Size = new System.Drawing.Size(320, 111);
            this.groupBoxPlayers.TabIndex = 5;
            this.groupBoxPlayers.TabStop = false;
            this.groupBoxPlayers.Text = "Score";
            // 
            // lblCurrentTurn
            // 
            this.lblCurrentTurn.AutoSize = true;
            this.lblCurrentTurn.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCurrentTurn.ForeColor = System.Drawing.Color.Black;
            this.lblCurrentTurn.Location = new System.Drawing.Point(7, 80);
            this.lblCurrentTurn.Name = "lblCurrentTurn";
            this.lblCurrentTurn.Size = new System.Drawing.Size(60, 18);
            this.lblCurrentTurn.TabIndex = 2;
            this.lblCurrentTurn.Text = "label2";
            // 
            // lblPlayerTwo
            // 
            this.lblPlayerTwo.AutoSize = true;
            this.lblPlayerTwo.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPlayerTwo.Location = new System.Drawing.Point(6, 50);
            this.lblPlayerTwo.Name = "lblPlayerTwo";
            this.lblPlayerTwo.Size = new System.Drawing.Size(58, 18);
            this.lblPlayerTwo.TabIndex = 1;
            this.lblPlayerTwo.Text = "label1";
            // 
            // lblPlayerOne
            // 
            this.lblPlayerOne.AutoSize = true;
            this.lblPlayerOne.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPlayerOne.Location = new System.Drawing.Point(7, 20);
            this.lblPlayerOne.Name = "lblPlayerOne";
            this.lblPlayerOne.Size = new System.Drawing.Size(58, 18);
            this.lblPlayerOne.TabIndex = 0;
            this.lblPlayerOne.Text = "label1";
            // 
            // btnLetters
            // 
            this.btnLetters.BackColor = System.Drawing.Color.Silver;
            this.btnLetters.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLetters.ForeColor = System.Drawing.Color.Black;
            this.btnLetters.Location = new System.Drawing.Point(311, 884);
            this.btnLetters.Name = "btnLetters";
            this.btnLetters.Size = new System.Drawing.Size(193, 38);
            this.btnLetters.TabIndex = 6;
            this.btnLetters.Text = "Letters Remaining";
            this.btnLetters.UseVisualStyleBackColor = false;
            this.btnLetters.Click += new System.EventHandler(this.btnLetters_Click);
            // 
            // groupTiles
            // 
            this.groupTiles.Controls.Add(this.label4);
            this.groupTiles.Controls.Add(this.label3);
            this.groupTiles.Controls.Add(this.label2);
            this.groupTiles.Controls.Add(this.label1);
            this.groupTiles.Controls.Add(this.lblTripleWord);
            this.groupTiles.Location = new System.Drawing.Point(829, 830);
            this.groupTiles.Name = "groupTiles";
            this.groupTiles.Size = new System.Drawing.Size(345, 81);
            this.groupTiles.TabIndex = 7;
            this.groupTiles.TabStop = false;
            this.groupTiles.Text = "Tiles";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Purple;
            this.label4.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(238, 16);
            this.label4.Name = "label4";
            this.label4.Padding = new System.Windows.Forms.Padding(3, 3, 3, 3);
            this.label4.Size = new System.Drawing.Size(92, 22);
            this.label4.TabIndex = 4;
            this.label4.Text = "Center Tile";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.RoyalBlue;
            this.label3.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(126, 44);
            this.label3.Name = "label3";
            this.label3.Padding = new System.Windows.Forms.Padding(3, 3, 3, 3);
            this.label3.Size = new System.Drawing.Size(112, 22);
            this.label3.TabIndex = 3;
            this.label3.Text = "Double Letter";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.OrangeRed;
            this.label2.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(126, 15);
            this.label2.Name = "label2";
            this.label2.Padding = new System.Windows.Forms.Padding(3, 3, 3, 3);
            this.label2.Size = new System.Drawing.Size(106, 22);
            this.label2.TabIndex = 2;
            this.label2.Text = "Double Word";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.ForestGreen;
            this.label1.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(18, 44);
            this.label1.Name = "label1";
            this.label1.Padding = new System.Windows.Forms.Padding(3, 3, 3, 3);
            this.label1.Size = new System.Drawing.Size(102, 22);
            this.label1.TabIndex = 1;
            this.label1.Text = "Triple Letter";
            // 
            // lblTripleWord
            // 
            this.lblTripleWord.AutoSize = true;
            this.lblTripleWord.BackColor = System.Drawing.Color.Orange;
            this.lblTripleWord.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTripleWord.ForeColor = System.Drawing.Color.White;
            this.lblTripleWord.Location = new System.Drawing.Point(18, 15);
            this.lblTripleWord.Name = "lblTripleWord";
            this.lblTripleWord.Padding = new System.Windows.Forms.Padding(3, 3, 3, 3);
            this.lblTripleWord.Size = new System.Drawing.Size(96, 22);
            this.lblTripleWord.TabIndex = 0;
            this.lblTripleWord.Text = "Triple Word";
            // 
            // btnHint
            // 
            this.btnHint.BackColor = System.Drawing.Color.Silver;
            this.btnHint.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnHint.Location = new System.Drawing.Point(700, 881);
            this.btnHint.Name = "btnHint";
            this.btnHint.Size = new System.Drawing.Size(98, 45);
            this.btnHint.TabIndex = 8;
            this.btnHint.Text = "Hint";
            this.btnHint.UseVisualStyleBackColor = false;
            this.btnHint.Click += new System.EventHandler(this.btnHint_Click);
            // 
            // lblScrabble
            // 
            this.lblScrabble.AutoSize = true;
            this.lblScrabble.Font = new System.Drawing.Font("Verdana", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblScrabble.Location = new System.Drawing.Point(47, 9);
            this.lblScrabble.Name = "lblScrabble";
            this.lblScrabble.Size = new System.Drawing.Size(273, 29);
            this.lblScrabble.TabIndex = 9;
            this.lblScrabble.Text = "Welcome to Scrabble!";
            // 
            // btnExit
            // 
            this.btnExit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.btnExit.ForeColor = System.Drawing.Color.White;
            this.btnExit.Location = new System.Drawing.Point(723, 17);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(75, 23);
            this.btnExit.TabIndex = 10;
            this.btnExit.Text = "Exit";
            this.btnExit.UseVisualStyleBackColor = false;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // ScrabbleForm
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.ClientSize = new System.Drawing.Size(1212, 961);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.lblScrabble);
            this.Controls.Add(this.btnHint);
            this.Controls.Add(this.groupTiles);
            this.Controls.Add(this.btnLetters);
            this.Controls.Add(this.groupBoxPlayers);
            this.Controls.Add(this.lblLog);
            this.Controls.Add(this.btnSwap);
            this.Controls.Add(this.btnPass);
            this.Controls.Add(this.btnPlay);
            this.MaximizeBox = false;
            this.Name = "ScrabbleForm";
            this.Text = "Scrabble!";
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.ScrabbleForm_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.ScrabbleForm_DragEnter);
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ScrabbleForm_MouseClick);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ScrabbleForm_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ScrabbleForm_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ScrabbleForm_MouseUp);
            this.groupBoxPlayers.ResumeLayout(false);
            this.groupBoxPlayers.PerformLayout();
            this.groupTiles.ResumeLayout(false);
            this.groupTiles.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.Button btnPlay;
        public System.Windows.Forms.Button btnPass;
        public System.Windows.Forms.Button btnSwap;
        public System.Windows.Forms.Label lblLog;
        public System.Windows.Forms.GroupBox groupBoxPlayers;
        public System.Windows.Forms.Label lblCurrentTurn;
        public System.Windows.Forms.Label lblPlayerTwo;
        public System.Windows.Forms.Label lblPlayerOne;
        public System.Windows.Forms.Button btnLetters;
        public System.Windows.Forms.GroupBox groupTiles;
        private System.Windows.Forms.Label lblTripleWord;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.Button btnHint;
        private System.Windows.Forms.Label lblScrabble;
        private System.Windows.Forms.Button btnExit;
    }
}