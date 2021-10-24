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

        private ViewModelBase _currentViewModel = null;
        public ViewModelBase CurrentViewModel {
            get => _currentViewModel;
            set
            {
                _currentViewModel = value;
                RaisePropertyChanged();
            }
        }
    }
}
