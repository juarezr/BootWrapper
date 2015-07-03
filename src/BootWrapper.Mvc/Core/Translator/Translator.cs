using System;
using System.Web;
using System.Data.SqlClient;
using System.Data;

namespace BootWrapper.Mvc.Core.Translator
{
    /// <summary>
    /// Classe para realizar a tradução dos recursos utilizados em uma view.
    /// Para ligar/desligar a tradução coloque no Web.config em appSettings: 
    /// 
    /// <code language="xml"><add key="TRADUTOR" value="false" /></code>
    /// <code language="xml"><add key="TRADUTOR_PROC" value="NOME_DA_PROCEDURE" /></code>
    /// 
    /// Uma vez ligado, os metodos extendidos do html procurarão a tradução automaticamente no banco de dados.
    ///
    ///  As traduções são organizadas no seguinte modelo;
    /// 1. ResourceGroup: Grupo de recursos, normalmente utiliza-se o padrão "/Controller/Action" como id do grupo.
    /// 2. ResourceKey: Trandução do recurso dentro do grupo.
    /// 
    /// </summary>
    /// <seealso cref="T:BootWrapper.Mvc.Controls.WebControls.BWLabel"/>
    public static class Translator
    {
        private const string COMMON = "CommonTerms";
        private const string TRADUTOR_ON_OFF = "TRADUTOR";
        private const string TRADUTOR_PROC = "TRADUTOR_PROC";

        /// <summary>
        /// Obtêm o idioma configurado no browser.
        /// </summary>
        /// <returns>O idioma/cultura do browser. Ex: pt-BR</returns>
        public static string GetUserLanguage()
        {
            if (HttpContext.Current.Request.UserLanguages != null)
                return HttpContext.Current.Request.UserLanguages[0];

            return String.Empty;
        }

        /// <summary>
        /// Obtêm tradução do recurso dentro do grupo informado.
        /// </summary>
        /// <param name="resourceGroup">Grupo do recurso, normalmente formado por /Controller/Action</param>
        /// <param name="resourceKey">Chave do recurso.</param>
        /// <param name="defaultValue">Valor padrão caso a tradução não seja encontrada.</param>
        /// <returns>O recurso traduzido no idioma ou o valor padrão.</returns>
        public static string GetLocalString(string resourceGroup, string resourceKey, string defaultValue = "")
        {
            var cultureInfo = GetUserLanguage();
            return GetLocalStringByCulture(resourceGroup, resourceKey, cultureInfo, defaultValue);
        } 

        /// <summary>
        /// Obtêm tradução do recurso dentro do grupo e idiomas informados.
        /// </summary>
        /// <param name="resourceGroup">Grupo do recurso, normalmente formado por /Controller/Action</param>
        /// <param name="resourceKey">Chave do recurso.</param>
        /// <param name="cultureInfo">Idioma a ser utilizado. Ex: pt-BR</param>
        /// <param name="defaultValue">Valor padrão caso a tradução não seja encontrada.</param>
        /// <returns>O recurso traduzido no idioma ou o valor padrão.</returns>
        public static string GetLocalStringByCulture(string resourceGroup, string resourceKey, string cultureInfo, string defaultValue="")
        {   
            var translated = Translator.GetString(resourceGroup, resourceKey, cultureInfo);
            if (String.IsNullOrEmpty(translated))
                translated = defaultValue;

            return translated;
        }

        /// <summary>
        /// Obtém tradução para recurso comum a todos os grupos. O recurso comum deve ser do grupo com nome CommonTerms.
        /// </summary>
        /// <param name="resourceKey">Chave do recurso.</param>
        /// <param name="defaultValue">Valor padrão caso a tradução não seja encontrada.</param>
        /// <returns>O recurso traduzido no idioma ou o valor padrão.</returns>
        public static string GetCommonString(string resourceKey, string defaultValue = "")
        {
            var cultureInfo = GetUserLanguage();
            return GetCommonStringByCulture(resourceKey, cultureInfo, defaultValue);
        }

        /// <summary>
        /// Obtém tradução para recurso comum a todos os grupos. O recurso comum deve ser do grupo com nome CommonTerms.
        /// </summary>
        /// <param name="resourceKey">Chave do recurso.</param>
        /// <param name="cultureCode">Idioma a ser utilizado. Ex: pt-BR</param>
        /// <param name="defaultValue">Valor padrão caso a tradução não seja encontrada.</param>
        /// <returns>O recurso traduzido no idioma ou o valor padrão.</returns>
        public static string GetCommonStringByCulture(string resourceKey, string cultureCode, string defaultValue = "")
        {
            var translated = Translator.GetString(COMMON, resourceKey, cultureCode);
            if (String.IsNullOrEmpty(translated))
                translated = defaultValue;

            return translated;
        }
     
        /// <summary>
        /// Se tradução não está ligada, retorna o valor default.
        /// </summary>
        /// <param name="resourceGroup">Grupo do recurso, normalmente formado por /Controller/Action</param>
        /// <param name="resourceKey">Chave do recurso.</param>
        /// <param name="cultureCode">Idioma a ser utilizado. Ex: pt-BR</param>
        /// <param name="defaultValue">Valor padrão caso a tradução não seja encontrada.</param>
        /// <returns>O recurso traduzido no idioma ou o valor padrão.</returns>
        private static string GetString(string resourceGroup, string resourceKey, string cultureCode, string defaultValue = "")
        {
            if (!IsTranslatorOn())
                return defaultValue;

            return GetTranslation(resourceGroup, resourceKey, cultureCode);
        }

        /// <summary>
        /// Verifica se tradutor está ligado.
        /// </summary>
        /// <returns>True se tradutor está ligado, False caso contrário.</returns>
        public static bool IsTranslatorOn()
        {
            bool result;
            if (Boolean.TryParse(System.Configuration.ConfigurationManager.AppSettings[TRADUTOR_ON_OFF], out result))
                return result;

            return false;
            
        }

        /// <summary>
        /// Obtém a proc a ser chamada para realizar a tradução.
        /// </summary>
        /// <returns>O nome da procedure do banco de dados.</returns>
        public static string GetTradutorProc()
        {
            string result = System.Configuration.ConfigurationManager.AppSettings.Get(TRADUTOR_PROC);
            if (!String.IsNullOrEmpty(result))
                return result;

            return String.Empty;
        }

        /// <summary>
        /// Faz a chamada da procedure para realizar a tradução.
        /// </summary>
        /// <param name="resourgeGroup">Grupo do recurso, normalmente formado por /Controller/Action</param>
        /// <param name="resourceKey">Chave do recurso.</param>
        /// <param name="cultureCode">Idioma a ser utilizado. Ex: pt-BR</param>
        /// <returns>O recurso traduzido no idioma ou o valor padrão.</returns>
        public static String GetTranslation(string resourgeGroup, string resourceKey, string cultureCode)
        {            
            using (var con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["FrameworkContext"].ConnectionString))
            {
                using (var cmd = new SqlCommand(GetTradutorProc(), con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@ResourceGroup", SqlDbType.VarChar).Value = resourgeGroup;
                    cmd.Parameters.Add("@ResourceKey", SqlDbType.VarChar).Value = resourceKey;
                    cmd.Parameters.Add("@CultureCode", SqlDbType.VarChar).Value = cultureCode;

                    con.Open();
                    return cmd.ExecuteScalar() as String;
                }
            }   
        }
    }
}
