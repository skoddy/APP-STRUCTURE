using System;
using GPB.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// Die Elementvorlage "Benutzersteuerelement" wird unter https://go.microsoft.com/fwlink/?LinkId=234236 dokumentiert.

namespace GPB.Views
{
    public sealed partial class AppLogsDetails : UserControl
    {
        public AppLogsDetails()
        {
            InitializeComponent();
        }

        #region ViewModel
        public AppLogDetailsViewModel ViewModel
        {
            get { return (AppLogDetailsViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register("ViewModel", typeof(AppLogDetailsViewModel), typeof(AppLogsDetails), new PropertyMetadata(null));
        #endregion
    }
}
