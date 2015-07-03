using BootWrapper.BW.Core.Translator;
using System.Text;
using BootWrapper.BW.Formatter;
using System.Collections.Generic;

namespace System.Web.Mvc.Html
{
    public static class MvcHtmlSiteMapExtensions
    {
        #region SitemapMenu
        private static int id = 1;
        private static Dictionary<string, string> mnuIDs = new Dictionary<string, string>();

        public static MvcHtmlString BWSitemapMenu(this HtmlHelper htmlHelper)
        {
            return BWSitemapMenu(htmlHelper, false);
        }

        public static MvcHtmlString BWSitemapMenu(this HtmlHelper htmlHelper, bool useLeftMenu)
        {
            var snippet = new StringBuilder();

            foreach (SiteMapNode SubNode in SiteMap.RootNode.ChildNodes)
            {
                string s = BuildChildNodes(SubNode, 1, useLeftMenu);
                snippet.Append(s);
            }

            return new MvcHtmlString(snippet.ToString());
        }

        private const string li_divider =       "<div class='divide-20'></div>";

        /// <summary>
        /// Vertical Menu
        /// </summary>
        private const string vm_ul_dropdown_menu = "<ul class='{0}' {2}>{1}</ul>";

        /// <summary>
        /// {0} - 'active' | 'has-sub' | ''
        /// {1} - '<i class="fa fa-? fa-BW"></i>', in level 1
        /// {2} - 'Text'
        /// {3} - 'selected' | 'arrow' | ''
        /// {4} - '<ul class="sub">...</ul>' | ''
        /// {5} - URL
        /// {6} - Id
        /// </summary>                
        private const string vm_li_dropdown_subm = "<li id='{6}' class='{0}'><a href='{5}'>{1}<span class='menu-text'>{2}</span><span class='{3}'></span></a>{4}</li>";
        private const string vm_li_menuitem = "<li id='{4}' class='{3}'><a class='' href='{2}'><span class='{0}menu-text'>{1}</span></a></li>";
        private const string vm_arrow = "arrow";


        /// <summary>
        /// Horizontal Menu
        /// </summary>
        private const string hm_ul_dropdown_menu = "<ul class='{0}'>{1}</ul>";
        /// <summary>
        /// {0} - Class        
        /// {1} - Icon
        /// {2} - Text
        /// {3} - Arrow: Informado vazio para esse tipo de menu.
        /// {4} - UL
        /// {5} - URL
        /// {6} - Id
        /// {7} - data-toggle
        /// </summary>
        private const string hm_li_dropdown_subm = "<li id='{6}' class='{0}'><a href='{5}' class='dropdown-toggle' {7}>{1}<span class='name'>{2}</span> <span class='{3}'> </span></a> {4}</li>";
        private const string hm_li_menuitem = "<li id='{4}' class='{3}'><a class='' href='{2}'><span class='{0}menu-text'>{1}</span></a></li>";
        private const string hm_arrow = "fa fa-angle-down";



        private const string ul_style_display = "style='display: block;'";
        

        private static string GetTitleTranslated(string title)
        {            
            return Translator.GetCommonString(title, title);
        
        }

        public static string GetMenuId(string text)
        {
            if (mnuIDs.ContainsKey(text))
                return mnuIDs[text];

            string strId = "mni" + (id++);
            mnuIDs.Add(text, strId);

            return strId;
        }

        private static string BuildChildNodes(SiteMapNode childNode, int level, bool leftMenu)
        {   
            string s = string.Empty;
            string data_icon = string.Empty;
            string closetag = string.Empty;
            bool invisible = childNode["visible"] == "false";

            string default_arrow = leftMenu ? vm_arrow : String.Empty;
   
            if (invisible)
                return String.Empty;
            
            if (childNode.Title == "divider")
                return li_divider;

            if (level == 1)
            {
                data_icon = String.Format("<i class='{0}'></i>", childNode["data-icon"]);
            }            

            var childs  = childNode.ChildNodes.Count;
            var hasSub  = childs > 0 ? "has" + "-sub".Repeat(level) : String.Empty;
            var subs    = "sub-".Repeat(level - 1);
            var text    = GetTitleTranslated(childNode.Title);
            var arrow   = childs > 0 ? default_arrow : String.Empty;

            var current = SiteMap.CurrentNode == childNode ? " current" : String.Empty;

            var active = String.Empty;
            if (SiteMap.CurrentNode != null)
            {                
                if (childNode.ChildNodes.Contains(SiteMap.CurrentNode.ParentNode) ||
                    childNode.ChildNodes.Contains(SiteMap.CurrentNode))
                    active = " open";

                if (SiteMap.CurrentNode.Equals(childNode))
                    current = "current";
            }
            
            string classes = leftMenu ? hasSub + active : "dropdown";

            if (childs > 0)
            {
                if (level > 1)
                    classes = leftMenu ? hasSub + active : "dropdown-submenu";

                string sChildNodes = String.Empty;
                foreach (SiteMapNode SubNode in childNode.ChildNodes)
                {
                    sChildNodes += BuildChildNodes(SubNode, level + 1, leftMenu) + "\r\n";
                }                

                //renderiza filhos
                string ul = RenderMenu(leftMenu, leftMenu ? "sub".Repeat(level, '-') : "dropdown-menu", sChildNodes, !String.IsNullOrEmpty(active) ? ul_style_display : String.Empty);
                string li = RenderSubMenu(leftMenu, classes, data_icon, text, arrow + active, ul, "#", GetMenuId(text), "data-toggle='dropdown'");

                return li;
            }
            else
            {
                if (level == 1)
                {
                    if (InternalClickable(childNode))
                    {
                        string url = String.Format("/{0}/{1}", childNode["controller"], childNode["action"]);//VirtualPathUtility.ToAbsolute(childNode.Url);
                        return RenderSubMenu(leftMenu, classes, data_icon, text, arrow, String.Empty, url, GetMenuId(text), String.Empty);
                    }
                    else
                        return RenderSubMenu(leftMenu, classes, data_icon, text, arrow, String.Empty, "#", GetMenuId(text), String.Empty);
                }
                else
                {
                    string url = VirtualPathUtility.ToAbsolute(childNode.Url);
                    return RenderSubMenuItem(leftMenu, leftMenu ? subs : string.Empty, GetTitleTranslated(childNode.Title), url, leftMenu ? current : string.Empty, GetMenuId(text));
                }
            }
        }

        internal static string RenderMenu(bool useLeftMenu, params object []args)
        {
            if (useLeftMenu)
                string.Format(vm_ul_dropdown_menu, args);

            return string.Format(hm_ul_dropdown_menu, args);

        }


        internal static string RenderSubMenu(bool useLeftMenu, params object[] args)
        {
            if (useLeftMenu)
                string.Format(vm_li_dropdown_subm, args);

            return string.Format(hm_li_dropdown_subm, args);
        }

        internal static string RenderSubMenuItem(bool useLeftMenu, params object[] args)
        {
            if (useLeftMenu)
                string.Format(vm_li_menuitem, args);

            return string.Format(hm_li_menuitem, args);

        }

        private static bool InternalClickable(SiteMapNode node)
        {
            string isClickableValue = node["is-clickable"];
            bool isClickable = false;
            if (isClickableValue != null)
                isClickable = Boolean.Parse(isClickableValue);

            return isClickable;
        }

        public static bool IsClickable(this HtmlHelper htmlHelper, SiteMapNode node)
        {
            return InternalClickable(node);
        }

        #endregion
    }
}