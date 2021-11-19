using System;
using System.Collections.ObjectModel;
using System.Text;
using DAL.Rows;
using DAL.Repos;

namespace Model
{
    public class MonthlyReportModel : MonthlyReport
    {
        public static ObservableCollection<MonthlyReportModel> LoadAll()
        {
            var reports = MonthlyReportRepo.Instance.GetAll();
            var resultList = new ObservableCollection<MonthlyReportModel>();

            foreach (var report in reports)
            {
                resultList.Add(new MonthlyReportModel(report));
            }
            return resultList;
        }

        public MonthlyReportModel() { }
        public MonthlyReportModel(MonthlyReport report) : base(report) { }
        public MonthlyReportModel(MonthlyReportModel report) : base(report) { }

        public static ObservableCollection<MonthlyReportModel> GetAllReportOfYear(int year)
        {
            var reports = MonthlyReportRepo.Instance.GetAllInYear(year);
            var resultList = new ObservableCollection<MonthlyReportModel>();

            foreach (var report in reports)
            {
                resultList.Add(new MonthlyReportModel(report));
            }
            return resultList;
        }
    }
}
