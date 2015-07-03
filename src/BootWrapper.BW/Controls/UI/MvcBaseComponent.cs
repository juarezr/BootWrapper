using BootWrapper.BW.Controls.Util;
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
    public abstract class MvcBaseComponent<T> : IDisposable where T : MvcBaseComponent<T>
    {
        

        #region Fields

        /// <summary>
        /// Tag 'div' padrão utilizada.
        /// </summary>
        public static string DEFAULT_TAG = "div";
        
        protected bool _disposed;
        protected readonly ViewContext _viewContext;
        protected readonly TextWriter _writer;
        
        #endregion

        #region properties

        public string Id { get; set; }
        public string CssClass{ get; set; }

        protected string Tag { get; set; }
        protected TagRenderMode TagRenderMode { get; set; }

        #endregion

        /// <summary>
        /// Construtor do componente.
        /// </summary>
        /// <param name="viewContext">Contexto da View.</param>
        public MvcBaseComponent(ViewContext viewContext)
            : this(viewContext, DEFAULT_TAG)
        {
        }

        /// <summary>
        /// Construtor do componente.
        /// </summary>
        /// <param name="viewContext">Context da View</param>
        /// <param name="tag">Nome da tag utilizada pelo componente html.</param>
        public MvcBaseComponent(ViewContext viewContext, string tag)
        {
            if (viewContext == null)
            {
                throw new ArgumentNullException("ViewContext is null!");
            }

            this.Tag = tag;

            _viewContext = viewContext;
            _writer = viewContext.Writer;
            TagRenderMode = TagRenderMode.Normal;
        }

        public virtual T SetId(string id) 
        {
            this.Id = id;
            return this as T;
        }

        public virtual T SetClass(string className)
        {
            this.Id = className;
            return this as T;
        }

        /// <summary>
        /// Obtém o contexto informado no construtor.
        /// </summary>
        /// <returns></returns>
        public ViewContext GetContext()
        {
            return _viewContext;
        }

        /// <summary>
        /// Obtém o stream de escrita para o contexto.
        /// </summary>
        /// <returns></returns>
        protected TextWriter GetWriter()
        {
            return _writer;
        }

        /// <summary>
        /// Obtém a rota atual da view.
        /// </summary>
        /// <returns>A rota no seguinte formato: /controller/action</returns>
        protected string GetCurrentRoute()
        {
            var httpContext = new HttpContextWrapper(HttpContext.Current);
            var routeData = System.Web.Routing.RouteTable.Routes.GetRouteData(httpContext);

            var controllerName = routeData.Values["controller"].ToString();
            var actionName = routeData.Values["action"].ToString();

            return String.Format("/{0}/{1}", controllerName, actionName);
        }
        
        /// <summary>
        /// Unifica o valor do atributo class original e se definido também no obetos htmlAttributes.
        /// </summary>
        /// <param name="originalValue">Classe original.</param>
        /// <param name="htmlAttributes">Attributos do componente.</param>
        /// <returns>Objeto contendo o atributo class contendo o valor original e valor definido emm htmlAttributes.</returns>
        public static RouteValueDictionary MergeClassAttributes(string originalValue, object htmlAttributes)
        {
            return AttributesHelper.MergeClassAttributes(originalValue, htmlAttributes);           
        }

        /// <summary>
        /// Finaliza o componenente.
        /// </summary>
        /// <returns>O componente será terminado no seguinte formado <c></nome_da_tag></c></returns>
        public virtual T End()
        {
            _writer.Write(String.Format("</{0}>", Tag));
            return this as T;
        }

        /// <summary>
        /// Finaliza o obeto.
        /// </summary>
        public virtual void Dispose()
        {
            EndIt();
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Finaliza o painel e fecha as tags necessárias.
        /// </summary>
        protected virtual void EndIt()
        {
            if (!this._disposed)
            {
                this._disposed = true;
                this.End();

                if (_viewContext != null)
                {
                    _viewContext.OutputClientValidation();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sizeStyle"></param>
        /// <returns></returns>
        protected static string GetStyleClass(Enum sizeStyle)
        {
            return BootstrapValueAttribute.GetStringValue(sizeStyle);
        }

        /// <summary>
        /// Cria um obeto TagBuilder que representa a Tag do componente.
        /// </summary>
        /// <param name="tagName">Nome da Tag.</param>
        /// <param name="id">Id da tag.</param>
        /// <param name="cssClass">Classe css</param>
        /// <param name="htmlAttributes">Atributos da tag.</param>
        /// <returns>Obeto TagBuilder.</returns>
        protected virtual TagBuilder CreateTag(object htmlAttributes = null)
        {
            return CreateTag(this.Tag, this.Id, this.CssClass, htmlAttributes);
        }


        /// <summary>
        /// Cria um obeto TagBuilder que representa a Tag do componente.
        /// </summary>
        /// <param name="tagName">Nome da Tag.</param>
        /// <param name="id">Id da tag.</param>
        /// <param name="cssClass">Classe css</param>
        /// <param name="htmlAttributes">Atributos da tag.</param>
        /// <returns>Obeto TagBuilder.</returns>
        protected virtual TagBuilder CreateTag(string id, string cssClass, object htmlAttributes = null)
        {
            return CreateTag(this.Tag, id, cssClass, htmlAttributes);
        }

        /// <summary>
        /// Cria um obeto TagBuilder que representa a Tag do componente.
        /// </summary>
        /// <param name="tagName">Nome da Tag.</param>
        /// <param name="id">Id da tag.</param>
        /// <param name="cssClass">Classe css</param>
        /// <param name="htmlAttributes">Atributos da tag.</param>
        /// <returns>Obeto TagBuilder.</returns>
        protected virtual TagBuilder CreateTag(string tagName, string id, string cssClass, object htmlAttributes = null)
        {
            var tag = new TagBuilder(tagName);

            if (!String.IsNullOrEmpty(id))
                tag.GenerateId(id);

            tag.MergeAttributes(AttributesHelper.UnwrapRouteValueDictionary(htmlAttributes));
            if (!String.IsNullOrEmpty(cssClass))
                tag.MergeAttributes(MergeClassAttributes(cssClass, htmlAttributes), false);

            return tag;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public MvcHtmlString ToHtml(object htmlAttributes = null)
        {
            return new MvcHtmlString(CreateTag(htmlAttributes).ToString(TagRenderMode));
        }

        /// <summary>
        /// Cria e escreve o botão no contexto da view.
        /// </summary>
        /// <param name="icon">Classe do ícone.</param>
        /// <returns>Este objeto.</returns>
        public MvcBaseComponent<T> Create(object htmlAttributes)
        {
            this.GetWriter().Write(CreateTag(htmlAttributes).ToString(TagRenderMode));
            return this;
        }
    }
}
