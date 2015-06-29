using System.Web.WebPages;
namespace System.Web.Mvc.Html
{
    /// <summary>
    /// Métodos extendidos para controle de seçoes nas views.
    /// </summary>
    public static class SectionExtensions
    {
        private static readonly object _o = new object();

        /// <summary>
        /// Cria nova section com o nome informado em sectionName. Se section já existe ela é sobrescrita.
        /// </summary>
        /// <param name="page">Objeto implicito para o método extendido.</param>
        /// <param name="sectionName">Nome da section.</param>     
        /// <param name="defaultContent">Conteúdo default caso a section não seja definifida pelo desenvolvedor.</param>
        /// <returns>HelperResult</returns>
        public static HelperResult BWRenderSection(this WebPageBase page, string sectionName, Func<object, HelperResult> defaultContent)
        {
            if (page.IsSectionDefined(sectionName))
                return page.BWRenderSection(sectionName);
            else
                return defaultContent(_o);
        }

        /// <summary>
        /// Cria nova section com o nome informado em sectionName. Se section já existe ela é sobrescrita.
        /// </summary>
        /// <param name="page">Objeto implicito para o método extendido.</param>
        /// <param name="sectionName">Nome da section.</param>        
        /// <returns>HelperResult</returns>
        public static HelperResult BWRenderSection(this WebPageBase page, string sectionName)
        {
            return BWRedefineSection(page, sectionName);
        }

        /// <summary>
        /// Cria nova section com o nome informado em sectionName. Se section já existe ela é sobrescrita.
        /// </summary>
        /// <param name="page">Objeto implicito para o método extendido.</param>
        /// <param name="sectionName">Nome da section.</param>
        /// <param name="defaultContent">Conteúdo default caso a section não seja definifida pelo desenvolvedor.</param>
        public static HelperResult BWRedefineSection(this WebPageBase page, string sectionName, Func<object, HelperResult> defaultContent = null)
        {
            if (page.IsSectionDefined(sectionName))
                page.DefineSection(sectionName, () => page.Write(page.RenderSection(sectionName)));
            else if (defaultContent != null)
                page.DefineSection(sectionName, () => page.Write(defaultContent(_o)));
            return new HelperResult(_ => { });
        }
    }
}
