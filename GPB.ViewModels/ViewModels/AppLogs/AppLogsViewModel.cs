﻿using GPB.Models;
using GPB.Services;
using System;
using System.Threading.Tasks;

namespace GPB.ViewModels
{
    public class AppLogsViewModel : ViewModelBase
    {
        public AppLogsViewModel(ICommonServices commonServices) : base(commonServices)
        {
            AppLogList = new AppLogListViewModel(commonServices);
            AppLogDetails = new AppLogDetailsViewModel(commonServices);
        }

        public AppLogListViewModel AppLogList { get; }
        public AppLogDetailsViewModel AppLogDetails { get; }

        public async Task LoadAsync(AppLogListArgs args)
        {
            await AppLogList.LoadAsync(args);
        }
        public void Unload()
        {
            AppLogList.Unload();
        }

        public void Subscribe()
        {
            MessageService.Subscribe<AppLogListViewModel>(this, OnMessage);
            AppLogList.Subscribe();
            AppLogDetails.Subscribe();
        }
        public void Unsubscribe()
        {
            MessageService.Unsubscribe(this);
            AppLogList.Unsubscribe();
            AppLogDetails.Unsubscribe();
        }

        private async void OnMessage(AppLogListViewModel viewModel, string message, object args)
        {
            if (viewModel == AppLogList && message == "ItemSelected")
            {
                await ContextService.RunAsync(() =>
                {
                    OnItemSelected();
                });
            }
        }

        private async void OnItemSelected()
        {
            if (AppLogDetails.IsEditMode)
            {
                StatusReady();
            }
            var selected = AppLogList.SelectedItem;
            if (!AppLogList.IsMultipleSelection)
            {
                if (selected != null && !selected.IsEmpty)
                {
                    await PopulateDetails(selected);
                }
            }
            AppLogDetails.Item = selected;
        }

        private async Task PopulateDetails(AppLogModel selected)
        {
            try
            {
                var model = await LogService.GetLogAsync(selected.Id);
                selected.Merge(model);
            }
            catch (Exception ex)
            {
                LogException("AppLogs", "Load Details", ex);
            }
        }
    }
}
