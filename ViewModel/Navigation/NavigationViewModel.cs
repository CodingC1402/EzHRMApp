using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel.Navigation
{
    public class NavigationViewModel : ViewModelBase
    {
        public override string ViewName => "Main";

        private List<ViewModelBase> _viewModels = new List<ViewModelBase>();
        public List<ViewModelBase> ViewModels { get => _viewModels; }

        public ViewModelBase CurrentViewModel { get; set; }


    }
}
