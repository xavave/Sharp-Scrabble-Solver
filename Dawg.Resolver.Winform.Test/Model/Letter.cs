﻿using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

using Dawg;
using Dawg.Solver.Winform;

namespace DawgSolver.Model
{
    public class Letter : ICloneable, INotifyPropertyChanged
    {
        private char @char;
        private int count;

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string caller = null)
        {
            // make sure only to call this if the value actually changes

            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(caller));
            }
        }
        public LetterType LetterType { get; set; }
        public Letter()
        {
            InitLetter(Game.EmptyChar, 0, 0, -1, LetterType.Regular);

        }
        public int DefaultCount { get; set; } = -1;

        public string Serialize
        {
            get
            {
                return $"L{Char};{Value};{Count}";
            }
        }
        public Letter(char mychar, int value, int count) : this()
        {
            InitLetter(mychar, value, count);
        }
        public void CopyFromOtherLetter(Letter other)
        {
            InitLetter(other.Char, other.Value, other.Count, other.DefaultCount, other.LetterType);
        }
        public void InitLetter(char mychar, int value, int count, int? defaultCount = null, LetterType letterType = LetterType.Regular)
        {
            Char = mychar;
            Value = value;
            Count = count;
            DefaultCount = defaultCount ?? count;
            LetterType = letterType;
        }

        public bool HasValue()
        {
            return this != null && this.Char != Game.EmptyChar;
        }
        public string GetLetterByIndex(int index)
        {
            return Solver.DefaultInstance.Find((char)(index + Dictionnaire.AscShift)).Char.ToString();
        }

        public char Char
        {
            get => @char; set { @char = value; }
        }
        public int Value { get; set; }
        public int Count
        {
            get => count; set
            {
                count = value;
                OnPropertyChanged();
            }
        }

        public override string ToString()
        {
            return $"{(HasValue() ? Char.ToString() : "")}";
        }

        public object Clone()
        {
            return new Letter(this.Char, this.Value, this.DefaultCount);

        }

    }
}