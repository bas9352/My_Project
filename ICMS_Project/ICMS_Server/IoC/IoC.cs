using Ninject;


namespace ICMS_Server
{
    public static class IoC
    {
        public static IKernel Kernel { get; private set; } = new StandardKernel();

        public static LoginViewModel LoginView => IoC.Get<LoginViewModel>();
        public static MainViewModel MainView => IoC.Get<MainViewModel>();
        public static AppViewModel Application => IoC.Get<AppViewModel>();
        public static ControlViewModel ControlView => IoC.Get<ControlViewModel>();
        public static MemberCouponViewModel MemberCouponView => IoC.Get<MemberCouponViewModel>();
        public static IncomeViewModel IncomeView => IoC.Get<IncomeViewModel>();
        public static LogHistoryViewModel LogHistoryView => IoC.Get<LogHistoryViewModel>();

        public static OptionViewModel OptionView => IoC.Get<OptionViewModel>();
        public static AdminViewModel AdminView => IoC.Get<AdminViewModel>();
        public static StaffViewModel StaffView => IoC.Get<StaffViewModel>();
        public static AddEditStaffViewModel AddEditStaffView => IoC.Get<AddEditStaffViewModel>();
        public static AddEditGroupViewModel AddEditGroupView => IoC.Get<AddEditGroupViewModel>();
        public static AddEditBonusViewModel AddEditBonusView => IoC.Get<AddEditBonusViewModel>();
        public static AddEditPromotionViewModel AddEditPromotionView => IoC.Get<AddEditPromotionViewModel>();
        public static AddEditMemberViewModel AddEditMemberView => IoC.Get<AddEditMemberViewModel>();
        public static AddEditCouponViewModel AddEditCouponView => IoC.Get<AddEditCouponViewModel>();
        public static DatabaseViewModel DatabaseView => IoC.Get<DatabaseViewModel>();
        public static UserGroupViewModel UserGroupView => IoC.Get<UserGroupViewModel>();
        public static PromotionViewModel PromotionView => IoC.Get<PromotionViewModel>();
        public static DelMemberViewModel DelMemberView => IoC.Get<DelMemberViewModel>();
        public static DelCouponViewModel DelCouponView => IoC.Get<DelCouponViewModel>();

        public static OptionCouponViewModel OptionCouponView => IoC.Get<OptionCouponViewModel>();
        public static ReportViewModel ReportView => IoC.Get<ReportViewModel>();
        public static ConfirmViewModel ConfirmView => IoC.Get<ConfirmViewModel>();
        public static WarningViewModel WarningView => IoC.Get<WarningViewModel>();

        public static MemberViewModel MemberView => IoC.Get<MemberViewModel>();
        public static CouponViewModel CouponView => IoC.Get<CouponViewModel>();

        public static TopUpViewModel TopUpView => IoC.Get<TopUpViewModel>();
        public static FreeTopUpViewModel FreeTopUpView => IoC.Get<FreeTopUpViewModel>();
        public static LendViewModel LendView => IoC.Get<LendViewModel>();
        public static DebtViewModel DebtView => IoC.Get<DebtViewModel>();
        public static PayDebtViewModel PayDebtView => IoC.Get<PayDebtViewModel>();
        public static GenerateCouponViewModel GenerateCouponView => IoC.Get<GenerateCouponViewModel>();
        public static AddEditOptionCouponViewModel AddEditOptionCouponView => IoC.Get<AddEditOptionCouponViewModel>();
        public static ProgressBarViewModel ProgressBarView => IoC.Get<ProgressBarViewModel>();

        public static MemberReportViewModel MemberReportView => IoC.Get<MemberReportViewModel>();
        public static CouponReportViewModel CouponReportView => IoC.Get<CouponReportViewModel>();
        public static TopUpReportViewModel TopUpReportView => IoC.Get<TopUpReportViewModel>();
        public static IncomeReportViewModel IncomeReportView => IoC.Get<IncomeReportViewModel>();
        public static OnlineHistoryReportViewModel OnlineHistoryReportView => IoC.Get<OnlineHistoryReportViewModel>();

