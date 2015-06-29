using System.Web.Mvc;

namespace BootWrapper.BW.Controls
{
    /// <summary>
    /// Classe que reprenta uma tag div.
    /// </summary>
    public class MvcDiv : MvcTag
    {    
        /// <summary>
        /// Initializes a new instance of the <see cref="MvcDiv"/> class.
        /// </summary>
        /// <param name="viewContext">The view context.</param>
        public MvcDiv(ViewContext viewContext)
            : base(viewContext, "div")
        {           
               
        }
    }
}
