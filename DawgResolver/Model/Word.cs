using System;
using System.Collections.Generic;
using System.Linq;

using Dawg;

namespace DawgResolver.Model
{
    public enum MovementDirection
    {
        Across, Down, None
    };
    public class Word
    {
        Dictionnaire dico { get; }
        public bool IsPlayedByPlayer1 { get; set; }
        public int Index { get; set; }
        private Game game { get; }
        private Resolver resolver { get; }
        private Word(Resolver r)
        {
            resolver = r;
            dico = r.Dico;
        }

        public Word(Game g) : this(g.Resolver)
        {
            game = g;
            StartTile = new BaseVirtualTile(g.Resolver, (int)game.BoardSize / 2, (int)game.BoardSize / 2);
        }
        public int SetWord(bool validate)
        {
            this.StartTile.SetWord(game, Text, Direction, validate);
            return this.Points;
        }
        public bool Scramble { get; set; }

        public MovementDirection Direction { get; set; }
        public IExtendedTile StartTile { get; set; }
        public int Points
        {
            get; set;
        }
        public string Text { get; set; }

        public string DisplayInList(bool isPlayer1, List<Letter> lst)
        {
            return $"{DateTime.Now:H:mm:ss} - {$"Player {(isPlayer1 ? '1' : '2')}:{lst.ToCharString()}"} --> {this}";
        }
     

        public string Serialize
        {
            get
            {
                return $"{StartTile.Ligne};{StartTile.Col};{Text};{Points};{Direction};{(Scramble ? "*" : "-")};{Index}" + Environment.NewLine;
            }
        }

        public bool IsAllowed
        {
            get
            {
                return dico.MotAdmis(Text);
            }
        }
        public HashSet<IExtendedTile> GetTiles()
        {
            var ret = new HashSet<IExtendedTile>();
            IExtendedTile t = null;
            if (StartTile.Col == game.BoardSize - 1)
                t = StartTile.LeftTile.RightTile;
            else
                t = StartTile.RightTile.LeftTile;

            ret.Add(t);
            for (int i = 1; i < Text.Length; i++)
            {
                if (t != null)
                    if (Direction == MovementDirection.Across)
                    {
                        t = t.RightTile;
                    }
                    else
                    {
                        t = t.DownTile;
                    }
                if (t != null)
                    ret.Add(t);
            }
            return ret;
        }


        public override string ToString()
        {
            var pos = $"{resolver.Alphabet.GetLetterFromAlphabetByIndex(StartTile.Ligne)}{StartTile.Col + 1}";
            if (Direction == MovementDirection.Down)
                pos = $"{StartTile.Col + 1}{resolver.Alphabet.ElementAt(StartTile.Ligne)}";
            return $"[{pos}] {Text} ({Points}){(Scramble ? "*" : "")}";
        }
        public bool Equals(Word w)
        {
            return w.Direction == Direction && w.StartTile.Col == StartTile.Col && w.StartTile.Ligne == StartTile.Ligne && w.Text == Text;
        }
    }
}
