using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace BootWrapper.BW.Controls.Util
{
    public class DropDownListUtil
    {

        public static SelectListItem GetDefaultItem()
        {
            return new SelectListItem { Value = "0", Text = "Selecione", Selected = true };
        }
    }
}
