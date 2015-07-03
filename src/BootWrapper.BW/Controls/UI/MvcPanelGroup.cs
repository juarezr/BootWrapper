using System.Web.Mvc;

namespace BootWrapper.BW.Controls
{
    /// <summary>
    /// Classe para criação de grupo de painéis.
    /// </summary>
    public class MvcPanelGroup : MvcBaseComponent<MvcPanelGroup>
    {
        #region Fields
        private static string COLLAPSE = "collapse";

        private int _nCollapseId;        
        private string _lastCollapseId = "collapse";

        private readonly string _id;
        private bool _open;     
        #endregion

        #region CTOR

        /// <summary>
        /// Initializes a new instance of the <see cref="MvcPanelGroup"/> class.
        /// </summary>
        /// <param name="viewContext">Contexto da view.</param>
        /// <param name="id">Id do grupo de painéis.</param>
        public MvcPanelGroup(ViewContext viewContext, string id = "accordion")
            : base(viewContext)
        {            
            _nCollapseId = 0;
            _id = id;            
        }

        #endregion

        #region Methods

        /// <summary>
        /// Inicializa o componente do grupo.
        /// </summary>
        /// <param name="htmlAttributes">Atributos do componente.</param>
        /// <returns>Retorna o próprio obeto.</returns>
        public MvcPanelGroup Begin(object htmlAttributes = null)
        {
            return Begin("panel-group", htmlAttributes);
        }

        /// <summary>
        /// Inicializa o componente do grupo.
        /// </summary>
        /// <param name="panelCssClass">CSS do painel.</param>
        /// <param name="htmlAttributes">Atributos do painel.</param>
        /// <returns>Retorna o próprio objeto.</returns>
        public MvcPanelGroup Begin(string panelCssClass, object htmlAttributes = null)
        {
            if (!_open)
            {
                TagBuilder tag = CreateTag("div", _id, panelCssClass, htmlAttributes);               

                this._writer.Write(tag.ToString(TagRenderMode.StartTag));
                _open = true;
            }

            return this;
        }

        /// <summary>
        /// Adiciona um painel ao grupo. <see cref="MvcPanel"/>
        /// </summary>
        /// <param name="title">Título do novo painel.</param>
        /// <param name="collapsed">Aberto ou Fechado.</param>
        /// <param name="panelHtmlAttributes">Atributos do painel.</param>
        /// <param name="titleHtmlAttributes">Atributos do painel de título.</param>
        /// <param name="contentHtmlAttributes">Atributos do painel de conteúdo.</param>
        /// <returns></returns>
        public MvcPanel AddPanel(string title, bool collapsed = true, 
                               object panelHtmlAttributes = null,
                               object titleHtmlAttributes = null,
                               object contentHtmlAttributes = null)
        {
            return AddPanel(string.Empty, title, collapsed, MvcPanel.DEFAULT_ICON, PanelColor.Default, panelHtmlAttributes, titleHtmlAttributes, contentHtmlAttributes);
        }      

        /// <summary>
        /// Adiciona um painel ao grupo. <see cref="MvcPanel"/>
        /// </summary>
        /// <param name="id">Id do novo painel.</param>
        /// <param name="title">Título do novo painel.</param>
        /// <param name="collapsed">Aberto ou Fechado.</param>
        /// <param name="icon">Ícone do painel.</param>
        /// <param name="panelHtmlAttributes">Atributos do painel.</param>
        /// <param name="titleHtmlAttributes">Atributos do painel de título.</param>
        /// <param name="contentHtmlAttributes">Atributos do painel de conteúdo.</param>
        /// <returns></returns>
        public MvcPanel AddPanel(string id, string title, bool collapsed = true,
                                string icon = MvcPanel.DEFAULT_ICON,
                                PanelColor color = PanelColor.Default,                                
                                object panelHtmlAttributes = null,
                                object titleHtmlAttributes = null,
                                object contentHtmlAttributes = null)
        {
            this._lastCollapseId = COLLAPSE + (++_nCollapseId);

            return new MvcPanel(_viewContext, true).Begin(id, color, panelHtmlAttributes).BeginCollapseHeader(title, collapsed, _lastCollapseId, icon, titleHtmlAttributes).BeginCollapseBody(_lastCollapseId, collapsed, contentHtmlAttributes);
        }

        #endregion
    }
}
