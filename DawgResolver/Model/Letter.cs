using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace DawgResolver.Model
{

    public class Letter : ICloneable
    {
        private char @char;
        private int defaultCount = -1;

        public Letter()
        {
            Char = Game.EmptyChar;
            Value = 0;
            Count = 0;

        }
        public int DefaultCount { get; set; } = -1;

        public string Serialize
        {
            get
            {
                return $"L{Char};{Value};{Count}";
            }
        }
       


        public Letter(char @char, int value, int count)
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
            return new Letter(this.Char, this.Value, this.DefaultCount);

        }
    }
}