using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICMS_Server
{
    public class CouponReportClass
    {
        public int r_coupon_id { get; set; }
        public string r_coupon_username { get; set; }
        public string r_coupon_c_date { get; set; }
        public double r_coupon_total_real_amount { get; set; }
        public double r_coupon_total_free_amount { get; set; }
        public double r_coupon_remaining_amount { get; set; }
        public int r_coupon_create_by { get; set; }
        public string r_staff_username { get; set; }
        public int r_group_id { get; set; }
        public string r_group_name { get; set; }
        public int r_type_id { get; set; }
        public string r_type_name { get; set; }
    }
}
