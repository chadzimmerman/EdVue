using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WguMauiMobileApplication
{
    public class StatusToColorConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value == null)
                return null;

            string status = value.ToString() ?? string.Empty;

            return status switch
            {
                "Open" => Colors.Green,
                "InProgress" => Colors.Orange,
                "Closed" => Colors.Red,
                _ => Colors.Gray
            };
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            // Usually not needed, just throw or return null
            throw new NotImplementedException();
        }
    }

}
