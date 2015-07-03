using System;
using System.Reflection;


namespace BootWrapper.BW.Controls
{
    /// <summary>
    /// Attributo para se decorar enumerations ou propriedades que definam algum valor/classe do bootstrap.
    /// </summary>
    public class BootstrapValueAttribute : System.Attribute
    {
        private readonly string _value;

        /// <summary>
        /// Construtor do atributo.
        /// </summary>
        /// <param name="value">Nome da classe do bootstrap.</param>
        public BootstrapValueAttribute(string value)
        {
            _value = value;
        }

        /// <summary>
        /// Nome da classe.
        /// </summary>
        public string Value
        {
            get { return _value; }
        }

        /// <summary>
        /// Obtém o valor definido pelo atributo.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetStringValue(Enum value)
        {
            string output = null;
            Type type = value.GetType();

            FieldInfo fi = type.GetField(value.ToString());
            var attrs = fi.GetCustomAttributes(typeof(BootstrapValueAttribute), false) as BootstrapValueAttribute[];

            if (attrs != null & attrs.Length > 0)
            {
                output = attrs[0].Value;
            }

            return output;
        }

    }
}
