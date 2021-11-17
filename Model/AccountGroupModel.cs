using System;
using System.Collections.ObjectModel;
using System.Text;
using DAL.Rows;
using DAL.Repos;
using Model.Utils;
using DAL.Others;

namespace Model
{
    public class AccountGroupModel : AccountGroup
    {
        public bool DashboardViewPermission { get; set; }
        public bool CheckInViewPermission { get; set; }
        public bool ScheduleViewPermission { get; set; }
        public bool StaffViewPermission { get; set; }
        public bool DepartmentsViewPermission { get; set; }
        public bool PositionsViewPermission { get; set; }
        public bool AccountGroupsViewPermission { get; set; }
        public bool PayrollViewPermission { get; set; }
        public bool ReportsViewPermission { get; set; }

        public static ObservableCollection<AccountGroupModel> LoadAll()
        {
            var list = AccountGroupRepo.Instance.GetAll();
            var resultList = new ObservableCollection<AccountGroupModel>();

            foreach (var item in list)
            {
                resultList.Add(new AccountGroupModel(item));
            }
            return resultList;
        }

        public AccountGroupModel(AccountGroup ag) : base(ag)
        {
            SetAndConvertQuyenHan(QuyenHan);
        }

        public AccountGroupModel(string ten, int quyenHan) : base()
        {
            TenNhomTaiKhoan = ten;
            SetAndConvertQuyenHan(quyenHan);
        }

        public void SetAndConvertQuyenHan(int quyenHan)
        {
            QuyenHan = quyenHan;
            DashboardViewPermission = CheckFlag(QuyenHan, QuyenHanBitmask.Dashboard);
            CheckInViewPermission = CheckFlag(QuyenHan, QuyenHanBitmask.CheckIn);
            ScheduleViewPermission = CheckFlag(QuyenHan, QuyenHanBitmask.Schedule);
            StaffViewPermission = CheckFlag(QuyenHan, QuyenHanBitmask.Staff);
            DepartmentsViewPermission = CheckFlag(QuyenHan, QuyenHanBitmask.Departments);
            PositionsViewPermission = CheckFlag(QuyenHan, QuyenHanBitmask.Positions);
            AccountGroupsViewPermission = CheckFlag(QuyenHan, QuyenHanBitmask.AccountGroups);
            PayrollViewPermission = CheckFlag(QuyenHan, QuyenHanBitmask.Payroll);
            ReportsViewPermission = CheckFlag(QuyenHan, QuyenHanBitmask.Reports);
        }

        private bool CheckFlag(int quyenHan, int mask)
        {
            return (quyenHan & mask) != 0;
        }
    }
}
