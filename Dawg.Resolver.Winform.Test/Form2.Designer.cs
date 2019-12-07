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
            this.button1 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.lsb = new System.Windows.Forms.ListBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.txtGrid2 = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnTranspose = new System.Windows.Forms.Button();
            this.btnBackToRack = new System.Windows.Forms.Button();
            this.txtTileProps = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(416, 3);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 26);
            this.button1.TabIndex = 7;
            this.button1.Text = "Search";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // textBox1
            // 
            this.textBox1.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.textBox1.Location = new System.Drawing.Point(257, 3);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(153, 26);
            this.textBox1.TabIndex = 5;
            this.textBox1.Text = "EUDNA*A";
            // 
            // lsb
            // 
            this.lsb.FormattingEnabled = true;
            this.lsb.ItemHeight = 20;
            this.lsb.Location = new System.Drawing.Point(22, 3);
            this.lsb.Name = "lsb";
            this.lsb.Size = new System.Drawing.Size(229, 444);
            this.lsb.TabIndex = 8;
            this.lsb.SelectedIndexChanged += new System.EventHandler(this.lsb_SelectedIndexChanged);
            // 
            // textBox3
            // 
            this.textBox3.BackColor = System.Drawing.SystemColors.InfoText;
            this.textBox3.ForeColor = System.Drawing.SystemColors.InactiveBorder;
            this.textBox3.Location = new System.Drawing.Point(22, 466);
            this.textBox3.Multiline = true;
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(469, 154);
            this.textBox3.TabIndex = 9;
            // 
            // txtGrid2
            // 
            this.txtGrid2.BackColor = System.Drawing.SystemColors.InfoText;
            this.txtGrid2.ForeColor = System.Drawing.SystemColors.InactiveBorder;
            this.txtGrid2.Location = new System.Drawing.Point(257, 77);
            this.txtGrid2.Multiline = true;
            this.txtGrid2.Name = "txtGrid2";
            this.txtGrid2.Size = new System.Drawing.Size(234, 370);
            this.txtGrid2.TabIndex = 10;
            // 
            // groupBox1
            // 
            this.groupBox1.Location = new System.Drawing.Point(516, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(701, 712);
            this.groupBox1.TabIndex = 11;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Board";
            // 
            // btnTranspose
            // 
            this.btnTranspose.Location = new System.Drawing.Point(257, 45);
            this.btnTranspose.Name = "btnTranspose";
            this.btnTranspose.Size = new System.Drawing.Size(97, 26);
            this.btnTranspose.TabIndex = 12;
            this.btnTranspose.Text = "Transpose";
            this.btnTranspose.UseVisualStyleBackColor = true;
            this.btnTranspose.Click += new System.EventHandler(this.btnTranspose_Click);
            // 
            // btnBackToRack
            // 
            this.btnBackToRack.Location = new System.Drawing.Point(373, 45);
            this.btnBackToRack.Name = "btnBackToRack";
            this.btnBackToRack.Size = new System.Drawing.Size(118, 26);
            this.btnBackToRack.TabIndex = 13;
            this.btnBackToRack.Text = "BackToRack";
            this.btnBackToRack.UseVisualStyleBackColor = true;
            this.btnBackToRack.Click += new System.EventHandler(this.btnBackToRack_Click);
            // 
            // txtTileProps
            // 
            this.txtTileProps.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTileProps.Location = new System.Drawing.Point(1224, 13);
            this.txtTileProps.Multiline = true;
            this.txtTileProps.Name = "txtTileProps";
            this.txtTileProps.Size = new System.Drawing.Size(301, 702);
            this.txtTileProps.TabIndex = 14;
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1537, 748);
            this.Controls.Add(this.txtTileProps);
            this.Controls.Add(this.btnBackToRack);
            this.Controls.Add(this.btnTranspose);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.txtGrid2);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.lsb);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textBox1);
            this.Name = "Form2";
            this.Text = "Form2";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.ListBox lsb;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.TextBox txtGrid2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnTranspose;
        private System.Windows.Forms.Button btnBackToRack;
        public System.Windows.Forms.TextBox txtTileProps;
    }
}