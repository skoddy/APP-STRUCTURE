using System.Threading.Tasks;
using Windows.Storage.Streams;

namespace GPB.Services
{
    public class LoginService : ILoginService
    {
        public LoginService(IMessageService messageService, IDialogService dialogService)
        {
            IsAuthenticated = false;
            MessageService = messageService;
            DialogService = dialogService;
        }

        public IMessageService MessageService { get; }
        public IDialogService DialogService { get; }

        public bool IsAuthenticated { get; set; }



        public Task<bool> SignInWithPasswordAsync(string userName, string password)
        {
            // Perform authentication here.
            // This sample accepts any user name and password.
            UpdateAuthenticationStatus(true);
            return Task.FromResult(true);
        }



        public void Logoff()
        {
            UpdateAuthenticationStatus(false);
        }

        private void UpdateAuthenticationStatus(bool isAuthenticated)
        {
            IsAuthenticated = isAuthenticated;
            MessageService.Send(this, "AuthenticationChanged", IsAuthenticated);
        }

        static private Task<bool> RegisterPassportCredentialWithServerAsync(IBuffer publicKey)
        {
            // TODO:
            // Register the public key and attestation of the key credential with the server
            // In a real-world scenario, this would likely also include:
            //      - Certificate chain for attestation endorsement if available
            //      - Status code of the Key Attestation result : Included / retrieved later / retry type
            return Task.FromResult(true);
        }
    }
}
