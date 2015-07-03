using BootWrapper.BW.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace BootWrapper.BW.Core
{
    public class ApplicationSession
    {
        private const string LAST_EXCEPTION = "_LAST_EXCEPTION";        
        private const string USER_LOGGED = "_USER_LOGGED";

        public static ExceptionContext LastExceptionContext
        {
            get 
            {
                return System.Web.HttpContext.Current.Session[LAST_EXCEPTION] as ExceptionContext;
            }
            set
            {
                System.Web.HttpContext.Current.Session[LAST_EXCEPTION] = value;
            }
        }


        /// <summary>
        /// Informações do usuário autenticado no sistema.
        /// </summary>
        public static ILoginUser LoginUser
        {
            get
            {
                return System.Web.HttpContext.Current.Session[USER_LOGGED] as ILoginUser;
            }
            set
            {
                System.Web.HttpContext.Current.Session[USER_LOGGED] = value;
            }
        }       
    }
}
