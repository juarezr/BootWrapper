using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace BootWrapper.BW.Controls.Settings
{
    public class ControlSettings
    {
        #region Generic Properties

        internal byte _cols;
        internal byte _labelSize;

        internal string _name;
        internal string _label;
        internal string _id;
        internal string _value;
        internal string _placeholder;
        internal bool _required;

        internal object _htmlAttributes;
        internal HtmlHelper _htmlHelper;

        internal int _maxlength;

        public ControlSettings(HtmlHelper helper)
        {
            _cols = 0;
            _labelSize = 0;
            _name = string.Empty;
            _label = string.Empty;
            _id = string.Empty;
            _placeholder = string.Empty;
            _value = string.Empty;
            _required = false;
            _htmlAttributes = null;
            _htmlHelper = helper;
            _maxlength = 0;
        }

        public ControlSettings Id(string Id)
        {
            _id = Id;
            return this;
        }

        public ControlSettings Value(string value)
        {
            _value = value;
            return this;
        }

        public ControlSettings Required(bool mustFill = true)
        {
            _required = mustFill;
            return this;
        }

        public ControlSettings Placeholder(string text)
        {
            _placeholder = text;
            return this;
        }

        public ControlSettings Attrib(object htmlAttributes)
        {
            _htmlAttributes = htmlAttributes;
            return this;
        }

        #endregion

        #region Generic Methods

        private Dictionary<string, object> MergeInputAttributes()
        {
            var htmlAttributes = new Dictionary<string, object>();
            htmlAttributes.Add("name", _name);

            if ((_labelSize > 0) && (_labelSize <= 12))
                htmlAttributes.Add("class", string.Format("col-xs-{0}", 12 - _labelSize));
            else
                htmlAttributes.Add("class", "form-control");

            if (_required)
                htmlAttributes.Add("required", "required");
            if (_maxlength > 0)
                htmlAttributes.Add("maxlength", _maxlength.ToString());

            if (string.IsNullOrWhiteSpace(_id) && !string.IsNullOrWhiteSpace(_name))
            {
                _id = _name;
            }
            htmlAttributes.Add("id", _id);

            if (_htmlAttributes != null)
            {
                var attribs = HtmlHelper.AnonymousObjectToHtmlAttributes(_htmlAttributes);
                foreach (var att in attribs)
                {
                    if (htmlAttributes.ContainsKey(att.Key))
                        htmlAttributes.Remove(att.Key);
                    htmlAttributes.Add(att.Key, att.Value);
                }
            }
            return htmlAttributes;
        }

        protected TagBuilder CreateTagBuilder(string tagName)
        {
            var htmlAttributes = MergeInputAttributes();
            TagBuilder tagBuilder = new TagBuilder(tagName);
            tagBuilder.MergeAttributes(htmlAttributes);

            return tagBuilder;
        }

        protected MvcHtmlString WrapFormControl(TagBuilder tagInput,
            TagRenderMode renderMode = TagRenderMode.SelfClosing)
        {
            if ((_labelSize > 0) && (_labelSize <= 12))
                tagInput.AddCssClass(string.Format("col-xs-{0}", 12 - _labelSize));

            string sInput = tagInput.ToString(renderMode);
            return WrapFormControl(sInput);
        }

        protected MvcHtmlString WrapFormControl(string tagInput)
        {
            var tagDiv = new TagBuilder("div");
            tagDiv.AddCssClass(string.Format("col-xs-{0}", _cols));

            var tagLabel = new TagBuilder("label");
            tagLabel.MergeAttribute("for", _id);
            tagLabel.InnerHtml = string.IsNullOrWhiteSpace(_label) ? _name : _label;

            if ((_labelSize > 0) && (_labelSize <= 12))
                tagLabel.AddCssClass(string.Format("col-xs-{0}", _labelSize));

            tagDiv.InnerHtml = tagLabel.ToString(TagRenderMode.Normal)
                + Environment.NewLine
                + tagInput;

            return new MvcHtmlString(tagDiv.ToString(TagRenderMode.Normal));
        }

        protected MvcHtmlString WrapLabeledControl(TagBuilder tagInput, TagBuilder tagHidden, string cssClass,
            TagRenderMode renderMode = TagRenderMode.SelfClosing)
        {
            var tagDiv = new TagBuilder("div");
            tagDiv.AddCssClass(cssClass);
            tagDiv.AddCssClass(string.Format("col-xs-{0}", this._cols));

            string hidden = (tagHidden == null) ? string.Empty :
                tagHidden.ToString(renderMode) + Environment.NewLine;

            var tagLabel = new TagBuilder("label");
            tagLabel.InnerHtml = tagInput.ToString(renderMode)
                + hidden
                + Environment.NewLine
                + (string.IsNullOrWhiteSpace(_label) ? _name : _label);

            tagDiv.InnerHtml = tagLabel.ToString(TagRenderMode.Normal);

            return new MvcHtmlString(tagDiv.ToString(TagRenderMode.Normal));
        }

        #endregion

        #region Common Controls

        public enum ContextualBg
        {
            [BootstrapValue("bg-primary")]
            Primary,

            [BootstrapValue("bg-success")]
            Success,

            [BootstrapValue("bg-info")]
            Info,

            [BootstrapValue("bg-warning")]
            Warning,

            [BootstrapValue("bg-danger")]
            Danger
        }

        public MvcHtmlString ContextPanel()
        {
            return ContextPanel(ContextualBg.Primary, null);
        }

        public MvcHtmlString ContextPanel(ContextualBg background)
        {
            return ContextPanel(background, null);
        }

        public MvcHtmlString ContextPanel(string text, params object[] formatting)
        {
            return ContextPanel(ContextualBg.Primary, text, formatting);
        }

        public MvcHtmlString Error(string text, params object[] formatting)
        {
            return ContextPanel(ContextualBg.Danger, text, formatting);
        }

        public MvcHtmlString Warning(string text, params object[] formatting)
        {
            return ContextPanel(ContextualBg.Warning, text, formatting);
        }

        public MvcHtmlString Info(string text, params object[] formatting)
        {
            return ContextPanel(ContextualBg.Info, text, formatting);
        }

        public MvcHtmlString Success(string text, params object[] formatting)
        {
            return ContextPanel(ContextualBg.Success, text, formatting);
        }

        public MvcHtmlString ContextPanel(ContextualBg background, string text, params object[] formatting)
        {
            TagBuilder tagBuilder = new TagBuilder("p");
            tagBuilder.AddCssClass(WebControls.GetStyleClass(background));
            tagBuilder.InnerHtml = string.IsNullOrWhiteSpace(text)
                ? _value : string.Format(text, formatting);

            return WrapFormControl(tagBuilder, TagRenderMode.Normal);
        }

        public MvcHtmlString TextBox(bool numbersOnly, int maxlength = 0)
        {
            return TextBox(maxlength, numbersOnly);
        }

        public MvcHtmlString TextBox(int maxlength = 0, bool numbersOnly = false)
        {
            TagBuilder tagBuilder = CreateTagBuilder("input");

            tagBuilder.MergeAttribute("type", "text");
            tagBuilder.MergeAttribute("value", _value);

            if (numbersOnly)
                tagBuilder.MergeAttribute("digits", "true");

            if (maxlength > 0)
                tagBuilder.MergeAttribute("maxlength", maxlength.ToString());

            return WrapFormControl(tagBuilder);
        }

        public MvcHtmlString Password(int maxlength = 0)
        {
            TagBuilder tagBuilder = CreateTagBuilder("input");

            tagBuilder.MergeAttribute("type", "password");
            tagBuilder.MergeAttribute("value", _value);

            if (maxlength > 0)
                tagBuilder.MergeAttribute("maxlength", maxlength.ToString());

            return WrapFormControl(tagBuilder);
        }

        public MvcHtmlString TextArea(byte rows = 3)
        {
            TagBuilder tagBuilder = CreateTagBuilder("textarea");

            tagBuilder.InnerHtml = _value;

            if (rows > 0)
                tagBuilder.MergeAttribute("rows", rows.ToString());

            return WrapFormControl(tagBuilder, TagRenderMode.Normal);
        }

        public MvcHtmlString FixedText()
        {
            TagBuilder tagBuilder = CreateTagBuilder("p");
            tagBuilder.AddCssClass("form-control-static");
            tagBuilder.InnerHtml = _value;

            return WrapFormControl(tagBuilder, TagRenderMode.Normal);
        }

        public MvcHtmlString CheckBox(bool isChecked = false)
        {
            return CheckBoxTag(isChecked, "checkbox");
        }

        public MvcHtmlString CheckBoxInline(bool isChecked = false)
        {
            return CheckBoxTag(isChecked, "checkbox-inline");
        }

        protected MvcHtmlString CheckBoxTag(bool isChecked, string cssClass)
        {
            TagBuilder tagBuilder = CreateTagBuilder("input");
            tagBuilder.MergeAttribute("type", "checkbox");
            tagBuilder.MergeAttribute("value", "true");

            TagBuilder tagBuilder2 = CreateTagBuilder("input");
            tagBuilder2.MergeAttribute("type", "hidden");
            tagBuilder2.MergeAttribute("value", "false");

            if (!isChecked)
                isChecked = _value.Equals("true")
                    || !_value.Equals("0")
                    || _value.Equals("Y")
                    || _value.Equals("S");

            if (isChecked)
                tagBuilder.MergeAttribute("checked", "checked");

            return WrapLabeledControl(tagBuilder, tagBuilder2, cssClass);
        }

        public MvcHtmlString Radio(bool isChecked = false, string value = "")
        {
            return Radio(value, isChecked);
        }

        public MvcHtmlString Radio(string value = null, bool isChecked = false)
        {
            return RadioTag(value, isChecked, "radio");
        }

        public MvcHtmlString RadioInline(bool isChecked = false, string value = "")
        {
            return RadioInline(value, isChecked);
        }

        public MvcHtmlString RadioInline(string value = null, bool isChecked = false)
        {
            return RadioTag(value, isChecked, "radio-inline");
        }

        protected MvcHtmlString RadioTag(string value, bool isChecked, string cssClass)
        {
            TagBuilder tagBuilder = CreateTagBuilder("input");
            tagBuilder.MergeAttribute("type", "Radio");
            if (value != "")
                tagBuilder.MergeAttribute("value", value);

            if (!isChecked)
                isChecked = _value.Equals("true")
                    || !_value.Equals("0")
                    || _value.Equals("Y")
                    || _value.Equals("S");

            if (isChecked)
                tagBuilder.MergeAttribute("checked", "checked");

            return WrapLabeledControl(tagBuilder, null, cssClass);
        }

        public MvcHtmlString DropDownList(IEnumerable items, object selectedValue = null, string emptyItemText = null)
        {
            SelectList selectList = (selectedValue == null)
                ? new SelectList(items)
                : new SelectList(items, selectedValue);
            return DropDownList(selectList, emptyItemText);
        }

        public MvcHtmlString DropDownList(IEnumerable items, string dataValueField, string dataTextField, object selectedValue = null, string emptyItemText = null)
        {
            SelectList selectList = (selectedValue == null)
                ? new SelectList(items, dataValueField, dataTextField)
                : new SelectList(items, dataValueField, dataTextField, selectedValue);
            return DropDownList(selectList, emptyItemText);
        }

        protected MvcHtmlString DropDownList(IEnumerable<SelectListItem> selectList, string emptyItemText = null)
        {
            var htmlAttributes = MergeInputAttributes();
            var tagSelect = _htmlHelper.DropDownList(_name, selectList, emptyItemText, htmlAttributes).ToString();
            return WrapFormControl(tagSelect);
        }

        public MvcHtmlString DropDownList<TEntityType>(Expression<Func<TEntityType, object>> dataValueField, Expression<Func<TEntityType, object>> dataTextField, object selectedValue = null, string emptyItemText = null)
        {
            var typeName = typeof(TEntityType).Name;
            var dataValueName = WebControls.BWPropertyName(dataValueField);
            var dataTextName = WebControls.BWPropertyName(dataTextField);
            return DropDownList(typeName, dataValueName, dataTextName, emptyItemText);
        }

        public MvcHtmlString DropDownList(string viewDataIEnumerable, string dataValueField, string dataTextField, object selectedValue = null, string emptyItemText = null)
        {
            var items = _htmlHelper.ViewData[viewDataIEnumerable];
            if (items == null)
                return Error("Dados de '{0}' não encontrado em ViewData.", viewDataIEnumerable);
            if (!(items is IEnumerable))
                return Error("Dados de '{0}' do ViewData não é IEnumerable.", viewDataIEnumerable);
            var enumitems = (IEnumerable)items;
            return DropDownList(enumitems, dataValueField, dataTextField, emptyItemText);
        }

        #endregion

        #region Especific Controls

        #endregion
    }
}
