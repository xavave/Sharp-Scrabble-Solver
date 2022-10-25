using System.Collections.Generic;

namespace Dawg.Scrabble.Model.Models
{
    public interface IExtendedTile : IBaseTile
    {
        bool? IsPlayedByPlayer1 { get; }
        string Serialize { get; }
        Word GetWordFromTile(Game game, MovementDirection currentWordDirection);
        void SetWord(Game game, string text, MovementDirection direction, bool validate);
        void CopyControllers(IDictionary<int, int> controlers);
        IExtendedTile Copy(Resolver r, bool transpose = false);
        void SetTilePos(int ligne, int col);
    }

}