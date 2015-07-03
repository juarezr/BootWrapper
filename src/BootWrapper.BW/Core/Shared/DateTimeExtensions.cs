using System;

namespace BootWrapper.BW.Core.TShared
{
    /// <summary>
    /// Métodos extendidos para Data e Hora.
    /// </summary>
    public static class DateTimeExtensions
    {
        static readonly DateTime _start = Convert.ToDateTime("01/01/2013");
        static readonly DateTime _end = Convert.ToDateTime("31/12/2099");

        /// <summary>
        /// Retorna se a data informada é uma data válida.
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static bool IsValidDate(this string date)
        {
            DateTime newDate;

            if (!DateTime.TryParse(date, out newDate))
                return false;

            if (!newDate.IsValidStart())
                return false;

            return true;
        }

        /// <summary>
        /// Retorna se a data informada é uma data válida para o período padrão.
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static bool IsValidStart(this DateTime date)
        {
            return (date.CompareTo(_start) > 0) && (date.CompareTo(_end) < 0);
        }

        /// <summary>
        /// Conversão de data valida 
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime FromStringToDate(this string date)
        {
            DateTime newDate;
            DateTime.TryParse(date, out newDate);                
            return newDate;
        }


        /// <summary>
        /// Checa se o hoário é válido.
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static bool IsValidTime(this string time)
        {
            DateTime newDate;
            string actualDate = string.Format("{0} {1}",_start.ToShortDateString(), time);

            if (!DateTime.TryParse(actualDate, out newDate))
                return false;

            return true;
        }

        /// <summary>
        /// Converte data adicionando o tempo informado.
        /// </summary>
        /// <param name="date"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public static DateTime FromDateWithTime(this DateTime date, string time)
        {
            DateTime newDate;
            string oldDate = string.Format("{0} {1}", date.ToShortDateString(), time);

            DateTime.TryParse(oldDate, out newDate);

            return newDate;
        }

        /// <summary>
        /// Formata a data para string no formato dd/MM/yyyy
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string FormatDate(this DateTime data)
        {
            return data.ToString(@"dd/MM/yyyy");
        }

        /// <summary>
        /// Formata a hora para string no formato hh:mm
        /// </summary>
        /// <param name="hora"></param>
        /// <returns></returns>
        public static string FormatTime(this TimeSpan hora)
        {
            return hora.ToString(@"hh\:mm");
        }

        /// <summary>
        /// Transforma o argumento informado para hh:mm
        /// </summary>
        /// <param name="hora"></param>
        /// <returns></returns>
        public static string ParseAndFormatTime(this string hora)
        {
            //return string.Format(@"hh:mm", TimeSpan.Parse(hora));
            return TimeSpan.Parse(hora).ToString(@"hh\:mm");
        }
    }
}