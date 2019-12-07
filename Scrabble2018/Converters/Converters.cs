using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Scrabble2018.Converters
{
    public class CharToLetterConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (char.IsLetter((char)value))
            {
                return DawgResolver.Model.Game.Alphabet.Find(c => c.Char == (char)value);
            }
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is bool)
            {
                if ((bool)value == true)
                    return "yes";
                else
                    return "no";
            }
            return "no";
        }
    }

}
