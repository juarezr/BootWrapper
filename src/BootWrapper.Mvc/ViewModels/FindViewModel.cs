using System;

namespace BootWrapper.Mvc.ViewModels
{
    public class FindViewModel<TEntityType> : BaseViewModel
    {
        public TEntityType Params { get; set; }

        public FindViewModel()            
        {
            this.Params = Activator.CreateInstance<TEntityType>();
        }
    }
}