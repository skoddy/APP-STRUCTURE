using GPB.Data.Services;

namespace GPB.Services
{
    public interface IDataServiceFactory
    {
        IDataService CreateDataService();
    }
}
