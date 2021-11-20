using System;
using System.Collections.ObjectModel;
using System.Text;
using DAL.Rows;
using DAL.Repos;
using System.Collections.Generic;

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

        public static List<MonthlyReportModel> GetReportsBetweenDates(DateTime startDate, DateTime endDate)
        {
            var reports = new List<MonthlyReportModel>();
            var startMonth = new DateTime(startDate.Year, startDate.Month, 1);
            var endMonth = new DateTime(endDate.Year, endDate.Month, DateTime.DaysInMonth(endDate.Year, endDate.Month));
            var resultList = MonthlyReportRepo.Instance.GetBetween(startMonth, endMonth);

            foreach (var report in resultList)
            {
                reports.Add(new MonthlyReportModel(report));
            }
            return reports;
        }
    }
}
