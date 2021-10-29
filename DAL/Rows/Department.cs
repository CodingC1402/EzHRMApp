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
        public DateTime NgayNgungHoatDong { get; set; }
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

        public override bool Save(UnitOfWork uow = null)
        {
            if (uow == null)
            {
                using (var uowNew = new UnitOfWork())
                {
                    DepartmentRepo.Instance.Update(new object[] { TenPhong }, this, uow);
                    return uowNew.Complete();
                }
            }

            return DepartmentRepo.Instance.Update(new object[] { TenPhong }, this, uow);
        }

        public bool Add(UnitOfWork uow = null)
        {
            // why ?
            if (uow == null)
            {
                using (var uowNew = new UnitOfWork())
                {
                    DepartmentRepo.Instance.Update(new object[] { TenPhong }, this, uow);
                    return uowNew.Complete();
                }
            }

            return DepartmentRepo.Instance.Add(this, uow);
        }
    }
}
