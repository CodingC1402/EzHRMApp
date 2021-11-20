using DAL.Repos;
using DAL.Rows;
using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class CompanyScheduleModel : CompanySchedule
    {
        public CompanyScheduleModel() { }
        public CompanyScheduleModel(CompanySchedule companySchedule) : base(companySchedule) { }

        public static CompanyScheduleModel GetLastestSchedule()
        {
            var result = CompanyScheduleRepo.Instance.GetLatestVariables();
            if (result == null)
                return null;
            else
                return new CompanyScheduleModel(result);
        }

        public static string UpdateCompanyRule(CompanyScheduleModel companySchedule)
        {
            return companySchedule.Save(null);
        }
    }
}
