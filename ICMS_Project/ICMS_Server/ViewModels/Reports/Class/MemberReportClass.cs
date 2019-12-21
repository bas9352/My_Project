using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICMS_Server
{
    public class MemberReportClass
    {
        public int r_member_id { get; set; }
        public string r_member_username { get; set; }
        public string r_member_name { get; set; }
        public string r_member_lastname { get; set; }
        public string r_member_nickname { get; set; }
        public string r_member_c_date { get; set; }
        public double r_member_total_real_amount { get; set; }
        public double r_member_total_free_amount { get; set; }
        public double r_member_total_debt_amount { get; set; }
        public double r_member_total_pay_debt { get; set; }
        public double r_member_remaining_debt_amount { get; set; }
        public double r_member_remaining_amount { get; set; }
        public int r_member_create_by { get; set; }
        public string r_staff_username { get; set; }
        public int r_group_id { get; set; }
        public string r_group_name { get; set; }
        public int r_type_id { get; set; }
        public string r_type_name { get; set; }
    }
}
