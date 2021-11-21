using DAL.Repos;
using DAL.Rows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Model
{
    public class PenaltyModel : Penalty
    {
        public PenaltyModel() { }
        public PenaltyModel(Penalty penalty) : base(penalty) { }

        public static ObservableCollection<PenaltyModel> LoadAll()
        {
            ObservableCollection<PenaltyModel> penaltyModels = new ObservableCollection<PenaltyModel>();
            var penalties = PenaltyRepo.Instance.GetAll();
            foreach (var penalty in penalties)
            {
                penaltyModels.Add(new PenaltyModel(penalty));
            }
            return penaltyModels;
        }
    }
}
