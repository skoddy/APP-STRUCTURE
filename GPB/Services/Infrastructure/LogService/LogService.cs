﻿using GPB.Data;
using GPB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPB.Services
{
    public class LogService : ILogService
    {
        public LogService(IMessageService messageService)
        {
            MessageService = messageService;
        }

        public IMessageService MessageService { get; }
        public async Task WriteAsync(LogType type, string source, string action, Exception ex)
        {
            await WriteAsync(LogType.Error, source, action, ex.Message, ex.ToString());
            Exception deepException = ex.InnerException;
            while (deepException != null)
            {
                await WriteAsync(LogType.Error, source, action, deepException.Message, deepException.ToString());
                deepException = deepException.InnerException;
            }
        }
        public async Task WriteAsync(LogType type, string source, string action, string message, string description)
        {
            var appLog = new AppLog()
            {
                User = AppSettings.Current.UserName ?? "App",
                Type = type,
                Source = source,
                Action = action,
                Message = message,
                Description = description,
            };

            appLog.IsRead = type != LogType.Error;

            await CreateLogAsync(appLog);
            MessageService.Send(this, "LogAdded", appLog);
        }

        private AppLogDb CreateDataSource()
        {
            return new AppLogDb(AppSettings.Current.AppLogConnectionString);
        }

        public async Task<AppLogModel> GetLogAsync(long id)
        {
            using (var ds = CreateDataSource())
            {
                var item = await ds.GetLogAsync(id);
                if (item != null)
                {
                    return CreateAppLogModel(item);
                }
                return null;
            }
        }

        public async Task<IList<AppLogModel>> GetLogsAsync(DataRequest<AppLog> request)
        {
            var collection = new LogCollection(this);
            await collection.LoadAsync(request);
            return collection;
        }

        public async Task<IList<AppLogModel>> GetLogsAsync(int skip, int take, DataRequest<AppLog> request)
        {
            var models = new List<AppLogModel>();
            using (var ds = CreateDataSource())
            {
                var items = await ds.GetLogsAsync(skip, take, request);
                foreach (var item in items)
                {
                    models.Add(CreateAppLogModel(item));
                }
                return models;
            }
        }

        public async Task<int> GetLogsCountAsync(DataRequest<AppLog> request)
        {
            using (var ds = CreateDataSource())
            {
                return await ds.GetLogsCountAsync(request);
            }
        }

        public async Task<int> CreateLogAsync(AppLog appLog)
        {
            using (var ds = CreateDataSource())
            {
                return await ds.CreateLogAsync(appLog);
            }
        }

        public async Task<int> DeleteLogAsync(AppLogModel model)
        {
            var appLog = new AppLog { Id = model.Id };
            using (var ds = CreateDataSource())
            {
                return await ds.DeleteLogsAsync(appLog);
            }
        }

        public async Task<int> DeleteLogRangeAsync(int index, int length, DataRequest<AppLog> request)
        {
            using (var ds = CreateDataSource())
            {
                var items = await ds.GetLogKeysAsync(index, length, request);
                return await ds.DeleteLogsAsync(items.ToArray());
            }
        }

        public async Task MarkAllAsReadAsync()
        {
            using (var ds = CreateDataSource())
            {
                await ds.MarkAllAsReadAsync();
            }
        }

        private AppLogModel CreateAppLogModel(AppLog source)
        {
            return new AppLogModel()
            {
                Id = source.Id,
                IsRead = source.IsRead,
                DateTime = source.DateTime,
                User = source.User,
                Type = source.Type,
                Source = source.Source,
                Action = source.Action,
                Message = source.Message,
                Description = source.Description,
            };
        }


    }
}
