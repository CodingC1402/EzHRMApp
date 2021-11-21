using System;
using System.Collections.ObjectModel;
using System.Text;
using DAL.Rows;
using DAL.Repos;
using System.Collections.Generic;

namespace Model
{
    public class SalaryModel : Salary
    {
        public static ObservableCollection<SalaryModel> LoadAll()
        {
            var result = new ObservableCollection<SalaryModel>();
            var salaryList = SalaryRepo.Instance.GetAll();

            foreach (var salary in salaryList)
            {
                result.Add(new SalaryModel(salary));
            }
            return result;
        }

        public static List<SalaryModel> GetAllBetween(DateTime startDate, DateTime endDate)
        {
            var result = new List<SalaryModel>();
            var salaryIEnum = SalaryRepo.Instance.GetAllBetween(startDate, endDate);

            foreach (var salary in salaryIEnum)
            {
                result.Add(new SalaryModel(salary));
            }
            return result;
        }

        public static List<SalaryModel> GetByEmployeeAllBetween(string id, DateTime startDate, DateTime endDate)
        {
            var result = new List<SalaryModel>();
            var salaryIEnum = SalaryRepo.Instance.GetByEmployeeBetween(id, startDate, endDate);

            foreach (var salary in salaryIEnum)
            {
                result.Add(new SalaryModel(salary));
            }
            return result;
        }

        public SalaryModel() { }
        public SalaryModel(Salary s) : base(s) { }
    }
}
