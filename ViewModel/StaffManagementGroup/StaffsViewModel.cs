using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace ViewModel
{
    public class StaffsViewModel : Navigation.ViewModelBase
    {
        public ObservableCollection<EmployeeModel> Employees { get; set; }

        public StaffsViewModel()
        {
            Employees = EmployeeModel.LoadAll();
        }
    }
}
