using Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using ViewModel.Helper;

namespace ViewModel
{
    public class HolidayViewModel : Navigation.CRUDViewModelBase
    {
        public override string ViewName => "Holiday";
        private static HolidayViewModel _instance = null;
        public static HolidayViewModel Instance { get => _instance; }

        public ObservableCollection<HolidayModel> Holidays { get; set; }

        private HolidayModel _selectedHoliday = null;
        private HolidayModel _currentHoliday = null;

        public HolidayModel CurrentHoliday
        {
            get => _currentHoliday;
            set
            {
                _currentHoliday = value;
                StartUpdateCommand.RaiseCanExecuteChangeEvent();
            }
        }

        public HolidayModel SelectedHoliday
        {
            get => _selectedHoliday;
            set
            {
                _selectedHoliday = value;
                CurrentHoliday = value;
                DeleteCommand.RaiseCanExecuteChangeEvent();
            }
        }

        private bool _waitingForDeleteConfirmation;

        protected override void PopUpMessageClosed(object sender, EventArgs e)
        {
            base.PopUpMessageClosed(sender, e);
            if (_waitingForDeleteConfirmation)
            {
                _waitingForDeleteConfirmation = false;
                if (MessageResult == PopUpMessage.Result.Ok)
                {
                    string result = SelectedHoliday.Delete();
                    if (result == "")
                    {
                        Holidays.Remove(SelectedHoliday);
                        SelectedHoliday = null;
                        IsInUpdateMode = false;
                    }
                    else
                    {
                        ErrorString = result;
                        HaveError = true;
                    }
                }
            }
        }

        private RelayCommand<object> _deleteCommand;
        public RelayCommand<object> DeleteCommand => _deleteCommand ??= new RelayCommand<object>(param => {
            PopUpMessage.Instance.Message = "Are you sure you want to delete this?";
            _waitingForDeleteConfirmation = true;
            ShowConfirmation = true;
        }, param => {
            return SelectedHoliday != null;
        });

        #region Function
        public override void ExecuteAdd(object param)
        {
            var newDepartment = new HolidayModel();
            newDepartment.ID = HolidayModel.GetNextHolidayID();
            newDepartment.SoNgayNghi = 1;
            newDepartment.Ngay = 1;
            newDepartment.Thang = 1;
            CurrentHoliday = newDepartment;

            base.ExecuteAdd(param);
        }
        public override void ExecuteUpdate(object param)
        {
            base.ExecuteUpdate(param);
            CurrentHoliday = new HolidayModel(SelectedHoliday);
        }

        public override void ExecuteConfirmAdd(object param)
        {
            var result = CurrentHoliday.Add();

            if (result == "")
            {
                base.ExecuteConfirmAdd(param);
                Holidays.Add(CurrentHoliday);
                SelectedHoliday = CurrentHoliday;
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
            var result = CurrentHoliday.Save();

            if (result == "")
            {
                base.ExecuteConfirmUpdate(param);
                var found = Holidays.FirstOrDefault(x => x.ID == SelectedHoliday.ID);
                Holidays[Holidays.IndexOf(found)] = CurrentHoliday;
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
            return base.CanExecuteUpdateStart(param) && CurrentHoliday != null;
        }
        #endregion

        public HolidayViewModel()
        {
            _instance = this;
            Holidays = HolidayModel.LoadAll();
        }

        private void SetCurrentModelBack()
        {
            CurrentHoliday = SelectedHoliday;
            StartUpdateCommand.RaiseCanExecuteChangeEvent();
        }
    }
}
