using DAL.Others;
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

        public string Delete()
        {
            if (IsSpecialType)
                return "You can't delete this template because it's a template used by the system!";

            var penalties = new List<Penalty>(PenaltyRepo.Instance.FindBy(nameof(Penalty.TenViPham), TenViPham));
            if (penalties.Count > 0)
            {
                return "There is at least one penalty that is using this template!";
            }

            using (UnitOfWork uow = new UnitOfWork())
            {
                PenaltyTypeRepo.Instance.Remove(new object[] { TenViPham }, uow);
                return uow.Complete() ? "" : "Unknow error!";
            }
           
        }
    }
}
