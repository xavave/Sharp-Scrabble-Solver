﻿using Dawg;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace DawgResolver
{
    public static class Extensions
    {

        public static Letter GetRandomLetter(this List<Letter> letters)
        {
            Random rng = new Random();
            int index = rng.Next(letters.Where(l => l.Count > 0).Count());
            letters[index].Count--;
            return letters[index];
        }
        public static string GetLetterByIndex(this List<Letter> l, int index)
        {
            return Game.AlphabetAvecJoker.Find(t => t.Char == (char)(index + Dictionnaire.AscShift)).Char.ToString();
        }
    }
}
