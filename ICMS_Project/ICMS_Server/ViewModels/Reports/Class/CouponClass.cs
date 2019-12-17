using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICMS_Server
{
    public class CouponClass
    {
        public int r_coupon_id { get; set; }
        public string r_coupon_username { get; set; }
        public string r_coupon_password { get; set; }
        public string r_coupon_e_date { get; set; }
        public double r_coupon_real_amount { get; set; }
        public double r_coupon_free_amount { get; set; }
        public double r_coupon_total_amount { get; set; }
        public double r_coupon_hr { get; set; }
        public double r_coupon_mn { get; set; }
    }
}
