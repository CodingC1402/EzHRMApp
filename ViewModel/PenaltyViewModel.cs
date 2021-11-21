using Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using ViewModel.Helper;

namespace ViewModel
{
    public class PenaltyViewModel : Navigation.CRUDViewModelBase
    {
        public override string ViewName => "Penalty";

        private bool _isWaitingForDeleteConfirmation = false;
        public static PenaltyViewModel Instance { get; private set; }
        public ObservableCollection<PenaltyModel> Penalties { get; set; }
        private PenaltyModel _selectedPenalty = null;
        public PenaltyModel SelectedPenalty
        {
            get => _selectedPenalty;
            set
            {
                _selectedPenalty = value;
                DeleteCommand.RaiseCanExecuteChangeEvent();
            }
        }

        private RelayCommand<object> _deleteCommand;
        public RelayCommand<object> DeleteCommand => _deleteCommand ?? (_deleteCommand = new RelayCommand<object>(Delete, CanDelete));

        private bool CanDelete(object param)
        {
            return SelectedPenalty != null;
        }

        private void Delete(object param)
        {
            _isWaitingForDeleteConfirmation = true;
            PopUpMessage.Instance.Message = "Are you sure you wan't to delete this penalty";
            ShowConfirmation = true;
        }

        protected override void PopUpMessageClosed(object sender, EventArgs e)
        {
            base.PopUpMessageClosed(sender, e);

            if (_isWaitingForDeleteConfirmation)
            {
                _isWaitingForDeleteConfirmation = false;
                if (MessageResult == PopUpMessage.Result.Ok)
                {
                    string result = SelectedPenalty.Delete();
                    if (result == "")
                    {
                        Penalties.Remove(SelectedPenalty);
                    }
                    else
                    {
                        ErrorString = result;
                        HaveError = true;
                    }
                }
            }
        }

        public PenaltyViewModel()
        {
            Instance = this;
            Penalties = PenaltyModel.LoadAll();
        }
    }
}
