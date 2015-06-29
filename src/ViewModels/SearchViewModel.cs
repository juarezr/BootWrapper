using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BootWrapper.BW.ViewModels
{
    public class SearchViewModel<TEntityType, TResultType> : GridViewModel<TResultType>
    {
        public TEntityType Params { get; set; }


        public SearchViewModel()
            : base()
        {
            this.Params = Activator.CreateInstance<TEntityType>();
        }
    }
}
