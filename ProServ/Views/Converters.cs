using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;
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
                    return new SolidColorBrush(Colors.Green);
                case 1:
                    return new SolidColorBrush(Colors.Yellow);
                case 2:
                    return new SolidColorBrush(Colors.Red);

                default:
                    return new SolidColorBrush(Colors.Black);

            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class ItemsToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var items = value as List<Item>;
            if (items == null) return string.Empty;

            return string.Join(", ", items.Select(x => x.itemName));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}
