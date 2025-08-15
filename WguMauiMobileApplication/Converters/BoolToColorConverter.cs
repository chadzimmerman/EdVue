using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WguMauiMobileApplication.Converters
{
    public class BoolToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isOverviewSelected && parameter is string tabName)
            {

                if ((isOverviewSelected && tabName == "Overview") ||
                    (!isOverviewSelected && tabName == "Assessments"))
                {
                    return Colors.DodgerBlue; // Selected color
                }
            }
            return Colors.LightGray; // Default/unselected color?
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
