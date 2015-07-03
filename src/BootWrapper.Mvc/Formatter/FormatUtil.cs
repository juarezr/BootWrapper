using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BootWrapper.BW.Formatter
{
    public static class FormatUtil
    {
        public static string FormatDate(this DateTime data)
        {
            return data.ToString(@"dd/MM/yyyy");
        }

        public static string FormatDateTime(this DateTime data)
        {
            return data.ToString(@"dd/MM/yyyy HH:mm");
        }

        public static string FormatTime(this TimeSpan hora)
        {
            return hora.ToString(@"hh\:mm");
        }

        public static string ParseAndFormatTime(this string hora)
        {
            //return string.Format(@"hh:mm", TimeSpan.Parse(hora));
            return TimeSpan.Parse(hora).ToString(@"hh\:mm");
        }

        public static string FormatMoney(this decimal valor)
        {
            return valor.ToString(@"C");
        }

        public static string FormatGenericMoney(this decimal valor)
        {
            return valor.ToString(@"G");
        }

        public static string FormatValue(this decimal valor)
        {
            return valor.ToString(@"N1");
        }

        public static string FormataValorParaStringComZeroEsquerda(this decimal valor)
        {
            return FormatValue(valor).PadLeft(4, '0');
        }
    }
}
