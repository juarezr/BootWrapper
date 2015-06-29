using System.Web.Mvc;

namespace BootWrapper.BW.Controls
{
    /// <summary>
    /// Classe que constrói uma tag para ícones.
    /// </summary>
    public class MvcCheckbox : MvcInput<MvcCheckbox>
    {
        #region properties

        public bool IsChecked { get; set; }

        public string Text { get; set; }

        #endregion

        public MvcCheckbox(ViewContext viewContext) : base(viewContext)
        {
            TagRenderMode = TagRenderMode.Normal;
        }

        public MvcCheckbox SetText(string text)
        {
            this.Text = text;
            this.Type = "checkbox";
            return this;
        }

        public MvcCheckbox SetChecked(bool isChecked)
        {
            this.IsChecked = isChecked;
            return this;
        }
 
        /// <summary>
        /// Obtém instância do objeto TagBuilder que representa a tag do ícone.
        /// </summary>
        /// <param name="icon">Classe font-awesome.</param>
        /// <returns>Retorna obeto TagBuilder</returns>
        protected override TagBuilder CreateTag(object htmlAttributes = null)
        {   
            var tag = base.CreateTag(htmlAttributes);
            tag.Attributes.Add("value", "true");
            tag.InnerHtml = Text;

            if (IsChecked)
                tag.Attributes.Add("checked", "checked");
            
            var divBuilder = new TagBuilder("div");
            divBuilder.AddCssClass("input-group");
            divBuilder.InnerHtml = tag.ToString(TagRenderMode.SelfClosing);

            return divBuilder;
        }
        
    }
}
