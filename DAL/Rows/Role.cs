using DAL.Others;
using DAL.Repos;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Rows
{
    public class Role : Row
    {
        public string TenChucVu { get; set; }
        public string CachTinhLuong { get; set; }
        public float TienLuongMoiGio { get; set; }
        public float TienLuongMoiThang { get; set; }
        public float PhanTramLuongNgoaiGio { get; set; }
        public int DaXoa { get; set; }

        public Role() { }
        public Role(Role role)
        {
            TenChucVu = role.TenChucVu;
            CachTinhLuong = role.CachTinhLuong;
            TienLuongMoiGio = role.TienLuongMoiGio;
            TienLuongMoiThang = role.TienLuongMoiThang;
            PhanTramLuongNgoaiGio = role.PhanTramLuongNgoaiGio;
            DaXoa = role.DaXoa;
        }

        public override string Add(UnitOfWork uow = null)
        {
            var result = this.CheckForError();

            if (result != "")
                return result;

            result = this.IsNameTaken(TenChucVu);

            if (result != "")
                return result;

            if (uow == null)
            {
                using (var uowNew = new UnitOfWork())
                {
                    RoleRepo.Instance.Add(this, uowNew);
                    return ExecuteAndReturn(uowNew);
                }
            }

            if (RoleRepo.Instance.Add(this, uow))
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
                    RoleRepo.Instance.Update(new object[] { TenChucVu }, this, uowNew);
                    return ExecuteAndReturn(uowNew);
                }
            }

            if (RoleRepo.Instance.Update(new object[] { TenChucVu }, this, uow))
                return "";
            else
                return "Failed!";
        }

        public override string CheckForError()
        {
            try
            {
                if (!IsValidRoletName(TenChucVu))
                    return "Role's name can't be empty!";

                return "";
            }
            catch (Exception e)
            {
                return $"Unknow error: {e.Message}";
            }
        }

        public bool IsValidRoletName(string name)
        {
            if (name == null || name == "")
                return false;

            return true;
        }

        public string IsNameTaken(string name)
        {
            if (RoleRepo.Instance.FindByID(new object[] { name }) != null)
                return "Role's name is already taken!";

            return "";
        }
    }
}
