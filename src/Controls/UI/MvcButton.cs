using BootWrapper.BW.Controls.Util;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace BootWrapper.BW.Controls
{
    /// <summary>
    /// Classe para criar um botão
    /// </summary>
    public class MvcButton : MvcSketchButton<MvcButton>
    {
        public MvcButton(ViewContext viewContext)
            : base(viewContext)
        {                 
            
        }
    }
}
