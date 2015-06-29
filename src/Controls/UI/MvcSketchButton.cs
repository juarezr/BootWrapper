using BootWrapper.BW.Controls.Util;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace BootWrapper.BW.Controls
{
    /// <summary>
    /// Classe para criar um botão
    /// </summary>
    public abstract class MvcSketchButton<T> : MvcBaseComponent<T> where T : MvcSketchButton<T>
    {
        /// <summary>
        /// Nome da tag do botão.
        /// </summary>
        public const string DEFAULT_BUTTON_TAG = "button";

        #region properties

        internal string _tagType;
        internal string _text;
        internal string _color;
        internal string _size;
        internal string _icon;
        internal bool _enabled;

        internal byte _cols = 0;
        internal object _htmlAttributes = null;

        #endregion

        public MvcSketchButton(ViewContext viewContext)
            : this(viewContext, DEFAULT_BUTTON_TAG)
        {           
        }

        public MvcSketchButton(ViewContext viewContext, string tag)
            : base(viewContext, tag)
        {
            Set(ButtonColor.Custom);
            Set(ButtonSize.Small);

            this._tagType = tag;
        }

        public T Set(ButtonColor color)
        {
            return SetColor(GetStyleClass(color)); 
        }

        public T Set(ButtonSize size)
        {
            return SetSize(GetStyleClass(size));
        }

        public T Set(ButtonAction action)
        {
            return SetIcon(GetStyleClass(action));
        }
      
        private T SetColor(string colorClass) 
        {
            this._color = colorClass;
            return this as T;
        }

        public T SetSize(string sizeClass)
        {
            this._size = sizeClass;
            return this as T;
        }

        public T SetIcon(string faClass)
        {
            this._icon = faClass;
            return this as T;
        }

        public T SetText(string text)
        {
            this._text = text;
            return this as T;
        }
        public T SetTagType(string tagType)
        {
            this._tagType = tagType;
            return this as T;
        }

        public T Enable(bool enable = true)
        {
            this._enabled = enable;
            return this as T;
        }

        public T Cols(byte widthIn12Columns = 1)
        {
            _cols = widthIn12Columns;
            return this as T;
        }

        public T Attrib(object htmlAttributes)
        {
            _htmlAttributes = htmlAttributes;
            return this as T;
        }


        /// <summary>
        /// Obtém instância do objeto TagBuilder que representa a tag do botão.
        /// </summary>
        /// <param name="icon">Classe font-awesome.</param>
        /// <returns>Retorna obeto TagBuilder</returns>
        protected override TagBuilder CreateTag(object btnAttributes)
        {
            //this.CssClass = String.Format("btn {0} {1}", _color, _size);

            //var tag = base.CreateTag(btnAttributes);           
            //tag.MergeAttribute("type", "button");
            
            //tag.InnerHtml = String.Format("<span class='{0}'></span> {1}\r\n", _icon, _text);

            //return tag;

            //ButtonSettings
            var baseAtt = AttributesHelper.UnwrapRouteValueDictionary(btnAttributes);
            if (!string.IsNullOrWhiteSpace(Id))
                baseAtt.Add("id", Id);

            TagBuilder tag = new TagBuilder(_tagType);
            
            string btn = String.Format("btn {0} {1}", _color, _size);
            
            if (_cols != 0)
                btn += string.Format(" col-xs-{0}", _cols);
            tag.AddCssClass(btn);

            tag.MergeAttributes(baseAtt);
            var extraAtt = WebControls.ToRouteValueDictionary(_htmlAttributes);
            tag.MergeAttributes(extraAtt);

            if (!_enabled)
                tag.MergeAttribute("disabled", "disabled");

            tag.InnerHtml = string.IsNullOrWhiteSpace(_icon)
                    ? _text
                    : string.Format("<span class='{0}'></span> {1}", _icon, _text);

            return tag;
        }

        public MvcHtmlString Button(string buttonCaption, string buttonOrSubmit = DEFAULT_BUTTON_TAG)
        {
            this._text = buttonCaption;
            var attrib = new { @type = buttonOrSubmit };
            
            return ToHtml(attrib);
        }

        /// <summary>
        /// Botão para realizar o collapse de um painel.
        /// </summary>
        /// <param name="buttonCaption"></param>
        /// <param name="tagId"></param>
        /// <returns></returns>
        public MvcHtmlString Collapse(string buttonCaption, string panelToCollpaseId = "#divcollapse")
        {
            var attrib = new
            {
                role = "button",
                data_toggle = "collapse",
                aria_expanded = "false",
                aria_controls = panelToCollpaseId,
                href = panelToCollpaseId
            };
                        
            SetText(buttonCaption);

            return ToHtml(attrib);            
        }
    }
}
