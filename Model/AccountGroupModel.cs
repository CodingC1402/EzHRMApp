using System;
using System.Collections.ObjectModel;
using System.Text;
using DAL.Rows;
using DAL.Repos;

namespace Model
{
    public class AccountGroupModel : AccountGroup
    {
        public ObservableCollection<AccountGroupModel> LoadAll()
        {
            var list = AccountGroupRepo.Instance.GetAll();
            var resultList = new ObservableCollection<AccountGroupModel>();

            foreach (var item in list)
            {
                resultList.Add(new AccountGroupModel(item));
            }
            return resultList;
        }

        public AccountGroupModel(AccountGroup ag) : base(ag) { }
    }
}
