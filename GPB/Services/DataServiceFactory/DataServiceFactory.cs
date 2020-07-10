﻿using GPB.Data.Services;
using System;

namespace GPB.Services
{
    public class DataServiceFactory : IDataServiceFactory
    {
        static private Random _random = new Random(0);

        public IDataService CreateDataService()
        {
            if (AppSettings.Current.IsRandomErrorsEnabled)
            {
                if (_random.Next(20) == 0)
                {
                    throw new InvalidOperationException("Random error simulation");
                }
            }

            switch (AppSettings.Current.DataProvider)
            {
                case DataProviderType.SQLite:
                    return new SQLiteDataService(AppSettings.Current.SQLiteConnectionString);

                case DataProviderType.SQLServer:
                    return new SQLServerDataService(AppSettings.Current.SQLServerConnectionString);

                default:
                    throw new NotImplementedException();
            }
        }
    }
}
