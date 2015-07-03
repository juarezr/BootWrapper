using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BootWrapper.BW.Controls
{
    /// <summary>
    /// Attributo para se decorar o tipo do DatePicker.
    /// </summary>
    public class DatePickerFormatAttribute : System.Attribute
    {
        private readonly string _value;
        private readonly int _minViewMode;


        /// <summary>
        /// Construtor do atributo.
        /// </summary>
        /// <param name="value">Nome da classe do bootstrap.</param>
        public DatePickerFormatAttribute()
        {
            _value = "dd/mm/yyyy";
            _minViewMode = 0;
        }

        /// <summary>
        /// Construtor do atributo.
        /// </summary>
        /// <param name="value">Nome da classe do bootstrap.</param>
        public DatePickerFormatAttribute(string value)
        {
            _value = value;
            _minViewMode = 0;
        }


        /// <summary>
        /// Construtor do atributo.
        /// </summary>
        /// <param name="value">Nome da classe do bootstrap.</param>
        public DatePickerFormatAttribute(string value, int minViewMode)
        {
            _value = value;
            _minViewMode = minViewMode;
        }



        /// <summary>
        /// Nome da classe.
        /// </summary>
        public string Value
        {
            get { return _value; }
        }


        /// <summary>
        /// Nome da classe.
        /// </summary>
        public int MinViewMode
        {
            get { return _minViewMode; }
        }

        /// <summary>
        /// Obtém o valor definido pelo atributo.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetFormatValue(Enum value)
        {
            string output = null;
            Type type = value.GetType();

            FieldInfo fi = type.GetField(value.ToString());
            var attrs = fi.GetCustomAttributes(typeof(DatePickerFormatAttribute), false) as DatePickerFormatAttribute[];

            if (attrs != null & attrs.Length > 0)
            {
                output = attrs[0].Value;
            }

            return output;
        }

        /// <summary>
        /// Obtém o valor definido pelo atributo.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int GetMinViewModeValue(Enum value)
        {
            int output = 0;
            Type type = value.GetType();

            FieldInfo fi = type.GetField(value.ToString());
            var attrs = fi.GetCustomAttributes(typeof(DatePickerFormatAttribute), false) as DatePickerFormatAttribute[];

            if (attrs != null & attrs.Length > 0)
            {
                output = attrs[0].MinViewMode;
            }

            return output;
        }

    }
}
