using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

using DawgResolver.Model;

namespace DawgResolver
{
    public static class Extensions
    {
        public static string ReverseString(this string s)
        {
            char[] array = s.ToCharArray();
            Array.Reverse(array);
            return new string(array);
        }
        //public static IList<T> Clone<T>(this IList<T> listToClone) where T : ICloneable
        //{
        //    return listToClone.Select(item => (T)item.Clone()).ToList();
        //}
        //public static T DeepClone<T>(this T obj)
        //{
        //    using (var ms = new MemoryStream())
        //    {
        //        var formatter = new BinaryFormatter();
        //        formatter.Serialize(ms, obj);
        //        ms.Position = 0;

        //        return (T)formatter.Deserialize(ms);
        //    }
        //}

        //public static Letter GetRandomLetter(this List<Letter> letters)
        //{
        //    Random rng = new Random();
        //    int index = rng.Next(letters.Where(l => l.Count > 0).Count());
        //    letters[index].Count--;
        //    return letters[index];
        //}


        //public static int FindIndex(this char c, Resolver r)
        //{
        //    return r.Alphabet.Where(t => t.Char == c).Select((Value, Index) => Index).First();

        //}

        // extension for arrays, lists, any Enumerable -> AsString
        //public static string AsString<T>(this IEnumerable<T> enumerable)
        //{
        //    var sb = new StringBuilder();
        //    enumerable.Select((idx, itm) => sb.Append($"{idx}: {itm}\r\n"));
        //    return sb.ToString();
        //}
        public static void RemoveLetterByIndex(this List<Letter> letters, Letter letterToRemove)
        {
            var charIndex= letters.IndexOf(letterToRemove);
            if (charIndex == -1) return;
            letters.RemoveAt(charIndex);
        }
        public static Letter FindLetterByIndex(this BindingList<Letter> letters, Letter letter)
        {
            var charIndex = letters.IndexOf(letter);
            if (charIndex == -1) return null;
            return letters[charIndex];
        }
        public static Letter FindLetterByChar(this BindingList<Letter> letters, Char c)
        {
            var foundLetter = letters.FirstOrDefault(l => l.Char == c);
          return FindLetterByIndex(letters, foundLetter);
        }


        public static Letter DeserializeLetter(this string s, Resolver r)
        {
            var l = s.Split(';');
            return new Letter(r, l[0].Skip(1).First(), int.Parse(l[1]), int.Parse(l[2]));
        }
        public static IExtendedTile DeserializeTile(this string s, Resolver r)
        {
            var l = s.Split(';');
            bool? isPlayedByPlayer1 = null;
            if (l.Count() > 7 && l[7] != "") isPlayedByPlayer1 = bool.Parse(l[7]);

            IExtendedTile t = new BaseVirtualTile(r, int.Parse(l[0].Substring(1)), int.Parse(l[1]), isPlayedByPlayer1, bool.Parse(l[4]))
            {
                LetterMultiplier = int.Parse(l[2]),
                WordMultiplier = int.Parse(l[3]),
                IsValidated = bool.Parse(l[5]),
            };
            if (l.Count() > 6 && l[6] != "") t.Letter = r.Find(l[6][0]);

            return t;
        }
        public static Word DeserializeMove(this string s, Game g)
        {
            var l = s.Split(';');
            return new Word(g)
            {
                StartTile = new BaseVirtualTile(g.Resolver, int.Parse(l[0].Substring(2)), int.Parse(l[1])),
                Text = l[2],
                Points = int.Parse(l[3]),
                Direction = (MovementDirection)Enum.Parse(typeof(MovementDirection), l[4]),
                Scramble = l[5].First() == '*',
                Index = l.Length > 6 ? int.Parse(l[6]) : 0
            };
        }

        public static string ToCharString(this List<Letter> lst)
        {
            var ret = "";
            for (int i = 0; i < lst.Count(); i++)
            {
                ret += lst[i].Char;
            }
            return ret;
        }

        //public static List<Letter> RemoveFromRack(this List<Letter> rack, Letter l)
        //{
        //    var ret = rack;
        //    var idxLetter = rack.Select(ra => ra.Char).ToList().IndexOf(l.Char);
        //    ret.RemoveAt(idxLetter);
        //    return ret;
        //}
    }
}
