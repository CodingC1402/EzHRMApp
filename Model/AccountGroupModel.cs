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
            ConvertQuyenHanToBoolsAndSet();
        }

        public AccountGroupModel(string ten, int quyenHan) : base()
        {
            TenNhomTaiKhoan = ten;
            QuyenHan = quyenHan;
            ConvertQuyenHanToBoolsAndSet();
        }

        public override string Add(UnitOfWork uow = null)
        {
            QuyenHan = ConvertQuyenHanToInt();
            AccountGroup ag = new AccountGroup(this);
            return ag.Add(uow);
        }

        public override string Save(UnitOfWork uow = null)
        {
            QuyenHan = ConvertQuyenHanToInt();
            AccountGroup ag = new AccountGroup(this);
            return ag.Save(uow);
        }

        public void ConvertQuyenHanToBoolsAndSet()
        {
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

        private int ConvertQuyenHanToInt()
        {
            int quyenHan = 0;
            if (DashboardViewPermission) quyenHan |= QuyenHanBitmask.Dashboard;
            if (CheckInViewPermission) quyenHan |= QuyenHanBitmask.CheckIn;
            if (ScheduleViewPermission) quyenHan |= QuyenHanBitmask.Schedule;
            if (StaffViewPermission) quyenHan |= QuyenHanBitmask.Staff;
            if (DepartmentsViewPermission) quyenHan |= QuyenHanBitmask.Departments;
            if (PositionsViewPermission) quyenHan |= QuyenHanBitmask.Positions;
            if (AccountGroupsViewPermission) quyenHan |= QuyenHanBitmask.AccountGroups;
            if (PayrollViewPermission) quyenHan |= QuyenHanBitmask.Payroll;
            if (ReportsViewPermission) quyenHan |= QuyenHanBitmask.Reports;

            return quyenHan;
        }

        private bool CheckFlag(int quyenHan, int mask)
        {
            return (quyenHan & mask) != 0;
        }
    }
}
