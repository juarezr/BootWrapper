using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;
using BootWrapper.BW.Controls.Settings;
using BootWrapper.BW.Formatter;
using BootWrapper.BW.Core.Translator;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using BootWrapper.BW.Controls.Util;

namespace BootWrapper.BW.Controls
{
    /// <summary>
    /// Controls to encapsulate classes from Boostrap. Translation is available as well. All controller will be the text translated for the given id .
    /// </summary>
    /// <author>Andre Farina</author>    
    public static class WebControls
    {
        // Common Resource Key for translations        
        public static string BTN_SAVE_KEY = "btnSave";
        public static string BTN_EDIT_KEY = "btnEdit";
        public static string BTN_REMOVE_KEY = "btnRemove";
        public static string BTN_LOGIN_KEY = "btnLogin";
        public static string BTN_SEARCH_KEY = "btnSearch";
        public static string BTN_RELOAD_KEY = "btnReload";
        // End Common Keys

        public static string BWPropertyName<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
        {
            return BWPropertyName(expression);
        }

        public static string BWPropertyName<TModel, TProperty>(Expression<Func<TModel, TProperty>> expression)
        {
            return AttributesHelper.GetPropertyName(expression);
            }

        public static RouteValueDictionary MergeAndOverrideAttributes(object baseAttributes, object htmlAttributes)
        {
            return AttributesHelper.MergeAndOverrideAttributes(baseAttributes, htmlAttributes);
        }

        public static RouteValueDictionary MergeAndOverrideAttributes(RouteValueDictionary baseAttributes, object htmlAttributes)
        {
            return AttributesHelper.MergeAndOverrideAttributes(baseAttributes, htmlAttributes);
        }


        public static RouteValueDictionary UnwrapRouteValueDictionary(object htmlAttributes)
        {
            return AttributesHelper.UnwrapRouteValueDictionary(htmlAttributes);
        }

        public static string DiscoverDisplay<TModel, TProperty>(Expression<Func<TModel, TProperty>> expression)
        {
            return AttributesHelper.DiscoverDisplay(expression);            
            }

        public static RouteValueDictionary ToRouteValueDictionary(object htmlAttributes)
        {
            var att = (RouteValueDictionary)((htmlAttributes is RouteValueDictionary) ? htmlAttributes : UnwrapRouteValueDictionary(htmlAttributes));
            return att;
        }

        public static string GetStyleClass(Enum sizeStyle)
        {
            return BootstrapValueAttribute.GetStringValue(sizeStyle);
        }

        public static string GetDateFormat(Enum datePicker)
        {
            return DatePickerFormatAttribute.GetFormatValue(datePicker);
        }

        public static int GetDateMinViewMode(Enum datePicker)
        {
            return DatePickerFormatAttribute.GetMinViewModeValue(datePicker);
        }

        public static TAttribute BWGetAttribute<TIn, TOut, TAttribute>(this Expression<Func<TIn, TOut>> expression) where TAttribute : Attribute
        {
            var memberExpression = expression.Body as MemberExpression;
            var attributes = memberExpression.Member.GetCustomAttributes(typeof(TAttribute), true);
            return attributes.Length > 0 ? attributes[0] as TAttribute : null;
        }

        #region Translator

        private static string _UseControllerName = String.Empty;
        public static void BWUseControllerForTranslation(this HtmlHelper htmlHelper, string controllerName)
        {
            WebControls._UseControllerName = controllerName;
        }

        private static string _UseActionName = String.Empty;
        public static void BWUseActionForTranslation(this HtmlHelper htmlHelper, string actionName)
        {
            WebControls._UseActionName = actionName;
        }

        public static void BWUseRouteForTranslation(this HtmlHelper htmlHelper, string controllerName, string actionName)
        {
            WebControls._UseActionName = actionName;
            WebControls._UseControllerName = controllerName;
        }

        public static string BWGetCurrentController(this HtmlHelper htmlHelper)
        {
            RouteData routeData = htmlHelper.ViewContext.RouteData;
            return String.IsNullOrEmpty(_UseControllerName) ? routeData.Values["controller"].ToString() : _UseControllerName;
        }

        public static string BWGetCurrentAction(this HtmlHelper htmlHelper)
        {
            RouteData routeData = htmlHelper.ViewContext.RouteData;
            return String.IsNullOrEmpty(_UseActionName) ? routeData.Values["action"].ToString() : _UseActionName;
        }

        public static string BWGetCurrentRoute(this HtmlHelper htmlHelper)
        {
            return String.Format("/{0}/{1}", htmlHelper.BWGetCurrentController(), htmlHelper.BWGetCurrentAction());
        }

        public static string BWGetLocalString(this HtmlHelper htmlHelper, string resourceKey, string defaultValue = "")
        {
            return Translator.GetLocalString(htmlHelper.BWGetCurrentRoute(), resourceKey, defaultValue);
        }

        public static string BWGetCommonString(this HtmlHelper htmlHelper, string resourceKey, string defaultValue = "")
        {
            return Translator.GetCommonString(resourceKey, defaultValue);
        }

        public static string BWTranslateLocalString(this HtmlHelper htmlHelper, string resourceKey, params object[] args)
        {
            try
            {
                return String.Format(htmlHelper.BWGetLocalString(resourceKey), args);
            }
            catch
            {
                return resourceKey;
            }
        }

        public static string BWTranslateCommonString(this HtmlHelper htmlHelper, string resourceKey, params object[] args)
        {
            try
            {
                return String.Format(htmlHelper.BWGetCommonString(resourceKey), args);
            }
            catch
            {
                return resourceKey;
            }
        }


        #endregion

        #region Control

        /// <summary>
        /// Helps build a html div element
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="id">The div id.</param>
        /// <returns></returns>
        public static MvcDiv BWDivHelper(this HtmlHelper htmlHelper, string id)
        {
            return new MvcDiv(htmlHelper.ViewContext).Begin(id, null, null) as MvcDiv;
        }

        /// <summary>
        /// Helps build a html div element
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <returns></returns>
        public static MvcDiv BWDivHelper(this HtmlHelper htmlHelper, string id, string cssClass, object htmlAttributes = null)
        {
            return new MvcDiv(htmlHelper.ViewContext).Begin(id, cssClass, htmlAttributes) as MvcDiv;
        }

        public static MvcDiv BWBeginControl(this HtmlHelper htmlHelper, object htmlAttributes = null)
        {
            return BWBeginControl(htmlHelper, null, htmlAttributes);
        }

        public static MvcDiv BWBeginControl(this HtmlHelper htmlHelper, string id, object htmlAttributes = null)
        {
            return BWDivHelper(htmlHelper, id, "form-group", htmlAttributes);
        }

        public static MvcDiv BWRow(this HtmlHelper htmlHelper, object htmlAttributes = null)
        {
            return BWDivHelper(htmlHelper, null, "row", htmlAttributes);
        }

        public static MvcDiv BWCol(this HtmlHelper htmlHelper, int colspan, object htmlAttributes = null)
        {
            return BWDivHelper(htmlHelper, null, String.Format("col-lg-{0}", colspan), htmlAttributes);
        }

        public static MvcDiv BWCol(this HtmlHelper htmlHelper, int colspan, int offset, object htmlAttributes = null)
        {
            return BWDivHelper(htmlHelper, null, String.Format("col-lg-{0} col-lg-offset-{1}", colspan, offset), htmlAttributes);
        }


        #endregion

        #region MessageHolder

        public static MvcHtmlString BWMessageContainer(this HtmlHelper htmlHelper, string id)
        {
            TagBuilder msgBuilder = new TagBuilder("div");
            msgBuilder.Attributes.Add("id", id);
            msgBuilder.MergeAttribute("style", "margin: 10px;");
            msgBuilder.AddCssClass("horizontal-padding");

            TagBuilder ctlTagBuilder = new TagBuilder("div");
            ctlTagBuilder.AddCssClass("form-group");
            ctlTagBuilder.InnerHtml += msgBuilder.ToString();

            return new MvcHtmlString(ctlTagBuilder.ToString());
        }

