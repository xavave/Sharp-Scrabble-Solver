namespace Scrabble.Forms
{
    partial class LettersForm
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
            this.gridLetters = new System.Windows.Forms.DataGridView();
            this.lblLetters = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.gridLetters)).BeginInit();
            this.SuspendLayout();
            // 
            // gridLetters
            // 
            this.gridLetters.AllowUserToAddRows = false;
            this.gridLetters.AllowUserToDeleteRows = false;
            this.gridLetters.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridLetters.Location = new System.Drawing.Point(12, 42);
            this.gridLetters.Name = "gridLetters";
            this.gridLetters.ReadOnly = true;
            this.gridLetters.Size = new System.Drawing.Size(345, 610);
            this.gridLetters.TabIndex = 0;
            // 
            // lblLetters
            // 
            this.lblLetters.AutoSize = true;
            this.lblLetters.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLetters.Location = new System.Drawing.Point(13, 13);
            this.lblLetters.Name = "lblLetters";
            this.lblLetters.Size = new System.Drawing.Size(136, 16);
            this.lblLetters.TabIndex = 1;
            this.lblLetters.Text = "Letters Remaining: ";
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(285, 658);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 2;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // LettersForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(372, 693);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.lblLetters);
            this.Controls.Add(this.gridLetters);
            this.Name = "LettersForm";
            this.Text = "Tile Bag";
            ((System.ComponentModel.ISupportInitialize)(this.gridLetters)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView gridLetters;
        private System.Windows.Forms.Label lblLetters;
        private System.Windows.Forms.Button btnClose;
    }
}