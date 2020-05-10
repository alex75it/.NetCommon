using System.Threading.Tasks;
using MongoDB.Driver;

namespace Alex75.Common.MongoDB
{
    public interface IMongoHelper
    {
        IMongoCollection<T> GetCollection<T>(string collectionName = null);

        Task<bool> Ping();
        bool IsConnected<T>(string collectionName = null);
        void SetIgnoreExtraElements<T>();
    }
}
