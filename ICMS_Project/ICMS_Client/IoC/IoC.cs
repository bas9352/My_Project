using Ninject;


namespace ICMS_Client
{
    public static class IoC
    {

        public static IKernel Kernel { get; private set; } = new StandardKernel();

        public static LoginViewModel LoginView => IoC.Get<LoginViewModel>();
        public static MainViewModel MainView => IoC.Get<MainViewModel>();
        public static AppViewModel Application => IoC.Get<AppViewModel>();
        public static AccountViewModel AccountView => IoC.Get<AccountViewModel>();
        public static PromotionViewModel PromotionView => IoC.Get<PromotionViewModel>();
        public static ConfirmViewModel ConfirmView => IoC.Get<ConfirmViewModel>();
        public static WarningViewModel WarningView => IoC.Get<WarningViewModel>();

        public static void Setup()
        {
            // Bind all required view models
            BindViewModels();
        }

        /// <summary>
        /// Binds all singleton view models
        /// </summary>
        private static void BindViewModels()
        {
            // Bind to a single instance of Application view model
            Kernel.Bind<LoginViewModel>().ToConstant(new LoginViewModel());

            Kernel.Bind<MainViewModel>().ToConstant(new MainViewModel());

            Kernel.Bind<AppViewModel>().ToConstant(new AppViewModel());
            Kernel.Bind<AccountViewModel>().ToConstant(new AccountViewModel());

            Kernel.Bind<ConfirmViewModel>().ToConstant(new ConfirmViewModel());
            Kernel.Bind<WarningViewModel>().ToConstant(new WarningViewModel());
        }

        public static T Get<T>()
        {
            return Kernel.Get<T>();
        }
    }
}
