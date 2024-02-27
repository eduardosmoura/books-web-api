using BooksWebApi.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace BooksWebApi.Services;

public class BooksService
{
    private readonly IMongoCollection<Book> _booksCollection;

    public BooksService(
        IOptions<BooksWebDatabaseSettings> booksWebDatabaseSettings)
    {
        var mongoClient = new MongoClient(
            booksWebDatabaseSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            booksWebDatabaseSettings.Value.DatabaseName);

        _booksCollection = mongoDatabase.GetCollection<Book>(
            booksWebDatabaseSettings.Value.BooksCollectionName);
    }

    public async Task<List<Book>> GetAsync()
    {
        return await _booksCollection.Find(_ => true).ToListAsync();
    }

    public async Task<Book?> GetAsync(string id)
    {
        return await _booksCollection.Find(x => x.Id == id).FirstOrDefaultAsync();
    }

    public async Task CreateAsync(Book newBook)
    {
        await _booksCollection.InsertOneAsync(newBook);
    }

    public async Task UpdateAsync(string id, Book updatedBook)
    {
        await _booksCollection.ReplaceOneAsync(x => x.Id == id, updatedBook);
    }

    public async Task RemoveAsync(string id)
    {
        await _booksCollection.DeleteOneAsync(x => x.Id == id);
    }
}