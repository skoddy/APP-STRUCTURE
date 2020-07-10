using GPB.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// Die Elementvorlage "Benutzersteuerelement" wird unter https://go.microsoft.com/fwlink/?LinkId=234236 dokumentiert.

namespace GPB.Views
{
    public sealed partial class AppLogsList : UserControl
    {
        public AppLogsList()
        {
            InitializeComponent();
        }

        #region ViewModel
        public AppLogListViewModel ViewModel
        {
            get { return (AppLogListViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register("ViewModel", typeof(AppLogListViewModel), typeof(AppLogsList), new PropertyMetadata(null));
        #endregion
    }
}
