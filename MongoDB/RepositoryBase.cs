using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using MongoDB.Driver;

namespace Alex75.Common.MongoDB
{
    public enum IndexDirection { Ascending, Descernding };


    public abstract class MongoRepositoryBase<T>
    {
        protected IMongoHelper MongoHelper { get; }
        protected static FindOneAndUpdateOptions<T> updateReturnNew = new FindOneAndUpdateOptions<T>() { ReturnDocument = ReturnDocument.After };
        private string collectionName;
        public const string TTL_ZERO_ERROR_MESSAGE = "A TTL of zero time is not accepted";

        public MongoRepositoryBase(IMongoHelper mongoHelper, string collectionName)
        {
            MongoHelper = mongoHelper;
            this.collectionName = collectionName;
            //MongoHelper.SetIgnoreExtraElements<T>();  // some instances can be using a custom ClassMap
        }

        [Obsolete("Use the constructor with the collection name")]
        public MongoRepositoryBase(IMongoHelper mongoHelper) : this(mongoHelper, typeof(T).Name.ToLower() + "s") { }

        public void SetIgnoreExtraElements() => MongoHelper.SetIgnoreExtraElements<T>();

        protected IMongoCollection<T> Collection => MongoHelper.GetCollection<T>(collectionName);

        protected IMongoCollection<A> GetCollection<A>(string collectionName) => MongoHelper.GetCollection<A>(collectionName);


        protected string CreateIndex(Expression<Func<T, object>> field, IndexDirection direction, TimeSpan? TTL = null)
        {
            if (TTL == TimeSpan.Zero) throw new Exception(TTL_ZERO_ERROR_MESSAGE);

            return Collection.Indexes.CreateOne(
                new CreateIndexModel<T>(
                    direction == IndexDirection.Ascending ?
                        Builders<T>.IndexKeys.Ascending(field) :
                        Builders<T>.IndexKeys.Descending(field),
                    new CreateIndexOptions() { ExpireAfter = TTL }
                ));
        }

        // utils //

        public FilterDefinitionBuilder<T> FilterBuilder => Builders<T>.Filter;
        public UpdateDefinitionBuilder<T> UpdateBuilder => Builders<T>.Update;
        public ProjectionDefinitionBuilder<T> ProjectionBuilder => Builders<T>.Projection;
        protected FilterDefinition<T> GetFilterOnId(string id) => new JsonFilterDefinition<T>($@"{{_id:""{id}""}}");


        // CRUD //

        public T Get(string id) => Collection.Find(GetFilterOnId(id)).SingleOrDefault();

        public IEnumerable<T> List() => Collection.Find(FilterBuilder.Empty).ToList();


        public void Create(T item) => Collection.InsertOne(item);

        public bool Delete(string id)
        {
            var result = Collection.DeleteOne(GetFilterOnId(id));
            return result.IsAcknowledged && result.DeletedCount == 1;
        }


    }
}
