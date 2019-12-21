using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICMS_Server
{
    public class TopUpReportClass
    {
        public string r_mt_member_id { get; set; }
        public string r_ct_coupon_id { get; set; }
        public string r_all_username { get; set; }
        public string r_all_type_name { get; set; }
        public string r_top_up_by { get; set; }
        public string r_staff_username { get; set; }
        public string r_staff_type_name { get; set; }
        public string r_top_up_real_amount { get; set; }
        public string r_top_up_free_amount { get; set; }
        public string r_mt_debt_amount { get; set; }
        public string r_mt_pay_debt { get; set; }
        public string r_mt_bonus_id { get; set; }
        public string r_bonus_point { get; set; }
        public string r_mt_promotion_id { get; set; }
        public string r_promo_rate { get; set; }
        public DateTime r_top_up_date { get; set; }
    }
}
