using DAL.Others;
using DAL.Repos;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Rows
{
    public class Variables : Row
    {
        public DateTime ThoiDiemTao { get; set; }
        public bool CoLamThuHai { get; set; }
        public bool CoLamThuBa { get; set; }
        public bool CoLamThuTu { get; set; }
        public bool CoLamThuNam { get; set; }
        public bool CoLamThuSau { get; set; }
        public bool CoLamThuBay { get; set; }
        public bool CoLamChuNhat { get; set; }
        public TimeSpan ThoiGianChoPhepDiTre { get; set; }
        public TimeSpan ThoiGianChoPhepVeSom { get; set; }
        public TimeSpan ThoiGianDiTreToiDa { get; set; }
        public TimeSpan ThoiGianVeSomToiDa { get; set; }

        public override bool Save(UnitOfWork uow)
        {
            ThoiDiemTao = DateTime.Now;
            bool res = VariablesRepo.Instance.Add(this, uow);
            VariablesRepo.Instance.FindLatestVariables();
            return res;
        }
    }
}
