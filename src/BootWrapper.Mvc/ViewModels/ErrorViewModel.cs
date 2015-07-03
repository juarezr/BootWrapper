using BootWrapper.Mvc.Model;
using System;
using System.Web.Mvc;

namespace BootWrapper.Mvc.ViewModels
{
    public class ErrorViewModel : BaseViewModel
    {
        private BWModelError _error;
        public BWModelError ErrorDetail { get { return _error; } }

        public ErrorViewModel()
        {
            _error = new BWModelError();
        }

        public ErrorViewModel(BWModelError error)
            : this()
        {
            if (error == null)
                throw new ArgumentNullException();

            _error = error;
        } 

        public ErrorViewModel(Exception ex) : this()
        {
            FormatException(ex);            
        }

        public ErrorViewModel(ExceptionContext ex)
            : this()
        {
            FormatException(ex == null ? new Exception("Erro Desconhecido") : ex.Exception);
        }

        private void FormatException(Exception ex)
        {
            _error = new BWModelError();
            _error.Title = ex != null ? ex.Message : "Erro Desconhecido.";
            _error.FriendlyMessage = "Ooops...";
            _error.DetailMessage = ex != null ? ex.StackTrace : String.Empty;
        }
    }
}
