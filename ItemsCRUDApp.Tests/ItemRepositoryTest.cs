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

namespace ItemsCRUDApp.Tests
{
    public class ItemRepositoryTest
    {
        const string LocalDbConnectionString = "Server=(localdb)\\mssqllocaldb;Database=ItemsCrudDBTest;Trusted_Connection=True;MultipleActiveResultSets=true";
        public ItemRepositoryTest()
        {
            ContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlServer(LocalDbConnectionString)
                .Options;

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
    }
}
