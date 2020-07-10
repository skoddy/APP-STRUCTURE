using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPB.Services
{
    public interface ILoginService
    {
        bool IsAuthenticated { get; set; }

        Task<bool> SignInWithPasswordAsync(string userName, string password);

        void Logoff();
    }
}
