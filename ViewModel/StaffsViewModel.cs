using DAL;
using Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel
{
    public class StaffsViewModel : Navigation.ViewModelBase
    {
        public ObservableCollection<Staffs.Employee> EmployeeList { get; set; }
        public override string ViewName => "Staffs";

        public StaffsViewModel()
        {
            EmployeeList = Staffs.GetList();
        }
    }
}
