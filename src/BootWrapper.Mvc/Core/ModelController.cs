using BootWrapper.Mvc.Core;
using BootWrapper.Mvc.Menu;
using BootWrapper.Mvc.ViewModels;


namespace System.Web.Mvc
{
    public class ModelController : ContextController
    {
        // For using another DbContext instead of CSOContext just inherit from ContextController

        //protected log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public ModelController()            
        {
        }        

        // Methods for creating ViewModel
        protected TViewModel NewViewModel<TViewModel>() where TViewModel : BaseViewModel
        {
            var result = Activator.CreateInstance<TViewModel>();

            //BaseViewModel.RedirectOut += new LogoutEventHandler(this.RedirectLogout);
            return result;
        }

        /* CRIAR METODO PARA PASSAR DBCONTEXT */
        protected TViewModel NewViewModel<TViewModel, TContext>(TContext context) where TViewModel : BaseViewModel
        {
            var result = Activator.CreateInstance<TViewModel>();

            //BaseViewModel.RedirectOut += new LogoutEventHandler(this.RedirectLogout);
            return result;
        }

        protected void RedirectLogout(object sender, EventArgs args)
        {
            if (!Response.IsRequestBeingRedirected)
                Response.Redirect("~/Account/LogOut");            
        }        

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!filterContext.ActionDescriptor.ControllerDescriptor.ControllerName.Equals("Account"))
            {
                HttpContextBase ctx = filterContext.HttpContext;
            
                // check if session is supported
                if (null != ctx.Session)// && UserSession.UserIsAuthenticated())
                {
                    // check if a new session id was generated
                    if (ctx.Session.IsNewSession || ApplicationSession.LoginUser == null)
                    {          
                        if (!Response.IsRequestBeingRedirected)
                            ctx.Response.Redirect("~/Account/Login");                     
                    }
                }
                else
                {
                    if (!Response.IsRequestBeingRedirected)
                        ctx.Response.Redirect("~/Account/Login");
                }

                if (!ctx.Response.IsRequestBeingRedirected)
                {
                    var url = ctx.Request.Url;
                    var route = ctx.Request.RequestContext.RouteData;
                    if (!UserIsAllowed(ctx.ApplicationInstance.Context))
                    {
                        ctx.Response.Redirect("~/Error/Forbidden");
                    }
                }
            }            
            
