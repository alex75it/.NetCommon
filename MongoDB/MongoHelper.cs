using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;

namespace Alex75.Common.MongoDB
{
    public class MongoHelper : IMongoHelper
    {
        private IMongoDatabase Database { get; }

        public MongoHelper(string connectionString, string databaseName = null)
        {
            // when database name is not provided try to get it from the connection string
            if (databaseName is null)
            {
                databaseName = new MongoUrl(connectionString).DatabaseName;
                if (databaseName == null)
                    throw new Exception($"Unable to obtain database name from the connection string. (Connection string: {connectionString}).");
            }

            Database = GetDatabase(connectionString, databaseName);
        }

        public async Task<bool> Ping()
        {
            //https://docs.mongodb.com/manual/reference/command/ping/

            try
            {
                await Database.RunCommandAsync<object>(new JsonCommand<object>("{ping:1}"));
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private IMongoDatabase GetDatabase(string connectionString, string databaseName)
        {
            // this is not applied because a "default" serializer already exists
            if (BsonSerializer.SerializerRegistry.GetSerializer<DateTime>() == null)
                BsonSerializer.RegisterSerializer(typeof(DateTime), new LocalDateTimeSerializer());

            var client = new MongoClient(connectionString);

            return client.GetDatabase(databaseName);
        }

        public DatabaseSettings GetDatabaseSettings() => new DatabaseSettings(Database.Client.Settings);



        public IMongoCollection<T> GetCollection<T>(string collectionName = null)
        {
            return Database.GetCollection<T>(collectionName ?? typeof(T).Name.ToLower() + "s");
        }

        public bool IsConnected<T>(string collectionName = null)
        {
            try
            {
                Database.GetCollection<T>(collectionName ?? typeof(T).Name.ToLower() + "s");
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Ignore new properties of the class that does not exist in the stored document.
        /// </summary>
        public void SetIgnoreExtraElements<T>()
        {
            if (!BsonClassMap.IsClassMapRegistered(typeof(T)))
            {
                BsonClassMap.RegisterClassMap<T>(map =>
                {
                    map.AutoMap();
                    map.SetIgnoreExtraElements(true);
                });
            }
        }
    }

    /// <summary>
    /// Store/Retrieve date as locale in MongoDB
    /// </summary>
    internal class LocalDateTimeSerializer : DateTimeSerializer
    {
        // MongoDB returns datetime as DateTimeKind.Utc, which can't be used in our timezone conversion logic
        // We overwrite it to be DateTimeKind.Unspecified
        public override DateTime Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            var date = base.Deserialize(context, args);
            return new DateTime(date.Ticks, DateTimeKind.Unspecified);
        }

        // MongoDB stores all datetime as UTC, any datetime value DateTimeKind is not DateTimeKind.Utc, will be converted to UTC first
        // We overwrite it to be DateTimeKind.Utc, becasue we want to preserve the raw value
        public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, DateTime date)
        {
            var utcDate = new DateTime(date.Ticks, DateTimeKind.Utc);
            base.Serialize(context, args, utcDate);
        }
    }
}
