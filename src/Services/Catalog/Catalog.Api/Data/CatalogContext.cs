using Catalog.Api.Entities;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System;

namespace Catalog.Api.Data
{
    public class CatalogContext : ICatalogContext
    {
        public CatalogContext(IConfiguration configuration)
        {
            var connectionString = configuration.GetValue<string>("DatabaseSettings:ConnectionString");
            var dbName = configuration.GetValue<string>("DatabaseSettings:DatabaseName");
            var collectionName = configuration.GetValue<string>("DatabaseSettings:CollectionName");

            if (connectionString is null || dbName is null || collectionName is null) throw new ArgumentNullException("Connection Settings can't be null");

            var client = new MongoClient(connectionString);
            //get database name or create one if it doesn't exist
            var database = client.GetDatabase(dbName);

            Products = database.GetCollection<Product>(collectionName);
            CatalogContextSeed.SeedData(Products);
        }
        public IMongoCollection<Product> Products { get; }
    }
}
