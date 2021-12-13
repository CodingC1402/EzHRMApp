using Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using ViewModel.Helper;

namespace ViewModel
{
    public class PenaltyTypeViewModel : Navigation.CRUDViewModelBase
    {
        public override string ViewName => "Penalty tempaltes";

        public ObservableCollection<PenaltyTypeModel> Collections { get; set; }

        private PenaltyTypeModel _selectedPenalty;
        public PenaltyTypeModel SelectedPenalty 
        {
            get => _selectedPenalty;
            set
            {
                _selectedPenalty = value;
                StartUpdateCommand.RaiseCanExecuteChangeEvent();
                DeleteCommand.RaiseCanExecuteChangeEvent();
                CurrentPenalty = value;
            }
        }
        public PenaltyTypeModel CurrentPenalty { get; set; }
        public bool NameIsReadOnly { get; set; } = true;

        private bool _waitingForDeleteConfirmation;

        protected override void PopUpMessageClosed(object sender, EventArgs e)
        {
            base.PopUpMessageClosed(sender, e);
            if (_waitingForDeleteConfirmation)
            {
                _waitingForDeleteConfirmation = false;
                if (MessageResult == PopUpMessage.Result.Ok)
                {
                    string result = SelectedPenalty.Delete();
                    if (result == "")
                    {
                        Collections.Remove(SelectedPenalty);
                        SelectedPenalty = null;
                    }
                    else
                    {
                        ErrorString = result;
                        HaveError = true;
                    }
                }
            }
        }

        #region Commands and Functions
        private RelayCommand<object> _deleteCommand;
        public RelayCommand<object> DeleteCommand => _deleteCommand ??= new RelayCommand<object>(param => {
            PopUpMessage.Instance.Message = "Are you sure you want to delete this?";
            _waitingForDeleteConfirmation = true;
            ShowConfirmation = true;
        }, param => {
            return SelectedPenalty != null && !SelectedPenalty.IsSpecialType;
        });

        public override void ExecuteAdd(object param)
        {
            base.ExecuteAdd(param);
            NameIsReadOnly = false;
            CurrentPenalty = new PenaltyTypeModel();
        }

        public override void ExecuteUpdate(object param)
        {
            base.ExecuteUpdate(param);
            CurrentPenalty = new PenaltyTypeModel(_selectedPenalty);
        }

        public override void ExecuteConfirmAdd(object param)
        {
            string result = CurrentPenalty.Add();
            if (result == "")
            {
                Collections.Add(CurrentPenalty);
                base.ExecuteConfirmAdd(param);
            }
            else
            {
                ErrorString = result;
                HaveError = true;
            }
        }

        public override void ExecuteConfirmUpdate(object param)
        {
            string result = CurrentPenalty.Save();
            if (result == "")
            {
                SelectedPenalty.TenViPham = CurrentPenalty.TenViPham;
                SelectedPenalty.TruLuongTheoPhanTram = CurrentPenalty.TruLuongTheoPhanTram;
                SelectedPenalty.TruLuongTrucTiep = CurrentPenalty.TruLuongTrucTiep;

                base.ExecuteConfirmUpdate(param);
            }
            else
            {
                ErrorString = result;
                HaveError = true;
            }
        }

        public override bool CanExecuteUpdateStart(object param)
        {
            return base.CanExecuteUpdateStart(param) && _selectedPenalty != null;
        }
        #endregion

        protected override void OnModeChangeBack()
        {
            base.OnModeChangeBack();
            CurrentPenalty = SelectedPenalty;
            NameIsReadOnly = true;
        }

        public override void OnGetTo()
        {
            base.OnGetTo();
            Collections = PenaltyTypeModel.LoadAll();
            if (Collections != null && Collections.Count > 0)
            {
                SelectedPenalty = Collections[0];
            }
        }
    }
}
