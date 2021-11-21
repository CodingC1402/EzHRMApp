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

        public PaymentMethodModel(PaymentMethod cs) : base(cs) { }

        public PaymentMethodModel(string ten, int kyHanTraLuongTheoNgay, DateTime lanTraLuongCuoi, DateTime ngayTinhLuongThangNay) : base()
        {
            Ten = ten;
            KyHanTraLuongTheoNgay = kyHanTraLuongTheoNgay;
            LanTraLuongCuoi = lanTraLuongCuoi;
            NgayTinhLuongThangNay = ngayTinhLuongThangNay;
        }
    }
}
