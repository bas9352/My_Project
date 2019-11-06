using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ICMS_Server
{
    /// <summary>
    /// Interaction logic for LoginView.xaml
    /// </summary>
    public partial class LoginView : UserControl
    {
        public LoginView()
        {
            InitializeComponent();
            DataContext = IoC.LoginView;
        }

        public class NameValidator : ValidationRule
        {
            public override ValidationResult Validate
              (object value, System.Globalization.CultureInfo cultureInfo)
            {
                if (value == null)
                    return new ValidationResult(false, "value cannot be empty.");
                else
                {
                    if (value.ToString().Length > 3)
                        return new ValidationResult
                        (false, "Name cannot be more than 3 characters long.");
                }
                return ValidationResult.ValidResult;
            }
        }
    }
}
