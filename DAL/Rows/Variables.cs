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

        public Variables() { }
        public Variables(Variables variable)
        {
            ThoiDiemTao = variable.ThoiDiemTao;
            CoLamThuHai = variable.CoLamThuHai;
            CoLamThuBa = variable.CoLamThuBa;
            CoLamThuTu = variable.CoLamThuTu;
            CoLamThuNam = variable.CoLamThuNam;
            CoLamThuSau = variable.CoLamThuSau;
            CoLamThuBay = variable.CoLamThuBay;
            CoLamChuNhat = variable.CoLamChuNhat;
            ThoiGianChoPhepDiTre = variable.ThoiGianChoPhepDiTre;
            ThoiGianChoPhepVeSom = variable.ThoiGianChoPhepVeSom;
            ThoiGianDiTreToiDa = variable.ThoiGianDiTreToiDa;
            ThoiGianVeSomToiDa = variable.ThoiGianVeSomToiDa;
        }

        public override string Save(UnitOfWork uow)
        {
            var result = this.CheckForError();

            if (result != "")
                return result;

            if (uow == null)
            {
                using (var uowNew = new UnitOfWork())
                {
                    VariablesRepo.Instance.Add(this, uowNew);
                    return ExecuteAndReturn(uowNew);
                }
            }

            if (VariablesRepo.Instance.Add(this, uow))
                return "";
            else
                return "Failed!";
        }

        public override string Add(UnitOfWork uow = null)
        {
            return Save(uow);
        }
    }
}
