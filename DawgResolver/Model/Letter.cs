using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace DawgResolver.Model
{

    public class Letter : ICloneable
    {
        private char @char;
        private int defaultCount = -1;

        public Letter()
        {
            Char = char.MinValue;
            Value = 0;
            Count = 0;

        }
        public int DefaultCount { get; set; } = -1;


        public Letter(char @char, int value, int count)
        {
            Char = @char;
            Value = value;
            Count = count;
            if (DefaultCount == -1) DefaultCount = count;
        }
        public bool HasValue()
        {
            return this != null && this.Char != char.MinValue;
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