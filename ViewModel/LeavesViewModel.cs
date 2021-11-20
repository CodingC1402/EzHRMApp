using Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace ViewModel
{
    public class LeavesViewModel : Navigation.CRUDViewModelBase
    {
        public override string ViewName => "Leaves";

        public ObservableCollection<LeaveModel> Collection { get; set; }
        public LeaveModel SelectedModel { get; set; }
        public LeaveModel ViewingModel { get; set; }

        public override void OnGetTo()
        {
            base.OnGetTo();
            Collection = LeaveModel.GetAll();
            if (Collection.Count > 0)
            {
                ViewingModel = SelectedModel = Collection[0];
            }
        }
    }
}
