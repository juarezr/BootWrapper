using System;
using System.Linq;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;
using System.Linq.Expressions;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace BootWrapper.BW.Controls.Util
{
    public static class AttributesHelper
    {
        public static RouteValueDictionary MergeAndOverrideAttributes(object baseAttributes, object htmlAttributes)
        {
            var d1 = UnwrapRouteValueDictionary(baseAttributes);
            return MergeAndOverrideAttributes(d1, htmlAttributes);
        }

        public static RouteValueDictionary MergeAndOverrideAttributes(RouteValueDictionary baseAttributes, object htmlAttributes)
        {
            var d2 = UnwrapRouteValueDictionary(htmlAttributes);
            return BWMerge(baseAttributes, d2);
        }

        public static RouteValueDictionary BWExtend(this RouteValueDictionary dest, IEnumerable<KeyValuePair<string, object>> src)
        {
            src.ToList().ForEach(x => { dest[x.Key] = x.Value; });
            return dest;
        }

        public static RouteValueDictionary BWMerge(this RouteValueDictionary source, IEnumerable<KeyValuePair<string, object>> newData)
        {
            return (new RouteValueDictionary(source)).BWExtend(newData);
        }


        public static RouteValueDictionary UnwrapRouteValueDictionary(object htmlAttributes)
        {
            if (htmlAttributes is RouteValueDictionary)
                return htmlAttributes as RouteValueDictionary;

            return HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
        }


        /// <summary>
        /// Unifica o valor do atributo class original e se definido também no obetos htmlAttributes.
        /// </summary>
        /// <param name="originalValue">Classe original.</param>
        /// <param name="htmlAttributes">Attributos do componente.</param>
        /// <returns>Objeto contendo o atributo class contendo o valor original e valor definido emm htmlAttributes.</returns>
        public static RouteValueDictionary MergeClassAttributes(string originalValue, object htmlAttributes)
        {
            if (string.IsNullOrEmpty(originalValue))
                originalValue = String.Empty;

            var attributes = UnwrapRouteValueDictionary(htmlAttributes);
            if (attributes.ContainsKey("class"))
                attributes["class"] = originalValue + " " + attributes["class"];
            else
                attributes.Add("class", originalValue);

            return attributes;
        }

        public static string GetPropertyName<TModel, TProperty>(Expression<Func<TModel, TProperty>> expression)
        {
            LambdaExpression lambdaExpression = null;
            string fieldName = string.Empty;

            if (expression.Body.NodeType == ExpressionType.Convert || expression.Body.NodeType == ExpressionType.ConvertChecked)
            {
                lambdaExpression = Expression.Lambda(((UnaryExpression)expression.Body).Operand, expression.Parameters);
                fieldName = ExpressionHelper.GetExpressionText(lambdaExpression);
            }
            else
            {
                fieldName = ExpressionHelper.GetExpressionText(expression);
            }

            return fieldName;
        }

        public static string DiscoverDisplay<TModel, TProperty>(Expression<Func<TModel, TProperty>> expression)
        {
            string display = String.Empty;
            var displayAttr = expression.BWGetAttribute<TModel, TProperty, DisplayAttribute>();
            if (displayAttr != null)
            {
                display = displayAttr.GetName();
            }
            else
            {
                var displayName = expression.BWGetAttribute<TModel, TProperty, DisplayNameAttribute>();
                if (displayName != null)
                    display = displayName.DisplayName;
            }

            return display;
        }
    }
}