        public static UserHistoryViewModel UserHistoryView => IoC.Get<UserHistoryViewModel>();
        public static UserTopUpHistoryViewModel UserTopUpHistorytView => IoC.Get<UserTopUpHistoryViewModel>();
        public static ReportViewerModel ReportViewer => IoC.Get<ReportViewerModel>();

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
            Kernel.Bind<ControlViewModel>().ToConstant(new ControlViewModel());
            Kernel.Bind<MemberCouponViewModel>().ToConstant(new MemberCouponViewModel());
            Kernel.Bind<IncomeViewModel>().ToConstant(new IncomeViewModel());
            Kernel.Bind<LogHistoryViewModel>().ToConstant(new LogHistoryViewModel());

            Kernel.Bind<OptionViewModel>().ToConstant(new OptionViewModel());
            Kernel.Bind<AdminViewModel>().ToConstant(new AdminViewModel());
            Kernel.Bind<StaffViewModel>().ToConstant(new StaffViewModel());
            Kernel.Bind<AddEditStaffViewModel>().ToConstant(new AddEditStaffViewModel());
            Kernel.Bind<AddEditGroupViewModel>().ToConstant(new AddEditGroupViewModel());
            Kernel.Bind<AddEditBonusViewModel>().ToConstant(new AddEditBonusViewModel());
            Kernel.Bind<AddEditPromotionViewModel>().ToConstant(new AddEditPromotionViewModel());
            Kernel.Bind<AddEditMemberViewModel>().ToConstant(new AddEditMemberViewModel());
            Kernel.Bind<AddEditCouponViewModel>().ToConstant(new AddEditCouponViewModel());
            Kernel.Bind<DatabaseViewModel>().ToConstant(new DatabaseViewModel());
            Kernel.Bind<UserGroupViewModel>().ToConstant(new UserGroupViewModel());
            Kernel.Bind<PromotionViewModel>().ToConstant(new PromotionViewModel());
            Kernel.Bind<DelMemberViewModel>().ToConstant(new DelMemberViewModel());
            Kernel.Bind<DelCouponViewModel>().ToConstant(new DelCouponViewModel());

            Kernel.Bind<CouponViewModel>().ToConstant(new CouponViewModel());
            Kernel.Bind<ReportViewModel>().ToConstant(new ReportViewModel());
            Kernel.Bind<ConfirmViewModel>().ToConstant(new ConfirmViewModel());
            Kernel.Bind<WarningViewModel>().ToConstant(new WarningViewModel());
            Kernel.Bind<MemberViewModel>().ToConstant(new MemberViewModel());

            Kernel.Bind<TopUpViewModel>().ToConstant(new TopUpViewModel());
            Kernel.Bind<FreeTopUpViewModel>().ToConstant(new FreeTopUpViewModel());
            Kernel.Bind<LendViewModel>().ToConstant(new LendViewModel());
            Kernel.Bind<DebtViewModel>().ToConstant(new DebtViewModel());
            Kernel.Bind<PayDebtViewModel>().ToConstant(new PayDebtViewModel());
            Kernel.Bind<GenerateCouponViewModel>().ToConstant(new GenerateCouponViewModel());
            Kernel.Bind<AddEditOptionCouponViewModel>().ToConstant(new AddEditOptionCouponViewModel());
            Kernel.Bind<ProgressBarViewModel>().ToConstant(new ProgressBarViewModel());

            Kernel.Bind<MemberReportViewModel>().ToConstant(new MemberReportViewModel());
            Kernel.Bind<CouponReportViewModel>().ToConstant(new CouponReportViewModel());
            Kernel.Bind<TopUpReportViewModel>().ToConstant(new TopUpReportViewModel());
            Kernel.Bind<IncomeReportViewModel>().ToConstant(new IncomeReportViewModel());
            Kernel.Bind<OnlineHistoryReportViewModel>().ToConstant(new OnlineHistoryReportViewModel());

            Kernel.Bind<UserHistoryViewModel>().ToConstant(new UserHistoryViewModel());
            Kernel.Bind<UserTopUpHistoryViewModel>().ToConstant(new UserTopUpHistoryViewModel());
            Kernel.Bind<ReportViewerModel>().ToConstant(new ReportViewerModel());

        }

        public static T Get<T>()
        {
            return Kernel.Get<T>();
        }
    }
}
