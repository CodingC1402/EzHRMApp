using System;
using System.Collections.Generic;
using System.Text;
using DAL.Others;
using DAL.Repos;
using DAL.Rows;

namespace Model
{
    public class CompanyRuleModel : Variables
    {
        public CompanyRuleModel() { }
        public CompanyRuleModel(Variables variable) : base(variable) { }

        public static CompanyRuleModel GetLastestRule()
        {
            var result = VariablesRepo.Instance.GetLatestVariables();
            if (result == null)
                return null;
            else
                return new CompanyRuleModel(result);
        }

        public static string UpdateCompanyRule(CompanyRuleModel companyRule)
        {
            return companyRule.Save(null);
        }

        public static string UpdateBothRuleAndSchedule(CompanyScheduleModel companyScheduleModel, CompanyRuleModel companyRuleModel)
        {
            using (UnitOfWork uow = new UnitOfWork())
            {
                string result = companyRuleModel.Save(uow);
                if (!string.IsNullOrEmpty(result))
                {
                    return result;
                }

                result = companyScheduleModel.Save(uow);
                if (!string.IsNullOrEmpty(result))
                {
                    return result;
                }

                return uow.Complete() ? "" : "Unknow error!";
            }
        }
    }
}
