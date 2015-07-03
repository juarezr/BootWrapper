using System;

namespace BootWrapper.Mvc.ViewModels
{
    public class EditViewModel<TEntityType> : BaseViewModel
    {
        public TEntityType Entity { get; set; }
        
        public class IdNameResult
        {
            public string Id { get; set; }
            public string Name { get; set; }
        }

        public EditViewModel()
        {
            Entity = Activator.CreateInstance<TEntityType>();
        }      
    }
}