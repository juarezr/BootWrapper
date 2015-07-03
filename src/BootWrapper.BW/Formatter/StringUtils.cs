using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BootWrapper.BW.Formatter
{
    public static class StringUtils
    {
        public static bool In(this string source, params string[] values)
        {
            foreach (string s in values)
                if (source == s)
                    return true;
            return false;
        }

        public static bool NotIn(this string source, params string[] values)
        {
            return !source.In(values);
        }

        public static bool AllEmpty(params string[] listOfStrings)
        {
            foreach (string s in listOfStrings)
                if (!string.IsNullOrWhiteSpace(s))
                    return false;
            return true;
        }

        public static string IfEmptyThen(this string source, string replaceByThisIfEmpty, params string[] otherReplacements)
        {
            if (!string.IsNullOrWhiteSpace(source))
                return source;
            if (!string.IsNullOrWhiteSpace(replaceByThisIfEmpty))
                return replaceByThisIfEmpty;
            foreach (string s in otherReplacements)
                if (!string.IsNullOrWhiteSpace(s))
                    return s;
            return string.Empty;
        }

        public static bool IsNullOrWhiteSpace(this string source)
        {
            return string.IsNullOrWhiteSpace(source);
        }

        public static bool Contains(this string source, string toCheck, StringComparison comp)
        {
            return source.FormatToSearch().IndexOf(toCheck.FormatToSearch(), comp) >= 0;
        }      

        public static string FormataTextoNull(this object caller, string textNull = "")
        {
            if (caller == null)
                return textNull;

            return caller.ToString();
        }


        public static bool AreEqual(this string texto, string other)
        {
            return texto.FormatToSearch().Equals(other.FormatToSearch());
        }

        public static string Truncate(this string texto, int tamanho)
        {
            if (String.IsNullOrEmpty(texto))
                return String.Empty;

            int tam = texto.Length > tamanho ? tamanho : texto.Length;
            return texto.Substring(0, tam);
        }

        public static string FormatToSearch(this string texto)
        {
            return texto.ToLower()
                        .Trim();
        }

        public static string Repeat(this char chatToRepeat, int repeat)
        {
            return new string(chatToRepeat, repeat);
        }

        public static string Repeat(this string stringToRepeat, int repeat)
        {
            var builder = new StringBuilder(repeat * stringToRepeat.Length);
            for (int i = 0; i < repeat; i++)
            {
                builder.Append(stringToRepeat);
            }
            return builder.ToString();
        }

        public static string Repeat(this string stringToRepeat, int repeat, char spliter)
        {
            var builder = new StringBuilder(repeat * stringToRepeat.Length);
            for (int i = 0; i < (2 * repeat) - 1; i++)
            {
                if (i.IsEven())
                    builder.Append(stringToRepeat);
                else
                    builder.Append(spliter);
            }
            return builder.ToString();
        }
    }
}
