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
        public ObservableCollection<DAL.Rows.Employee> EmployeeList;
        public override string ViewName => "Staffs";

        public StaffsViewModel()
        {
            EmployeeList = new ObservableCollection<DAL.Rows.Employee>(DAL.Repos.EmployeeRepo.Instance.GetAll());
        }
    }
}
