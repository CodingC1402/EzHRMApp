using DAL.Repos;
using DAL.Rows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Model
{
    public class PenaltyTypeModel : PenaltyType
    {
        public PenaltyTypeModel() { }
        public PenaltyTypeModel(PenaltyType penaltyType) : base(penaltyType) { }

        public static ObservableCollection<PenaltyTypeModel> LoadAll()
        {
            ObservableCollection<PenaltyTypeModel> penaltyTypeModels = new ObservableCollection<PenaltyTypeModel>();
            var types = PenaltyTypeRepo.Instance.GetAll();
            foreach (var type in types)
            {
                penaltyTypeModels.Add(new PenaltyTypeModel(type));
            }
            return penaltyTypeModels;
        }
    }
}
