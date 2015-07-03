using System;
using System.Web.Mvc;

namespace BootWrapper.BW.Controls
{
    /// <summary>
    /// Classe que constrói uma tag para ícones.
    /// </summary>
    public class MvcTextBox : MvcInput<MvcTextBox>
    {
        #region properties

        public int MaxLength{ get; set; }

        public string ClassSize { get; set; }

        #endregion

        public MvcTextBox(ViewContext viewContext)
            : base(viewContext)
        {
            SetType("text");
            Set(InputTextSize.Medium);
            this.TagRenderMode = TagRenderMode.SelfClosing;
        }

        public MvcTextBox SetMaxLength(int maxLen)
        {
            this.MaxLength = maxLen;
            return this;
        }

        public MvcTextBox Set(InputTextSize size)
        {
            return SetSize(GetStyleClass(size));
        }

        public MvcTextBox SetSize(string sizeClass)
        {
            this.ClassSize = sizeClass;
            return this;
        }
 
        /// <summary>
        /// Obtém instância do objeto TagBuilder que representa a tag do ícone.
        /// </summary>
        /// <param name="icon">Classe font-awesome.</param>
        /// <returns>Retorna obeto TagBuilder</returns>
        protected override TagBuilder CreateTag(object htmlAttributes = null)
        {
            this.CssClass = String.Format("form-control {0}", ClassSize);

            var tag = base.CreateTag(htmlAttributes);

            if (MaxLength > 0)
                tag.Attributes.Add("maxlength", MaxLength.ToString());

            return tag;
        }       
        
    }
}
