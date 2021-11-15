using DAL.Repos;
using DAL.Rows;
using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class EmployeeReportModel
    {
        public int NumberOfPenalty { get; set; } = 0;
        public int BeingLate { get; set; } = 0;
        public int BeingAbsence { get; set; } = 0;
        public int BeingAbsenceWithLeave { get; set; } = 0;

        public static EmployeeReportModel Compile(EmployeeModel employee, DateTime start, DateTime end)
        {
            EmployeeReportModel report = new EmployeeReportModel();
            var deductions = new List<Penalty>(PenaltyRepo.Instance.GetEmployeeDeductions(employee, start, end));
            report.NumberOfPenalty = deductions.Count;
            foreach (Penalty deduction in deductions)
            {
                if (deduction.TenViPham == Penalty.Absence)
                    report.BeingAbsence++;
                else if (deduction.TenViPham == Penalty.BeingLate)
                    report.BeingLate++;
            }

            report.BeingAbsenceWithLeave = LeaveRepo.GetLeavesDays(employee, start, end);
            return report;
        }
    }
}