            base.OnActionExecuting(filterContext);
        }        

        protected override void OnException(ExceptionContext filterContext)
        {
            // Output a nice error page
            if (filterContext.HttpContext.IsCustomErrorEnabled)
            {
                ApplicationSession.LastExceptionContext = filterContext;

                filterContext.ExceptionHandled = true;
                
                filterContext.Result = this.RedirectToAction("Index", "Error");

                base.OnException(filterContext);                               
            }           
        }

        protected bool UserIsAllowed(HttpContext ctx)
        {            
            var login = ApplicationSession.LoginUser.GetLogin();
            var controllerName = (string)ctx.Request.RequestContext.RouteData.Values["controller"];
            var actionName = (string)ctx.Request.RequestContext.RouteData.Values["action"];

            return Permissions.Instance.IsUserAllowed(login, controllerName, actionName);
        }
    }

    public class ContextController : Controller
    {  
        public ContextController()            
        {
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        // Export the html generated for importing in Excel directly from Browser
        // The Razor view should generate one or more html tables
        public ViewResult ExcelView()
        {
            return ExcelView(null /* viewName */, null /* masterName */, null /* model */);
        }

        public ViewResult ExcelView(object model)
        {
            return ExcelView(null /* viewName */, null /* masterName */, model);
        }

        public ViewResult ExcelView(string viewName)
        {
            return ExcelView(viewName, null /* masterName */, null /* model */);
        }

        public ViewResult ExcelView(string viewName, string masterName)
        {
            return ExcelView(viewName, masterName, null /* model */);
        }

        public ViewResult ExcelView(string viewName, object model)
        {
            return ExcelView(viewName, null /* masterName */, model);
        }

        public ViewResult ExcelView(string viewName, string masterName, object model)
        {
            if (model != null)
            {
                this.ViewData.Model = model;
            }

            Response.ContentType = "application/vnd.ms-excel";
            Response.AppendHeader("content-disposition", "attachment: filename=" + this.GetType().Name + ".xls");

            var result = new ViewResult
            {
                ViewName = viewName,
                MasterName = masterName,
                ViewData = this.ViewData,
                TempData = this.TempData               
            };

            return result;
        }

        public ViewResult ExcelView(IView view)
        {
            return ExcelView(view, null /* model */);
        }

        public ViewResult ExcelView(IView view, object model)
        {
            if (model != null)
            {
                this.ViewData.Model = model;
            }

            Response.ContentType = "application/vnd.ms-excel";
            Response.AppendHeader("content-disposition", "attachment: filename=" + this.GetType().Name + ".xls");

            var result = new ViewResult
            {
                View = view,
                ViewData = this.ViewData,
                TempData = this.TempData
            };

            return result;
        }

        // Export the html generated for importing in Microsoft directly from Browser
        // Not all conversion would be great, but you can test a adjust
        public ViewResult WordView()
        {
            return WordView(null /* viewName */, null /* masterName */, null /* model */);
        }

        public ViewResult WordView(object model)
        {
            return WordView(null /* viewName */, null /* masterName */, model);
        }

        public ViewResult WordView(string viewName)
        {
            return WordView(viewName, null /* masterName */, null /* model */);
        }

        public ViewResult WordView(string viewName, string masterName)
        {
            return WordView(viewName, masterName, null /* model */);
        }

        public ViewResult WordView(string viewName, object model)
        {
            return WordView(viewName, null /* masterName */, model);
        }

        public ViewResult WordView(string viewName, string masterName, object model)
        {
            if (model != null)
            {
                this.ViewData.Model = model;
            }

            Response.ContentType = "application/vnd.ms-word";
            Response.AppendHeader("content-disposition", "attachment: filename=" + this.GetType().Name + ".doc");

            var result = new ViewResult
            {
                ViewName = viewName,
                MasterName = masterName,
                ViewData = this.ViewData,
                TempData = this.TempData
            };

            return result;
        }

        public ViewResult WordView(IView view)
        {
            return WordView(view, null /* model */);
        }

        public ViewResult WordView(IView view, object model)
        {
            if (model != null)
            {
                this.ViewData.Model = model;
            }

            Response.ContentType = "application/vnd.ms-word";
            Response.AppendHeader("content-disposition", "attachment: filename=" + this.GetType().Name + ".doc");

            var result = new ViewResult
            {
                View = view,
                ViewData = this.ViewData,
                TempData = this.TempData
            };

            return result;
        }

        // Export the html generated for openning in Reader from Browser
        // For viewing in browser should install Adobe Acrobat Reader plugin
        public ViewResult PDFView()
        {
            return PDFView(null /* viewName */, null /* masterName */, null /* model */);
        }

        public ViewResult PDFView(object model)
        {
            return PDFView(null /* viewName */, null /* masterName */, model);
        }

        public ViewResult PDFView(string viewName)
        {
            return PDFView(viewName, null /* masterName */, null /* model */);
        }

        public ViewResult PDFView(string viewName, string masterName)
        {
            return PDFView(viewName, masterName, null /* model */);
        }

        public ViewResult PDFView(string viewName, object model)
        {
            return PDFView(viewName, null /* masterName */, model);
        }

        public ViewResult PDFView(string viewName, string masterName, object model)
        {
            if (model != null)
            {
                this.ViewData.Model = model;
            }

            Response.ContentType = "application/vnd.ms-word";

            var result =  new PdfFromHtmlResult
            {
                ViewName = viewName,
                MasterName = masterName,
                ViewData = this.ViewData,
                TempData = this.TempData
            };

            return result;
        }

        public ViewResult PDFView(IView view)
        {
            return PDFView(view, null /* model */);
        }

        public ViewResult PDFView(IView view, object model)
        {
            if (model != null)
            {
                this.ViewData.Model = model;
            }

            Response.ContentType = "application/pdf";

            var result = new PdfFromHtmlResult
            {
                View = view,
                ViewData = this.ViewData,
                TempData = this.TempData
            };

            return result;
        }    
    }
}

