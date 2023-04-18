using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using ProServ.models;

namespace ProServ.Views
{
    public class ZoneToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int zoneID = (int)value;

            return GlobalAccess.globalAccess.GetZoneHexByID(zoneID);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class StatusToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int state = (int)value;
            switch (state)
            {
                case 0:
                    return "Green";
                case 1:
                    return "Red";
                case 2:
                    return "Yellow";

                default:
                    return "Red";

            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}
