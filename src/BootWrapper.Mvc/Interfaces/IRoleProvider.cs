using BootWrapper.Mvc.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BootWrapper.Mvc.Interfaces
{
    public interface IRoleProvider
    {
        List<String> GetAllRoles();

        List<String> GetRolesForUser(string login);

        bool IsUserAllowed(string login, string controllerName, string actionName);

        bool IsUserInRole(string login, string role);

        /// <summary>
        /// Autentica usuário. Verifica usuário e senha.
        /// </summary>
        /// <param name="login"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        ILoginUser Auth(string login, string password);

        /// <summary>
        /// Verificar se usuário tem a role Admin
        /// </summary>
        /// <returns></returns>
        bool IsAdmin(string login);
    }

    /// <summary>
    /// Default role provider when it is not declared.
    /// </summary>
    public class DefaultRoleProvider : IRoleProvider
    {
        public DefaultRoleProvider()
        {

        }

        protected List<String> empty = new List<String>();

        public virtual List<String> GetAllRoles() { return empty; }

        public virtual List<String> GetRolesForUser(string login) { return empty; }

        public virtual ILoginUser Auth(string login, string password)
        {
            return new DefaultLoginUser();
        }

        public virtual bool IsUserAllowed(string login, string controllerName, string actionName)
        { 
            return true;
        }

        public virtual bool IsUserInRole(string login, string regra)
        {
            var regrasUsuario = GetRolesForUser(login);
            if (regrasUsuario.Contains(regra))
                return true;

            return false;
        }

        public virtual bool IsAdmin(string login)
        {
            return IsUserInRole(login, "Admin");
        }
    }
}
