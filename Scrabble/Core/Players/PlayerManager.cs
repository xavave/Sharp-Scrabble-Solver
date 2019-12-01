using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scrabble.Core.Players
{
    public class PlayerManager
    {
        public ScrabbleForm ScrabbleForm { get; set; }
        public Player PlayerOne { get; set; }
        public Player PlayerTwo { get; set; }
        public Player CurrentPlayer { get; set; }

        /// <summary>
        /// Initalize the players.
        /// </summary>
        public void SetupPlayers()
        {
            this.PlayerOne = new Player { Name = "Player One", Score = 0, Tiles = ScrabbleForm.RackManager.SetupRack() };
            this.PlayerTwo = new Player { Name = "Player Two", Score = 0, Tiles = ScrabbleForm.RackManager.SetupRack() };

            SwapCurrentPlayer();
        }

        /// <summary>
        /// Swap the currently active player.
        /// </summary>
        public void SwapCurrentPlayer()
        {
            if (CurrentPlayer == null)
            {
                CurrentPlayer = PlayerOne;
                ScrabbleForm.lblPlayerOne.Font= new System.Drawing.Font(ScrabbleForm.lblPlayerOne.Font, System.Drawing.FontStyle.Bold);
            }
            else
            {
                CurrentPlayer.Tiles.ForEach(t => t.Visible = false);
                CurrentPlayer = CurrentPlayer == PlayerOne ? PlayerTwo : PlayerOne;
                if (CurrentPlayer== PlayerOne)
                {
                    ScrabbleForm.lblPlayerOne.Font = new System.Drawing.Font(ScrabbleForm.lblPlayerOne.Font, System.Drawing.FontStyle.Bold);
                    ScrabbleForm.lblPlayerOne.Font = new System.Drawing.Font(ScrabbleForm.lblPlayerOne.Font, System.Drawing.FontStyle.Regular);
                }
                else
                {
                    ScrabbleForm.lblPlayerOne.Font = new System.Drawing.Font(ScrabbleForm.lblPlayerOne.Font, System.Drawing.FontStyle.Regular);
                    ScrabbleForm.lblPlayerOne.Font = new System.Drawing.Font(ScrabbleForm.lblPlayerOne.Font, System.Drawing.FontStyle.Bold);
                }
            }

            ScrabbleForm.lblCurrentTurn.Text = $"It's {CurrentPlayer.Name}'s turn";
            ScrabbleForm.lblPlayerOne.Text = $"{PlayerOne.Name}: {PlayerOne.Score}";
            ScrabbleForm.lblPlayerTwo.Text = $"{PlayerTwo.Name}: {PlayerTwo.Score}";
            CurrentPlayer.Tiles.ForEach(t => t.Visible = true);
        }
    }
}
