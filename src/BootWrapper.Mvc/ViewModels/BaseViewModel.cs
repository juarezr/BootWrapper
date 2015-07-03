using System;
using System.Collections.Generic;
using System.Web;
using BootWrapper.Mvc.Core.Translator;
using BootWrapper.Mvc.Exceptions;

namespace BootWrapper.Mvc.ViewModels
{
    public delegate void LogoutEventHandler(object sender, EventArgs e);

    public partial class BaseViewModel : IDisposable
    {
        //public static event LogoutEventHandler RedirectOut;
        public List<String> Messages { get; set; }
        public List<String> Warnings { get; set; }
        public List<String> Errors { get; set; }

        public BaseViewModel()
        {            
            Messages = new List<String>();
            Warnings = new List<String>();
            Errors = new List<String>();
        }

        public virtual void Dispose()
        {
        }         

        public void AddError(string message, params object[] paramsToFormat)
        {
            message = string.Format(message, paramsToFormat);
            Errors.Add(message);

            new BWException().Log(message);
        }

        public void AddMessage(string message, params object[] paramsToFormat)
        {
            message = string.Format(message, paramsToFormat);
            Messages.Add(message);
        }

        public void AddWarning(string message, params object[] paramsToFormat)
        {
            message = string.Format(message, paramsToFormat);
            Warnings.Add(message);
        }
        protected void HandleErrors(Exception ex)
        {
            AddError(ex.Message);
        }

        /*
        protected string GetCurrentRoute()
        {
            var httpContext = new HttpContextWrapper(HttpContext.Current);
            var routeData = System.Web.Routing.RouteTable.Routes.GetRouteData(httpContext);

            var controllerName = routeData.Values["controller"].ToString();
            var actionName = routeData.Values["action"].ToString();

            return String.Format("/{0}/{1}", controllerName, actionName);
        }

        public string GetLocalString(string resourceKey, string defaultString = "")
        {
            return Translator.GetLocalString(GetCurrentRoute(), resourceKey, defaultString);
        }

        protected string GetCommonString(string resourceKey, string defaultString = "")
        {
            return Translator.GetCommonString(resourceKey, defaultString);
        }
        */
    }
}