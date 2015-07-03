using System;
using System.Web;
using System.Web.Security;
using BootWrapper.Mvc.Model;
using BootWrapper.Mvc.Menu;
using BootWrapper.Mvc.Core;
using BootWrapper.Mvc.Interfaces;
using System.Net;

namespace BootWrapper.Mvc.ViewModels
{
    public class LoginViewModel : EditViewModel<BWModelLogin>
    {
        public bool logado = false;
        
        public LoginViewModel()
        {
            BWModelLogin Model = new BWModelLogin();
            
            this.Entity = Model;
        }

        public LoginViewModel LogIn()
        {
            return LogIn(this.Entity);
        }

        public LoginViewModel LogIn(BWModelLogin Model)
        {
            try
            {                
                this.logado = false;

                ILoginUser user = Permissions.Instance.Auth(Model.Login, Model.Password);
                if (user != null && user.IsAuthenticated())
                {
                    FormsAuthentication.SetAuthCookie(Model.Login, true);                    
                    ApplicationSession.LoginUser = user;
                    this.logado = true;                   
                }
                else
                {                    
                    this.Messages.Add("Login e/ou Senha inválidos.");
                }                
            }
            catch (Exception ex)
            {
                this.Messages.Add("Ocorreu um erro ao tentar logar no sistema." + ex.Message);
            }

            this.Entity = Model;

            return this;
        }

        

        public void LogOut()
        {            
            FormsAuthentication.SignOut();
            HttpContext.Current.Session.Abandon();            
        }
    }
}