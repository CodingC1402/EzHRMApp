using DAL.Repos;
using DAL.Rows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Model
{
    public class CalculateSalaryModel : CalculateSalary
    {
        public static ObservableCollection<CalculateSalaryModel> LoadAll()
        {
            var list = CalculateSalaryRepo.Instance.GetAll();
            var resultList = new ObservableCollection<CalculateSalaryModel>();

            foreach (var item in list)
            {
                resultList.Add(new CalculateSalaryModel(item));
            }

            return resultList;
        }

        public CalculateSalaryModel(CalculateSalary cs) : base(cs) { }

        public CalculateSalaryModel(string ten, int kyHanTraLuongTheoNgay, DateTime lanTraLuongCuoi, DateTime ngayTinhLuongThangNay) : base()
        {
            Ten = ten;
            KyHanTraLuongTheoNgay = kyHanTraLuongTheoNgay;
            LanTraLuongCuoi = lanTraLuongCuoi;
            NgayTinhLuongThangNay = ngayTinhLuongThangNay;
        }
    }
}
