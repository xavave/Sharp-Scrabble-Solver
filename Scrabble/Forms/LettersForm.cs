using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Scrabble.Forms
{
    public partial class LettersForm : Form
    {
        public ScrabbleForm ScrabbleForm { get; set; }

        public LettersForm()
        {
            InitializeComponent();
        }

        public void BindLetters()
        {
            lblLetters.Text = $"Letters Remaining: {ScrabbleForm.TileManager.TileBag.LetterCountRemaining()}";

            var table = new DataTable();
            table.Columns.Add("Letter");
            table.Columns.Add("Remaining");
            table.Columns.Add("StartedWith");

            for (var x = 'A'; x <= 'Z'; x++)
            {
                var row = table.NewRow();
                row["Letter"] = x;
                row["Remaining"] = ScrabbleForm.TileManager.TileBag.Letters.Where(l => l == x).Count();
                row["StartedWith"] = ScrabbleForm.TileManager.TileBag.LetterCount(x);

                table.Rows.Add(row);
            }

            gridLetters.DataSource = table;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
