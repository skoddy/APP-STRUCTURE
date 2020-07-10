using GPB.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPB.ViewModels
{
    public class DashboardViewModel : ViewModelBase
    {
        public DashboardViewModel(ICommonServices commonServices) : base(commonServices)
        {
        }
    }
}
