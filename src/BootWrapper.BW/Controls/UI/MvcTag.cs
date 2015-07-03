using System;
using System.Web.Mvc;

namespace BootWrapper.BW.Controls
{
    /// <summary>
    /// Classe que representa uma tag em html.
    /// </summary>
    public class MvcTag : MvcBaseComponent<MvcTag>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MvcDiv"/> class.
        /// </summary>
        /// <param name="viewContext">The view context.</param>
        public MvcTag(ViewContext viewContext, string tag)
            : base(viewContext, tag)
        {           
                
        }

        /// <summary>
        /// Inicia a tag no writer do context.
        /// </summary>
        /// <returns>Retorna a instância do próprio objeto.</returns>
        public MvcTag Begin()
        {
            _writer.Write(String.Format("<{0}>", Tag));
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cssClass"></param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public MvcTag Begin(string cssClass, object htmlAttributes = null)
        {
            Begin(String.Empty, cssClass, htmlAttributes);

            return this;
        }  

        public MvcTag Begin(string id, string cssClass, object htmlAttributes = null)
        {   
            this.GetWriter().Write(CreateTag(base.Tag, id, cssClass, htmlAttributes).ToString(TagRenderMode.StartTag));

            return this;
        }  
    }
}
