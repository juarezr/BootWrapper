using System;
using System.Web.Mvc;

namespace BootWrapper.BW.Controls
{
    /// <summary>
    /// Contrói um painel que pode ou não conter a função collapse.
    /// 
    /// Painel sem função de collapse:
    /// <code language='xml'>
    /// <![CDATA[
    /// <div class='box border default'>
    ///     <div class='box-title'>
    ///         <h4><span class='fa fa-bars'></span>Título do Painel</h4>
    ///     </div>
    ///     <div class='box-body'>
    ///         <!-- Contéudo -->
    ///     </div>
    /// </div>]]>
    /// </code>
    /// 
    /// Painel com função de collapse:
    /// <code language='xml'>
    /// <![CDATA[
    /// <div class='box border default'>
    ///     <div class='box-title collapsed' data-target='#collapse1' data-toggle='collapse'>
    ///         <h4><span class='fa fa-bars'></span>Título do Painel</h4>
    ///         <div class='tools'>
    ///             <a class="collapse" data-toggle="collapse" data-parent="#accordion" href="#collapse1"></a>
    ///         </div>   
    ///     </div>
    ///     <div class='panel-collapse collapse in' id='collapse1'>
    ///         <div class='box-body'>
    ///             <!-- Contéudo -->
    ///         </div>
    ///     </div>
    /// </div>]]>
    /// </code>
    /// </summary>
    public class MvcPanel : MvcBaseComponent<MvcPanel>
    {
        #region Fields

        private bool _hasCollapse;

        /// <summary>
        /// Íconde default do painel. CSS do font-awesome.
        /// </summary>
        public const string DEFAULT_ICON   = "fa fa-bars";

        /// <summary>
        /// CSS principal do painel.
        /// </summary>
        public const string CSS_PANEL = "panel";

        /// <summary>
        /// CSS do painel header.
        /// </summary>
        public const string CSS_HEADER = "panel-heading";

        /// <summary>
        ///  CSS do painel body.
        /// </summary>
        public const string CSS_TITLE = "panel-title";

        /// <summary>
        ///  CSS do painel body.
        /// </summary>
        public const string CSS_BODY = "panel-body";

        /// <summary>
        /// CSS para realizar a função de collapse.
        /// </summary>
        public const string CSS_COLLAPSE = "collapse";

        #endregion

        #region CTOR

        /// <summary>
        /// Inicia nova instância da classe <see cref="MvcPanel"/>.
        /// </summary>
        /// <param name="viewContext">Contexto da View</param>
        /// <param name="hasCollapsed">Indica se tem a função collapse</param>
        public MvcPanel(ViewContext viewContext, bool hasCollapsed = false)
            : base(viewContext)
        {
            this._hasCollapse = hasCollapsed;
        }     

        #endregion

        #region Methods

        /// <summary>
        /// Inicia o painel
        /// </summary>
        /// <param name="htmlAttributes">Atributos do painel</param>
        /// <returns>Este objeto</returns>
        public MvcPanel Begin(object htmlAttributes = null)
        {
            return Begin(String.Empty, htmlAttributes);
        }

        /// <summary>
        /// Inicia o painel
        /// </summary>
        /// <param name="id">Id do painel</param>
        /// <param name="htmlAttributes">Atributos do painel</param>
        /// <returns>Este objeto</returns>
        public MvcPanel Begin(string id, object htmlAttributes = null)
        {
            return Begin(id, PanelColor.Default, htmlAttributes);
        }

        /// <summary>
        /// Inicia o painel
        /// </summary>
        /// <param name="color">Cor do painel</param>
        /// <param name="htmlAttributes">Atributos do painel</param>
        /// <returns>Este objeto</returns>
        public MvcPanel Begin(PanelColor color, object htmlAttributes = null)
        {
            return Begin(String.Empty, color, htmlAttributes);
        }

        /// <summary>
        /// Inicia o painel
        /// </summary>
        /// <param name="color">Cor do painel</param>
        /// <param name="htmlAttributes">Atributos do painel</param>
        /// <param name="id">Id do painel</param>
        /// <returns>Este objeto</returns>
        public MvcPanel Begin(string id, PanelColor color, object htmlAttributes = null)
        {
            if (color == PanelColor.NoColor)
                return Begin(id, CSS_PANEL, htmlAttributes);

            return Begin(id, String.Format("{0} {1}", CSS_PANEL, BootstrapValueAttribute.GetStringValue(color)), htmlAttributes);
        }

        /// <summary>
        /// Inicia o painel
        /// </summary>
        /// <param name="panelCssClass">CSS do painel</param>
        /// <param name="htmlAttributes">Atributos do painel</param>
        /// <param name="id">Id do painel</param>
        /// <returns>Este objeto</returns>
        private MvcPanel Begin(string id, string panelCssClass, object htmlAttributes = null)
        {
            var tag = new TagBuilder("div");
            if (!string.IsNullOrEmpty(id))
                tag.GenerateId(id);

            tag.AddCssClass(panelCssClass);
            tag.MergeAttributes(MergeClassAttributes(panelCssClass, htmlAttributes), false);
            tag.MergeAttribute("style", "margin: 5px;");

            this.GetWriter().Write(tag.ToString(TagRenderMode.StartTag));

            //this._writer.Write("<div class='panel panel-default'>");

            return this;
        }

