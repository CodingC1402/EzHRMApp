using DAL.Others;
using DAL.Repos;
using DAL.Rows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Model
{
    public class PaymentMethodModel : PaymentMethod
    {
        //public bool IsSpecialType { get => Ten == "TheoThang" || Ten == "TheoGio"; }

        public static ObservableCollection<PaymentMethodModel> LoadAll()
        {
            var list = PaymentMethodRepo.Instance.GetAll();
            var resultList = new ObservableCollection<PaymentMethodModel>();

            foreach (var item in list)
            {
                resultList.Add(new PaymentMethodModel(item));
            }

            return resultList;
        }

        public PaymentMethodModel() { }

        public PaymentMethodModel(PaymentMethod cs) : base(cs) { }

        public PaymentMethodModel(string ten, int kyHanTraLuongTheoNgay, DateTime lanTraLuongCuoi, DateTime ngayTinhLuongThangNay) : base()
        {
            Ten = ten;
            KyHanTraLuongTheoNgay = kyHanTraLuongTheoNgay;
            LanTraLuongCuoi = lanTraLuongCuoi;
            NgayTinhLuongThangNay = ngayTinhLuongThangNay;
        }

        //public string Delete()
        //{
        //    if (IsSpecialType)
        //        return "You can't delete this template because it's a template used by the system!";

        //    //DB contraints perhaps?
        //    //var keys = new KeyValuePair<string, object>[2];
        //    //keys[0] = new KeyValuePair<string, object>("CachTinhLuong", Ten);
        //    //keys[1] = new KeyValuePair<string, object>("DaXoa", 0);

        //    //var roles = new List<Role>(RoleRepo.Instance.FindBy(keys, false));
        //    //if (roles.Count > 0)
        //    //{
        //    //    return "There is at least one role that is using this method!";
        //    //}

        //    using (UnitOfWork uow = new UnitOfWork())
        //    {
        //        PaymentMethodRepo.Instance.Remove(new object[] { Ten }, uow);
        //        return uow.Complete() ? "" : "Unknow error!";
        //    }
        //}
    }
}
