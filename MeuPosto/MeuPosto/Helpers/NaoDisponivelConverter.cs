using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MeuPosto.Helpers
{
    public class NaoDisponivelConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            CultureInfo br = new CultureInfo("pt-BR");
            if ((float) value <= 0)
                    return "Não Disponível.";
                return ((float)value).ToString("c2",br);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
