namespace DawgSolver.Model
{
    public interface IColor { }
    public struct TileColor : IColor
    {
        public TileColor(TileType tileType) : this()
        {
            switch (tileType)
            {
                case TileType.TripleWord: Name = "OrangeRed"; break;
                case TileType.DoubleWord: Name = "Coral"; break;
                case TileType.DoubleLetter: Name = "LightSkyBlue"; break;
                case TileType.TripleLetter: Name = "MediumBlue"; break;
                case TileType.Center: Name = "Gold"; break;
                default: Name = "Bisque"; break;
            }
        }
        public TileColor(LetterType letterType) : this()
        {
            switch (letterType)
            {
                case LetterType.Joker: Name = "Gold"; break;
                default:  break;
            }
        }
        public string Name { get; set; }
    }

}
