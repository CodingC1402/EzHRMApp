using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModel.Helper;
using ViewModel.Navigation;

namespace ViewModel
{
    public class ScheduleManagementViewModel : CRUDViewModelBase
    {
        public override string ViewName => "Schedule management";

        private static ScheduleManagementViewModel _instance = null;
        public static ScheduleManagementViewModel Instance { get => _instance; }

        // The actual rule that is referencing
        public CompanyRuleModel CurrentRule { get; set; }
        public CompanyScheduleModel CurrentSchedule { get; set; }

        // The rule that go rendered
        public CompanyRuleModel ViewingRule { get; set; }
        public CompanyScheduleModel ViewingSchedule { get; set; }

        private bool _isWaitingForConfirmation = false;

        public override void ExecuteConfirmUpdate(object param)
        {
            _isWaitingForConfirmation = true;
            ViewingRule = new CompanyRuleModel(CurrentRule);
            ViewingSchedule = new CompanyScheduleModel(CurrentSchedule);

            ViewingRule.ThoiDiemTao = DateTime.Now;
            ViewingSchedule.ThoiDiemTao = DateTime.Now;

            ErrorString = "Are you sure you want to update the company rules?";
            ShowConfirmation = true;
        }

        protected override void PopUpMessageClosed(object sender, EventArgs e)
        {
            base.PopUpMessageClosed(sender, e);

            if (_isWaitingForConfirmation)
            {
                _isWaitingForConfirmation = false;
                if (MessageResult == PopUpMessage.Result.Ok)
                {
                    var result = CompanyRuleModel.UpdateBothRuleAndSchedule(ViewingSchedule, ViewingRule);
                    if (result == "")
                    {
                        CurrentSchedule = ViewingSchedule;
                        CurrentRule = ViewingRule;
                        base.ExecuteConfirmUpdate(null);
                    }
                    else
                    {
                        ErrorString = result;
                        HaveError = true;
                    }
                }
            }
        }

        protected override void OnModeChangeBack()
        {
            ViewingRule = CurrentRule;
            ViewingSchedule = CurrentSchedule;
        }

        public ScheduleManagementViewModel()
        {
            _instance = this;
        }

        public override void OnGetTo()
        {
            ViewingRule = CurrentRule = CompanyRuleModel.GetLastestRule();
            ViewingSchedule = CurrentSchedule = CompanyScheduleModel.GetLastestSchedule();
        }

        private RelayCommand<object> _toggleWorkCommand;
        public RelayCommand<object> ToggleWorkCommand => _toggleWorkCommand ?? (_toggleWorkCommand = new RelayCommand<object>(param => {
            string paramStr = (string)param;
            switch (paramStr)
            {
                case "mon":
                    ViewingRule.CoLamThuHai = !ViewingRule.CoLamThuHai;
                    break;
                case "tue":
                    ViewingRule.CoLamThuBa = !ViewingRule.CoLamThuBa;
                    break;
                case "wed":
                    ViewingRule.CoLamThuTu = !ViewingRule.CoLamThuTu;
                    break;
                case "thu":
                    ViewingRule.CoLamThuNam = !ViewingRule.CoLamThuNam;
                    break;
                case "fri":
                    ViewingRule.CoLamThuSau = !ViewingRule.CoLamThuSau;
                    break;
                case "sat":
                    ViewingRule.CoLamThuBay = !ViewingRule.CoLamThuBay;
                    break;
                case "sun":
                    ViewingRule.CoLamChuNhat = !ViewingRule.CoLamChuNhat;
                    break;
            }
        }));
    }
}
