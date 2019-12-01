using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Scrabble.Core
{
    public class RackTile : TextBox
    {
        public RackTile()
        {
            InitializeComponent();
            CharacterCasing = CharacterCasing.Upper;
        }

        public char Letter { get; set; }
        public int LetterValue { get; set; }
        public bool LetterSelected { get; set; }

        public void ClearDisplay()
        {
            this.LetterSelected = false;
            this.BorderStyle = BorderStyle.FixedSingle;
            this.Text = string.Empty;
        }

        public void OnLetterSelected()
        {
            this.BorderStyle = BorderStyle.Fixed3D;
            //this.FlatAppearance.BorderColor = Color.LimeGreen;
            //this.FlatAppearance.BorderSize = 5;
            this.LetterSelected = true;
        }

        public void OnLetterDeselected()
        {
            this.BorderStyle = BorderStyle.FixedSingle;
            this.LetterSelected = false;
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // RackTile
            // 
            this.AllowDrop = true;
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.RackTile_DragEnter);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.RackTile_MouseDown);
            this.ResumeLayout(false);

        }

        private void RackTile_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.All;
            else
            {
                String[] strGetFormats = e.Data.GetFormats();
                e.Effect = DragDropEffects.None;
            }
        }

        private void RackTile_MouseDown(object sender, MouseEventArgs e)
        {
            this.DoDragDrop(this.Text, DragDropEffects.Copy | DragDropEffects.Move);
        }
    }
}