        public static MvcHtmlString BWMessageContainer(this HtmlHelper htmlHelper)
        {
            return BWMessageContainer(htmlHelper, "message-container");
        }

        #endregion

        #region Labels

        public static MvcHtmlString BWLabel(this HtmlHelper htmlHelper, string label, string fieldName, bool translate = false, object htmlAttributes = null)
        {
            var translated = translate ? htmlHelper.BWGetLocalString(label, fieldName) : label;

            TagBuilder tagLabel = new TagBuilder("label");
            tagLabel.MergeAttribute("for", fieldName);
            tagLabel.AddCssClass("control-label");
            tagLabel.MergeAttributes(UnwrapRouteValueDictionary(htmlAttributes));
            tagLabel.InnerHtml = translated;

            return new MvcHtmlString(tagLabel.ToString(TagRenderMode.Normal));

            //var snippet = new StringBuilder();
            //snippet.AppendFormat("<label class='control-label' id='{0}' for='{1}'>{2}</label>\r\n", id, fieldName, translated);
            //return new MvcHtmlString(snippet.ToString());
        }

        public static MvcHtmlString BWLabel(this HtmlHelper htmlHelper, string fieldName, bool translate = false, object htmlAttributes = null)
        {
            return BWLabel(htmlHelper, fieldName, fieldName, translate, htmlAttributes);
        }

        public static MvcHtmlString BWLabelFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, bool translate = false, object htmlAttributes = null)
        {
            //var name = ExpressionHelper.GetExpressionText(expression);
            var metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            var txtName = BWPropertyName(htmlHelper, expression);
            var displayName = DiscoverDisplay(expression);

            if (!translate && !String.IsNullOrEmpty(displayName))
            {
                return BWLabel(htmlHelper, displayName, txtName, translate, htmlAttributes);
            }

            return BWLabel(htmlHelper, metadata.PropertyName, txtName, translate, htmlAttributes);
        }

        #endregion

        #region Password

        public static MvcHtmlString BWPassword(this HtmlHelper htmlHelper, InputTextSize size, string id, string name, int maxLength, object htmlAttributes = null)
        {
            var translated = htmlHelper.BWGetLocalString(id, id);

            var snippet = new StringBuilder();
            snippet.AppendFormat(@"<i class='fa fa-lock'></i>");            
            htmlHelper.ViewContext.Writer.Write(snippet.ToString());

            //var snippet = new StringBuilder();
            //snippet.AppendFormat("<i class='fa fa-lock'></i><input class='form-control {0}' type='password' id='{1}' placeholder='{3}' value='' name='{2}'  required>\r\n", GetStyleClass(size), id, name, translated);

            
            var tagBuilder = new TagBuilder("input");

            tagBuilder.AddCssClass(String.Format("form-control {0}", GetStyleClass(size)));
            tagBuilder.Attributes.Add("type", "password");
            tagBuilder.Attributes.Add("id", id);
            tagBuilder.Attributes.Add("name", name);
            tagBuilder.Attributes.Add("required", "required");
            tagBuilder.Attributes.Add("maxlength", maxLength.ToString());

            tagBuilder.MergeAttributes(UnwrapRouteValueDictionary(MergeAndOverrideAttributes(htmlAttributes, new { placeholder = translated })));

            return new MvcHtmlString(tagBuilder.ToString(TagRenderMode.Normal));
        }

        public static MvcHtmlString BWPassword(this HtmlHelper htmlHelper, string id, string name, object htmlAttributes = null)
        {
            return BWPassword(htmlHelper, InputTextSize.Medium, id, name, Int32.MaxValue, htmlAttributes);
        }

        public static MvcHtmlString BWPassword(this HtmlHelper htmlHelper, string id, object htmlAttributes = null)
        {
            return BWPassword(htmlHelper, id, id, htmlAttributes);
        }

