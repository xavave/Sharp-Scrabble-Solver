namespace Test_DAWG
{
    partial class Form1
    {
        /// <summary>
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur Windows Form

        /// <summary>
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnReadDicoText = new System.Windows.Forms.Button();
            this.brtnSortTxtDico = new System.Windows.Forms.Button();
            this.btnCreationDawg = new System.Windows.Forms.Button();
            this.pgb2etapes = new System.Windows.Forms.ProgressBar();
            this.lstAvancement2Etapes = new System.Windows.Forms.ListBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lstChargementASCII = new System.Windows.Forms.ListBox();
            this.grpAjouterMot = new System.Windows.Forms.GroupBox();
            this.txtNvMot = new System.Windows.Forms.TextBox();
            this.butAjouterUnMot = new System.Windows.Forms.Button();
            this.lstAjoutMot = new System.Windows.Forms.ListBox();
            this.grpLectureDAWG = new System.Windows.Forms.GroupBox();
            this.butLectureDAWG = new System.Windows.Forms.Button();
            this.lstLectureDAWG = new System.Windows.Forms.ListBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.grpAjouterMot.SuspendLayout();
            this.grpLectureDAWG.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnReadDicoText);
            this.groupBox1.Controls.Add(this.brtnSortTxtDico);
            this.groupBox1.Controls.Add(this.btnCreationDawg);
            this.groupBox1.Controls.Add(this.pgb2etapes);
            this.groupBox1.Controls.Add(this.lstAvancement2Etapes);
            this.groupBox1.Location = new System.Drawing.Point(10, 120);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox1.Size = new System.Drawing.Size(978, 255);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Création du DAWG";
            // 
            // btnReadDicoText
            // 
            this.btnReadDicoText.Location = new System.Drawing.Point(0, 29);
            this.btnReadDicoText.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnReadDicoText.Name = "btnReadDicoText";
            this.btnReadDicoText.Size = new System.Drawing.Size(239, 35);
            this.btnReadDicoText.TabIndex = 3;
            this.btnReadDicoText.Text = "Lecture du dico Texte";
            this.btnReadDicoText.UseVisualStyleBackColor = true;
            this.btnReadDicoText.Click += new System.EventHandler(this.btnReadDicoText_Click);
            // 
            // brtnSortTxtDico
            // 
            this.brtnSortTxtDico.Enabled = false;
            this.brtnSortTxtDico.Location = new System.Drawing.Point(260, 29);
            this.brtnSortTxtDico.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.brtnSortTxtDico.Name = "brtnSortTxtDico";
            this.brtnSortTxtDico.Size = new System.Drawing.Size(156, 35);
            this.brtnSortTxtDico.TabIndex = 2;
            this.brtnSortTxtDico.Text = "Tri dico réel";
            this.brtnSortTxtDico.UseVisualStyleBackColor = true;
            this.brtnSortTxtDico.Visible = false;
            this.brtnSortTxtDico.Click += new System.EventHandler(this.button2_Click);
            // 
            // btnCreationDawg
            // 
            this.btnCreationDawg.Enabled = false;
            this.btnCreationDawg.Location = new System.Drawing.Point(446, 29);
            this.btnCreationDawg.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnCreationDawg.Name = "btnCreationDawg";
            this.btnCreationDawg.Size = new System.Drawing.Size(156, 35);
            this.btnCreationDawg.TabIndex = 1;
            this.btnCreationDawg.Text = "Création du dawg";
            this.btnCreationDawg.UseVisualStyleBackColor = true;
            this.btnCreationDawg.Click += new System.EventHandler(this.btnCreationDawg_Click);
            // 
            // pgb2etapes
            // 
            this.pgb2etapes.Location = new System.Drawing.Point(9, 189);
            this.pgb2etapes.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.pgb2etapes.Name = "pgb2etapes";
            this.pgb2etapes.Size = new System.Drawing.Size(960, 52);
            this.pgb2etapes.TabIndex = 1;
            // 
            // lstAvancement2Etapes
            // 
            this.lstAvancement2Etapes.FormattingEnabled = true;
            this.lstAvancement2Etapes.ItemHeight = 20;
            this.lstAvancement2Etapes.Location = new System.Drawing.Point(4, 74);
            this.lstAvancement2Etapes.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.lstAvancement2Etapes.Name = "lstAvancement2Etapes";
            this.lstAvancement2Etapes.Size = new System.Drawing.Size(962, 104);
            this.lstAvancement2Etapes.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lstChargementASCII);
            this.groupBox2.Location = new System.Drawing.Point(6, 18);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox2.Size = new System.Drawing.Size(978, 92);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Chargement du dictionnaire ASCII de référence";
            // 
            // lstChargementASCII
            // 
            this.lstChargementASCII.FormattingEnabled = true;
            this.lstChargementASCII.ItemHeight = 20;
            this.lstChargementASCII.Location = new System.Drawing.Point(9, 29);
            this.lstChargementASCII.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.lstChargementASCII.Name = "lstChargementASCII";
            this.lstChargementASCII.Size = new System.Drawing.Size(958, 44);
            this.lstChargementASCII.TabIndex = 0;
            // 
            // grpAjouterMot
            // 
            this.grpAjouterMot.Controls.Add(this.txtNvMot);
            this.grpAjouterMot.Controls.Add(this.butAjouterUnMot);
            this.grpAjouterMot.Controls.Add(this.lstAjoutMot);
            this.grpAjouterMot.Location = new System.Drawing.Point(10, 583);
            this.grpAjouterMot.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.grpAjouterMot.Name = "grpAjouterMot";
            this.grpAjouterMot.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.grpAjouterMot.Size = new System.Drawing.Size(978, 226);
            this.grpAjouterMot.TabIndex = 2;
            this.grpAjouterMot.TabStop = false;
            this.grpAjouterMot.Text = "Ajouter un mot qui n\'existe pas";
            // 
            // txtNvMot
            // 
            this.txtNvMot.Location = new System.Drawing.Point(446, 37);
            this.txtNvMot.Name = "txtNvMot";
            this.txtNvMot.Size = new System.Drawing.Size(312, 26);
            this.txtNvMot.TabIndex = 2;
            // 
            // butAjouterUnMot
            // 
            this.butAjouterUnMot.Location = new System.Drawing.Point(198, 29);
            this.butAjouterUnMot.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.butAjouterUnMot.Name = "butAjouterUnMot";
            this.butAjouterUnMot.Size = new System.Drawing.Size(231, 35);
            this.butAjouterUnMot.TabIndex = 1;
            this.butAjouterUnMot.Text = "Ajouter le mot";
            this.butAjouterUnMot.UseVisualStyleBackColor = true;
            this.butAjouterUnMot.Click += new System.EventHandler(this.butAjouterUnMot_Click);
            // 
            // lstAjoutMot
            // 
            this.lstAjoutMot.FormattingEnabled = true;
            this.lstAjoutMot.ItemHeight = 20;
            this.lstAjoutMot.Location = new System.Drawing.Point(9, 74);
            this.lstAjoutMot.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.lstAjoutMot.Name = "lstAjoutMot";
            this.lstAjoutMot.Size = new System.Drawing.Size(958, 124);
            this.lstAjoutMot.TabIndex = 0;
            // 
            // grpLectureDAWG
            // 
            this.grpLectureDAWG.Controls.Add(this.butLectureDAWG);
            this.grpLectureDAWG.Controls.Add(this.lstLectureDAWG);
            this.grpLectureDAWG.Location = new System.Drawing.Point(10, 385);
            this.grpLectureDAWG.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.grpLectureDAWG.Name = "grpLectureDAWG";
            this.grpLectureDAWG.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.grpLectureDAWG.Size = new System.Drawing.Size(978, 189);
            this.grpLectureDAWG.TabIndex = 2;
            this.grpLectureDAWG.TabStop = false;
            this.grpLectureDAWG.Text = "Lecture du fichier compressé";
            // 
            // butLectureDAWG
            // 
            this.butLectureDAWG.Location = new System.Drawing.Point(198, 29);
            this.butLectureDAWG.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.butLectureDAWG.Name = "butLectureDAWG";
            this.butLectureDAWG.Size = new System.Drawing.Size(156, 35);
            this.butLectureDAWG.TabIndex = 1;
            this.butLectureDAWG.Text = "Lecture du fichier DAWG";
            this.butLectureDAWG.UseVisualStyleBackColor = true;
            this.butLectureDAWG.Click += new System.EventHandler(this.btnLectureDAWG_Click);
            // 
            // lstLectureDAWG
            // 
            this.lstLectureDAWG.FormattingEnabled = true;
            this.lstLectureDAWG.ItemHeight = 20;
            this.lstLectureDAWG.Location = new System.Drawing.Point(4, 74);
            this.lstLectureDAWG.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.lstLectureDAWG.Name = "lstLectureDAWG";
            this.lstLectureDAWG.Size = new System.Drawing.Size(962, 104);
            this.lstLectureDAWG.TabIndex = 0;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(998, 820);
            this.Controls.Add(this.grpLectureDAWG);
            this.Controls.Add(this.grpAjouterMot);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "Form1";
            this.Text = "Test de création et d\'utilisation d\'un DAWG";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.grpAjouterMot.ResumeLayout(false);
            this.grpAjouterMot.PerformLayout();
            this.grpLectureDAWG.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ProgressBar pgb2etapes;
        private System.Windows.Forms.ListBox lstAvancement2Etapes;
        private System.Windows.Forms.Button btnCreationDawg;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ListBox lstChargementASCII;
        private System.Windows.Forms.GroupBox grpAjouterMot;
        private System.Windows.Forms.Button butAjouterUnMot;
        private System.Windows.Forms.ListBox lstAjoutMot;
        private System.Windows.Forms.GroupBox grpLectureDAWG;
        private System.Windows.Forms.Button butLectureDAWG;
        private System.Windows.Forms.ListBox lstLectureDAWG;
        private System.Windows.Forms.TextBox txtNvMot;
        private System.Windows.Forms.Button brtnSortTxtDico;
        private System.Windows.Forms.Button btnReadDicoText;
    }
}

