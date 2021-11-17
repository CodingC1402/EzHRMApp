using Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel
{
    public class RoleManagementViewModel : Navigation.CRUDViewModelBase
    {
        public override string ViewName => "Positions";
        private static RoleManagementViewModel _instance = null;
        public static RoleManagementViewModel Instance { get => _instance; }

        public List<string> CachTinhLuong { get; set; }

        public ObservableCollection<RoleModel> Roles { get; set; }

        public RoleManagementViewModel()
        {
            _instance = this;
            Roles = RoleModel.LoadAll();
            CachTinhLuong = new List<string>();
            CachTinhLuong.Add("Theo giờ");
            CachTinhLuong.Add("Theo tháng");
        }
    }
}
