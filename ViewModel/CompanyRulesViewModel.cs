using Model;
using System;
using System.Collections.Generic;
using System.Text;
using ViewModel.Helper;

namespace ViewModel
{
    public class CompanyRulesViewModel : Navigation.CRUDViewModelBase
    {
        public override string ViewName => "Company Rules";

        private static CompanyRulesViewModel _instance = null;
        public static CompanyRulesViewModel Instance { get => _instance; }

        // The actual rule that is referencing
        public CompanyRuleModel CurrentRule { get; set; }
        // The rule that go rendered
        public CompanyRuleModel ViewingRule { get; set; }

        private bool _isWaitingForConfirmation = false;

        public override void ExecuteConfirmUpdate(object param)
        {
            _isWaitingForConfirmation = true;
            ViewingRule = new CompanyRuleModel(CurrentRule);

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
                    var result = CompanyRuleModel.UpdateCompanyRule(ViewingRule);
                    if (result == "")
                    {
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
        }

        public CompanyRulesViewModel()
        {
            _instance = this;
        }

        public override void OnGetTo()
        {
            ViewingRule = CurrentRule = CompanyRuleModel.GetLastestRule();
        }
    }
}