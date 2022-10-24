using System.Collections;
using System.Collections.Generic;

namespace DawgResolver.Model
{
    public class CustomExtendedTilesGrid : CustomGrid<IExtendedTile>
    {
        public override void SetArrayTile(int ligne, int colonne)
        {
            if (Tile != null)
            {
                Tile.Col = colonne;
                Tile.Ligne = ligne;
            }
            base.SetArrayTile(ligne, colonne);
        }

        public CustomExtendedTilesGrid(Game g, int boardSize, bool initialize, IExtendedTile defaultTile=default(IExtendedTile)) : base(boardSize, defaultTile, initialize)
        {
        }

    }

    public class CustomGrid<T> : IEnumerable<T>
    {
        public T[,] Array;
        public T Tile;
        private bool _initialized;
        private int _ligne;
        private int _col;
        public T this[int ligne, int col]
        {
            get => Array[ligne, col];
            set => Array[ligne, col] = value;
        }
        public int GetLength(int dimension) => Array.GetLength(dimension);
        public CustomGrid(int size, T value, bool initialize)
        {
            _ligne = size;
            _col = size;
            Tile = value;
            if (initialize)
            {
                InitializeArray();
            }
        }
        public void InitializeArray()
        {
            int iLigne = _ligne;
            int iCol = _col;
            Array = new T[iLigne, iCol];
            for (int x = 0; x < iLigne; x++)
            {
                for (int y = 0; y < iCol; y++)
                {
                    SetArrayTile(x, y);
                }
            }
            _initialized = true;
        }

        public virtual void SetArrayTile(int x, int y)
        {
            Array[x, y] = Tile;
        }

        public T[,] CreateArray()
        {
            if (!_initialized)
            {
                InitializeArray();
            }
            return Array;
        }

        public IEnumerator<T> GetEnumerator()
        {
            foreach (var item in Array)
            {
                yield return item;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Array.GetEnumerator();
        }

    }
}
