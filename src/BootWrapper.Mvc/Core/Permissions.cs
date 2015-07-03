using BootWrapper.Mvc.Core;
using BootWrapper.Mvc.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BootWrapper.Mvc.Core
{   
    public class Permissions
    {
        // Singleton pattern
        private static Permissions _instance = null;

        private IRoleProvider _provider;
        public IRoleProvider Provider
        {
            get
            {
                if (_provider == null)
                    throw new InvalidOperationException("IPermissonProvider has not been initialized.");

                return _provider;
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("IPermissonProvider can not be set to null.");

                this._provider = value;
            }
        }        

        private Permissions()
        {
            InitProvider();
        }

        public static Permissions Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Permissions();
                }

                return _instance;
            }
        }

        private void InitProvider()
        {
            _provider = BWConfigurationSection.GetConfig().RoleProvider.GetProvider();
        }
        
                
        public List<String> GetRolesForUser(string login)
        {
            return Provider.GetRolesForUser(login);
        }

        public bool IsUserInRole(string login, string regra)
        {
            return Provider.IsUserInRole(login, regra);
        }

        public bool IsUserAllowed(string login, string controllerName, string actionName)
        {
            return _provider.IsUserAllowed(login, controllerName, actionName);            
        }

        public string[] GetAllRoles()
        {
            return Provider.GetAllRoles().ToArray<String>();
        }

        public bool RoleExists(string regra)
        {
            var regras = GetAllRoles();
            if (regras.Contains(regra))
                return true;

            return false;
        }

        public ILoginUser Auth(string login, string password)
        {
            return Provider.Auth(login, password);
        }       
    }
}
