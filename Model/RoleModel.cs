using DAL.Rows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Model
{
    public class RoleModel : Role
    {
        public static ObservableCollection<RoleModel> LoadAll()
        {
            var rows = DAL.Repos.RoleRepo.Instance.GetAll();
            var result = new ObservableCollection<RoleModel>();

            foreach (var row in rows)
            {
                result.Add(new RoleModel(row));
            }

            return result;
        }

        public RoleModel() { }
        public RoleModel(Role role) : base(role) { }
        public RoleModel(RoleModel roleModel) : base(roleModel) { }
    }
}
