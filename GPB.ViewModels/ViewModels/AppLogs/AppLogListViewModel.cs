﻿using GPB.Data;
using GPB.Models;
using GPB.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace GPB.ViewModels
{
    #region AppLogListArgs
    public class AppLogListArgs
    {
        static public AppLogListArgs CreateEmpty() => new AppLogListArgs { IsEmpty = true };

        public AppLogListArgs()
        {
            OrderByDesc = r => r.DateTime;
        }

        public bool IsEmpty { get; set; }

        public string Query { get; set; }

        public Expression<Func<AppLog, object>> OrderBy { get; set; }
        public Expression<Func<AppLog, object>> OrderByDesc { get; set; }
    }
    #endregion

    public class AppLogListViewModel : GenericListViewModel<AppLogModel>
    {
        public AppLogListViewModel(ICommonServices commonServices) : base(commonServices)
        {
        }

        public AppLogListArgs ViewModelArgs { get; private set; }

        public async Task LoadAsync(AppLogListArgs args)
        {
            ViewModelArgs = args ?? AppLogListArgs.CreateEmpty();
            Query = ViewModelArgs.Query;

            StartStatusMessage("Loading logs...");
            if (await RefreshAsync())
            {
                EndStatusMessage("Logs loaded");
            }
        }
        public void Unload()
        {
            ViewModelArgs.Query = Query;
        }

        public void Subscribe()
        {
            MessageService.Subscribe<AppLogListViewModel>(this, OnMessage);
            MessageService.Subscribe<AppLogDetailsViewModel>(this, OnMessage);
            MessageService.Subscribe<ILogService, AppLog>(this, OnLogServiceMessage);
        }
        public void Unsubscribe()
        {
            MessageService.Unsubscribe(this);
        }

        public AppLogListArgs CreateArgs()
        {
            return new AppLogListArgs
            {
                Query = Query,
                OrderBy = ViewModelArgs.OrderBy,
                OrderByDesc = ViewModelArgs.OrderByDesc
            };
        }

        public async Task<bool> RefreshAsync()
        {
            bool isOk = true;

            Items = null;
            ItemsCount = 0;
            SelectedItem = null;

            try
            {
                Items = await GetItemsAsync();
            }
            catch (Exception ex)
            {
                Items = new List<AppLogModel>();
                StatusError($"Error loading Logs: {ex.Message}");
                LogException("AppLogs", "Refresh", ex);
                isOk = false;
            }

            ItemsCount = Items.Count;
            if (!IsMultipleSelection)
            {
                SelectedItem = Items.FirstOrDefault();
            }
            NotifyPropertyChanged(nameof(Title));

            return isOk;
        }

        private async Task<IList<AppLogModel>> GetItemsAsync()
        {
            if (!ViewModelArgs.IsEmpty)
            {
                DataRequest<AppLog> request = BuildDataRequest();
                return await LogService.GetLogsAsync(request);
            }
            return new List<AppLogModel>();
        }

        protected override void OnNew()
        {
            throw new NotImplementedException();
        }

        protected override async void OnRefresh()
        {
            StartStatusMessage("Loading logs...");
            if (await RefreshAsync())
            {
                EndStatusMessage("Logs loaded");
            }
        }

        protected override async void OnDeleteSelection()
        {
            StatusReady();
            if (await DialogService.ShowAsync("Confirm Delete", "Are you sure you want to delete selected logs?", "Ok", "Cancel"))
            {
                int count = 0;
                try
                {
                    if (SelectedIndexRanges != null)
                    {
                        count = SelectedIndexRanges.Sum(r => r.Length);
                        StartStatusMessage($"Deleting {count} logs...");
                        await DeleteRangesAsync(SelectedIndexRanges);
                        MessageService.Send(this, "ItemRangesDeleted", SelectedIndexRanges);
                    }
                    else if (SelectedItems != null)
                    {
                        count = SelectedItems.Count();
                        StartStatusMessage($"Deleting {count} logs...");
                        await DeleteItemsAsync(SelectedItems);
                        MessageService.Send(this, "ItemsDeleted", SelectedItems);
                    }
                }
                catch (Exception ex)
                {
                    StatusError($"Error deleting {count} Logs: {ex.Message}");
                    LogException("AppLogs", "Delete", ex);
                    count = 0;
                }
                await RefreshAsync();
                SelectedIndexRanges = null;
                SelectedItems = null;
                if (count > 0)
                {
                    EndStatusMessage($"{count} logs deleted");
                }
            }
        }

        private async Task DeleteItemsAsync(IEnumerable<AppLogModel> models)
        {
            foreach (var model in models)
            {
                await LogService.DeleteLogAsync(model);
            }
        }

        private async Task DeleteRangesAsync(IEnumerable<IndexRange> ranges)
        {
            DataRequest<AppLog> request = BuildDataRequest();
            foreach (var range in ranges)
            {
                await LogService.DeleteLogRangeAsync(range.Index, range.Length, request);
            }
        }

        private DataRequest<AppLog> BuildDataRequest()
        {
            return new DataRequest<AppLog>()
            {
                Query = Query,
                OrderBy = ViewModelArgs.OrderBy,
                OrderByDesc = ViewModelArgs.OrderByDesc
            };
        }

        private async void OnMessage(ViewModelBase sender, string message, object args)
        {
            switch (message)
            {
                case "NewItemSaved":
                case "ItemDeleted":
                case "ItemsDeleted":
                case "ItemRangesDeleted":
                    await ContextService.RunAsync(async () =>
                    {
                        await RefreshAsync();
                    });
                    break;
            }
        }

        private async void OnLogServiceMessage(ILogService logService, string message, AppLog log)
        {
            if (message == "LogAdded")
            {
                await ContextService.RunAsync(async () =>
                {
                    await RefreshAsync();
                });
            }
        }
    }
}
