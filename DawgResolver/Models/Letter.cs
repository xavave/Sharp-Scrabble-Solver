using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Dawg.Scrabble.Model.Models
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
        public Resolver Resolver { get; }
        public LetterType LetterType { get; set; }
        public Letter(Resolver r)
        {
            Resolver = r;
            Char = Game.EmptyChar;
            Value = 0;
            Count = 0;
            LetterType = LetterType.Regular;

        }
        public int DefaultCount { get; set; } = -1;

        public string Serialize
        {
            get
            {
                return $"L{Char};{Value};{Count}";
            }
        }
        public Letter(Resolver r, char @char, int value, int count) : this(r)
        {
            Char = @char;
            Value = value;
            Count = count;
            if (DefaultCount == -1) DefaultCount = count;
        }
        public bool HasValue()
        {
            return this != null && this.Char != Game.EmptyChar;
        }
        public string GetLetterByIndex(int index)
        {
            return Resolver.Find((char)(index + Dictionnaire.AscShift)).Char.ToString();
        }

        public char Char
        {
            get => @char; set { @char = value; }
        }
        public int Value { get; set; }
        public int Count { get => count; set
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
            return new Letter(Resolver, this.Char, this.Value, this.DefaultCount);

        }

    }
}