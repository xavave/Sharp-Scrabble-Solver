namespace Dawg
{
    public class Letter
    {
        public Letter()
        {
        }

        public Letter(char @char, int value, int count)
        {
            Char = @char;
            Value = value;
            Count = count;
        }

        public char Char { get; set; }
        public int Value { get; set; }
        public int Count { get; set; }
    }
}