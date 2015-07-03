using System;
using System.Web.Mvc;

namespace BootWrapper.BW.Controls
{
    /// <summary>
    /// Essa classe define o conteúdo das tabs que são definidas pela classe <see cref="MvcTabPanel"/>.
    /// Código gerado por essa classe:
    /// <code language='xml'>
    /// <![CDATA[
    /// <div class="tab-content">
    ///     <div class="tab-pane fade active in" id="tabBlue">
    ///         <!-- Conteúdo da Tab -->
    ///     </div>
    ///     <div class="tab-pane fade" id="tabRed">
    ///         <!-- Conteúdo da Tab -->
    ///     </div>
    /// </div>
    /// 
    /// ]]>
    /// </code>
    /// </summary>
    public class MvcTabContent : MvcBaseComponent<MvcTabContent>
    {
        /// <summary>
        /// Inicia nova instância da classe <see cref="MvcTabContent"/>.
        /// </summary>
        /// <param name="viewContext">Contexto da View</param>
        public MvcTabContent(ViewContext viewContext)
            : base(viewContext, "div")
        {           
           
        }        

        #region Methods

        /// <summary>
        /// Inicia declaração da div de conteúdo das tabs.
        /// <code language='xml'>
        /// <![CDATA[
        ///     <div class="tab-content">
        /// ]]>
        /// </code>
        /// </summary>
        /// <param name="htmlAttributes">Atributos da tag.</param>
        /// <returns>A própria instância.</returns>
        public MvcTabContent Begin(object htmlAttributes = null)
        {
            return Begin("tab-content", htmlAttributes);
        }

        /// <summary>
        /// Inicia declaração da div de conteúdo das tabs.
        /// <code language='xml'>
        /// <![CDATA[
        ///     <div class="tab-content">
        /// ]]>
        /// </code>
        /// </summary>
        /// <param name="panelCssClass">Classe CSS da tag.</param>
        /// <param name="htmlAttributes">Atributos da tag.</param>
        /// <returns>A própria instância.</returns>
        private MvcTabContent Begin(string panelCssClass, object htmlAttributes = null)
        {
            TagBuilder tag = CreateTag(Tag, String.Empty, panelCssClass, htmlAttributes);       
            
            this._writer.Write(tag.ToString(TagRenderMode.StartTag));

            return this;
        }

        /// <summary>
        /// Inicia declaração do contéudo da tab.
        /// <code language='xml'>
        /// <![CDATA[
        ///     <div class="tab-pane fade active in" id="tabBlue">
        /// ]]>
        /// </code> 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="active"></param>
        /// <returns></returns>
        public MvcDiv DeclareTab(string id, bool active = false)
        {
            var div = new MvcDiv(_viewContext);
                        
            if (active)
                div.Begin(id, "tab-pane fade active in");                
            else
                div.Begin(id, "tab-pane fade");

            return div;
        }

        
        #endregion
    }
}
