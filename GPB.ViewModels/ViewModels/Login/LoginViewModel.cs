using GPB.Services;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GPB.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        public LoginViewModel(ILoginService loginService, ISettingsService settingsService, ICommonServices commonServices) : base(commonServices)
        {
            LoginService = loginService;
            SettingsService = settingsService;
        }

        public ILoginService LoginService { get; }
        public ISettingsService SettingsService { get; }

        private ShellArgs ViewModelArgs { get; set; }

        private bool _isBusy = false;
        public bool IsBusy
        {
            get { return _isBusy; }
            set { Set(ref _isBusy, value); }
        }

        private bool _isLoginWithPassword = false;
        public bool IsLoginWithPassword
        {
            get { return _isLoginWithPassword; }
            set { Set(ref _isLoginWithPassword, value); }
        }


        private string _userName = null;
        public string UserName
        {
            get { return _userName; }
            set { Set(ref _userName, value); }
        }

        private string _password = "UserPassword";
        public string Password
        {
            get { return _password; }
            set { Set(ref _password, value); }
        }

        public ICommand ShowLoginWithPasswordCommand => new RelayCommand(ShowLoginWithPassword);
        public ICommand LoginWithPasswordCommand => new RelayCommand(LoginWithPassword);

        public Task LoadAsync(ShellArgs args)
        {
            ViewModelArgs = args;

            UserName = SettingsService.UserName ?? args.UserInfo.AccountName;

            IsBusy = false;

            return Task.CompletedTask;
        }

        public void Login()
        {

                LoginWithPassword();


        }

        private void ShowLoginWithPassword()
        {
            IsLoginWithPassword = true;
        }

        public async void LoginWithPassword()
        {
            IsBusy = true;
            var result = ValidateInput();
            if (result.IsOk)
            {
                if (await LoginService.SignInWithPasswordAsync(UserName, Password))
                {

                    SettingsService.UserName = UserName;
                    EnterApplication();
                    return;
                }
            }
            await DialogService.ShowAsync(result.Message, result.Description);
            IsBusy = false;
        }


        private void EnterApplication()
        {
            if (ViewModelArgs.UserInfo.AccountName != UserName)
            {
                ViewModelArgs.UserInfo = new UserInfo
                {
                    AccountName = UserName,
                    FirstName = UserName,
                    PictureSource = null
                };
            }
            NavigationService.Navigate<MainShellViewModel>(ViewModelArgs);
        }

        private Result ValidateInput()
        {
            if (String.IsNullOrWhiteSpace(UserName))
            {
                return Result.Error("Login error", "Please, enter a valid user name.");
            }
            if (String.IsNullOrWhiteSpace(Password))
            {
                return Result.Error("Login error", "Please, enter a valid password.");
            }
            return Result.Ok();
        }
    }
}