        /// <summary>
        /// Inicia o painel
        /// </summary>
        /// <param name="title">Título do painel</param>
        /// <param name="htmlAttributes">Atributos do painel</param>
        /// <returns>Este objeto</returns>
        public MvcPanel BeginHeader(string title, object htmlAttributes = null)
        {
            return BeginHeader(title, DEFAULT_ICON, htmlAttributes);
        }

        /// <summary>
        /// Inicia o painel
        /// </summary>
        /// <param name="icon">Ícone do painel</param>
        /// <param name="htmlAttributes">Atributos do painel</param>
        /// <param name="title">Título do painel</param>
        /// <returns>Este objeto</returns>
        public MvcPanel BeginHeader(string title, string icon, object htmlAttributes = null)
        {
            return BeginHeader(title, icon, CSS_HEADER, htmlAttributes);
        }

        /// <summary>
        /// Inicia o painel
        /// </summary>
        /// <param name="cssTitle">CSS do painel header</param>
        /// <param name="htmlAttributes">Atributos do painel header</param>
        /// <param name="title">Título</param>
        /// <param name="icon">Ícone</param>
        /// <returns>Este objeto</returns>
        private MvcPanel BeginHeader(string title, string icon, string cssTitle, object htmlAttributes = null)
        {
            TagBuilder tag = CreateTag("div", String.Empty, cssTitle, htmlAttributes);
            tag.InnerHtml = CreateTitle(title, icon);

            this._writer.Write(tag.ToString(TagRenderMode.Normal));

            return this;
        }

        /// <summary>
        /// Inicia o painel com função collapse.
        /// </summary>
        /// <param name="title"></param>
        /// <param name="collapsed"></param>
        /// <param name="collapsePanel"></param>
        /// <param name="icon"></param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public MvcPanel BeginCollapseHeader(string title, bool collapsed, string collapsePanel, string icon = DEFAULT_ICON, object htmlAttributes = null)
        {
            this._hasCollapse = true;

            // o icone do link do chevron é alterado pela classe collapsed. O evento é tratado pelo CSS abaixo
            /*           
            .box-title[data-toggle]:after {    
                font-family: 'FontAwesome';
                content: "\f077";  /* symbol for "opening" panels * /
                float: right;
                color: white;
            }
            .box-title[data-toggle].collapsed:after {    
                content: "\f078";  /* symbol for "collapsed" panels * / 
            }
            */

            string cssCollapsed = !collapsed ? CSS_HEADER + " collapsed" : CSS_HEADER;
            string targetName = String.Format("#{0}", collapsePanel);

            TagBuilder tools = CreateTag("div", String.Empty, "tools");
            tools.InnerHtml = String.Format("<a class='{1}' data-toggle='" + CSS_COLLAPSE + "' data-parent='#accordion' href='{0}'></a>", targetName, CSS_COLLAPSE);
            
            TagBuilder tag = CreateTag("div", String.Empty, cssCollapsed, htmlAttributes);
            tag.InnerHtml = String.Format("{0}{1}", CreateTitle(title, icon), tools.ToString(TagRenderMode.Normal));
            tag.MergeAttribute("data-toggle", CSS_COLLAPSE);
            tag.MergeAttribute("data-target", targetName);

            this._writer.Write(tag.ToString(TagRenderMode.Normal));

            return this;
        }

        private string CreateTitle(string title, string icon = DEFAULT_ICON)
        {
            return String.Format("<h4 class='{0}'><span class='{1}'></span> {2}</h4>", CSS_TITLE, icon, title);
        }

        /// <summary>
        /// Inicia o corpo do painel
        /// </summary>
        /// <param name="htmlAttributes">Atributos da tag div contendo o conteúdo do painel.</param>
        /// <returns>Este objeto</returns>
        public MvcPanel BeginBody(object htmlAttributes = null)
        {
            return BeginBody(CSS_BODY, htmlAttributes);
        }

        private MvcPanel BeginBody(string cssBody, object htmlAttributes = null)
        {
            TagBuilder tag = CreateTag("div", String.Empty, cssBody, htmlAttributes);                  
            this._writer.Write(tag.ToString(TagRenderMode.StartTag));

            //this._writer.Write("<div class='" + cssBody + "'>");

            return this;
        }

        /// <summary>
        /// Iniciar o corpo do painel com função de collapse
        /// </summary>
        /// <param name="id">Id do painel que sofrerá o collapse</param>
        /// <param name="collapsed">Painel aberto ou fechado.</param>
        /// <param name="htmlAttributes">Atributos do painel.</param>
        /// <returns>Este objeto</returns>
        public MvcPanel BeginCollapseBody(string id, bool collapsed = true, object htmlAttributes = null)
        {
            TagBuilder collapse = CreateTag("div", id, "panel-collapse collapse" + (collapsed ? " in" : ""), htmlAttributes);                  
            this._writer.Write(collapse.ToString(TagRenderMode.StartTag));

            TagBuilder tag = CreateTag("div", String.Empty, CSS_BODY, htmlAttributes);            
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
                this.End().End(); // body and panel

                if (_hasCollapse)
                    this.End();

                if (_viewContext != null)
                {
                    _viewContext.OutputClientValidation();                    
                }
            }
        }     

        #endregion
    }
}
