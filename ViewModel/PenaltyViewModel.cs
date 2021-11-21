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
            if (param == null)
                return;

            if ((bool)param)
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

        public PenaltyViewModel()
        {
            Instance = this;
            Penalties = PenaltyModel.LoadAll();
        }
    }
}
