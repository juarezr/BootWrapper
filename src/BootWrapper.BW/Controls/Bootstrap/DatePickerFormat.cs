using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BootWrapper.BW.Controls
{
    /// <summary>
    /// Define o format do datepicker.
    /// </summary>
    public enum DatePickerFormat
    {
        [DatePickerFormatAttribute("dd/mm/yyyy")]
        Day,

        [DatePickerFormatAttribute("mm/yyyy", 1)]
        Month,

        [DatePickerFormatAttribute("yyyy", 2)]
        Year
    }
}
