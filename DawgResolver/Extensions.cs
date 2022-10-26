using System;

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
      
        //public static List<Letter> RemoveFromRack(this List<Letter> rack, Letter l)
        //{
        //    var ret = rack;
        //    var idxLetter = rack.Select(ra => ra.Char).ToList().IndexOf(l.Char);
        //    ret.RemoveAt(idxLetter);
        //    return ret;
        //}
    }
}
