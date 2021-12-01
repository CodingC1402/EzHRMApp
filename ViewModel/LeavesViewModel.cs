using Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using ViewModel.Helper;

namespace ViewModel
{
    public class LeavesViewModel : Navigation.CRUDViewModelBase
    {
        public override string ViewName => "Leaves";

        public ObservableCollection<LeaveModel> Collection { get; set; }

        private LeaveModel _selectedModel = null;
        public LeaveModel SelectedModel 
        {
            get => _selectedModel;
            set
            {
                _selectedModel = value;
                ViewingModel = value;
                StartUpdateCommand.RaiseCanExecuteChangeEvent();
                DeleteCommand.RaiseCanExecuteChangeEvent();
            }
        }
        public bool IsNotInAddMode { get => !IsInAddMode; }

        private bool _waitingForDeleteConfirmation = false;
        private RelayCommand<object> _deleteCommand;
        public RelayCommand<object> DeleteCommand => _deleteCommand ??= new RelayCommand<object>(param => {
            PopUpMessage.Instance.Message = "Are you sure you want to delete this?";
            _waitingForDeleteConfirmation = true;
            ShowConfirmation = true;
        }, param => {
            return SelectedModel != null && SelectedModel.CanDelete();
        });
        public LeaveModel ViewingModel { get; set; }

        public override void ExecuteUpdate(object param)
        {
            base.ExecuteUpdate(param);
            ViewingModel = new LeaveModel(_selectedModel);
        }

        protected override void PopUpMessageClosed(object sender, EventArgs e)
        {
            base.PopUpMessageClosed(sender, e);
            if (_waitingForDeleteConfirmation)
            {
                _waitingForDeleteConfirmation = false;
                if (MessageResult == PopUpMessage.Result.Ok)
                {
                    string result = SelectedModel.Delete();
                    if (result == "")
                    {
                        Collection.Remove(SelectedModel);
                        SelectedModel = null;
                        DeleteCommand.RaiseCanExecuteChangeEvent();
                    }
                    else
                    {
                        ErrorString = result;
                        HaveError = true;
                    }
                }
            }
        }

        public override void ExecuteAdd(object param)
        {
            base.ExecuteAdd(param);
            RaisePropertyChanged(nameof(IsNotInAddMode));
            ViewingModel = new LeaveModel
            {
                NgayBatDauNghi = DateTime.Today
            };
        }

        public override void ExecuteConfirmAdd(object param)
        {
            string result = ViewingModel.Add();
            if (result == "")
            {
                Collection.Add(ViewingModel);
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
            string result = ViewingModel.Save();
            if (result == "")
            {
                SelectedModel.NgayBatDauNghi = ViewingModel.NgayBatDauNghi;
                SelectedModel.IDNhanVien = ViewingModel.IDNhanVien;
                SelectedModel.LyDoNghi = ViewingModel.LyDoNghi;
                SelectedModel.SoNgayNghi = ViewingModel.SoNgayNghi;
                base.ExecuteConfirmUpdate(param);
            }
            else
            {
                ErrorString = result;
                HaveError = true;
            }
        }

        protected override void OnModeChangeBack()
        {
            base.OnModeChangeBack();
            ViewingModel = SelectedModel;
        }

        public override bool CanExecuteUpdateStart(object param)
        {
            return base.CanExecuteUpdateStart(param) && _selectedModel.CanDelete();
        }

        public override void OnGetTo()
        {
            base.OnGetTo();
            Collection = LeaveModel.GetAll();
            if (Collection.Count > 0)
            {
                 SelectedModel = Collection[0];
            }
        }
    }
}
