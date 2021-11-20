using DAL.Others;
using DAL.Repos;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Rows
{
    public class PenaltyType : Row
    {
        public string TenViPham { get; set; }
        public float TruLuongTheoPhanTram { get; set; }
        public float TruLuongTrucTiep { get; set; }

        public PenaltyType() { }
        public PenaltyType(PenaltyType penaltyType)
        {
            TenViPham = penaltyType.TenViPham;
            TruLuongTheoPhanTram = penaltyType.TruLuongTheoPhanTram;
            TruLuongTrucTiep = penaltyType.TruLuongTrucTiep;
        }

        public override string Add(UnitOfWork uow = null)
        {
            if (uow == null)
            {
                using (var uowNew = new UnitOfWork())
                {
                    PenaltyTypeRepo.Instance.Add(this, uowNew);
                    return ExecuteAndReturn(uowNew);
                }
            }

            if (PenaltyTypeRepo.Instance.Add(this, uow))
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
                    PenaltyTypeRepo.Instance.Update(new object[] { TenViPham }, this, uowNew);
                    return ExecuteAndReturn(uowNew);
                }
            }

            if (PenaltyTypeRepo.Instance.Update(new object[] { TenViPham }, this, uow))
                return "";
            else
                return "Failed!";
        }
    }
}
