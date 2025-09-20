
using MongoDB.Driver;
using System.Linq.Expressions;

namespace Play.Common.MongoDB
{
    public class MongoRepositry<T>:IMongoRepositry<T> where T : IEntity
    {
        
        private readonly IMongoCollection<T> dbCollection;
        
        private readonly FilterDefinitionBuilder<T> filterBuilder = Builders<T>.Filter;

        public MongoRepositry(IMongoDatabase database, string collectionName)
        {
            
            dbCollection = database.GetCollection<T>(collectionName);

        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await dbCollection.Find(filterBuilder.Empty).ToListAsync();
        }

        public async Task<T> GetAsync(Guid Id)
        {
            FilterDefinition<T> filter = filterBuilder.Eq(entity => entity.Id, Id);
            return await dbCollection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task CreateAsync(T item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            await dbCollection.InsertOneAsync(item);
        }
        public async Task UpdateAsync(T item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }
            FilterDefinition<T> filter = filterBuilder.Eq(existingEntity => existingEntity.Id, item.Id);
            await dbCollection.ReplaceOneAsync(filter,item);
        }

        public async Task DeleteAsync(Guid id)
        {

            FilterDefinition<T> filter = filterBuilder.Eq(entity => entity.Id, id);
            await dbCollection.DeleteOneAsync(filter);
        }

        public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> filter)
        {
            return await dbCollection.Find(filter).ToListAsync();
        }

        public async Task<T> GetAsync(Expression<Func<T, bool>> filter)
        {
            return await dbCollection.Find(filter).FirstOrDefaultAsync();
        }
    }
}
