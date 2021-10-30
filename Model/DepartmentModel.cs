using DAL.Rows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Model
{
    public class DepartmentModel : Department
    {
        public enum SaveResult
        {
            Ok = 0,
            FailData = 1
        }

        public static ObservableCollection<DepartmentModel> LoadAll()
        {
            var rows = DAL.Repos.DepartmentRepo.Instance.GetAll();
            var result = new ObservableCollection<DepartmentModel>();

            foreach (var row in rows)
            {
                result.Add(new DepartmentModel(row));
            }
            return result;
        }

        public static int GetIndex(EmployeeModel employee, ObservableCollection<DepartmentModel> arr)
        {
            for (int i = 0; i < arr.Count; i++)
            {
                if (employee.PhongBan == arr[i].TenPhong)
                    return i;
            }

            return -1;
        }

        public DepartmentModel(Department department)
        {
            TenPhong = department.TenPhong;
            TruongPhong = department.TruongPhong;
        }
    }
}
