﻿using GPB.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// Die Elementvorlage "Leere Seite" wird unter https://go.microsoft.com/fwlink/?LinkId=234238 dokumentiert.

namespace GPB.Views
{
    public sealed partial class ValidateConnectionView : ContentDialog
    {
        private string _connectionString = null;

        public ValidateConnectionView(string connectionString)
        {
            _connectionString = connectionString;
            ViewModel = ServiceLocator.Current.GetService<ValidateConnectionViewModel>();
            InitializeComponent();
            Loaded += OnLoaded;
        }

        public ValidateConnectionViewModel ViewModel { get; }

        public Result Result { get; private set; }

        private async void OnLoaded(object sender, RoutedEventArgs e)
        {
            await ViewModel.ExecuteAsync(_connectionString);
            Result = ViewModel.Result;
        }

        private void OnOkClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }

        private void OnCancelClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            Result = Result.Ok("Operation cancelled by user");
        }
    }
}
