using DAL.Others;
using DAL.Repos;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Rows
{
    public class Penalty : Row
    {
        public const string BeingLate = "DiTre";
        public const string Absence = "VangMat";

        public int ID { get; set; }
        public DateTime Ngay { get; set; }
        public string IDNhanVien { get; set; }
        public string TenViPham { get; set; }
        public float SoTienTru { get; set; }
        public float SoPhanTramTru { get; set; }
        public string GhiChu { get; set; }

        public override string Add(UnitOfWork uow = null)
        {
            if (uow == null)
            {
                using (var uowNew = new UnitOfWork())
                {
                    PenaltyRepo.Instance.Add(this, uowNew);
                    return ExecuteAndReturn(uowNew);
                }
            }

            if (PenaltyRepo.Instance.Add(this, uow))
                return "";
            else
                return "Failed!";
        }

        public override string Save(UnitOfWork uow = null)
        {
            if (uow == null)
            {
                using (var uowNew = new UnitOfWork())
                {
                    PenaltyRepo.Instance.Update(new object[] { ID }, this, uowNew);
                    return ExecuteAndReturn(uowNew);
                }
            }

            if (PenaltyRepo.Instance.Update(new object[] { ID }, this, uow))
                return "";
            else
                return "Failed!";
        }
    }
}
