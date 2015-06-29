using System;
using System.Web.Mvc;

namespace BootWrapper.BW.Controls
{
    /// <summary>
    /// Classe que representa painel modal.
    /// Um painel modal segue a seguinte estrutura:
    /// <code language='xml'>
    /// <![CDATA[
    /// <div class='modal fade in'>
    ///     <div class='modal-dialog'>
    ///         <div class='modal-content'>
    ///             <div class='modal-header'>
    ///                 <h4 class='modal-title'>Título do Modal</h4>
    ///             </div>
    ///             <div class='modal-body'>
    ///                 <!-- Contéudo -->
    ///             </div>
    ///         </div>
    ///     </div>
    /// </div>]]>
    /// </code>
    /// </summary>
    public class MvcModal : MvcBaseComponent<MvcModal>
    {
        #region CTOR

        /// <summary>
        /// Initializes a new instance of the <see cref="MvcModal"/> class.
        /// </summary>
        /// <param name="viewContext"></param>
        public MvcModal(ViewContext viewContext) 
            : base(viewContext)
        {            
        }

        #endregion

        #region Methods

        /// <summary>
        /// Inicia o painel.
        /// </summary>
        /// <param name="htmlAttributes">Atributos do painel</param>
        /// <returns>Este obeto.</returns>
        public MvcModal Begin(object htmlAttributes = null)
        {
            return Begin(String.Empty, htmlAttributes);
        }       

        /// <summary>
        /// Inicia o painel.
        /// </summary>
        /// <param name="id">Id do painel</param>
        /// <param name="htmlAttributes">Atributos do painel.</param>
        /// <returns>Este objeto.</returns>
        public MvcModal Begin(string id, object htmlAttributes = null)
        {
            return Begin(id, "modal fade in", htmlAttributes);
        }

        /// <summary>
        /// Inicia o painel
        /// </summary>
        /// <param name="id">Id do painel.</param>
        /// <param name="panelCssClass">css do painel</param>
        /// <param name="htmlAttributes">Atributos do painel.</param>
        /// <returns>O próprio objeto.</returns>
        private MvcModal Begin(string id, string panelCssClass, object htmlAttributes = null)
        {
            TagBuilder tag = CreateTag("div", id, panelCssClass, htmlAttributes);            
            this._writer.Write(tag.ToString(TagRenderMode.StartTag));

            BeginDialog().BeginContent();

            return this;
        }
       
        private MvcModal BeginDialog()
        {
            TagBuilder tag = CreateTag("div", String.Empty, "modal-dialog");

            this._writer.Write(tag.ToString(TagRenderMode.StartTag));

            return this;
        }

        private MvcModal BeginContent()
        {
            TagBuilder tag = CreateTag("div", String.Empty, "modal-content");            
            this._writer.Write(tag.ToString(TagRenderMode.StartTag));
            
            return this;
        }

        /// <summary>
        /// Inicia a div de header.
        /// </summary>
        /// <param name="title">Título do painel.</param>
        /// <param name="htmlAttributes">Atributos do painel.</param>
        /// <returns>Este objeto.</returns>
        public MvcModal BeginHeader(string title, object htmlAttributes = null)
        {
            return BeginHeader(title, "modal-header", htmlAttributes);
        }
        /// <summary>
        /// Inicia a div de header.
        /// </summary>
        /// <param name="title">Título do painel.</param>
        /// /// <param name="cssTitle">Classe CSS.</param>
        /// <param name="htmlAttributes">Atributos do painel.</param>
        /// <returns>Este objeto.</returns>
        private MvcModal BeginHeader(string title, string cssTitle, object htmlAttributes = null)
        {
            TagBuilder tag = CreateTag("div", String.Empty, cssTitle, htmlAttributes);            
            tag.InnerHtml = CreateCloseButton().ToString(TagRenderMode.Normal);
            tag.InnerHtml += CreateHeaderText(title);

            this._writer.Write(tag.ToString(TagRenderMode.Normal));

            return this;
        }

        private string CreateHeaderText(string title)
        {
            return String.Format("<h4 class='modal-title'>{0}</h4>", title);
        }

        private TagBuilder CreateCloseButton()
        {
            TagBuilder tag = CreateTag("button", String.Empty, "close", new { type= "button", data_dismiss= "modal"});

            //TagBuilder tag = new TagBuilder("button");
            //tag.MergeAttribute("type", "button");
            //tag.MergeAttribute("data-dismiss", "modal");
            //tag.AddCssClass("close");
            tag.InnerHtml = "<span aria-hidden='true'>&times;</span><span class='sr-only'>Close</span>";

            return tag;
        }

        /// <summary>
        /// Inicia o conteúdo do modal.
        /// </summary>
        /// <param name="htmlAttributes">Atributos do div do body.</param>
        /// <returns>Este objeto.</returns>
        public MvcModal BeginBody(object htmlAttributes = null)
        {
            return BeginBody("modal-body", htmlAttributes);
        }

        /// <summary>
        /// Inicia o conteúdo do modal.
        /// </summary>
        /// <param name="cssBody">Classe CSS</param>
        /// <param name="htmlAttributes">Atributos do painel modal.</param>
        /// <returns>Este objeto</returns>
        private MvcModal BeginBody(string cssBody, object htmlAttributes = null)
        {
            //TagBuilder tag = new TagBuilder("div");            
            //tag.MergeAttributes(MergeClassAttributes(cssBody, htmlAttributes));
            //this._writer.Write(tag.ToString(TagRenderMode.StartTag));

            TagBuilder tag = CreateTag("div", String.Empty, cssBody, htmlAttributes);
            this._writer.Write(tag.ToString(TagRenderMode.StartTag));
                        
            return this;
        }

        /// <summary>
        /// Finaliza o painel e fecha as tags necessárias.
        /// </summary>
        protected override void EndIt()
        {
            if (!this._disposed)
            {
                this._disposed = true;
                this.End().End().End().End(); // body, header, content, modal

                if (_viewContext != null)
                {
                    _viewContext.OutputClientValidation();
                }
            }
        }
       

        #endregion
    }
}
