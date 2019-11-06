using System;
using System.Globalization;

namespace ICMS_Client
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
                case ApplicationPage.Account:
                    return new AccountView();
                case ApplicationPage.Promotion:
                    return new PromotionView();


                //case ApplicationPage.Working:
                //    return new WorkingView();

                //case ApplicationPage.Agreement:
                //    return new AgreementView();

                //case ApplicationPage.EasyMode:
                //    return new EasyModeView();

                //case ApplicationPage.Help:
                //    return new HelpView();


                //case ApplicationPage.Setting:
                //    return new SettingView();

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
