using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
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
            IsPlayedByPlayer1 = Game.DefaultInstance.IsPlayer1;
        }

        public int SetText(string text = "", bool validate = true)
        {
            if (text == "") text = this.Text;
            this.StartTile.SetWord(text, Direction, validate);
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


        public string Serialize => $"{StartTile.Ligne};{StartTile.Col};{Text};{Points};{Direction};{(Scramble ? "*" : "-")};{Index}" + Environment.NewLine;


        public bool IsAllowed => Dictionnaire.DefaultInstance.MotAdmis(Text);

        public List<IExtendedTile> GetTiles()
        {
            var ret = new List<IExtendedTile>();
            IExtendedTile tile = StartTile;
            //if (StartTile.Col == Game.DefaultInstance.BoardCenter)
            //    tile = StartTile.LeftTile.RightTile;
            //else
            //    tile = StartTile.RightTile.LeftTile;

            ret.Add(tile);
            for (int i = 1; i < Text.Length; i++)
            {
                if (tile != null)
                    if (Direction == MovementDirection.Across)
                    {
                        tile = tile.RightTile;
                    }
                    else
                    {
                        tile = tile.DownTile;
                    }
                if (tile != null)
                    ret.Add(tile);
            }
            return ret;
        }
        public bool Equals(Word w) => w.Direction == Direction && w.StartTile.Col == StartTile.Col && w.StartTile.Ligne == StartTile.Ligne && w.Text == Text;

        public override string ToString()
        {
            var pos = $"{Solver.DefaultInstance.Alphabet[StartTile.Ligne]}{StartTile.Col + 1}";
            if (Direction == MovementDirection.Down)
                pos = $"{StartTile.Col + 1}{Solver.DefaultInstance.Alphabet[StartTile.Ligne]}";
            return $"[{pos}] {Text} ({Points}){(Scramble ? "*" : "")}";
        }


    }
}
