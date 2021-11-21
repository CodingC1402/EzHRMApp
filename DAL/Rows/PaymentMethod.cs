using DAL.Others;
using DAL.Repos;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Rows
{
    public class PaymentMethod : Row
    {
        public string Ten { get; set; }
        public int KyHanTraLuongTheoNgay { get; set; }
        public DateTime LanTraLuongCuoi { get; set; }
        public DateTime? NgayTinhLuongThangNay { get; set; }

        public PaymentMethod(PaymentMethod cs)
        {
            Ten = cs.Ten;
            KyHanTraLuongTheoNgay = cs.KyHanTraLuongTheoNgay;
            LanTraLuongCuoi = cs.LanTraLuongCuoi;
            NgayTinhLuongThangNay = cs.NgayTinhLuongThangNay;
        }
        public PaymentMethod() { }

        //public override string Add(UnitOfWork uow = null)
        //{
        //    if (uow == null)
        //    {
        //        using (var uowNew = new UnitOfWork())
        //        {
        //            PaymentMethod method = PaymentMethodRepo.Instance.FindByID(new object[] { Ten });
        //            if (method != null)
        //            {
        //                return "There is already a penalty of the same name!";
        //            }

        //            PaymentMethodRepo.Instance.Add(this, uowNew);

        //            return ExecuteAndReturn(uowNew);
        //        }
        //    }

        //    if (PaymentMethodRepo.Instance.Add(this, uow))
        //        return "";
        //    else
        //        return "Failed!";
        //}

        public override string Save(UnitOfWork uow = null)
        {
            if (uow == null)
            {
                using (var uowNew = new UnitOfWork())
                {
                    PaymentMethodRepo.Instance.Update(new object[] { Ten }, this, uowNew);
                    return ExecuteAndReturn(uowNew);
                }
            }

            if (PaymentMethodRepo.Instance.Update(new object[] { Ten }, this, uow))
                return "";
            else
                return "Failed!";
        }

        public override string CheckForError()
        {
            if (string.IsNullOrEmpty(Ten))
            {
                return "Method name can't be empty!";
            }

            return "";
        }
    }
}
