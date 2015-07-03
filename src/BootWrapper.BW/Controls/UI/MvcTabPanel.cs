using System;
using System.Web.Mvc;

namespace BootWrapper.BW.Controls
{
    /// <summary>
    /// Cria um painel para conter as tabs. As tabs precisam ser criadas através do <see cref="MvcTabContent">MvcTabContent</see>. 
    /// Segue exemplo do contéudo de código gerado.
    /// <code language='xml'>
    /// <![CDATA[
    /// <div class="box border" style="margin: 5px;">
    ///     <div length="10" class="box-title">
    ///         <h4><span class="fa fa-bars"></span> Título do Painel</h4>
    ///     </div>
    ///     <div class="box-body">
    ///         <div class="tabbable header-tabs">
    ///             <ul class="nav nav-tabs">
    ///                 <li class="active">
    ///                     <a data-toggle="tab" href="#tabID1">
    ///                         <span class="fa fa-bar-chart-o"></span>
    ///                         <span class="hidden-inline-mobile"> Título Tab 1</span>
    ///                     </a>
    ///                 </li>
    ///                 <li>
    ///                     <a data-toggle="tab" href="#tabID2">
    ///                         <span class="fa fa-search-plus"></span>
    ///                         <span class="hidden-inline-mobile"> Título Tab 2</span></a>
    ///                 </li>
    ///             </ul>
    ///             <!-- Conteúdo das tabs é gerado pela classe <see cref="MvcTabContent"/> -->
    ///         </div>
    ///     </div>
    /// </div>
    /// ]]>
    /// </code>
    /// </summary>
    /// <author>Andre Farina</author>
    public class MvcTabPanel : MvcBaseComponent<MvcTabPanel>
    {
        private MvcPanel pnlSurrounded;
        private MvcTabContent tabContent;

        private const string CSS_TABBABLE   = "tabbable header-tabs";
        private const string CSS_PILLS      = "nav nav-tabs";

        /// <summary>
        /// Inicia nova instância da classe <see cref="MvcTabPanel"/>.
        /// </summary>
        /// <param name="viewContext">Contexto da View</param>
        public MvcTabPanel(ViewContext viewContext)
            : base(viewContext, "div")
        {
             
        }        

        #region Methods

        /// <summary>
        /// Inicia o painel.
        /// </summary>
        /// <param name="title">Título</param>
        /// <param name="icon">Ícone do painel</param>
        /// <param name="htmlAttributes">Atributos do painel.</param>
        /// <returns>Retorna a própria instância.</returns>
        public MvcTabPanel Begin(string title, string icon, object htmlAttributes = null)
        {
            return Begin(title, icon, PanelColor.Default, htmlAttributes);
        }

        /// <summary>
        /// Inicia o painel.
        /// </summary>
        /// <param name="title">Título</param>
        /// <param name="icon">Ícone do painel</param>
        /// <param name="color">Cor do painel.</param>
        /// <param name="htmlAttributes">Atributos do painel</param>
        /// <returns>Retorna a própria instância.</returns>
        public MvcTabPanel Begin(string title, string icon, PanelColor color, object htmlAttributes = null)
        {
            pnlSurrounded = new MvcPanel(GetContext());
            pnlSurrounded.Begin(color, htmlAttributes).BeginHeader(title, icon).BeginBody();

            CreateTabbable();

            return this;
        }

        private void CreateTabbable()
        {
            new MvcDiv(GetContext()).Begin(CSS_TABBABLE); 
        }

        /// <summary>
        /// Inicia a declaração das tabs.
        /// <code language="xml"> 
        /// <![CDATA[
        /// <ul class="nav nav-tabs">
        ///     <!--  Código gerado pelo método AddTab -->
        /// </ul>
        /// ]]>
        /// </code>
        /// </summary>
        /// <param name="htmlAttributes">Atributos da tag 'ul'</param>
        /// <returns>Retorna a própria instância.</returns>
        public MvcTag CreateTabs(object htmlAttributes = null)
        {
            MvcTag ul = new MvcTag(GetContext(), "ul");
            ul.Begin(CSS_PILLS, htmlAttributes);
                  
            return ul; 
        }        

        /// <summary>
        /// Declaração da Tab. Define as propriedades da tab.
        /// <code language="xml"> 
        /// <![CDATA[
        /// <li class="active">
        ///     <a data-toggle="tab" href="#tabID1">
        ///         <span class="fa fa-bar-chart-o"></span>
        ///         <span class="hidden-inline-mobile"> Título Tab 1</span>
        ///     </a>
        /// </li>
        /// ]]>
        /// </code>
        /// </summary>
        /// <param name="target">ID do painel que será mostrado ao se clicar na Tab.</param>
        /// <param name="title">Título da Tab</param>
        /// <param name="icon">Ícone da tab</param>
        /// <param name="active">Indica se a tab está ativa.</param>
        /// <returns>Retorna a própria instância.</returns>
        public MvcTabPanel AddTab(string target, string title, string icon, bool active = false)
        {            
            var anchor = new TagBuilder("a");            
            anchor.MergeAttribute("href", String.Format("#{0}", target));
            anchor.MergeAttribute("data-toggle", "tab");
            anchor.InnerHtml = String.Format("<span class='{0}'></span><span class='hidden-inline-mobile'> {1}</span>", icon, title);

            var tag = new TagBuilder("li");
            tag.InnerHtml = anchor.ToString();
            if (active)
                tag.AddCssClass("active");
                                
            this._writer.Write(tag.ToString());
                        
            return this;
        }

        /// <summary>
        /// Inicia o painel de declaração das tabs. Cria objeto da classe <see cref="MvcTabContent"/>
        /// </summary>
        /// <returns>Retorna objeto MvcTabContent.</returns>
        public MvcTabContent BeginTabContent()
        {
            tabContent = new MvcTabContent(GetContext()).Begin();

            return tabContent;
        }

        /// <summary>
        /// Sobrescreve método EndIt para fechar as tags abertas.
        /// </summary>
        protected override void EndIt()
        {            
            if (!this._disposed)
            {
                this._disposed = true;               
               
                if (tabContent != null)
                    tabContent.Dispose(); // tab-content

                this.End(); // tabbable

                if (pnlSurrounded != null)
                    pnlSurrounded.Dispose(); // panel                

                if (_viewContext != null)
                {
                    _viewContext.OutputClientValidation();
                }
            }
        }
     
        #endregion
    }
}
