using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using ViewModel;
using System.Linq;
using Model;

namespace ViewModel
{
    public class AccountGroupsViewModel : Navigation.CRUDViewModelBase
    {
        public override string ViewName => "Account Groups";

        public ObservableCollection<AccountGroupModel> AccountGroups { get; set; }

        public AccountGroupModel CurrentAccountGroup
        {
            get => _currentAG;
            set
            {
                _currentAG = value;
                StartUpdateCommand.RaiseCanExecuteChangeEvent();
            }
        }
        public AccountGroupModel SelectedAccountGroup
        {
            get => _selectedAG;
            set
            {
                _selectedAG = value;
                CurrentAccountGroup = value;
            }
        }

        private AccountGroupModel _currentAG;
        private AccountGroupModel _selectedAG;

        public AccountGroupsViewModel()
        {
            AccountGroups = AccountGroupModel.LoadAll();
        }

        public override void ExecuteAdd(object param)
        {
            CurrentAccountGroup = new AccountGroupModel("", 0);
            base.ExecuteAdd(param);
        }

        public override void ExecuteUpdate(object param)
        {
            base.ExecuteUpdate(param);
            CurrentAccountGroup = new AccountGroupModel(SelectedAccountGroup);
        }

        public override void ExecuteConfirmAdd(object param)
        {
            var result = CurrentAccountGroup.Add();

            if (result == "")
            {
                base.ExecuteConfirmAdd(param);
                AccountGroups.Add(CurrentAccountGroup);
                SelectedAccountGroup = CurrentAccountGroup;
                StartUpdateCommand.RaiseCanExecuteChangeEvent();
            }
            else
            {
                ErrorString = result;
                HaveError = true;
            }
        }

        public override void ExecuteConfirmUpdate(object param)
        {
            var result = CurrentAccountGroup.Save();

            if (result == "")
            {
                base.ExecuteConfirmUpdate(param);
                var found = AccountGroups.FirstOrDefault(x => x.TenNhomTaiKhoan == SelectedAccountGroup.TenNhomTaiKhoan);
                AccountGroups[AccountGroups.IndexOf(found)] = CurrentAccountGroup;
                CurrentAccountGroup = SelectedAccountGroup;
                StartUpdateCommand.RaiseCanExecuteChangeEvent();
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
            CurrentAccountGroup = SelectedAccountGroup;
        }
        public override void ExecuteCancleUpdate(object param)
        {
            base.ExecuteCancleUpdate(param);
            CurrentAccountGroup = SelectedAccountGroup;
        }

        public override bool CanExecuteAddStart(object param)
        {
            return base.CanExecuteAddStart(param);
        }

        public override bool CanExecuteUpdateStart(object param)
        {
            return base.CanExecuteUpdateStart(param) && CurrentAccountGroup != null;
        }
    }
}
