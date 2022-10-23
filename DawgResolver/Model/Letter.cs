using System;

using Dawg;

namespace DawgResolver.Model
{

    public class Letter : ICloneable
    {
        private char @char;
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
        public int Count { get; set; }

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