        public static MvcHtmlString BWPasswordFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, object htmlAttributes = null)
        {
            var metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            var htmlFieldName = ExpressionHelper.GetExpressionText(expression);
            var txtId = metadata.PropertyName ?? htmlFieldName.Split('.').Last();
            var txtName = BWPropertyName(htmlHelper, expression);

            object htmlAttributes2 = null;
            var displayName = DiscoverDisplay(expression);
            if (!String.IsNullOrEmpty(displayName))
            {
                htmlAttributes2 = new { placeholder = displayName };
            }

            int maxlength = Int32.MaxValue;
            var strLenAttribute = expression.BWGetAttribute<TModel, TProperty, StringLengthAttribute>();
            if (strLenAttribute != null)
            {
                maxlength = strLenAttribute.MaximumLength;
            }

            return BWPassword(htmlHelper, InputTextSize.Medium, txtId, txtName, maxlength, MergeAndOverrideAttributes(htmlAttributes, htmlAttributes2));
        }


        #endregion

        #region AutoComplete
        public static string ConvertJsonToString(object json)
        {
            var routeValues = UnwrapRouteValueDictionary(json);

            var builder = new StringBuilder("{");
            var k = 0;
            for (int i = 0; i < (2 * routeValues.Keys.Count) - 1; i++)
            {
                if (i.IsEven())
                {
                    builder.Append(String.Format("\"{0}\" : \"{1}\"", routeValues.Keys.ElementAt(k), routeValues.Values.ElementAt(k).ToString()));
                    k++;
                }
                else
                    builder.Append(",");
            }

            builder.Append("}");

            return builder.ToString();
        }

        public static MvcHtmlString BWAutoComplete(this HtmlHelper htmlHelper, string id, string urlSource, string bindField, object updateFields, bool required = true, object htmlAttributes = null)
        {
            var bindFields = UnwrapRouteValueDictionary(updateFields);

            TagBuilder tagBuilder = new TagBuilder("input");

            tagBuilder.AddCssClass("form-control input-large");
            tagBuilder.Attributes.Add("type", "text");
            tagBuilder.Attributes.Add("id", id);
            tagBuilder.Attributes.Add("name", id);
            tagBuilder.Attributes.Add("data-provide", "typeahead");
            tagBuilder.Attributes.Add("data-lookup-action", urlSource);
            tagBuilder.Attributes.Add("data-lookup-field", bindField);

            if (updateFields != null)
            tagBuilder.Attributes.Add("data-lookup-update", ConvertJsonToString(updateFields));

            tagBuilder.MergeAttributes(UnwrapRouteValueDictionary(htmlAttributes));

            if (required)
                tagBuilder.Attributes.Add("required", "");


            return new MvcHtmlString(tagBuilder.ToString(TagRenderMode.SelfClosing));
        }
        #endregion

        #region TextBox

        public static MvcHtmlString BWLoginBox(this HtmlHelper htmlHelper, InputTextSize size, string id, string name, int maxlength, object htmlAttributes = null)
        {

            var snippet = new StringBuilder();

            snippet.AppendFormat(@"<i class='fa fa-user'></i>");
            //snippet.AppendFormat(@"<input type='text' class='form-control' id='Login' name='Login' maxlength=''>", id, name, maxlength);


            htmlHelper.ViewContext.Writer.Write(snippet.ToString());

            TagBuilder tagBuilder = new TagBuilder("input");

            tagBuilder.AddCssClass(String.Format("form-control {0}", GetStyleClass(size)));
            tagBuilder.Attributes.Add("type", "text");
            tagBuilder.Attributes.Add("id", id);
            tagBuilder.Attributes.Add("name", name);
            tagBuilder.Attributes.Add("required", "required");

            tagBuilder.Attributes.Add("maxlength", maxlength.ToString());

            tagBuilder.MergeAttributes(UnwrapRouteValueDictionary(htmlAttributes));
            
            return new MvcHtmlString(tagBuilder.ToString(TagRenderMode.SelfClosing));
        }

        public static MvcHtmlString BWLoginBoxFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, InputTextSize size = InputTextSize.Medium, object htmlAttributes = null)
        {
            var metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            string htmlFieldName = ExpressionHelper.GetExpressionText(expression);
            string txtId = metadata.PropertyName ?? htmlFieldName.Split('.').Last();
            string txtName = BWPropertyName(htmlHelper, expression);
            string valueText = metadata.Model == null ? String.Empty : metadata.Model.ToString();
            
            int maxlength = Int32.MaxValue;
            var strLenAttribute = expression.BWGetAttribute<TModel, TProperty, StringLengthAttribute>();
            if (strLenAttribute != null)
            {
                maxlength = strLenAttribute.MaximumLength;
            }

            object htmlAttributes2 = null;
            var displayName = expression.BWGetAttribute<TModel, TProperty, DisplayAttribute>();
            if (displayName != null)
            {
                htmlAttributes2 = new { placeholder = displayName.GetName() };
            }

            return BWLoginBox(htmlHelper, size, txtId, txtName, maxlength, MergeAndOverrideAttributes(htmlAttributes, htmlAttributes2));
        }

        public static MvcHtmlString BWTextArea(this HtmlHelper htmlHelper, string id, string name, string text, int rows, int cols, int maxlength, bool required = true, bool disabled = false, object htmlAttributes = null)
        {
            var translated = htmlHelper.BWGetLocalString(id, name);

            TagBuilder tagBuilder = new TagBuilder("textarea");

            tagBuilder.AddCssClass("form-control");
            tagBuilder.Attributes.Add("type", "text");
            tagBuilder.Attributes.Add("id", id);
            tagBuilder.Attributes.Add("name", name);
            tagBuilder.Attributes.Add("value", text);

            if (disabled)
                tagBuilder.Attributes.Add("readonly", "readonly");

            tagBuilder.Attributes.Add("maxlength", maxlength.ToString());
            tagBuilder.MergeAttributes(UnwrapRouteValueDictionary(htmlAttributes));

            if (required)
                tagBuilder.Attributes.Add("required", "");


            return new MvcHtmlString(tagBuilder.ToString(TagRenderMode.Normal));
        }

        public static MvcHtmlString BWTextBox(this HtmlHelper htmlHelper, InputTextSize size, string id, string name, string text, int maxlength, bool required = true, bool numbersOnly = false, bool disabled = false, object htmlAttributes = null)
        {
            var translated = htmlHelper.BWGetLocalString(id, name);

            TagBuilder tagBuilder = new TagBuilder("input");

            tagBuilder.AddCssClass(String.Format("form-control {0}", GetStyleClass(size)));
            tagBuilder.Attributes.Add("type", "text");
            tagBuilder.Attributes.Add("id", id);
            tagBuilder.Attributes.Add("name", name);
            tagBuilder.Attributes.Add("value", text);

            if (numbersOnly)
                tagBuilder.Attributes.Add("digits", "true");

            if (disabled)
                tagBuilder.Attributes.Add("readonly", "readonly");

            tagBuilder.Attributes.Add("maxlength", maxlength.ToString());
            tagBuilder.MergeAttributes(UnwrapRouteValueDictionary(htmlAttributes));

            if (required)
                tagBuilder.Attributes.Add("required", "");


            return new MvcHtmlString(tagBuilder.ToString(TagRenderMode.SelfClosing));
        }

        public static MvcHtmlString BWTextBox(this HtmlHelper htmlHelper, InputTextSize size, string id, string text, int maxlength, object htmlAttributes = null)
        {
            return BWTextBox(htmlHelper, size, id, text, maxlength, true, htmlAttributes);
        }

        public static MvcHtmlString BWTextBox(this HtmlHelper htmlHelper, InputTextSize size, string id, string text, int maxlength, bool required, object htmlAttributes = null)
        {
            return BWTextBox(htmlHelper, size, id, text, maxlength, required, false, false, htmlAttributes);
        }

        public static MvcHtmlString BWTextBox(this HtmlHelper htmlHelper, InputTextSize size, string id, string text, int maxlength, bool required, bool numbersOnly = false, bool disabled = false, object htmlAttributes = null)
        {
            return BWTextBox(htmlHelper, size, id, id, text, maxlength, required, numbersOnly, disabled, htmlAttributes);
        }

        public static MvcHtmlString BWTextBox(this HtmlHelper htmlHelper, string id, string name, string text, int maxlength, bool required, bool numbersOnly, bool disabled, object htmlAttributes = null)
        {
            return BWTextBox(htmlHelper, InputTextSize.Medium, id, name, text, maxlength, required, numbersOnly, disabled, htmlAttributes);
        }

        public static MvcHtmlString BWTextBox(this HtmlHelper htmlHelper, string id, string name, int maxlength, bool required = true, bool numbersOnly = false, bool disabled = false, object htmlAttributes = null)
        {
            return BWTextBox(htmlHelper, InputTextSize.Medium, id, name, String.Empty, maxlength, required, numbersOnly, disabled, htmlAttributes);
        }

        public static MvcHtmlString BWTextBox(this HtmlHelper htmlHelper, string id, int maxlength, bool required = true, bool numbersOnly = false, bool disabled = false, object htmlAttributes = null)
        {
            return BWTextBox(htmlHelper, InputTextSize.Medium, id, id, String.Empty, maxlength, required, numbersOnly, disabled, htmlAttributes);
        }

        public static MvcHtmlString BWTextBoxFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, object htmlAttributes = null)
        {
            return BWTextBoxFor(htmlHelper, expression, InputTextSize.Medium, false, htmlAttributes);
        }

        public static MvcHtmlString BWTextBoxFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, bool isReadOnly, object htmlAttributes = null)
        {
            return BWTextBoxFor(htmlHelper, expression, InputTextSize.Medium, isReadOnly, htmlAttributes);
        }

        public static MvcHtmlString BWTextBoxFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, InputTextSize size, bool isReadOnly, object htmlAttributes = null)
        {
            var metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            string htmlFieldName = ExpressionHelper.GetExpressionText(expression);
            string txtId = metadata.PropertyName ?? htmlFieldName.Split('.').Last();
            string txtName = BWPropertyName(htmlHelper, expression);
            string valueText = metadata.Model == null ? String.Empty : metadata.Model.ToString();


            bool required = false;
            var requiredAttribute = expression.BWGetAttribute<TModel, TProperty, RequiredAttribute>();
            if (requiredAttribute != null)
            {
                required = true;
            }

            var maxlength = Int32.MaxValue;            
            var maxLenAttribute = expression.BWGetAttribute<TModel, TProperty, MaxLengthAttribute>();
            if (maxLenAttribute != null)
            {
                maxlength = maxLenAttribute.Length;
            }

            var strLenAttribute = expression.BWGetAttribute<TModel, TProperty, StringLengthAttribute>();
            if (strLenAttribute != null)
            {
                maxlength = strLenAttribute.MaximumLength;
            }

            bool readOnly = false;
            var readOnlyAttribute = expression.BWGetAttribute<TModel, TProperty, ReadOnlyAttribute>();
            if (readOnlyAttribute != null)
            {
                readOnly = true;
            }

            bool numbersOnly = false;

            return BWTextBox(htmlHelper, size, txtId, txtName, valueText, maxlength, required, numbersOnly, isReadOnly || readOnly, htmlAttributes);
        }

        public static MvcHtmlString BWTextAreaFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, int rows, int cols, object htmlAttributes = null)
        {
            var metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            string htmlFieldName = ExpressionHelper.GetExpressionText(expression);
            string txtId = metadata.PropertyName ?? htmlFieldName.Split('.').Last();
            string txtName = BWPropertyName(htmlHelper, expression);
            string valueText = metadata.Model == null ? String.Empty : metadata.Model.ToString();

            bool required = false;
            var requiredAttribute = expression.BWGetAttribute<TModel, TProperty, RequiredAttribute>();
            if (requiredAttribute != null)
            {
                required = true;
            }

            int maxlength = Int32.MaxValue;
            var maxLenAttribute = expression.BWGetAttribute<TModel, TProperty, MaxLengthAttribute>();
            if (maxLenAttribute != null)
            {
                maxlength = maxLenAttribute.Length;
            }

            var strLenAttribute = expression.BWGetAttribute<TModel, TProperty, StringLengthAttribute>();
            if (strLenAttribute != null)
            {
                maxlength = strLenAttribute.MaximumLength;
            }

            bool readOnly = false;
            var readOnlyAttribute = expression.BWGetAttribute<TModel, TProperty, ReadOnlyAttribute>();
            if (readOnlyAttribute != null)
            {
                readOnly = true;
            }

            return BWTextArea(htmlHelper, txtId, txtName, valueText, rows, cols, maxlength, required, readOnly, htmlAttributes);
        }

        public static MvcHtmlString BWHiddenFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, object htmlAttributes = null)
        {
            var metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            string htmlFieldName = ExpressionHelper.GetExpressionText(expression);
            string txtId = metadata.PropertyName ?? htmlFieldName.Split('.').Last();
            string txtName = BWPropertyName(htmlHelper, expression);

            string valueText = metadata.Model != null ? metadata.Model.ToString() : String.Empty;

            return htmlHelper.BWHidden(txtId, txtName, valueText, htmlAttributes);
        }

        public static MvcHtmlString BWHidden(this HtmlHelper htmlHelper, string id, string name, string value, object htmlAttributes = null)
        {
            TagBuilder tagBuilder = new TagBuilder("input");

            tagBuilder.Attributes.Add("type", "hidden");
            tagBuilder.Attributes.Add("id", id);
            tagBuilder.Attributes.Add("name", name);
            tagBuilder.Attributes.Add("value", value);


            tagBuilder.MergeAttributes(UnwrapRouteValueDictionary(htmlAttributes));

            return new MvcHtmlString(tagBuilder.ToString(TagRenderMode.SelfClosing));
        }


        #endregion

        #region Buttons

        public static MvcHtmlString BWButtonLink(this HtmlHelper htmlHelper, ButtonSize size, ButtonAction actionStyle, ButtonColor color, string id, string text, string url, bool translate = false, object htmlAttributes = null)
        {
            var translated = translate ? htmlHelper.BWGetLocalString(id, text) : text;

            var tag = new TagBuilder("a");
            tag.Attributes.Add("id", id);
            tag.MergeAttribute("href", url);
            tag.MergeAttribute("type", "button");
            tag.MergeAttributes(AttributesHelper.MergeClassAttributes(String.Format("btn {0} {1}", GetStyleClass(color), GetStyleClass(size)), htmlAttributes));
            tag.InnerHtml = String.Format("<span class='{0}'></span> {1}\r\n", GetStyleClass(actionStyle), translated);

            return new MvcHtmlString(tag.ToString(TagRenderMode.Normal));
        }


        public static MvcButton BWButton(this HtmlHelper htmlHelper)
        {
            return new MvcButton(htmlHelper.ViewContext);
        }

        public static MvcHtmlString BWButton(this HtmlHelper htmlHelper, ButtonSize size, ButtonAction actionStyle, ButtonColor color, string id, string text, bool translate = false, object htmlAttributes = null)
        {
            var translated = translate ? htmlHelper.BWGetLocalString(id, text) : text;
            var tag = new MvcButton(htmlHelper.ViewContext).Set(size).Set(actionStyle).Set(color).SetText(translated);

            return tag.ToHtml(htmlAttributes);
        }

        public static MvcLinkButton BWLinkButton(this HtmlHelper htmlHelper)
        {
            return new MvcLinkButton(htmlHelper.ViewContext);            
        }

        public static MvcLinkButton BWLinkButton(this HtmlHelper htmlHelper,  string id)
        {
            var tag2 = new MvcLinkButton(htmlHelper.ViewContext);
            tag2.SetId(id);

            return tag2;
        }

        public static MvcHtmlString BWLinkButton(this HtmlHelper htmlHelper, ButtonSize size, ButtonAction actionStyle, ButtonColor color, string id, string text, string url, bool translate = false, object htmlAttributes = null)
        {
            var translated = translate ? htmlHelper.BWGetLocalString(id, text) : text;

            //var tag = new TagBuilder("a");
            //tag.Attributes.Add("id", id);
            //tag.MergeAttributes(AttributesHelper.MergeClassAttributes(String.Format("btn {0} {1}", GetStyleClass(color), GetStyleClass(size)), htmlAttributes));
            //tag.MergeAttribute("href", url);
            //tag.InnerHtml = String.Format("<span class='{0}'></span> {1}\r\n", GetStyleClass(actionStyle), translated);

            var tag2 = new MvcLinkButton(htmlHelper.ViewContext);
            tag2.SetId(id);
            tag2.Set(color);
            tag2.Set(size);
            tag2.Set(actionStyle);
            return tag2.UrlLink(translated, url, htmlAttributes);
            
        }

        public static MvcHtmlString FWButtonSubmit(this HtmlHelper htmlHelper, ButtonSize size, ButtonAction actionStyle, ButtonColor color, string id, string text, bool translate = false, bool disabled = false, object htmlAttributes = null)
        {
            var translated = translate ? htmlHelper.BWGetLocalString(id, text) : text;

            var tag = new TagBuilder("button");
            tag.GenerateId(id);
            tag.MergeAttributes(AttributesHelper.MergeClassAttributes(
                String.Format("btn {0} {1}{2}", GetStyleClass(color), GetStyleClass(size), disabled ? " hidden" : ""), 
                htmlAttributes));
            tag.MergeAttribute("type", "submit");
            tag.InnerHtml = String.Format("<span class='{0}'></span> {1}\r\n", GetStyleClass(actionStyle), translated);

            return new MvcHtmlString(tag.ToString(TagRenderMode.Normal));
        }

        #region Remove
        public static MvcHtmlString BWBtnRemove(this HtmlHelper htmlHelper, ButtonSize size, string id, string text)
        {
            var translated = htmlHelper.BWGetCommonString(BTN_REMOVE_KEY, text);
            return BWButton(htmlHelper, size, ButtonAction.Remove, ButtonColor.Red, id, translated, false);
        }

        public static MvcHtmlString BWBtnRemove(this HtmlHelper htmlHelper, string id, string text)
        {
            return BWBtnRemove(htmlHelper, ButtonSize.Default, id, text);
        }

        /// <summary>
        ///     Gera um botão na view. Primeira parte. Segunda parte são os métodos ActionLink, ButtonClick ou Button
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="id">atributo html id do botão</param>
        /// <returns></returns>
        public static MvcButton BWButton(this HtmlHelper htmlHelper, string id = null)
        {
            var ctl = new MvcButton(htmlHelper.ViewContext)
            {
                Id = id,
            };

            return ctl;
        }


        /// <summary>
        ///     Gera um botão na view que chama um código javascript.
        ///     Ex: @(Html.BWButtonFor(m => m.Model.Codigo).AddActionId("Delete").ButtonClick("Excluir", "deleteFuncion('{0}'')"))
        ///         Vai substituir os {0} e {1} pela URL gerada por AddAction correspondente ficando o código como: deleteFuncion('Controller/Delete/1')
        /// </summary>
        /// <typeparam name="TModel">Entidade do Modelo</typeparam>
        /// <typeparam name="TProperty">Propriedade para pegar o valor utilizado como Id no método do controler</typeparam>
        /// <param name="htmlHelper"></param>
        /// <param name="expression">Propriedade para pegar o valor utilizado como Id no método do controler</param>
        /// <param name="id">atributo html id do botão</param>
        /// <returns></returns>
        public static MvcButton BWButtonFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, string id = null)
        {
            var fieldName = AttributesHelper.GetPropertyName(expression);
            var ctl = new MvcButton(htmlHelper.ViewContext)
            {
                Id = id,
            };

            ctl.SetText(fieldName);

            return ctl;
        }

        #endregion

        #region Login
        public static MvcHtmlString BWBtnLogin(this HtmlHelper htmlHelper, ButtonSize size, string id, string text)
        {
            var translated = htmlHelper.BWGetCommonString(BTN_LOGIN_KEY, text);
            return BWButton(htmlHelper, size, ButtonAction.Login, ButtonColor.Custom, id, text, false);
        }

        public static MvcHtmlString BWBtnLogin(this HtmlHelper htmlHelper, string id, string text)
        {
            return BWBtnLogin(htmlHelper, ButtonSize.Default, id, text);
        }

        #endregion

        #region Save
        public static MvcHtmlString BWBtnSave(this HtmlHelper htmlHelper, ButtonSize size, string id, string text)
        {
            var translated = htmlHelper.BWGetCommonString(BTN_SAVE_KEY, text);
            return BWButton(htmlHelper, size, ButtonAction.Save, ButtonColor.Green, id, translated, false);
        }

        public static MvcHtmlString BWBtnSave(this HtmlHelper htmlHelper, string id, string text)
        {
            return BWBtnSave(htmlHelper, ButtonSize.Default, id, text);
        }
        #endregion

        #region Back

        public static MvcHtmlString FWBtnEditAjax(this HtmlHelper htmlHelper, bool disabled = false)
        {
            return FWButtonSubmit(htmlHelper, ButtonSize.Default, ButtonAction.Edit, ButtonColor.Blue, "btn-edit", "Editar", disabled: disabled);
        }

        public static MvcHtmlString FWBtnSaveAjax(this HtmlHelper htmlHelper, bool disabled = false)
        {
            return FWButtonSubmit(htmlHelper, ButtonSize.Default, ButtonAction.Save, ButtonColor.Green, "btn-save", "Salvar", disabled: disabled);
        }

        public static MvcHtmlString FWBtnBack(this HtmlHelper htmlHelper, string url)
        {
            return String.IsNullOrEmpty(url)
                ? new MvcHtmlString("")
                : BWLinkButton(htmlHelper, ButtonSize.Default, ButtonAction.Back, ButtonColor.Orange, "btn-back", "Voltar", url);
        }

        #endregion


        #region Edit
        public static MvcHtmlString BWBtnEdit(this HtmlHelper htmlHelper, ButtonSize size, string id, string text, object htmlAttributes = null)
        {
            var translated = htmlHelper.BWGetCommonString(BTN_EDIT_KEY, text);
            return BWButton(htmlHelper, size, ButtonAction.Edit, ButtonColor.LighBlue, id, text, false, htmlAttributes);
        }

        public static MvcHtmlString BWBtnEdit(this HtmlHelper htmlHelper, string id, object htmlAttributes = null)
        {
            return BWBtnEdit(htmlHelper, ButtonSize.Default, id, String.Empty, htmlAttributes);
        }

        public static MvcHtmlString BWBtnEdit(this HtmlHelper htmlHelper, string url)
        {
            return BWLinkButton(htmlHelper, ButtonSize.Default, ButtonAction.Edit, ButtonColor.LighBlue, "btn-edit", "Editar", url);
        }

        #endregion

        #region Search
        public static MvcHtmlString BWBtnSearch(this HtmlHelper htmlHelper, ButtonSize size, string id, string text)
        {
            var translated = htmlHelper.BWGetCommonString(BTN_SEARCH_KEY, text);
            return BWButton(htmlHelper, size, ButtonAction.Search, ButtonColor.LighBlue, id, text, false);
        }

        public static MvcHtmlString BWBtnSearch(this HtmlHelper htmlHelper, string id, string text)
        {
            return BWBtnSearch(htmlHelper, ButtonSize.Default, id, text);
        }
        #endregion

        #region Reload
        public static MvcHtmlString BWBtnReload(this HtmlHelper htmlHelper, ButtonSize size, string id, string text)
        {
            var translated = htmlHelper.BWGetCommonString(BTN_RELOAD_KEY, text);
            return BWButton(htmlHelper, size, ButtonAction.Reload, ButtonColor.Custom, id, text, false);
        }

        public static MvcHtmlString BWBtnReload(this HtmlHelper htmlHelper, string id, string text)
        {
            return BWBtnReload(htmlHelper, ButtonSize.Default, id, text);
        }
        #endregion

        #region Add
        public static MvcHtmlString BWBtnAdd(this HtmlHelper htmlHelper, ButtonSize size, string id)
        {
            return BWButton(htmlHelper, size, ButtonAction.Add, ButtonColor.Green, id, String.Empty, false);
        }

        public static MvcHtmlString BWBtnAdd(this HtmlHelper htmlHelper, string id)
        {
            return BWBtnAdd(htmlHelper, ButtonSize.Default, id);
        }
        #endregion

        #region Del
        public static MvcHtmlString BWBtnDel(this HtmlHelper htmlHelper, ButtonSize size, string id, object htmlAttributes = null)
        {
            return BWButton(htmlHelper, size, ButtonAction.Del, ButtonColor.Red, id, String.Empty, false, htmlAttributes);
        }

        public static MvcHtmlString BWBtnDel(this HtmlHelper htmlHelper, string id, object htmlAttributes = null)
        {
            return BWBtnDel(htmlHelper, ButtonSize.Default, id, htmlAttributes);
        }
        #endregion

        #endregion

        #region Checkbox

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="htmlHelper"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static MvcHtmlString BWCheckBoxFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
        {
            var metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            var htmlFieldName = ExpressionHelper.GetExpressionText(expression);
            var txtId = metadata.PropertyName ?? htmlFieldName.Split('.').Last();
            var txtName = BWPropertyName(htmlHelper, expression);
            var isChecked = metadata.Model.ToString().Equals("Y") || metadata.Model.ToString().ToLower().Equals("true");

            var displayName = expression.BWGetAttribute<TModel, TProperty, DisplayAttribute>();
            if (displayName != null)
                return BWCheckBox(htmlHelper, txtId, txtName, displayName.GetName(), isChecked);

            bool readOnly = false;
            var readOnlyAttribute = expression.BWGetAttribute<TModel, TProperty, ReadOnlyAttribute>();
            if (readOnlyAttribute != null)
            {
                readOnly = true;
            }

            return BWCheckBox(htmlHelper, txtId, txtName, String.Empty, isChecked, readOnly);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="id"></param>
        /// <param name="isChecked"></param>
        /// <param name="readOnly"></param>
        /// <returns></returns>
        public static MvcHtmlString BWCheckBox(this HtmlHelper htmlHelper, string id, bool isChecked = false, bool isReadOnly = false)
        {
            return BWCheckBox(htmlHelper, id, id, String.Empty, isChecked, isReadOnly);
        }

        /// <summary>
        /// <code>
        /// 
        /// <div class="input-group">
        ///     <label class="checkbox">
        ///         <input type="checkbox" id="RememberPassword" value="true" name="RememberPassword" class="uniform">
        ///             Text for checkbox
        ///     </label>
        /// </div>        
        ///
        /// </code>
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="id"></param>
        /// <param name="text"></param>
        /// <param name="isChecked"></param>
        /// <param name="readOnly"></param>
        /// <returns></returns>
        public static MvcHtmlString BWCheckBox(this HtmlHelper htmlHelper, string id, string name, string text, bool isChecked = false, bool isReadOnly = false)
        {
            var tag = new MvcCheckbox(htmlHelper.ViewContext)
                            .SetId(id)                 
                            .SetName(name)
                            .SetText(text)                            
                            .SetChecked(isChecked)
                            .SetReadonly(isReadOnly);

            return tag.ToHtml();            
        }

        /// <summary>
        /// Cria checkbox vazio para ser preenchido na própria view.
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <returns></returns>
        public static MvcCheckbox BWCheckBox(this HtmlHelper htmlHelper)
        {
            return new MvcCheckbox(htmlHelper.ViewContext);
        }

        #endregion

        #region Panel

        public static MvcPanelGroup BWBeginPanelGroup(this HtmlHelper htmlHelper,
                                       string id,
                                       object panelHtmlAttributes = null)
        {
            return new MvcPanelGroup(htmlHelper.ViewContext, id).Begin(panelHtmlAttributes);
        }

        public static MvcPanel BWBeginSimplePanel(this HtmlHelper htmlHelper,
                                PanelColor color = PanelColor.Default,
                                object panelHtmlAttributes = null,
                                object contentHtmlAttributes = null)
        {
            return BWBeginSimplePanel(htmlHelper, String.Empty, color, panelHtmlAttributes, contentHtmlAttributes);
        }

        public static MvcPanel BWBeginSimplePanel(this HtmlHelper htmlHelper,
                                string id,
                                PanelColor color = PanelColor.Default,
                                object panelHtmlAttributes = null,
                                object contentHtmlAttributes = null)
        {
            return new MvcPanel(htmlHelper.ViewContext).Begin(id, color, panelHtmlAttributes).BeginBody(contentHtmlAttributes);
        }

        public static MvcPanel BWBeginPanel(this HtmlHelper htmlHelper,
                                string id,
                                object panelHtmlAttributes = null,
                                object titleHtmlAttributes = null,
                                object contentHtmlAttributes = null)
        {
            return BWBeginPanel(htmlHelper, id, PanelColor.Default, panelHtmlAttributes, titleHtmlAttributes, contentHtmlAttributes);
        }

        public static MvcPanel BWBeginPanel(this HtmlHelper htmlHelper,
                                string id,
                                PanelColor color,
                                object panelHtmlAttributes = null,
                                object titleHtmlAttributes = null,
                                object contentHtmlAttributes = null)
        {
            var title = htmlHelper.BWGetLocalString(id, id);
            return BWBeginPanel(htmlHelper, id, title, color, panelHtmlAttributes, titleHtmlAttributes,
                contentHtmlAttributes);
        }

        public static MvcPanel BWBeginPanel(this HtmlHelper htmlHelper,
                                string id,
                                string title,
                                PanelColor color,
                                object panelHtmlAttributes = null,
                                object titleHtmlAttributes = null,
                                object contentHtmlAttributes = null)
        {
            return BWBeginPanel(htmlHelper, id, title, MvcPanel.DEFAULT_ICON, color, panelHtmlAttributes, titleHtmlAttributes,
                contentHtmlAttributes);
        }

        public static MvcPanel BWBeginPanel(this HtmlHelper htmlHelper,
                                string id,
                                string title,
                                string icon,
                                PanelColor color,
                                object panelHtmlAttributes = null,
                                object titleHtmlAttributes = null,
                                object contentHtmlAttributes = null)
        {
            return new MvcPanel(htmlHelper.ViewContext).Begin(id, color, panelHtmlAttributes).BeginHeader(title, icon, titleHtmlAttributes).BeginBody(contentHtmlAttributes);
        }



        public static MvcPanel BWAddPanel(this HtmlHelper htmlHelper,
                                MvcPanelGroup pnlGroup,
                                string id,
                                string title,
                                bool collapsed = true,
                                string icon = MvcPanel.DEFAULT_ICON,
                                PanelColor color = PanelColor.Default,
                                object panelHtmlAttributes = null,
                                object titleHtmlAttributes = null,
                                object contentHtmlAttributes = null)
        {
            var translated = htmlHelper.BWGetLocalString(id, title);
            return pnlGroup.AddPanel(id, translated, collapsed, icon, color, panelHtmlAttributes, titleHtmlAttributes, contentHtmlAttributes);
        }

        #endregion

        #region Media

        public static MvcHtmlString BWMediaBox(this HtmlHelper htmlHelper, string id, bool status, string urlContent, string img1, string img2, string urlLink = "", params object[] args)
        {
            return BWMediaBox(htmlHelper, id, String.Empty, status, urlContent, img1, img2, urlLink, args);
        }

        public static MvcHtmlString BWMediaBox(this HtmlHelper htmlHelper, string id, string title, bool status, string urlContent, string img1, string img2, string urlLink = "", params object[] args)
        {
            var text = htmlHelper.BWTranslateLocalString(id, args);

            return BWMediaBox(htmlHelper, id, title, text, status, urlContent, img1, img2, urlLink);
        }

        public static MvcHtmlString BWMediaBox(this HtmlHelper htmlHelper, string id, string title, string text, bool status, string urlContent, string img1, string img2, string urlLink = "")
        {
            var urlHelper = new UrlHelper(HttpContext.Current.Request.RequestContext);
            string urlMedia = urlHelper.Content(urlContent + (status ? img1 : img2));

            var snippet = new StringBuilder();

            snippet.AppendFormat("<div id='{0}' class='media'>", id);
            snippet.AppendFormat("<div class='media-left media-bottom'>");
            snippet.AppendFormat("<img src='{0}' alt='...'></div>", urlMedia);

            if (String.IsNullOrWhiteSpace(urlLink))
            {
                if (String.IsNullOrEmpty(title))
                    snippet.AppendFormat("<div class='media-body'><br /><p>{0}</p></div></div>", text);
                else
                    snippet.AppendFormat("<div class='media-body'><br /><h5 class='media-heading'>{0}</h5><p style='font-size: 10px;'>{1}</p></div></div>", title, text);
            }
            else
            {
                if (String.IsNullOrEmpty(title))
                    snippet.AppendFormat("<div class='media-body'><br /><a href='{1}'>{0}</a></p></div></div>", text, urlLink);
                else
                    snippet.AppendFormat("<div class='media-body'><br /><h5 class='media-heading'><a href='{2}'>{0}</a></h5><p style='font-size: 10px;'>{1}</p></div></div>", title, text, urlLink);

            }

            return new MvcHtmlString(snippet.ToString());
        }
        #endregion

        #region DropdownList

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="htmlHelper"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static MvcHtmlString BWDdlFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<SelectListItem> selectList)
        {
            return BWDdlFor(htmlHelper, expression, selectList, null);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="htmlHelper"></param>
        /// <param name="expression"></param>
        /// <param name="selectList"></param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public static MvcHtmlString BWDdlFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<SelectListItem> selectList, object htmlAttributes = null)
        {
            return BWDdlFor(htmlHelper, expression, "Selecione", selectList, htmlAttributes);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="htmlHelper"></param>
        /// <param name="expression"></param>
        /// <param name="optiontext"></param>
        /// <param name="selectList"></param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public static MvcHtmlString BWDdlFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, string optiontext, IEnumerable<SelectListItem> selectList, object htmlAttributes = null)
        {
            var metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            var attributes = MergeAndOverrideAttributes(new { @class = "form-control", @id = metadata.PropertyName }, htmlAttributes);
            string txtName = BWPropertyName(htmlHelper, expression);

            return htmlHelper.DropDownList(txtName, selectList, optiontext, attributes);
        }

        public static MvcHtmlString FWDdlFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, string optiontext, IEnumerable<SelectListItem> selectList, bool isReadOnly, object htmlAttributes = null)
        {
            var metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            var attributes = MergeAndOverrideAttributes(new { @class = "form-control selectpicker", @id = metadata.PropertyName }, htmlAttributes);

            if (isReadOnly)
                attributes = MergeAndOverrideAttributes(new { disabled = "" }, attributes);

            return htmlHelper.DropDownList(BWPropertyName(htmlHelper, expression), selectList, optiontext, attributes);
        }
        
        /// <summary>
        /// Create a new dropdown-list empty with the id.
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static MvcHtmlString BWDdl(this HtmlHelper htmlHelper,
                                      string id)
        {
            var options = new List<SelectListItem>() { new SelectListItem() { Value = "0", Text = "Selecione" } };
            return htmlHelper.DropDownList(id, options, new { @class = "form-control" });
        }


        /// <summary>
        /// Create a new dropdown-list with the id and items informed.
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="id"></param>
        /// <param name="items"></param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public static MvcHtmlString BWDdl(this HtmlHelper htmlHelper,
                                      string id,
                                      IEnumerable<SelectListItem> items,
                                      object htmlAttributes = null)
        {
            var attributes = UnwrapRouteValueDictionary(htmlAttributes);
            var classAttribute = attributes.FirstOrDefault(q => q.Key == "class");
            if (classAttribute.Key == null)
                attributes.Add("class", "form-control");
            else
            {
                attributes.Remove("class");
                attributes.Add("class", "form-control " + classAttribute.Value);
            }
            return htmlHelper.DropDownList(id, items, attributes);
        }

        /// <summary>
        /// Create a new dropdown-list with the id and items informed.
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="id"></param>
        /// <param name="selectedText"></param>
        /// <param name="items"></param>
        /// <returns></returns>
        public static MvcHtmlString BWDdl(this HtmlHelper htmlHelper,
                                      string id,
                                      string selectedText,
                                      IList items)
        {
            var list = new List<SelectListItem>();
            foreach (object o in items)
            {
                var objToString = o.ToString();
                var valueId = 0;

                var pi = o.GetType().GetProperty("Id");
                if (pi != null)
                    valueId = (Int32)pi.GetValue(o, null);

                var i = new SelectListItem
                {
                    Text = objToString,
                    Value = valueId > 0 ? valueId.ToString() : objToString,
                    Selected = objToString.Equals(selectedText)
                };

                list.Add(i);
            }

            return htmlHelper.DropDownList(id, list, new { @class = "form-control" });
        }

        /// <summary>
        /// Create a new dropdown-list with the id and items informed.
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="id"></param>
        /// <param name="translate"></param>
        /// <param name="itemsId"></param>
        /// <returns></returns>
        public static MvcHtmlString BWDdl(this HtmlHelper htmlHelper,
                                      string id,
                                      bool translate = false,
                                      params KeyValuePair<int, string>[] itemsId)
        {
            var items = new List<SelectListItem>();
            foreach (var item in itemsId.AsEnumerable())
            {
                var translated = translate ? htmlHelper.BWGetLocalString(id, item.Value) : item.Value;
                items.Add(new SelectListItem() { Text = translated, Value = item.Key.ToString() });
            }

            items.Sort();

            return htmlHelper.DropDownList(id, items, new { @class = "form-control" });
        }

        /// <summary>
        /// This dropdown is based on a button dropdown from bootstrap.
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="id"></param>
        /// <param name="listIds"></param>
        /// <returns></returns>
        public static MvcHtmlString BWButtonDdl(this HtmlHelper htmlHelper,
                                      string id,
                                      params string[] listIds)
        {
            return BWButtonDdl(htmlHelper, id, ButtonColor.NoColor, true, listIds);
        }

        /// <summary>
        /// This dropdown is based on a button dropdown from bootstrap.
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="id"></param>
        /// <param name="color"></param>
        /// <param name="translate"></param>
        /// <param name="listIds"></param>
        /// <returns></returns>
        public static MvcHtmlString BWButtonDdl(this HtmlHelper htmlHelper,
                                      string id,
                                      ButtonColor color,
                                      bool translate = false,
                                      params string[] listIds)
        {
            var snippet = new StringBuilder();
            snippet.AppendFormat("<div class='btn-group'>");
            snippet.AppendFormat("  <button type='button' class='btn {0}'>Action</button>", GetStyleClass(color));
            snippet.AppendFormat("  <button type='button' class='btn {0} dropdown-toggle' data-toggle='dropdown'>", GetStyleClass(color));
            snippet.AppendFormat("      <span class='caret'></span>");
            snippet.AppendFormat("      <span class='sr-only'>Toggle Dropdown</span>");
            snippet.AppendFormat("  </button>");
            snippet.AppendFormat("  <ul class='dropdown-menu' role='menu'>");

            foreach (var itemId in listIds)
            {
                var translated = translate ? htmlHelper.BWGetLocalString(itemId, itemId) : itemId;
                snippet.AppendFormat("<li><a href='#'>{0}</a></li>", translated);
            }

            snippet.AppendFormat("  </ul>");
            snippet.AppendFormat("</div>");


            return new MvcHtmlString(snippet.ToString());
        }


        #endregion

        #region DatePicker

        public static MvcHtmlString BWDateFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, DatePickerFormat format = DatePickerFormat.Day)
        {
            var metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            string txtName = BWPropertyName(htmlHelper, expression);

            return BWDate(htmlHelper, metadata.PropertyName, txtName, (metadata.Model as DateTime?).Value, format);
        }

        public static MvcHtmlString BWDate(this HtmlHelper htmlHelper, string id, DatePickerFormat format = DatePickerFormat.Day)
        {
            return BWDate(htmlHelper, id, id, DateTime.Now, format);
        }

        public static MvcHtmlString BWDate(this HtmlHelper htmlHelper, string id, string name, DatePickerFormat format = DatePickerFormat.Day)
        {
            return BWDate(htmlHelper, id, name, DateTime.Now, format);
        }

        public static MvcHtmlString BWDate(this HtmlHelper htmlHelper, string id, string name, DateTime value, DatePickerFormat formatPicker = DatePickerFormat.Day)
        {
            string format = GetDateFormat(formatPicker);
            int view_mode = GetDateMinViewMode(formatPicker);

            var snippet = new StringBuilder();
            snippet.AppendFormat("<div class='input-group date'>");

            if (formatPicker == DatePickerFormat.Day)
                snippet.AppendFormat("<input id='{0}' type='text'class='form-control input-small' data-control='datepicker' name='{1}' data-date-format='{2}' date-min-viewmode='{3}' value='{4}'/>", id, name, format, view_mode, value.FormatDate());
            else
                snippet.AppendFormat("<input id='{0}' type='text'class='form-control input-small' data-control='datepicker' name='{1}' data-date-format='{2}' date-min-viewmode='{3}'/>", id, name, format, view_mode);

            snippet.AppendFormat("<span class='input-group-addon'><span class='fa fa-calendar'></span></span></div>");
            return new MvcHtmlString(snippet.ToString());
        }

        public static MvcHtmlString BWTimeFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
        {
            var metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            string txtName = BWPropertyName(htmlHelper, expression);

            return BWTime(htmlHelper, metadata.PropertyName, txtName, (metadata.Model as TimeSpan?).Value);
        }

        public static MvcHtmlString BWTime(this HtmlHelper htmlHelper, string id)
        {
            return BWTime(htmlHelper, id, id);
        }

        public static MvcHtmlString BWTime(this HtmlHelper htmlHelper, string id, string name)
        {
            return BWTime(htmlHelper, id, name, DateTime.Now.TimeOfDay);
        }

        public static MvcHtmlString BWTime(this HtmlHelper htmlHelper, string id, string name, TimeSpan value)
        {
            var snippet = new StringBuilder();
            snippet.AppendFormat("<div class='input-group clockpicker'>");
            snippet.AppendFormat("<input id='{0}' type='text' class='form-control input-small' data-control='timepicker' name='{1}' value='{2}' readonly/>", id, name, value.FormatTime());
            snippet.AppendFormat("<span class='input-group-addon'><span class='fa fa-clock-o'></span></span></div>");

            return new MvcHtmlString(snippet.ToString());
        }


        #endregion

        #region Tabs

        public static MvcTabPanel BWTabPanel(this HtmlHelper htmlHelper, string title, string icon)
        {
            return BWTabPanel(htmlHelper, title, icon, PanelColor.Default);
        }

        public static MvcTabPanel BWTabPanel(this HtmlHelper htmlHelper, string title, string icon, PanelColor color = PanelColor.Default)
        {
            return new MvcTabPanel(htmlHelper.ViewContext).Begin(title, icon, color);
        }

        public static MvcTabContent BWTabContent(this HtmlHelper htmlHelper, string icon)
        {
            return new MvcTabContent(htmlHelper.ViewContext).Begin();
        }

        #endregion

        #region Modal

        public static MvcModal BWBeginModal(this HtmlHelper htmlHelper,
                                string id,
                                object panelHtmlAttributes = null,
                                object titleHtmlAttributes = null,
                                object contentHtmlAttributes = null)
        {
            var title = htmlHelper.BWGetLocalString(id, id);
            return new MvcModal(htmlHelper.ViewContext).Begin(id, panelHtmlAttributes).BeginHeader(title, titleHtmlAttributes).BeginBody(contentHtmlAttributes);
        }

        #endregion

        #region Jz Generic Controls



        #region Form Control

        public static ControlSettings BWControl(this HtmlHelper htmlHelper, string name, byte cols_1of12)
        {
            return htmlHelper.BWControl(name, cols_1of12, string.Empty, 0);
        }

        public static ControlSettings BWControl(this HtmlHelper htmlHelper, string name, byte cols_1of12, byte labelsize_1of12)
        {
            return htmlHelper.BWControl(name, cols_1of12, string.Empty, labelsize_1of12);
        }

        public static ControlSettings BWControl(this HtmlHelper htmlHelper, string name, byte cols_1of12, string label)
        {
            return htmlHelper.BWControl(name, cols_1of12, label, 0);
        }

        public static ControlSettings BWControl(this HtmlHelper htmlHelper, string name, byte cols_1of12, string label, byte labelsize_1of12)
        {
            var ctl = new ControlSettings(htmlHelper)
            {
                _name = name,
                _cols = cols_1of12,
                _label = label,
                _labelSize = labelsize_1of12
            };

            return ctl;
        }

        public static ControlSettings BWControlFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, byte cols_1of12)
        {
            return htmlHelper.BWControlFor(expression, cols_1of12, 0);
        }

        public static ControlSettings BWControlFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, byte cols_1of12, byte labelsize_1of12)
        {
            var metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);

            string htmlFieldName = ExpressionHelper.GetExpressionText(expression);
            string _id = metadata.PropertyName ?? htmlFieldName.Split('.').Last();
            string _name = BWPropertyName(htmlHelper, expression);

            var requiredAttribute = expression.BWGetAttribute<TModel, TProperty, RequiredAttribute>();
            bool _required = (requiredAttribute != null);

            var maxLenAttribute = expression.BWGetAttribute<TModel, TProperty, MaxLengthAttribute>();
            int _maxlength = (maxLenAttribute != null) ? maxLenAttribute.Length : 0;

            var strLenAttribute = expression.BWGetAttribute<TModel, TProperty, StringLengthAttribute>();
            int _strlength = (strLenAttribute != null) ? strLenAttribute.MaximumLength : 0;

            string _value = metadata.Model == null ? String.Empty : metadata.Model.ToString();

            var label = DiscoverDisplay(expression);            

            var result = htmlHelper.BWControl(_name, cols_1of12, label, labelsize_1of12);
            result._maxlength = Math.Max(_maxlength, _strlength);
            result._required = _required;
            result._value = _value;

            return result;
        }

        #endregion

        #region Find/Edit Forms

        public static MvcForm BWBeginFormEdit(this HtmlHelper htmlHelper, string id, string actionName,
            string controllerName, object htmlAttributes = null)
        {
            return htmlHelper.BWBeginFormFind(id, actionName, controllerName, null, htmlAttributes);
        }

        public static MvcForm BWBeginFormEdit(this HtmlHelper htmlHelper, string id, string actionName,
            string controllerName, object routeValues, object htmlAttributes = null)
        {
            return htmlHelper.BWBeginFormFind(id, actionName, controllerName, routeValues, htmlAttributes);
        }

        // FormFind       

        public static MvcForm BWBeginFormFind(this HtmlHelper htmlHelper, string id, string actionName,
            object htmlAttributes = null)
        {
            return htmlHelper.BWBeginFormFind(id, actionName, null, null, htmlAttributes);
        }

        public static MvcForm BWBeginFormFind(this HtmlHelper htmlHelper, string id, string actionName,
            string controllerName, object htmlAttributes = null)
        {
            return htmlHelper.BWBeginFormFind(id, actionName, controllerName, null, htmlAttributes);
        }

        public static MvcForm BWBeginFormFind(this HtmlHelper htmlHelper, string id, string actionName,
            string controllerName, object routeValues, object htmlAttributes = null)
        {
            var baseAttributes = new
            {
                method = "POST",
                id = id,
                data_validate = "true",
                data_action = "ajax"
            };

            return BeginFrameworkForm(htmlHelper, actionName, controllerName, routeValues, FormMethod.Post, baseAttributes, htmlAttributes);
        }

        internal static MvcForm BeginFrameworkForm(HtmlHelper htmlHelper, string actionName,
            string controllerName, object routeValues, FormMethod formMethod, object baseAttributes, object htmlAttributes = null)
        {
            var atributes = MergeAndOverrideAttributes(baseAttributes, htmlAttributes);
            if (routeValues != null)
            {
                var r1 = UnwrapRouteValueDictionary(routeValues);
                return htmlHelper.BeginForm(actionName, controllerName, r1, formMethod, atributes);
            }
            return htmlHelper.BeginForm(actionName, controllerName, formMethod, atributes);
        }

        #endregion

        #region Grid DataTable

        public static MvcGrid<TEntityType> BWBeginGridFind<TEntityType>(this HtmlHelper htmlHelper, object htmlAttributes = null)
        {
            return htmlHelper.BWBeginGridFind<TEntityType>("datatable", htmlAttributes);
        }

        public static MvcGrid<TEntityType> BWBeginGridFind<TEntityType>(this HtmlHelper htmlHelper, string id, object htmlAttributes = null)
        {
            return new MvcGrid<TEntityType>(htmlHelper.ViewContext).Begin(id, htmlAttributes);
        }

        public static MvcGrid<object> BWBeginGridFind(this HtmlHelper htmlHelper, string entityName, object htmlAttributes = null)
        {
            return htmlHelper.BWBeginGridFind(entityName, "datatable", htmlAttributes);
        }

        public static MvcGrid<object> BWBeginGridFind(this HtmlHelper htmlHelper, string entityName, string id, object htmlAttributes = null)
        {
            return new MvcGrid<object>(htmlHelper.ViewContext).Begin(id, htmlAttributes);
        }

        #endregion

        #endregion

        public static MvcHtmlString BWFile(this HtmlHelper htmlHelper, InputTextSize size, string id, string name, bool required = true, object htmlAttributes = null)
        {
            var translated = htmlHelper.BWGetLocalString(id, name);

            TagBuilder tagBuilder = new TagBuilder("input");

            tagBuilder.Attributes.Add("type", "file");
            tagBuilder.Attributes.Add("id", id);
            tagBuilder.Attributes.Add("name", name);

            return new MvcHtmlString(tagBuilder.ToString(TagRenderMode.SelfClosing));
        }

        public static MvcHtmlString BWLink(this HtmlHelper htmlHelper, string text, string url)
        {
            TagBuilder tagBuilder = new TagBuilder("a");

            tagBuilder.Attributes.Add("href", url);
            tagBuilder.SetInnerText(text);

            return new MvcHtmlString(tagBuilder.ToString(TagRenderMode.Normal));
        }
    }
}
