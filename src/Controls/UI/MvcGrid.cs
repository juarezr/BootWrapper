using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web;
using System.Web.Mvc;
using iTextSharp.tool.xml.html;
using BootWrapper.BW.Controls.Util;
using System.Web.Routing;

namespace BootWrapper.BW.Controls
{
    /// <summary>
    /// Representa um Jquery DataTable. Essa classe funciona em parceria com o javacript Raizen.Primavera.DataTable.js
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    public class MvcGrid<TModel> : MvcBaseComponent<MvcGrid<TModel>>
    {
        #region Table Body

        /// <summary>
        /// Construtor padrão.
        /// </summary>
        /// <param name="viewContext">ViewContext da View criando o datatable.</param>
        public MvcGrid(ViewContext viewContext)
            : base(viewContext) { }

        /// <summary>
        /// Inicializa o datatable
        /// </summary>
        /// <param name="id"></param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public MvcGrid<TModel> Begin(string id, object htmlAttributes = null)
        {
            TagBuilder tag = new TagBuilder("table");
            tag.MergeAttribute("id", id);
            tag.MergeAttribute("data-table", "true");

            tag.AddCssClass("table table-striped");
            tag.MergeAttributes(MergeClassAttributes("table table-striped", htmlAttributes), false);

            var w = this.GetWriter();
            w.WriteLine(tag.ToString(TagRenderMode.StartTag));
            w.WriteLine("  <thead>");
            w.WriteLine("    <tr>");

            return this;
        }

        /// <summary>
        /// Realizar o dispose do objeto.
        /// </summary>
        public override void Dispose()
        {
            if (!this._disposed)
            {
                this._disposed = true;

                var w = this.GetWriter();
                w.WriteLine("    </tr>");
                w.WriteLine("  </thead>");
                w.WriteLine("  <tbody>");
                w.WriteLine("          <!-- será preenchido por ajax no click do botão find -->");
                w.WriteLine("  </tbody>");
                w.WriteLine("  </table>");

                if (_viewContext != null)
                {
                    _viewContext.OutputClientValidation();
                }
            }
        }

        #endregion

        #region Column Methods

        /// <summary>
        /// Propriedade auxiliar para dar título da coluna.
        /// </summary>
        protected string _title = string.Empty;

        /// <summary>
        /// Propriedade auxiliar para armazear os atributos da coluna.
        /// </summary>
        protected object _htmlAttributes = null;

        /// <summary>
        /// Define o título e atributos da coluna
        /// </summary>
        /// <param name="Title"></param>
        /// <param name="Attributes"></param>
        /// <returns>O próprio objeto</returns>
        public MvcGrid<TModel> Set(string Title, object Attributes = null)
        {
            _title = Title;
            _htmlAttributes = Attributes;
            return this;
        }

        /// <summary>
        /// Cria a coluna com as propriedades informadas.
        /// </summary>
        /// <param name="fieldName">Nome da coluna</param>
        /// <param name="baseAttributes">Atributos da coluna</param>
        /// <returns>O próprio objeto</returns>
        protected MvcGrid<TModel> RenderColumn(string fieldName, object baseAttributes = null)
        {
            TagBuilder tag = new TagBuilder("th");
            tag.MergeAttribute("data-field", fieldName);
            tag.MergeAttribute("data-header", _title);

            var atributes = WebControls.MergeAndOverrideAttributes(baseAttributes, _htmlAttributes);
            tag.MergeAttributes(atributes, true);

            tag.InnerHtml = string.IsNullOrWhiteSpace(_title) ? fieldName : _title;
            var w = this.GetWriter();
            w.WriteLine(tag.ToString(TagRenderMode.Normal));

            _title = null;
            _htmlAttributes = null;
            return this;
        }

        /// <summary>
        /// Criar uma coluna.
        /// </summary>
        /// <param name="fieldName">Nome da coluna.</param>
        /// <returns>O próprio objeto</returns>
        public MvcGrid<TModel> Column(string fieldName)
        {
            return RenderColumn(fieldName);
        }

        /// <summary>
        /// Cria uma coluna para a propriedade de algum Model.
        /// </summary>
        /// <param name="expression">Expressão lambda do model</param>
        /// <returns>O próprio objeto</returns>
        public MvcGrid<TModel> Column(Expression<Func<TModel, object>> expression)
        {
            Set(AttributesHelper.DiscoverDisplay(expression));
            string fieldName = AttributesHelper.GetPropertyName(expression);

            return Column(fieldName);
        }

        /// <summary>
        /// Cria uma coluna de data.
        /// </summary>
        /// <param name="fieldName">Nome da coluna</param>
        /// <returns>O próprio objeto</returns>
        public MvcGrid<TModel> DateColumn(string fieldName)
        {
            var baseAttributes = new
            {
                data_display_date = "true"
            };
            return RenderColumn(fieldName, baseAttributes);
        }

        /// <summary>
        /// Cria uma coluna de data.
        /// </summary>
        /// <param name="expression">Expressão lambda do model</param>
        /// <returns>O próprio objeto</returns>
        public MvcGrid<TModel> DateColumn(Expression<Func<TModel, object>> expression)
        {            
            Set(AttributesHelper.DiscoverDisplay(expression));
            string fieldName = WebControls.BWPropertyName(expression);
            return DateColumn(fieldName);
        }

        /// <summary>
        /// Cria uma coluna com checkbox
        /// </summary>
        /// <param name="fieldName">Nome da coluna</param>
        /// <returns>O próprio objeto</returns>
        public MvcGrid<TModel> Checkbox(string fieldName)
        {
            var baseAttributes = new
            {
                data_display_checkbox = "true"
            };
            return RenderColumn(fieldName, baseAttributes);
        }

        /// <summary>
        /// Cria uma coluna com checkbox
        /// </summary>
        /// <param name="expression">Expressão lambda do model</param>
        /// <returns>O próprio objeto</returns>
        public MvcGrid<TModel> Checkbox(Expression<Func<TModel, object>> expression)
        {
            Set(AttributesHelper.DiscoverDisplay(expression));
            string fieldName = WebControls.BWPropertyName(expression);
            return Checkbox(fieldName);
        }

        /// <summary>
        /// Cria uma coluna com link qualquer.
        /// </summary>
        /// <param name="fieldName">Nome da coluna</param>
        /// <param name="linkURL">Texto Url da coluna</param>
        /// <returns>O próprio objeto</returns>
        public MvcGrid<TModel> Link(string fieldName, string linkURL)
        {
            var baseAttributes = new 
            {
                data_action_link= linkURL
            }            

            return RenderColumn(fieldName, baseAttributes);
        }

        /// <summary>
        /// Cria uma coluna com link qualquer
        /// </summary>
        /// <param name="expression">Expressão lambda do model</param>
        /// <param name="linkURL">Texto Url da coluna</param>
        /// <returns>O próprio objeto</returns>
        public MvcGrid<TModel> Link(Expression<Func<TModel, object>> expression, string linkURL)
        {
            Set(AttributesHelper.DiscoverDisplay(expression));
            string fieldName = WebControls.BWPropertyName(expression);
            return Link(fieldName, linkURL);
        }
        

        /// <summary>
        /// Cria uma coluna com link qualquer
        /// </summary>
        /// <param name="expression">Expressão lambda do model</param>
        /// <param name="linkURL">Texto Url da coluna</param>
        /// <returns>O próprio objeto</returns>
        public MvcGrid<TModel> TemplateColumn(Expression<Func<TModel, object>> expression, string jsFunction)
        {
            Set(AttributesHelper.DiscoverDisplay(expression));
            string fieldName = WebControls.BWPropertyName(expression);

            var baseAttributes = new
            {                
                data_template=jsFunction
            };

            return RenderColumn(fieldName, baseAttributes);
        }

       
        /// <summary>
        /// Cria uma coluna com link de acordo com os parametros informados.
        /// </summary>
        /// <param name="fieldName">Nome da coluna</param>
        /// <param name="actionName">Nome da Action</param>
        /// <param name="controllerName">Nome do Controller</param>
        /// <param name="routeValues">Valores da rota</param>
        /// <returns>O próprio objeto</returns>
        private string BuildActionURL(string fieldName, string actionName, string controllerName, object routeValues)
        {
            var route = GetContext().RouteData.Values;
            var newId = new { Id = fieldName };
            route = WebControls.MergeAndOverrideAttributes(route, newId);
            route = WebControls.MergeAndOverrideAttributes(route, routeValues);

            var helper = new UrlHelper(GetContext().RequestContext);
            string url = string.IsNullOrWhiteSpace(controllerName)
                ? helper.Action(actionName, route)
                : helper.Action(actionName, controllerName, route);
            url = url.Replace(fieldName, "{" + fieldName + "}");

            var f1 = route["function"];
            string function = (f1 == null) ? "gotoURL" : f1.ToString();
            string onclick = string.Format("{0}('{1}')", function, url);
            return onclick;
        }

        /// <summary>
        /// Cria coluna com link que chama uma action.
        /// </summary>
        /// <param name="fieldName">Nome da coluna</param>
        /// <param name="actionName">Nome da Action</param>
        /// <param name="routeValues">Parâmetros da rota.</param>
        /// <returns>O próprio objeto</returns>
        public MvcGrid<TModel> LinkClick(string fieldName, string actionName, object routeValues = null)
        {
            return LinkClick(fieldName, actionName, null, routeValues);
        }

        /// <summary>
        /// Cria coluna com link que chama uma action.
        /// </summary>
        /// <param name="fieldName">Nome da coluna</param>
        /// <param name="actionName">Nome da Action</param>
        /// <param name="controllerName">Nome do Controller</param>
        /// <param name="routeValues">Parâmetros da rota.</param>
        /// <returns>O próprio objeto</returns>
        public MvcGrid<TModel> LinkClick(string fieldName, string actionName, string controllerName, object routeValues = null)
        {
            string onclick = BuildActionURL(fieldName, actionName, controllerName, routeValues);
            var baseAttributes = new
            {
                data_action_function = string.Format("{0}('{{{1}}}')", onclick)
            };
            return RenderColumn(fieldName, baseAttributes);
        }

        /// <summary>
        /// Cria coluna com link que chama uma action.
        /// </summary>
        /// <param name="expression">Expressão Lambda do Model</param>
        /// <param name="actionName">Nome da Action</param>        
        /// <param name="routeValues">Parâmetros da rota.</param>
        /// <returns>O próprio objeto</returns>
        public MvcGrid<TModel> LinkClick(Expression<Func<TModel, object>> expression, string actionName, object routeValues = null)
        {
            return LinkClick(expression, actionName, null, routeValues);
        }

        /// <summary>
        /// Cria coluna com link que chama uma action.
        /// </summary>
        /// <param name="expression">Expressão Lambda do Model</param>
        /// <param name="actionName">Nome da Action</param>    
        /// <param name="controllerName">Nome do Controller</param>
        /// <param name="routeValues">Parâmetros da rota.</param>
        /// <returns>O próprio objeto</returns>
        public MvcGrid<TModel> LinkClick(Expression<Func<TModel, object>> expression, string actionName, string controllerName, object routeValues = null)
        {
            string fieldName = WebControls.BWPropertyName(expression);
            return LinkClick(fieldName, actionName, controllerName, routeValues);
        }

        /// <summary>
        /// Cria coluna com botão que chama Action
        /// </summary>
        /// <param name="fieldName">Nome da Coluna</param>
        /// <param name="actionName">Nome da action</param>        
        /// <param name="routeValues">Parâmetros da rota.</param>
        /// <returns>O próprio objeto</returns>
        public MvcGrid<TModel> ButtonClick(string fieldName, string actionName, object routeValues = null)
        {
            return ButtonClick(fieldName, actionName, null, routeValues);
        }

        /// <summary>
        /// Cria coluna com botão que chama Action
        /// </summary>
        /// <param name="fieldName">Nome da Coluna</param>
        /// <param name="actionName">Nome da action</param>        
        /// <param name="controllerName">Nome do Controller</param>
        /// <param name="routeValues">Parâmetros da rota.</param>
        /// <returns>O próprio objeto</returns>
        public MvcGrid<TModel> ButtonClick(string fieldName, string actionName, string controllerName, object routeValues = null)
        {
            string onclick = BuildActionURL(fieldName, actionName, controllerName, routeValues);

            object base1 = new { data_action_button = onclick };
            object base2 = new { data_action_button = onclick, data_display_format = _title };
            var base3 = string.IsNullOrWhiteSpace(_title) ? base1 : base2;

            return RenderColumn(fieldName, base3);
        }

        /// <summary>
        /// Cria coluna com botão que chama Action
        /// </summary>
        /// <param name="expression">Expressao  Lambda do model</param>
        /// <param name="actionName">Nome da action</param>                
        /// <param name="routeValues">Parâmetros da rota.</param>
        /// <returns>O próprio objeto</returns>s"></param>        
        public MvcGrid<TModel> ButtonClick(Expression<Func<TModel, object>> expression, string actionName, object routeValues = null)
        {
            return ButtonClick(expression, actionName, null, routeValues);
        }

        /// <summary>
        /// Cria coluna com botão que chama Action
        /// </summary>
        /// <param name="expression">Expressao  Lambda do model</param>
        /// <param name="actionName">Nome da action</param>        
        /// <param name="controllerName">Nome do Controller</param>
        /// <param name="routeValues">Parâmetros da rota.</param>
        /// <returns>O próprio objeto</returns>
        public MvcGrid<TModel> ButtonClick(Expression<Func<TModel, object>> expression, string actionName, string controllerName, object routeValues = null)
        {
            string fieldName = WebControls.BWPropertyName(expression);
            return ButtonClick(fieldName, actionName, controllerName, routeValues);
        }

        /// <summary>
        /// Cria coluna com botão que faz chamada a função js.
        /// </summary>
        /// <param name="fieldName">Nome da coluna</param>
        /// <param name="jsFunction">Nome da função JavaScript</param>
        /// <returns>O próprio objeto</returns>
        public MvcGrid<TModel> JSButtonClick(string fieldName, string jsFunction)
        {
            object base1 = new { data_action_button = jsFunction };
            object base2 = new { data_action_button = jsFunction, data_display_format = _title };
            var base3 = string.IsNullOrWhiteSpace(_title) ? base1 : base2;

            return RenderColumn(fieldName, base3);
        }

        public MvcGrid<TModel> DeleteClick(Expression<Func<TModel, object>> expression, string itemColumnName, string route)
        {
            string fieldName = WebControls.BWPropertyName(expression);
            string dataDelete = HttpUtility.UrlDecode(route);

            object base1 = new { data_delete = dataDelete, item_column_Name = itemColumnName };
            object base2 = new { data_delete = dataDelete, item_column_Name = itemColumnName, data_display_format = _title };
            var baseAttributes = string.IsNullOrWhiteSpace(_title) ? base1 : base2;

            return RenderColumn(fieldName, baseAttributes);
        }

        #endregion
    }
}
