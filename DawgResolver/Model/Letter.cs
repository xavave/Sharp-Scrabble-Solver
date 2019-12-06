using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace DawgResolver.Model
{
    public class Letter { 
        private char @char;

        public Letter()
        {
        }

        public Letter(char @char, int value, int count)
        {
            Char = @char;
            Value = value;
            Count = count;
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
    }
}