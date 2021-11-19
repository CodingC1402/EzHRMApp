using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModel.Navigation;
using Model;

namespace ViewModel
{
    public class WeeklyScheduleViewModel :  ViewModelBase
    {
        public override string ViewName => "Weekly Schedule";

        public TimetableModel CurrentTimetable => TimetableModel.CurrentTimetableModel;

        public CompanyRuleModel CurrentRules => CompanyRuleModel.GetLastestRule();
    }
}
