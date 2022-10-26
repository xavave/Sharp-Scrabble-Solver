using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using Dawg;
using Dawg.Solver.Winform;

namespace DawgResolver.Model
{
    public enum MovementDirection
    {
        Across, Down, None
    };
    public class Word
    {
        public bool IsPlayedByPlayer1 { get; }
        public int Index { get; set; }
        public bool Scramble { get; set; }
        public MovementDirection Direction { get; set; }
        public IExtendedTile StartTile { get; set; }
        public int Points { get; set; }
        public string Text { get; set; }
        public Word()
        {
            IsPlayedByPlayer1= Game.DefaultInstance.IsPlayer1;
        }

        public int SetWord(bool validate)
        {
            this.StartTile.SetWord(Text, Direction, validate);
            return this.Points;
        }
        public string ToCharString(List<Letter> lst)
        {
            var ret = "";
            for (int i = 0; i < lst.Count(); i++)
            {
                ret += lst[i].Char;
            }
            return ret;
        }
        public string DisplayInList(bool isPlayer1, List<Letter> lst)
        {
            return $"{DateTime.Now:H:mm:ss} - {$"Player {(isPlayer1 ? '1' : '2')}:{ToCharString(lst)}"} --> {this}";
        }
        public void ShowDefinition()
        {
            if (!string.IsNullOrWhiteSpace(this.Text))
                Process.Start($"https://1mot.net/{this.Text.ToLower()}");
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
                return Dictionnaire.DefaultInstance.MotAdmis(Text);
            }
        }
        public HashSet<IExtendedTile> GetTiles()
        {
            var ret = new HashSet<IExtendedTile>();
            IExtendedTile t = null;
            if (StartTile.Col == Game.DefaultInstance.BoardSize - 1)
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
            var pos = $"{Solver.DefaultInstance.Alphabet[StartTile.Ligne]}{StartTile.Col + 1}";
            if (Direction == MovementDirection.Down)
                pos = $"{StartTile.Col + 1}{Solver.DefaultInstance.Alphabet[StartTile.Ligne]}";
            return $"[{pos}] {Text} ({Points}){(Scramble ? "*" : "")}";
        }
        public bool Equals(Word w)
        {
            return w.Direction == Direction && w.StartTile.Col == StartTile.Col && w.StartTile.Ligne == StartTile.Ligne && w.Text == Text;
        }
    }
}
