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
            if (uow == null)
            {
                using (var uowNew = new UnitOfWork())
                {
                    DepartmentRepo.Instance.Add(this, uowNew);
                    return ExecuteAndReturn(uowNew);
                }
            }

            return BoolToString(DepartmentRepo.Instance.Add(this, uow));
        }

        public override string Save(UnitOfWork uow = null)
        {
            if (uow == null)
            {
                using (var uowNew = new UnitOfWork())
                {
                    DepartmentRepo.Instance.Update(new object[] { TenPhong }, this, uowNew);
                    return ExecuteAndReturn(uowNew);
                }
            }

            return BoolToString(DepartmentRepo.Instance.Update(new object[] { TenPhong }, this, uow));
        }
    }
}
