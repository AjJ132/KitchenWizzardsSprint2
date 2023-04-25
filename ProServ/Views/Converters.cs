using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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

            List<String> newItems = items.Select(p => p.itemName).ToList();
            return newItems;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class EmployeeIdToPasswordConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values == null || values.Length != 2 || values[0] == DependencyProperty.UnsetValue || values[1] == DependencyProperty.UnsetValue)
            {
                return Binding.DoNothing;
            }

            int employeeId = (int)values[0];
            ObservableCollection<LoginCredentials> loginCredentials = (ObservableCollection<LoginCredentials>)values[1];

            var matchingCredential = loginCredentials.FirstOrDefault(x => x.userId == employeeId);
            return matchingCredential?.password ?? string.Empty;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

}
