using DAL.Repos;
using DAL.Rows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Linq;

namespace Model
{
    public class DailyReportModel : DailyReport
    {
        public static ObservableCollection<DailyReportModel> LoadAll()
        {
            var reports = DailyReportRepo.Instance.GetAll();
            var resultList = new ObservableCollection<DailyReportModel>();

            foreach (var report in reports)
            {
                resultList.Add(new DailyReportModel(report));
            }
            return resultList;
        }

        public DailyReportModel() { }
        public DailyReportModel(DailyReport report) : base(report) { }

        public static DailyReportModel GetReportOfDate(DateTime date)
        {
            var report = DailyReportRepo.Instance.FindByID(new object[] { date });
            if (report == null)
                return null;
            else
            {
                return new DailyReportModel(report);
            }
        }

        public static List<DailyReportModel> GetDailyReportsInTimeSpan(DateTime start, DateTime end)
        {
            var queryResult = DailyReportRepo.Instance.GetAllReportInTimeSpan(start, end);
            List<DailyReportModel> result = new List<DailyReportModel>();
            foreach (var report in queryResult)
            {
                result.Add(new DailyReportModel(report));
            }

            return new List<DailyReportModel>(result.OrderBy(p => p.NgayBaoCao));
        }

        public bool CompareData(DailyReportModel model)
        {
            return !(model.SoNVDenDungGio == SoNVDenDungGio || 
                model.SoNVDenSom == SoNVDenSom || 
                model.SoNVDenTre == SoNVDenTre || 
                model.SoNVLamThemGio == SoNVLamThemGio ||
                model.SoNVTanLamDungGio == SoNVTanLamDungGio ||
                model.SoNVTanLamSom == SoNVTanLamSom);
        }
    }
}