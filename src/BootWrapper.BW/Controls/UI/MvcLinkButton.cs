using BootWrapper.BW.Controls.Util;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace BootWrapper.BW.Controls
{
    /// <summary>
    /// Classe para criar um botão
    /// </summary>
    public class MvcLinkButton : MvcSketchButton<MvcLinkButton>
    {
         /// <summary>
        /// Nome da tag do botão.
        /// </summary>
        public const string DEFAULT_LINK_BUTTON_TAG = "a";

        #region properties

        internal string href;
   
        internal List<string> _params = new List<string>();

        #endregion

        public MvcLinkButton(ViewContext viewContext)
            : base(viewContext, DEFAULT_LINK_BUTTON_TAG)
        {
            Set(ButtonColor.Custom);
            Set(ButtonSize.Small);

            this._tagType = DEFAULT_BUTTON_TAG;
        }      

        public void SetHref(string url)
        {
            this.href = url;
        }

        public MvcLinkButton AddAction(string actionName, object routeValues = null)
        {
            return AddAction(actionName, string.Empty, routeValues);
        }

        public MvcLinkButton AddAction(string actionName, string controllerName, object routeValues = null)
        {
            AddUrlParam(actionName, controllerName, routeValues);
            return this;
        }

        private void AddUrlParam(string actionName, string controllerName, object routeValues)
        {
            var helper = new UrlHelper(GetContext().RequestContext);
            string url = string.IsNullOrWhiteSpace(controllerName)
                ? helper.Action(actionName, routeValues)
                : helper.Action(actionName, controllerName, routeValues);

            _params.Add(url);
        }

        private string ResolveUrl(string actionName, string controllerName, object routeValues)
        {
            var helper = new UrlHelper(GetContext().RequestContext);
            string url = string.IsNullOrWhiteSpace(controllerName)
                ? helper.Action(actionName, routeValues)
                : helper.Action(actionName, controllerName, routeValues);
            return url;
        }

        public MvcHtmlString ActionLink(string buttonCaption, string actionName, object routeValues = null)
        {
            return ActionLink(buttonCaption, actionName, string.Empty, routeValues);
        }

        public MvcHtmlString ActionLink(string buttonCaption, string actionName,
            string controllerName, object routeValues = null)
        {

            SetText(buttonCaption);
            string url = ResolveUrl(actionName, controllerName, routeValues);

            var attrib = new { @role = "button", href = url };            
            return ToHtml(attrib);
        }

        public MvcHtmlString UrlLink(string buttonCaption, string url, object htmlAttributes = null)
        {
            SetText(buttonCaption);            
                        
            var mergedAttributes = AttributesHelper.MergeAndOverrideAttributes(new { @role = "button", href = url }, htmlAttributes);
            
            return ToHtml(mergedAttributes);             
        }
     
        /// <summary>
        ///     Gera um botão na view que chama um código javascript.
        ///     Ex: @(Html.BWButton().AddAction("Delete").AddAction("Find").ButtonClick("Excluir", "executeAndGotoAction('{0}','{1}')"))
        ///         Vai substituir os {0} e {1} pela URL gerada por AddAction correspondente ficando o código como: executeAndGotoAction('Controller/Delete','Controller/Find')
        /// </summary>
        /// <param name="buttonCaption">Texto que será exibido no botão</param>
        /// <param name="jsCode">Código javascript para ser chamado</param>
        /// <returns>Gera um botão para ser usado no Razor da View</returns>
        public MvcHtmlString OnClick(string buttonCaption, string jsCode)
        {
            jsCode = jsCode.Replace(@"""", "'");
            int i = 0;
            foreach (var param in _params)
            {
                string pattern = "{" + i.ToString() + "}";
                jsCode = jsCode.Replace(pattern, param);
                i++;
            }
            
            SetText(buttonCaption);
            var attrib = new { @role = "button", onclick = jsCode };
            
            return ToHtml(attrib);
        }
       
    }
}
