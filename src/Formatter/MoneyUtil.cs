using System;

namespace BootWrapper.BW.Formatter
{
    public static class MoneyUtil
    {
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
