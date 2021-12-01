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
        public bool UserInfoViewPermission { get; set; }
        public bool WeeklyScheduleViewPermission { get; set; }

        public bool DashboardViewPermission { get; set; }
        public bool StaffViewPermission { get; set; }
        public bool CheckInManagementViewPermission { get; set; }
        public bool LeavesViewPermission { get; set; }
        public bool PenaltyViewPermission { get; set; }
        public bool EmployeePayrollViewPermission { get; set; }
        public bool ReportsViewPermission { get; set; }

        public bool RolesViewPermission { get; set; }
        public bool DepartmentsViewPermission { get; set; }
        public bool PayrollTypesViewPermission { get; set; }
        public bool PenaltyTypesViewPermission { get; set; }
        public bool AccountGroupsViewPermission { get; set; }
        public bool ScheduleManagementViewPermission { get; set; }

        public static ObservableCollection<AccountGroupModel> LoadAll()
        {
            var list = AccountGroupRepo.Instance.GetAll();
            var resultList = new ObservableCollection<AccountGroupModel>();

            foreach (var item in list)
            {
                if (!item.DaXoa)
                    resultList.Add(new AccountGroupModel(item));
            }
            return resultList;
        }

        public AccountGroupModel(AccountGroup ag) : base(ag)
        {
            ConvertQuyenHanToBoolsAndSet();
        }

        public AccountGroupModel(string ten, uint quyenHan, bool daXoa) : base()
        {
            TenNhomTaiKhoan = ten;
            QuyenHan = quyenHan;
            DaXoa = daXoa;
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
            CheckInManagementViewPermission = CheckFlag(QuyenHan, QuyenHanBitmask.CheckInManagement);
            WeeklyScheduleViewPermission = CheckFlag(QuyenHan, QuyenHanBitmask.WeeklySchedule);
            ScheduleManagementViewPermission = CheckFlag(QuyenHan, QuyenHanBitmask.ScheduleManagement);
            StaffViewPermission = CheckFlag(QuyenHan, QuyenHanBitmask.Staff);
            DepartmentsViewPermission = CheckFlag(QuyenHan, QuyenHanBitmask.Departments);
            RolesViewPermission = CheckFlag(QuyenHan, QuyenHanBitmask.Roles);
            AccountGroupsViewPermission = CheckFlag(QuyenHan, QuyenHanBitmask.AccountGroups);
            EmployeePayrollViewPermission = CheckFlag(QuyenHan, QuyenHanBitmask.EmployeePayroll);
            UserInfoViewPermission = CheckFlag(QuyenHan, QuyenHanBitmask.UserInfo);
            PayrollTypesViewPermission = CheckFlag(QuyenHan, QuyenHanBitmask.PayrollTypes);
            PenaltyTypesViewPermission = CheckFlag(QuyenHan, QuyenHanBitmask.PenaltyTypes);
            PenaltyViewPermission = CheckFlag(QuyenHan, QuyenHanBitmask.Penalty);
            LeavesViewPermission = CheckFlag(QuyenHan, QuyenHanBitmask.Leaves);
            ReportsViewPermission = CheckFlag(QuyenHan, QuyenHanBitmask.Reports);
        }

        private uint ConvertQuyenHanToInt()
        {
            uint quyenHan = 0;
            if (DashboardViewPermission) quyenHan |= QuyenHanBitmask.Dashboard;
            if (CheckInManagementViewPermission) quyenHan |= QuyenHanBitmask.CheckInManagement;
            if (WeeklyScheduleViewPermission) quyenHan |= QuyenHanBitmask.WeeklySchedule;
            if (StaffViewPermission) quyenHan |= QuyenHanBitmask.Staff;
            if (DepartmentsViewPermission) quyenHan |= QuyenHanBitmask.Departments;
            if (RolesViewPermission) quyenHan |= QuyenHanBitmask.Roles;
            if (AccountGroupsViewPermission) quyenHan |= QuyenHanBitmask.AccountGroups;
            if (EmployeePayrollViewPermission) quyenHan |= QuyenHanBitmask.EmployeePayroll;
            if (PayrollTypesViewPermission) quyenHan |= QuyenHanBitmask.PayrollTypes;
            if (UserInfoViewPermission) quyenHan |= QuyenHanBitmask.UserInfo;
            if (LeavesViewPermission) quyenHan |= QuyenHanBitmask.Leaves;
            if (PenaltyViewPermission) quyenHan |= QuyenHanBitmask.Penalty;
            if (PenaltyTypesViewPermission) quyenHan |= QuyenHanBitmask.PenaltyTypes;
            if (ScheduleManagementViewPermission) quyenHan |= QuyenHanBitmask.ScheduleManagement;
            if (ReportsViewPermission) quyenHan |= QuyenHanBitmask.Reports;

            return quyenHan;
        }

        private bool CheckFlag(uint quyenHan, uint mask)
        {
            return (quyenHan & mask) != 0;
        }
    }
}
