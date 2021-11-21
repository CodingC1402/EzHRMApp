using Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace ViewModel
{
    public class PenaltyTypeViewModel : Navigation.CRUDViewModelBase
    {
        public override string ViewName => "Penalty Type";

        public static PenaltyTypeViewModel Instance { get; private set; }
        public ObservableCollection<PenaltyTypeModel> PenaltyTypes { get; set; }
        private PenaltyTypeModel _selectedPenaltyType = null;
        private PenaltyTypeModel _currentPenaltyType = null;

        public PenaltyTypeModel SelectedPenaltyType
        {
            get => _selectedPenaltyType;
            set
            {
                _selectedPenaltyType = value;
                CurrentPenaltyType = value;
            }
        }

        public PenaltyTypeModel CurrentPenaltyType
        {
            get => _currentPenaltyType;
            set
            {
                _currentPenaltyType = value;
                StartUpdateCommand.RaiseCanExecuteChangeEvent();
            }
        }

        #region Function
        public override void ExecuteAdd(object param)
        {
            base.ExecuteAdd(param);
            CurrentPenaltyType = new PenaltyTypeModel();
        }
        public override void ExecuteUpdate(object param)
        {
            base.ExecuteUpdate(param);
            CurrentPenaltyType = new PenaltyTypeModel(SelectedPenaltyType);
        }

        public override void ExecuteConfirmAdd(object param)
        {
            var result = CurrentPenaltyType.Add();

            if (result == "")
            {
                base.ExecuteConfirmAdd(param);
                PenaltyTypes.Add(CurrentPenaltyType);
                SelectedPenaltyType = CurrentPenaltyType;
                SetCurrentModelBack();
            }
            else
            {
                ErrorString = result;
                HaveError = true;
            }
        }

        public override void ExecuteConfirmUpdate(object param)
        {
            var result = CurrentPenaltyType.Save();

            if (result == "")
            {
                base.ExecuteConfirmUpdate(param);
                var found = PenaltyTypes.FirstOrDefault(x => x.TenViPham == SelectedPenaltyType.TenViPham);
                PenaltyTypes[PenaltyTypes.IndexOf(found)] = CurrentPenaltyType;
                SetCurrentModelBack();
            }
            else
            {
                ErrorString = result;
                HaveError = true;
            }
        }

        public override void ExecuteCancleAdd(object param)
        {
            base.ExecuteCancleAdd(param);
            SetCurrentModelBack();
        }
        public override void ExecuteCancleUpdate(object param)
        {
            base.ExecuteCancleUpdate(param);
            SetCurrentModelBack();
        }

        public override bool CanExecuteAddStart(object param)
        {
            return base.CanExecuteAddStart(param);
        }

        public override bool CanExecuteUpdateStart(object param)
        {
            return base.CanExecuteUpdateStart(param) && CurrentPenaltyType != null;
        }
        #endregion

        public PenaltyTypeViewModel()
        {
            Instance = this;
            PenaltyTypes = PenaltyTypeModel.LoadAll();
        }

        private void SetCurrentModelBack()
        {
            CurrentPenaltyType = SelectedPenaltyType;
            StartUpdateCommand.RaiseCanExecuteChangeEvent();
        }
    }
}
