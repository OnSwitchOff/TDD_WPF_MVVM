using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace TDD_WPF_MVVM.Converters
{
    public class BooleanToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var IsVisible = value as bool?;
            if (IsVisible.HasValue && IsVisible.Value)
            {
                return Visibility.Visible;
            }
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var visiblility = value as Visibility?;
            if (visiblility.HasValue && visiblility.Value == Visibility.Visible)
            {
                return true;
            }
            return false;
        }
    }
}
