using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

using DawgResolver.Model;

namespace Dawg.Solver.Winform
{
    public class CustomExtendedTilesGridBuilder
    {
        private bool Init { get; }
        public CustomExtendedTilesGridBuilder( bool init)
        {
            Init = init;
        
        }
        public CustomExtendedTilesGrid Build()
        {
            var grid = new CustomExtendedTilesGrid(Game.DefaultInstance.BoardSize, true);
            if (Init) grid.InitGameBoardTiles();
            return grid;
        }
    }
    public class CustomExtendedTilesGrid : CustomGrid<IExtendedTile>
    {
        private static CustomExtendedTilesGrid instance = null;
        public static CustomExtendedTilesGrid Instance => instance ?? (instance = new CustomExtendedTilesGridBuilder(true).Build());
        public override void SetArrayTile(int ligne, int colonne)
        {
            if (Tile != null)
            {
                Tile.Col = colonne;
                Tile.Ligne = ligne;
            }
            base.SetArrayTile(ligne, colonne);
        }

        public CustomExtendedTilesGrid(int boardSize, bool initialize, IExtendedTile defaultTile = default(IExtendedTile)) : base(boardSize, defaultTile, initialize)
        {

        }

        public void InitGameBoardTiles()
        {
            // Définition des cases bonus
            var assembly = Assembly.GetExecutingAssembly();

            string resourceName = assembly.GetManifestResourceNames().SingleOrDefault(str => str.EndsWith($"initial_board{Game.DefaultInstance.BoardSize}{Game.DefaultInstance.Solver.Mode}.txt"));
            if (string.IsNullOrWhiteSpace(resourceName)) return;
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream, true))
            {
                string content = reader.ReadToEnd();

                int row = 0;
                int col = 0;
                foreach (var w in content.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries))
                {
                    foreach (var tp in w.Trim().Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        if (this[row, col] == null)
                            this[row, col] = new FormTile(row, col);
                        else
                        {

                        }

                        if (string.IsNullOrEmpty(tp))
                            continue;
                        else


                            switch (tp.Trim())
                            {
                                case "RE":
                                case "__":
                                    this[row, col].WordMultiplier = 1;
                                    this[row, col].LetterMultiplier = 1;
                                    break;
                                case "CE":
                                    this[row, col].WordMultiplier = 1;
                                    this[row, col].LetterMultiplier = 1;
                                    break;
                                case "TW":
                                case "3W":
                                    this[row, col].WordMultiplier = 3;
                                    this[row, col].LetterMultiplier = 1;
                                    break;
                                case "TL":
                                case "3L":
                                    this[row, col].LetterMultiplier = 3;
                                    this[row, col].WordMultiplier = 1;
                                    break;
                                case "DW":
                                case "2W":
                                    this[row, col].WordMultiplier = 2;
                                    this[row, col].LetterMultiplier = 1;
                                    break;
                                case "DL":
                                case "2L":
                                    this[row, col].LetterMultiplier = 2;
                                    this[row, col].WordMultiplier = 1;
                                    break;
                                default:
                                    throw new Exception($"Unknown tile type in inital_board file: {tp}");
                            }
                        col += 1;
                    }

                    col = 0;
                    row += 1;
                }

                ////'Initialisation du contenu du sac d'où sont tirées les lettres
                //for (int nl = 0; nl < 27; nl++)
                //{
                //    BagContent[nl] = LettersCount[nl];
                //    //LetterValue[nl] = LetterPoints[nl];
                //}
                //Afficher_Contenu_Sac
            }
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
