using System.Collections.Generic;

using Dawg.Solver.Winform;

namespace DawgResolver.Model
{

    public interface IBaseTile
    {
        string Name { get; set; }
        TileType TileType { get; }
        int Ligne { get; set; }
        int Col { get; set; }
        Letter Letter { get; }
        int LetterMultiplier { get; set; }
        int WordMultiplier { get; set; }
        int AnchorLeftMinLimit { get; set; }
        int AnchorLeftMaxLimit { get; set; }
        bool IsAnchor { get; }
        IDictionary<int, int> Controllers { get; }
        bool IsEmpty { get; }
        int WordIndex { get; set; }
        bool IsValidated { get; set; }
        IExtendedTile LeftTile { get; }
        IExtendedTile RightTile { get; }
        IExtendedTile UpTile { get; }
        IExtendedTile DownTile { get; }
        IExtendedTile WordMostRightTile { get; }
        IExtendedTile WordMostLeftTile { get; }
        IExtendedTile WordLowerTile { get; }
        IExtendedTile WordUpperTile { get; }
    }
    public interface IExtendedTile : IBaseTile
    {
        bool? IsPlayer1 { get; }
        string Serialize { get; }
        System.Drawing.Color BackColor { get; set; }

        Word GetWordFromTile(MovementDirection currentWordDirection);
        void SetWord(string text, MovementDirection direction, bool validate);
        void CopyControllers(IDictionary<int, int> controlers);
        IExtendedTile Copy(bool transpose = false);
        void SetBackColorFrom(IExtendedTile t);
        void SetBackColorFromInnerTile();
        void SetBackColorFromInnerLetterType();
        IExtendedTile FindFormTile(HashSet<IExtendedTile> boardTiles);
    }

}