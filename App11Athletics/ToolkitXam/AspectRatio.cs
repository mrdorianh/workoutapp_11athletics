using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ToolkitXam
{
    [TypeConverter(typeof(AspectRatioTypeConverter))]
    public struct AspectRatio
    {
        public AspectRatio(double value)
        {
            if (value < 0)
                throw new FormatException("AspectRatio value must be greater than 0, " +
                                          "or set to 0 to indicate Auto");
            Value = value;
        }

        public static AspectRatio Auto
        {
            get
            {
                return new AspectRatio();
            }
        }

        public double Value { private set; get; }

        public bool IsAuto { get { return Value == 0; } }

        public override string ToString()
        {
            return Value == 0 ? "Auto" : Value.ToString();
        }
    }
}
