using Scrabble.Core.Words;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Scrabble.Core.Tile
{
    public class TileManager
    {
        public ScrabbleForm ScrabbleForm { get; set; }
        public TileBag TileBag { get; set; }

        public ScrabbleTile[,] Tiles;

        public TileManager(ScrabbleForm scrabbleForm)
        {
            ScrabbleForm = scrabbleForm;
        }

        /// <summary>
        /// Initialize the tiles.
        /// </summary>
        public void SetupTiles()
        {
            Tiles = new ScrabbleTile[ScrabbleForm.BOARD_WIDTH, ScrabbleForm.BOARD_HEIGHT];
            TileBag = new TileBag();
            var specialTilePositions = ScrabbleForm.WordScorer.GetTileTypes();

            for (int x = 1; x <= ScrabbleForm.BOARD_WIDTH; x++)
            {
                for (int y = 1; y <= ScrabbleForm.BOARD_HEIGHT; y++)
                {
                    var tile = new ScrabbleTile(ScrabbleForm)
                    {
                        XLoc = x - 1,
                        YLoc = y - 1
                    };
                    tile.BackColor = SystemColors.ButtonFace;
                    tile.Location = new Point(x * (ScrabbleForm.TILE_SIZE + 2), y * (ScrabbleForm.TILE_SIZE + 2));
                    tile.Size = new Size(ScrabbleForm.TILE_SIZE, ScrabbleForm.TILE_SIZE);
                    //tile.UseVisualStyleBackColor = false;
                    tile.Font = new Font("Verdana", 25.75F, FontStyle.Regular);
                    tile.Click += Tile_Click;
                    tile.DragDrop += Tile_DragDrop;
                    tile.TileType = specialTilePositions[x - 1, y - 1];
                    tile.SetRegularBackgroundColour();
                    ScrabbleForm.Controls.Add(tile);


                    Tiles[x - 1, y - 1] = tile;
                }
            }
        }

        private void Tile_DragDrop(object sender, DragEventArgs e)
        {
            var tile = (ScrabbleTile)sender;
            var letter = e.Data.GetData(DataFormats.Text).ToString();
            tile.OnLetterPlaced(letter);
            var rackTile = ScrabbleForm.PlayerManager.CurrentPlayer.Tiles.Find(r => r.Text == letter);
            if (rackTile != null)
                rackTile.Text = "";

        }

        /// <summary>
        /// Reset the tiles which are flagged as 'in play' after a players turn.
        /// </summary>
        public void ResetTilesInPlay()
        {
            for (int x = 0; x < ScrabbleForm.BOARD_WIDTH; x++)
            {
                for (int y = 0; y < ScrabbleForm.BOARD_HEIGHT; y++)
                {
                    Tiles[x, y].TileInPlay = false;
                    Tiles[x, y].SetRegularBackgroundColour();
                }
            }

            ClearTileHighlights();
        }

        /// <summary>
        /// Reset any tile highlighting on the board
        /// </summary>
        public void ClearTileHighlights()
        {
            for (int x = 0; x < ScrabbleForm.BOARD_WIDTH; x++)
            {
                for (int y = 0; y < ScrabbleForm.BOARD_HEIGHT; y++)
                {
                    Tiles[x, y].ClearHighlight();
                }
            }

            ScrabbleForm.btnPlay.Text = "Play";
        }

        /// <summary>
        /// Return the direction the player is moving on the board with their tiles. 
        /// Either across, down or none.
        /// </summary>
        /// <returns></returns>
        public MovementDirection GetMovementDirection()
        {
            var tilesInPlay = new List<ScrabbleTile>();

            for (int x = 0; x < ScrabbleForm.BOARD_WIDTH; x++)
            {
                for (int y = 0; y < ScrabbleForm.BOARD_HEIGHT; y++)
                {
                    if (Tiles[x, y].TileInPlay)
                        tilesInPlay.Add(Tiles[x, y]);
                }
            }

            // No direction because less than 2 tiles have been played
            if (tilesInPlay.Count <= 1)
                return MovementDirection.None;

            int xChange = tilesInPlay[1].XLoc - tilesInPlay[0].XLoc;
            int yChange = tilesInPlay[1].YLoc - tilesInPlay[0].YLoc;

            return xChange > 0 ? MovementDirection.Across : yChange > 0 ? MovementDirection.Down : MovementDirection.None;
        }

        /// <summary>
        /// Ensure that where the tiles have been placed on the board are valid locations.
        /// </summary>
        /// <returns></returns>
        public bool ValidateTilePositions()
        {
            var tilesInPlay = new List<ScrabbleTile>();

            if (ScrabbleForm.StatManager.Moves == 0)
            {
                // On the first move the centre tile needs to be in play.
                if (!Tiles[(int)ScrabbleForm.BOARD_WIDTH / 2, (int)ScrabbleForm.BOARD_HEIGHT / 2].TileInPlay)
                {
                    MessageBox.Show("One of your letters must be on the centre tile for the first move.");
                    return false;
                }
            }
            else
            {
                // Evey move other than the first one, at least one of your tiles needs to be touching an existing tile.
                if (!ValidateATileIsAdjacent())
                {
                    MessageBox.Show("Invalid letter positioning. At least one of your tiles must be adjacent to existing letters");
                    return false;
                }
            }

            // Grab all the tiles in play in this turn.
            for (int x = 0; x < ScrabbleForm.BOARD_WIDTH; x++)
            {
                for (int y = 0; y < ScrabbleForm.BOARD_HEIGHT; y++)
                {
                    if (Tiles[x, y].TileInPlay)
                        tilesInPlay.Add(Tiles[x, y]);
                }
            }

            var direction = GetMovementDirection();

            // Only one tile in play and/or we aren't moving in any direction so the move should be valid.
            if (direction == MovementDirection.None || tilesInPlay.Count() <= 1)
                return true;

            // For every tile in play, ensure the player hasn't tried to move in two directions at once.
            for (int x = 1; x < tilesInPlay.Count; x++)
            {
                int xChange = tilesInPlay[x - 1].XLoc - tilesInPlay[x].XLoc;
                int yChange = tilesInPlay[x - 1].YLoc - tilesInPlay[x].YLoc;

                if (direction == MovementDirection.Across)
                {
                    if (yChange != 0)
                        return false;
                }

                if (direction == MovementDirection.Down)
                {
                    if (xChange != 0)
                        return false;
                }
            }

            int xLoc = tilesInPlay[0].XLoc;
            int yLoc = tilesInPlay[0].YLoc;
            int lastX = tilesInPlay[tilesInPlay.Count() - 1].XLoc;
            int lastY = tilesInPlay[tilesInPlay.Count() - 1].YLoc;

            // Ensure that there are no gaps inbetween the tile placements even in the same direction
            if (direction == MovementDirection.Across)
            {
                for (int x = xLoc; x <= lastX; x++)
                {
                    if (!Tiles[x, yLoc].TileInPlay && string.IsNullOrEmpty(Tiles[x, yLoc].Text))
                        return false;
                }
            }
            if (direction == MovementDirection.Down)
            {
                for (int y = yLoc; y <= lastY; y++)
                {
                    if (!Tiles[xLoc, y].TileInPlay && string.IsNullOrEmpty(Tiles[xLoc, y].Text))
                        return false;
                }
            }

            return true;
        }

        /// <summary>
        /// For all the tiles in play, validate that at least one is adjacent to an existing tile on the board.
        /// </summary>
        /// <returns></returns>
        public bool ValidateATileIsAdjacent()
        {
            var adjacentTiles = new List<ScrabbleTile>();

            for (int x = 0; x < ScrabbleForm.BOARD_WIDTH; x++)
            {
                for (int y = 0; y < ScrabbleForm.BOARD_HEIGHT; y++)
                {
                    if (Tiles[x, y].TileInPlay)
                    {
                        if (x > 0)
                            adjacentTiles.Add(Tiles[x - 1, y]);

                        if (x < ScrabbleForm.BOARD_WIDTH - 1)
                            adjacentTiles.Add(Tiles[x + 1, y]);

                        if (y > 0)
                            adjacentTiles.Add(Tiles[x, y - 1]);

                        if (y < ScrabbleForm.BOARD_HEIGHT - 1)
                            adjacentTiles.Add(Tiles[x, y + 1]);
                    }
                }
            }

            return adjacentTiles.Any(t => !t.TileInPlay && !string.IsNullOrEmpty(t.Text));
        }

        /// <summary>
        /// Resets any tiles that the user had attempted to put down on the board.
        /// This should be called to clean up the board before allowing the user to do actions such
        /// as swap their letters or pass their turn.
        /// </summary>
        public void ResetTilesOnBoardFromTurn()
        {
            // Reset any tiles the player had put on the board
            for (int x = 0; x < ScrabbleForm.BOARD_WIDTH; x++)
            {
                for (int y = 0; y < ScrabbleForm.BOARD_HEIGHT; y++)
                {
                    if (Tiles[x, y].TileInPlay)
                        TileClick(Tiles[x, y]);
                }
            }
        }

        /// <summary>
        /// Handles clicking on a tile on the game board. If the player has previously selected
        /// a letter, this will place that selected letter down.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Tile_Click(object sender, EventArgs e)
        {
            if (!ScrabbleForm.GamePlaying) return;

            var tile = (ScrabbleTile)sender;
            TileClick(tile);
        }

        private void TileClick(ScrabbleTile tile)
        {
            ClearTileHighlights();

            Console.WriteLine("Clicking a tile!");

            // Clicked on a tile that they have just put down so move it back to the rack.
            if (tile.TileInPlay)
            {
                if (tile.Text == "") tile.TileInPlay = false;

                foreach (var t in ScrabbleForm.PlayerManager.CurrentPlayer.Tiles)
                {
                    if (t.Letter.ToString() == tile.Text && string.IsNullOrEmpty(t.Text))
                    {
                        // Put the tile back in the rack
                        t.Text = tile.Text;

                        // Reset the scrabble tile
                        tile.OnLetterRemoved();

                        // Highlight on the remaining tiles if there are any valid words.
                        var mr = ScrabbleForm.WordValidator.ValidateWordsInPlay();
                        ScrabbleForm.btnPlay.Text = $"Play {mr.TotalScore} pts";
                        return;
                    }
                }
            }

            // Tile already in use, can't move there
            if (!string.IsNullOrEmpty(tile.Text))
                return;

            // All is good - handle placing the tile on the board from the rack.
            foreach (var t in ScrabbleForm.PlayerManager.CurrentPlayer.Tiles)
            {
                if (t.LetterSelected)
                {
                    tile.OnLetterPlaced(t.Letter.ToString());

                    t.ClearDisplay();
                    break;
                }
            }

            var moveResult = ScrabbleForm.WordValidator.ValidateWordsInPlay();
            ScrabbleForm.btnPlay.Text = $"Play {moveResult.TotalScore} pts";
        }
    }
}
