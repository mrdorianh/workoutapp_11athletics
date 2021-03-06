using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ToolkitXam
{
    public class ThresholdToObjectConverter<T> : IValueConverter
    {
        public T TrueObject { set; get; }

        public T FalseObject { set; get; }

        public object Convert(object value, Type targetType,
                              object parameter, CultureInfo culture)
        {
            // Code assumes that all input is valid!
            double number = (double)value;
            string arg = parameter as string;
            char op = arg[0];
            double criterion = Double.Parse(arg.Substring(1).Trim());

            switch (op)
            {
                case '<': return number < criterion ? TrueObject : FalseObject;
                case '>': return number > criterion ? TrueObject : FalseObject;
                case '=': return number == criterion ? TrueObject : FalseObject;
            }
            return FalseObject;
        }

        public object ConvertBack(object value, Type targetType,
                                  object parameter, CultureInfo culture)
        {
            return 0;
        }
    }
}
