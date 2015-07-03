using BootWrapper.BW.Controls.Util;
using BootWrapper.BW.Core.Translator;
using System;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace BootWrapper.BW.Controls
{
    /// <summary>
    /// Classe mãe para as sub-classes que represetam algum componente html.
    /// </summary>
    public abstract class MvcInput<T> : MvcBaseComponent<T> where T : MvcInput<T>
    {
        

        #region Fields
        /// <summary>
        /// Nome default da tag
        /// </summary>
        public const string DEFAULT_INPUT_TAG = "input";

        public const string DEFAULT_TYPE = "textbox";
      
        #endregion

        #region properties

        public string Type { get; set; }

        public string Name { get; set; }

        public bool IsReadOnly { get; set; }

        public bool IsRequired { get; set; }

        #endregion

        /// <summary>
        /// Construtor do componente.
        /// </summary>
        /// <param name="viewContext">Contexto da View.</param>
        public MvcInput(ViewContext viewContext)
            : base(viewContext, DEFAULT_INPUT_TAG)
        {
            TagRenderMode  = TagRenderMode.SelfClosing;
        }

        public T SetName(string name) 
        {
            this.Name = name;
            return this as T;
        }

        public T SetType(string type) 
        {
            this.Type = type;
            return this as T;
        }

        public T SetReadonly(bool isReadOnly)
        {
            this.IsReadOnly = isReadOnly;
            return this as T;
        }

        public T SetRequired(bool isRequired)
        {
            this.IsRequired = isRequired;
            return this as T;
        }

        /// <summary>
        /// Cria um obeto TagBuilder que representa a Tag do componente.
        /// </summary>
        /// <param name="tagName">Nome da Tag.</param>
        /// <param name="id">Id da tag.</param>
        /// <param name="cssClass">Classe css</param>
        /// <param name="htmlAttributes">Atributos da tag.</param>
        /// <returns>Obeto TagBuilder.</returns>
        protected override TagBuilder CreateTag(object htmlAttributes = null)
        {
            var tag = base.CreateTag(htmlAttributes);

            if (!String.IsNullOrEmpty(Id))
                tag.GenerateId(Id);

            tag.Attributes.Add("name", Name);
            tag.Attributes.Add("type", Type);

            if (IsReadOnly)
                tag.Attributes.Add("readonly", "readonly");

            if(IsRequired)
                tag.Attributes.Add("required", "required");

            tag.MergeAttributes(MergeClassAttributes(CssClass, htmlAttributes), false);

            return tag;            
        }      
    }
}
