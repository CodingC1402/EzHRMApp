using DAL.Others;
using DAL.Rows;
using System.Collections.ObjectModel;

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

        public DepartmentModel() { }
        public DepartmentModel(Department department) : base(department) 
        {
            TenPhong = department.TenPhong;
            TruongPhong = department.TruongPhong;
            NgayNgungHoatDong = department.NgayNgungHoatDong;
            NgayThanhLap = department.NgayThanhLap;
        }
        public DepartmentModel(DepartmentModel department) : base(department) { }

        public SaveResult Update(Department updatedValue)
        {
            var result = SaveResult.Ok;

            UnitOfWork uow = new UnitOfWork();

            if (!Update(updatedValue, uow))
                result |= SaveResult.FailData;

            uow.Complete();
            return result;
        }

        public SaveResult Add()
        {
            var result = SaveResult.Ok;

            UnitOfWork uow = new UnitOfWork();

            if (!Add(uow))
                result |= SaveResult.FailData;

            uow.Complete();
            return result;
        }

        public static int GetIndex(EmployeeModel employee, ObservableCollection<DepartmentModel> arr)
        {
            for (int i = 0; i<arr.Count; i++)
            {
                if (employee.PhongBan == arr[i].TenPhong)
                    return i;
            }

            return -1;
        }
    }
}
