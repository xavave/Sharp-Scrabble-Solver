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
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.txtGrid2 = new System.Windows.Forms.TextBox();
            this.btnTranspose = new System.Windows.Forms.Button();
            this.btnBackToRack = new System.Windows.Forms.Button();
            this.txtTileProps = new System.Windows.Forms.TextBox();
            this.btnValidate = new System.Windows.Forms.Button();
            this.btnDemo = new System.Windows.Forms.Button();
            this.lblPlayer1Score = new System.Windows.Forms.Label();
            this.lblPlayer2Score = new System.Windows.Forms.Label();
            this.btnDemoAll = new System.Windows.Forms.Button();
            this.groupBox1 = new Dawg.Resolver.Winform.Test.CustomGroupBox();
            this.txtRackP2 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(416, 3);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 36);
            this.btnSearch.TabIndex = 7;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // txtRackP1
            // 
            this.txtRackP1.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtRackP1.Location = new System.Drawing.Point(275, 3);
            this.txtRackP1.Name = "txtRackP1";
            this.txtRackP1.Size = new System.Drawing.Size(135, 26);
            this.txtRackP1.TabIndex = 5;
            // 
            // lsb
            // 
            this.lsb.FormattingEnabled = true;
            this.lsb.ItemHeight = 20;
            this.lsb.Location = new System.Drawing.Point(3, 3);
            this.lsb.Name = "lsb";
            this.lsb.Size = new System.Drawing.Size(232, 544);
            this.lsb.TabIndex = 8;
            this.lsb.Click += new System.EventHandler(this.lsb_Click);
            // 
            // textBox3
            // 
            this.textBox3.BackColor = System.Drawing.SystemColors.InfoText;
            this.textBox3.ForeColor = System.Drawing.SystemColors.InactiveBorder;
            this.textBox3.Location = new System.Drawing.Point(3, 555);
            this.textBox3.Multiline = true;
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(488, 154);
            this.textBox3.TabIndex = 9;
            // 
            // txtGrid2
            // 
            this.txtGrid2.BackColor = System.Drawing.SystemColors.InfoText;
            this.txtGrid2.ForeColor = System.Drawing.SystemColors.InactiveBorder;
            this.txtGrid2.Location = new System.Drawing.Point(241, 176);
            this.txtGrid2.Multiline = true;
            this.txtGrid2.Name = "txtGrid2";
            this.txtGrid2.Size = new System.Drawing.Size(250, 371);
            this.txtGrid2.TabIndex = 10;
            // 
            // btnTranspose
            // 
            this.btnTranspose.Location = new System.Drawing.Point(241, 138);
            this.btnTranspose.Name = "btnTranspose";
            this.btnTranspose.Size = new System.Drawing.Size(97, 39);
            this.btnTranspose.TabIndex = 12;
            this.btnTranspose.Text = "Transpose";
            this.btnTranspose.UseVisualStyleBackColor = true;
            this.btnTranspose.Click += new System.EventHandler(this.btnTranspose_Click);
            // 
            // btnBackToRack
            // 
            this.btnBackToRack.Location = new System.Drawing.Point(373, 138);
            this.btnBackToRack.Name = "btnBackToRack";
            this.btnBackToRack.Size = new System.Drawing.Size(118, 39);
            this.btnBackToRack.TabIndex = 13;
            this.btnBackToRack.Text = "BackToRack";
            this.btnBackToRack.UseVisualStyleBackColor = true;
            this.btnBackToRack.Click += new System.EventHandler(this.btnBackToRack_Click);
            // 
            // txtTileProps
            // 
            this.txtTileProps.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTileProps.Location = new System.Drawing.Point(1294, 90);
            this.txtTileProps.Multiline = true;
            this.txtTileProps.Name = "txtTileProps";
            this.txtTileProps.Size = new System.Drawing.Size(365, 683);
            this.txtTileProps.TabIndex = 14;
            // 
            // btnValidate
            // 
            this.btnValidate.Location = new System.Drawing.Point(394, 715);
            this.btnValidate.Name = "btnValidate";
            this.btnValidate.Size = new System.Drawing.Size(97, 58);
            this.btnValidate.TabIndex = 15;
            this.btnValidate.Text = "Validate Word";
            this.btnValidate.UseVisualStyleBackColor = true;
            this.btnValidate.Click += new System.EventHandler(this.btnValidate_Click);
            // 
            // btnDemo
            // 
            this.btnDemo.Location = new System.Drawing.Point(22, 720);
            this.btnDemo.Name = "btnDemo";
            this.btnDemo.Size = new System.Drawing.Size(97, 58);
            this.btnDemo.TabIndex = 16;
            this.btnDemo.Text = "AutoPlay 1";
            this.btnDemo.UseVisualStyleBackColor = true;
            this.btnDemo.Click += new System.EventHandler(this.btnDemo_Click);
            // 
            // lblPlayer1Score
            // 
            this.lblPlayer1Score.AutoSize = true;
            this.lblPlayer1Score.Location = new System.Drawing.Point(1295, 13);
            this.lblPlayer1Score.Name = "lblPlayer1Score";
            this.lblPlayer1Score.Size = new System.Drawing.Size(0, 20);
            this.lblPlayer1Score.TabIndex = 17;
            // 
            // lblPlayer2Score
            // 
            this.lblPlayer2Score.AutoSize = true;
            this.lblPlayer2Score.Location = new System.Drawing.Point(1295, 54);
            this.lblPlayer2Score.Name = "lblPlayer2Score";
            this.lblPlayer2Score.Size = new System.Drawing.Size(0, 20);
            this.lblPlayer2Score.TabIndex = 18;
            // 
            // btnDemoAll
            // 
            this.btnDemoAll.Location = new System.Drawing.Point(125, 720);
            this.btnDemoAll.Name = "btnDemoAll";
            this.btnDemoAll.Size = new System.Drawing.Size(97, 58);
            this.btnDemoAll.TabIndex = 19;
            this.btnDemoAll.Text = "AutoPlay All";
            this.btnDemoAll.UseVisualStyleBackColor = true;
            this.btnDemoAll.Click += new System.EventHandler(this.btnDemoAll_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Location = new System.Drawing.Point(516, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(772, 770);
            this.groupBox1.TabIndex = 11;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Board";
            // 
            // txtRackP2
            // 
            this.txtRackP2.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtRackP2.Location = new System.Drawing.Point(275, 54);
            this.txtRackP2.Name = "txtRackP2";
            this.txtRackP2.Size = new System.Drawing.Size(135, 26);
            this.txtRackP2.TabIndex = 20;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(241, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(28, 20);
            this.label1.TabIndex = 21;
            this.label1.Text = "P1";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(241, 57);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(28, 20);
            this.label2.TabIndex = 22;
            this.label2.Text = "P2";
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1668, 790);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtRackP2);
            this.Controls.Add(this.btnDemoAll);
            this.Controls.Add(this.lblPlayer2Score);
            this.Controls.Add(this.lblPlayer1Score);
            this.Controls.Add(this.btnDemo);
            this.Controls.Add(this.btnValidate);
            this.Controls.Add(this.txtTileProps);
            this.Controls.Add(this.btnBackToRack);
            this.Controls.Add(this.btnTranspose);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.txtGrid2);
            this.Controls.Add(this.textBox3);
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
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.TextBox txtGrid2;
        private CustomGroupBox groupBox1;
        private System.Windows.Forms.Button btnTranspose;
        private System.Windows.Forms.Button btnBackToRack;
        public System.Windows.Forms.TextBox txtTileProps;
        private System.Windows.Forms.Button btnValidate;
        private System.Windows.Forms.Button btnDemo;
        private System.Windows.Forms.Label lblPlayer1Score;
        private System.Windows.Forms.Label lblPlayer2Score;
        private System.Windows.Forms.Button btnDemoAll;
        private System.Windows.Forms.TextBox txtRackP2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}