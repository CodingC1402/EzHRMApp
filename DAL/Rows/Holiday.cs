using DAL.Others;
using DAL.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL.Rows
{
    public class Holiday : Row
    {
        public int ID { get; set; }
        public int Ngay { get; set; }
        public int Thang { get; set; }
        public int SoNgayNghi { get; set; }
        public string TenDipNghiLe { get; set; }

        public Holiday() { }
        public Holiday(Holiday h)
        {
            ID = h.ID;
            Ngay = h.Ngay;
            Thang = h.Thang;
            SoNgayNghi = h.SoNgayNghi;
            TenDipNghiLe = h.TenDipNghiLe;
        }

        public override string Add(UnitOfWork uow = null)
        {
            var result = this.CheckForError();

            if (result != "")
                return result;

            if (uow == null)
            {
                using (var uowNew = new UnitOfWork())
                {
                    HolidayRepo.Instance.Add(this, uowNew);
                    return ExecuteAndReturn(uowNew);
                }
            }

            if (HolidayRepo.Instance.Add(this, uow))
                return "";
            else
                return "Failed!";
        }

        public override string Save(UnitOfWork uow = null)
        {
            var result = this.CheckForError();

            if (result != "")
                return result;

            if (uow == null)
            {
                using (var uowNew = new UnitOfWork())
                {
                    HolidayRepo.Instance.Update(new object[] { ID }, this, uowNew);
                    return ExecuteAndReturn(uowNew);
                }
            }

            if (HolidayRepo.Instance.Update(new object[] { ID }, this, uow))
                return "";
            else
                return "Failed!";
        }

        public string Delete(UnitOfWork uow = null)
        {
            if (uow == null)
            {
                using (var uowNew = new UnitOfWork())
                {
                    HolidayRepo.Instance.Remove(new object[] { ID }, uowNew);
                    return ExecuteAndReturn(uowNew);
                }
            }

            if (HolidayRepo.Instance.Remove(new object[] { ID }, uow))
                return "";
            else
                return "Failed!";
        }

        public override string CheckForError()
        {
            try
            {
                if (!IsValidHolidayName(TenDipNghiLe))
                    return "Holiday's name can't be empty!";

                if (!IsNameTaken(TenDipNghiLe, ID))
                    return "Holiday's name is already taken!";

                if (!IsDateTimeValid(Ngay, Thang))
                    return "Day or month is invalid!";

                if (SoNgayNghi <= 0 || SoNgayNghi > 365)
                {
                    return "Number of holidays is invalid!";
                }

                return "";
            }
            catch (Exception e)
            {
                return $"Unknow error: {e.Message}";
            }
        }

        public bool IsDateTimeValid(int ngay, int thang)
        {
            DateTime tempDate;
            string date = ngay.ToString() + "/" + thang.ToString() + "/" + DateTime.Now.Year;
            return DateTime.TryParse(date, out tempDate);
        }

        public bool IsValidHolidayName(string ten)
        {
            if (ten == null || ten == "")
                return false;

            return true;
        }

        public bool IsNameTaken(string ten, int ID)
        {
            try
            {
                var temp = HolidayRepo.Instance.FindBy("TenDipNghiLe", ten).FirstOrDefault();
                if (temp != null && temp.ID != ID)
                    return false;
                else
                    return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
