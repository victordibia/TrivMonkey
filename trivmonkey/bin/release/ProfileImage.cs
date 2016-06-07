using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace TrivMonkey
{
    public class ProfileImage : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
            {
                return "img/holdere.jpg";
            }

            return "/img/" + (String)value;
            //return "ms-appdata:///local/DataFolder" + (String)value;

        }

        public object ConvertBack(object value, Type targetType,
            object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }

    }

    public class LockImage : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (((String)value).Equals("0"))
            {
                return "img/locked.png";
            }
            else {
                return "img/check.png";
            }

           // return "/img/" + (String)value;
            //return "ms-appdata:///local/DataFolder" + (String)value;

        }

        public object ConvertBack(object value, Type targetType,
            object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }

    }

    public class TimesPlayedConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            

            if (value == null)
            {
                return "played 0 times";
            }

            return "played " + (String)value + " times";

        }

        public object ConvertBack(object value, Type targetType,
            object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }

    }

    public class PlayTimeConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            double thetime = (double)value;
             
            if (thetime > 60)
            {
                return (thetime / 60).ToString("#.#") + " min(s)";
            }
            else if (thetime > (60 * 60))
            {
               return (thetime / (60 * 60)).ToString("#.#") + " hours";
            }
            else if (thetime > (60 * 60 * 24))
            {
                return (thetime / (60 * 60 * 24)).ToString("#.#") + " day(s)";
            }
            else
            {
                return "0 sec(s)"; ;
            } 

            

        }

        public object ConvertBack(object value, Type targetType,
            object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }

    }
}
