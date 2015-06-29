using System;
using System.Web;
using System.Web.Security;
using BootWrapper.BW.Model;
using BootWrapper.BW.Menu;
using BootWrapper.BW.Core;
using BootWrapper.BW.Interfaces;
using System.Net;

namespace BootWrapper.BW.ViewModels
{
    public class LoginViewModel : EditViewModel<BWModelLogin>
    {
        public bool logado = false;
        
        public LoginViewModel()
        {
            BWModelLogin Model = new BWModelLogin();
            
            this.Entity = Model;
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