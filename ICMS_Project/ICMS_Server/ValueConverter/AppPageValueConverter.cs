using System;
using System.Globalization;

namespace ICMS_Server
{
    public class AppPageValueConverter : BaseValueConverter<AppPageValueConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((ApplicationPage)value)
            {
                case ApplicationPage.Login:
                    return new LoginView();
                case ApplicationPage.Main:
                    return new MainView();
                case ApplicationPage.Control:
                    return new ControlView();
                case ApplicationPage.MemberCoupon:
                    return new MemberCouponView();
                case ApplicationPage.Income:
                    return new IncomeView();
                case ApplicationPage.LogHistory:
                    return new LogHistoryView();

                case ApplicationPage.Option:
                    return new OptionView();
                case ApplicationPage.Admin:
                    return new AdminView();
                case ApplicationPage.Staff:
                    return new StaffView();
                case ApplicationPage.Database:
                    return new DatabaseView();
                case ApplicationPage.UserGroup:
                    return new UserGroupView();
                case ApplicationPage.Promotion:
                    return new PromotionView();
                case ApplicationPage.DelMember:
                    return new DelMemberView();
                case ApplicationPage.DelCoupon:
                    return new DelCouponView();

                case ApplicationPage.OptionCoupon:
                    return new OptionCouponView();
                case ApplicationPage.Report:
                    return new ReportView();

                case ApplicationPage.Member:
                    return new MemberView();
                case ApplicationPage.Coupon:
                    return new CouponView();

                case ApplicationPage.Lend:
                return new LendView();
                case ApplicationPage.PayDebt:
                    return new PayDebtView();

                case ApplicationPage.MemberReport:
                    return new MemberReportView();
                case ApplicationPage.CouponReport:
                    return new CouponReportView();
                case ApplicationPage.TopUpReport:
                    return new TopUpReportView();
                case ApplicationPage.IncomeReport:
                    return new IncomeReportView();
                case ApplicationPage.OnlineHistoryReport:
                    return new OnlineHistoryReportView();
                //case ApplicationPage.Confirm:
                //return new ConfirmView();
                //case ApplicationPage.Warning:
                //return new WarningView();
                default:
                    return null;
            }
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
