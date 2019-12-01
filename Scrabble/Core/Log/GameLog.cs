using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Scrabble.Core.Log
{
    public class GameLog : TextBox
    {
        public ScrabbleForm ScrabbleForm { get; set; }

        public GameLog(ScrabbleForm _scrabbleForm)
        {
            this.Enabled = false;
            this.Location = new Point(836, 160);
            this.Multiline = true;
            this.Name = "logTxtBox";
            this.ScrollBars = ScrollBars.Both;
            this.Font = new Font("Verdana", 12.75F, FontStyle.Regular);
            this.Size = new Size(327, 335);
            this.TabIndex = 3;
            this.ScrabbleForm = _scrabbleForm;

            LogMessage("Welcome to Scrabble - good luck!");
            ScrabbleForm.Controls.Add(this);
        }

        public void LogMessage(string message)
        {
            this.AppendText(message);
            this.AppendText(Environment.NewLine);
        }
    }
   
}
