namespace Dawg.Scrabble.Model.Models
{
    public interface ITileLetterPosition
    {
        int Ligne { get; }
        int Col { get; }
        Letter Letter { get; set; }
    }

    public class TileLetterPosition : ITileLetterPosition
    {
        public TileLetterPosition(int ligne, int col)
        {
            Ligne = ligne;
            Col = col;
        }

        public int Ligne { get; }

        public int Col { get; }

        public Letter Letter { get; set; }
    }

}