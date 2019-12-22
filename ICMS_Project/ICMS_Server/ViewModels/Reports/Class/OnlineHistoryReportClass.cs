using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICMS_Server
{
    public class OnlineHistoryReportClass
    {
        public string r_online_id { get; set; }
        public string r_member_id { get; set; }
        public string r_coupon_id { get; set; }
        public string r_all_username { get; set; }
        public string r_online_pc_id { get; set; }
        public string r_online_pc_name { get; set; }
        public string r_online_status { get; set; }
        public string r_online_ordinal { get; set; }
        public string r_online_s_date { get; set; }
        public string r_online_s_time { get; set; }
        public string r_online_e_date { get; set; }
        public string r_online_e_time { get; set; }
        public string r_online_total_use_amount { get; set; }
        public string r_all_online_hr { get; set; }
        public string r_all_online_mn { get; set; }
    }
}
