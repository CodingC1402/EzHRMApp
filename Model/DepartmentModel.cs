using DAL.Others;
using DAL.Rows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Model
{
    public class DepartmentModel : Department
    {
        public enum SaveResult
        {
            Ok = 0,
            FailData = 1,
        }

        public static ObservableCollection<DepartmentModel> LoadAll()
        {
            var rows = DAL.Repos.DepartmentRepo.Instance.GetAll();
            var result = new ObservableCollection<DepartmentModel>();

            foreach (var row in rows)
            {
                result.Add(new DepartmentModel(row));
            }
            return result;
        }

        public DepartmentModel() { }
        public DepartmentModel(Department department) : base(department) { }
        public DepartmentModel(DepartmentModel department) : base(department) { }

        public SaveResult Save()
        {
            var result = SaveResult.Ok;

            UnitOfWork unit = new UnitOfWork();

            if (!Save(unit))
                result |= SaveResult.FailData;

            unit.Complete();
            return result;
        }

        // try something
        public SaveResult Add()
        {
            var result = SaveResult.Ok;

            UnitOfWork unit = new UnitOfWork();

            if (!Add(unit))
                result |= SaveResult.FailData;

            unit.Complete();
            return result;
        }

        protected new bool Save(UnitOfWork unitOfWork = null)
        {
            return base.Save(unitOfWork);
        }
    }
}
