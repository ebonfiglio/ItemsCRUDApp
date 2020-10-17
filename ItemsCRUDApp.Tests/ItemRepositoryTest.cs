using System;
using Xunit;
using Moq;
using ItemsCRUDApp.Data.Repositories;
using ItemsCRUDApp.Data.Entities;
using ItemsCRUDApp.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace ItemsCRUDApp.Tests
{
    public class ItemRepositoryTest
    {
        public ItemRepositoryTest()
        {
            var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()
                .BuildServiceProvider();

    
            var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
            builder.UseInMemoryDatabase("ItemsCrudDBTest")
                   .UseInternalServiceProvider(serviceProvider);
            ContextOptions = builder.Options;

            Seed();
        }
        protected DbContextOptions<ApplicationDbContext> ContextOptions { get; set; }
        private void Seed()
        {
            using (var context = new ApplicationDbContext(ContextOptions))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                var one = new Item();
                one.Cost = 100;
                one.ItemName = "Item 1";
                var two = new Item();
                two.Cost = 200;
                two.ItemName = "Item 2";
                var three = new Item();
                three.Cost = 250;
                three.ItemName = "Item 2";
                context.AddRange(one, two, three);
                context.SaveChanges();
            }
        }
        [Fact]
        public void GetItems()
        {

            using (var context = new ApplicationDbContext(ContextOptions))
            {
                var itemsRepository = new ItemRepository(context);

                var items =  itemsRepository.All().GetAwaiter().GetResult().ToList();

                Assert.Equal(3, items.Count);

                Assert.Equal("Item 1", items[0].ItemName);
                Assert.Equal("Item 2", items[1].ItemName);
                Assert.Equal("Item 2", items[2].ItemName);

                Assert.Equal(100, items[0].Cost);
                Assert.Equal(200, items[1].Cost);
                Assert.Equal(250, items[2].Cost);
            }


        }
        [Theory]
        [InlineData(1, "Item 1", 100)]
        [InlineData(2, "Item 2", 200)]
        [InlineData(3, "Item 2", 250)]
        public void GetItem(int id, string itemName, decimal cost)
        {

            using (var context = new ApplicationDbContext(ContextOptions))
            {
                var itemsRepository = new ItemRepository(context);

                var item = itemsRepository.Get(id).GetAwaiter().GetResult();

                Assert.Equal(itemName, item.ItemName);
                Assert.Equal(cost, item.Cost);

            }
        }

        [Theory]
        [InlineData(1,150)]
        [InlineData(2, 900)]
        [InlineData(3, 35)]
        public void UpdateItem(int id, decimal newCost)
        {

            using (var context = new ApplicationDbContext(ContextOptions))
            {
                var itemsRepository = new ItemRepository(context);

                var item = itemsRepository.Get(id).GetAwaiter().GetResult();
                item.Cost = newCost;

                var updatedItem = itemsRepository.Update(item).GetAwaiter().GetResult();

                Assert.Equal(newCost, updatedItem.Cost);

            }
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void DeleteItem(int id)
        {

            using (var context = new ApplicationDbContext(ContextOptions))
            {
                var itemsRepository = new ItemRepository(context);
                var item = itemsRepository.Get(id).GetAwaiter().GetResult();
                itemsRepository.Delete(item).GetAwaiter().GetResult();
            }
        }

        [Theory]
        [InlineData("Item 4", 750)]
        public void AddItem(string itemName, decimal cost)
        {

            using (var context = new ApplicationDbContext(ContextOptions))
            {
                var itemsRepository = new ItemRepository(context);
                var item = new Item()
                {
                    ItemName = itemName,
                    Cost = cost
                };

                itemsRepository.Add(item).GetAwaiter().GetResult();

                Assert.NotEqual(0m,item.Id);
            }
        }
    }
}
