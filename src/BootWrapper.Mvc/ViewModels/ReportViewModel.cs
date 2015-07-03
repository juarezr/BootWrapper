using System;
using System.Collections.Generic;

namespace BootWrapper.Mvc.ViewModels
{
    public class ReportViewModel<TEntityType> : BaseViewModel
    {
        public TEntityType Params { get; set; }
        public List<TEntityType> List { get; set; }
        
        public ReportViewModel() : base()
        {
            this.Params = Activator.CreateInstance<TEntityType>();
            this.List = new List<TEntityType>();            
        }        
    }
}