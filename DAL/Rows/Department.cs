using DAL.Others;
using DAL.Repos;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Rows
{
    public class Department : Row
    {
        public string TenPhong { get; set; }
        public DateTime NgayThanhLap { get; set; }
        public DateTime? NgayNgungHoatDong { get; set; }
        public string TruongPhong { get; set; }

        public Department() { }
        public Department(Department department)
        {
            TenPhong = department.TenPhong;
            NgayThanhLap = department.NgayThanhLap;
            NgayNgungHoatDong = department.NgayNgungHoatDong;
            TruongPhong = department.TruongPhong;
        }

        public bool ChangeManager(Employee employee)
        {
            this.TruongPhong = employee.ID;
            using (var uow = new UnitOfWork())
            {
                Save(uow);
                return uow.Complete();
            }
        }

        public override string Add(UnitOfWork uow = null)
        {
            var result = this.CheckForError();

            if (result != "")
                return result;

            result = this.IsNameTaken(TenPhong);

            if (result != "")
                return result;

            if (uow == null)
            {
                using (var uowNew = new UnitOfWork())
                {
                    DepartmentRepo.Instance.Add(this, uowNew);
                    return ExecuteAndReturn(uowNew);
                }
            }

            if (DepartmentRepo.Instance.Add(this, uow))
                return "";
            else
                return "Failed!";
        }

        public override string Save(UnitOfWork uow = null)
        {
            var result = this.CheckForError();
        
            if (result != "")
                return result;

            if (uow == null)
            {
                using (var uowNew = new UnitOfWork())
                {
                    DepartmentRepo.Instance.Update(new object[] { TenPhong }, this, uowNew);
                    return ExecuteAndReturn(uowNew);
                }
            }

            if (DepartmentRepo.Instance.Update(new object[] { TenPhong }, this, uow))
                return "";
            else
                return "Failed!";
        }

        public override string CheckForError()
        {
            try
            {
                if (!IsValidDepartmentName(TenPhong))
                    return "Department's name can't be empty!";

                if (NgayNgungHoatDong.HasValue && NgayNgungHoatDong.Value < NgayThanhLap)
                {
                    return "Shutdown date can't be before founding date!";
                }

                if (!IsValidManagerID(TruongPhong))
                {
                    return "Unknown manager ID: " + TruongPhong;
                }

                return "";
            }
            catch (Exception e)
            {
                return $"Unknow error: {e.Message}";
            }
        }

        public bool IsValidDepartmentName(string name)
        {
            if (name == null || name == "")
                return false;

            return true;
        }

        public string IsNameTaken(string name)
        {
            if (DepartmentRepo.Instance.FindByID(new object[] { name }) != null)
                return "Department's name is already taken!";

            return "";
        }

        public bool IsValidManagerID(string id)
        {
            if (id == null || id == "")
                return true;

            try
            {
                if (EmployeeRepo.Instance.FindByID(new object[] { id }) == null)
                    return false;
                else
                    return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
