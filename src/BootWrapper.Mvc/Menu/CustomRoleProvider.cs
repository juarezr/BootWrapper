using System;
using System.Linq;
using System.Web.Security;

using System.Collections.Generic;
using BootWrapper.Mvc.ViewModels;
using BootWrapper.Mvc.Core;

namespace BootWrapper.Mvc.Menu
{
    /// <summary>
    /// A classe CustomeRoleProvider deve ser declarada no arquivo web.config para utilização de perfis junto com o Sitemap.
    /// Os perfis são verificados utilizandos os perfis criados no UserSystem.
    /// Segue exemplo de declaração no web.config:
    /// <code>
    /// <![CDATA[
    /// <roleManager enabled="true" cacheRolesInCookie="true" defaultProvider="CustomRoleProvider">
    ///     <providers>
    ///         <clear />
    ///         <add name="CustomRoleProvider" type="BootWrapper.Mvc.Menu.CustomRoleProvider, BootWrapper.BW" />
    ///     </providers>
    ///     </roleManager> 
    /// ]]>
    ///  </code>
    ///  
    /// No arquivo sitemap informe os perfis que poderão ter acesso à página através do atributo 'roles'.
    /// 
    /// </summary>
    public class CustomRoleProvider : RoleProvider
    {
        /// <summary>
        /// Obtém todos os perfis cadastrados para o usuário.
        /// </summary>
        /// <param name="username">Esse parâmetro pode ser ignorado. O usuário é pego da Session.</param>
        /// <returns>Lista com os perfis encontrados no UserSysem. A lista de perfis é carregada durante o Login.</returns>
        public override string[] GetRolesForUser(string username)
        {
            return Permissions.Instance.GetRolesForUser(username).ToArray();
        }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override string ApplicationName
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings.Get("APP_NAME");
            }
            set
            {                
            }
        }

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            return Permissions.Instance.GetAllRoles();
        }

        public override string[] GetUsersInRole(string roleName)
        {
            //return Permissions.Instance.GetUsersInRole(roleName);
            throw new NotImplementedException();
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            return Permissions.Instance.IsUserInRole(username, roleName);
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            return Permissions.Instance.RoleExists(roleName);
        }
    }
}
