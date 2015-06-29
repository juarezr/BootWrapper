using System.Web.Mvc;

namespace BootWrapper.BW.Controls
{
    /// <summary>
    /// Classe que constrói uma tag para ícones.
    /// </summary>
    public class MvcIcon : MvcBaseComponent<MvcIcon>
    {
        /// <summary>
        /// Nome da tag do ícone.
        /// </summary>
        public const string DEFAULT_ICON_TAG = "span";


        #region properties

        public string Icon { get; set; }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="MvcIcon"/> class.
        /// </summary>
        /// <param name="viewContext">Contexto da View.</param>
        public MvcIcon(ViewContext viewContext)
            : base(viewContext, DEFAULT_ICON_TAG)
        {
        }  

        public MvcIcon SetIcon(string icon)
        {
            this.CssClass = icon;
            return this;
        }      

    }
}
