using AutoMapper;
using ItemsCRUDApp.Data;
using ItemsCRUDApp.Data.Entities;
using ItemsCRUDApp.Domain.Mapper;
using ItemsCRUDApp.Domain.Services;
using ItemsCRUDApp.Shared.Requests;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace ItemsCRUDApp.Tests
{
    public class ItemServiceTest
    {
        public ItemServiceTest()
        {
            var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()
                .BuildServiceProvider();


            var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
            builder.UseInMemoryDatabase("ItemsCrudDBTest")
                   .UseInternalServiceProvider(serviceProvider);
            ContextOptions = builder.Options;

            Seed();

            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new ItemProfile());
            });
            mapper = mockMapper.CreateMapper();
        }
        protected DbContextOptions<ApplicationDbContext> ContextOptions { get; set; }
        protected IMapper mapper { get; set; }
     
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
                var unitOfWork = new UnitOfWork(context);
                var itemService = new ItemService(unitOfWork, mapper);

                var items = itemService.GetAllAsync().GetAwaiter().GetResult();

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
                var unitOfWork = new UnitOfWork(context);
                var itemService = new ItemService(unitOfWork, mapper);

                var item = itemService.GetAsync(id).GetAwaiter().GetResult();

                Assert.Equal(itemName, item.ItemName);
                Assert.Equal(cost, item.Cost);
            }
        }

        [Theory]
        [InlineData(1, 150)]
        [InlineData(2, 900)]
        [InlineData(3, 35)]
        public void UpdateItem(int id, decimal newCost)
        {
        
            using (var context = new ApplicationDbContext(ContextOptions))
            {
                var unitOfWork = new UnitOfWork(context);
                var itemService = new ItemService(unitOfWork, mapper);

                var itemResponse = itemService.GetAsync(id).GetAwaiter().GetResult();
                itemResponse.Cost = newCost;
                var item = mapper.Map<Item>(itemResponse);
                var requestItem = mapper.Map<ItemRequest>(item);
                var updatedItem = itemService.UpdateAsync(requestItem).GetAwaiter().GetResult();

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
                var unitOfWork = new UnitOfWork(context);
                var itemService = new ItemService(unitOfWork, mapper);
                itemService.DeleteAsync(id).GetAwaiter().GetResult();

                var item = itemService.GetAsync(id).GetAwaiter().GetResult();

                Assert.Null(item);
            }
        }

        [Theory]
        [InlineData("Item 4", 750)]
        public void AddItem(string itemName, decimal cost)
        {

            using (var context = new ApplicationDbContext(ContextOptions))
            {
                var unitOfWork = new UnitOfWork(context);
                var itemService = new ItemService(unitOfWork, mapper);
                var itemRequest = new ItemRequest()
                {
                    ItemName = itemName,
                    Cost = cost
                };

                var item = itemService.AddAsync(itemRequest).GetAwaiter().GetResult();

                Assert.NotNull(item);
            }
        }

        [Theory]
        [InlineData("Item 1", 100)]
        [InlineData("Item 2", 250)]
        public void MaxPriceByItemName(string itemName, decimal maxPrice)
        {

            using (var context = new ApplicationDbContext(ContextOptions))
            {
                var unitOfWork = new UnitOfWork(context);
                var itemService = new ItemService(unitOfWork, mapper);

                var itemMaxPrice = itemService.MaxPriceByItemName(itemName).GetAwaiter().GetResult();

                Assert.Equal(maxPrice, itemMaxPrice);
            }
        }

        [Fact]
        public void MaxPricesOfItems()
        {

            using (var context = new ApplicationDbContext(ContextOptions))
            {
                var unitOfWork = new UnitOfWork(context);
                var itemService = new ItemService(unitOfWork, mapper);

                var maxPricesOfItems = itemService.MaxPricesOfItems().GetAwaiter().GetResult();

                Assert.Equal("Item 1", maxPricesOfItems[0].ItemName);
                Assert.Equal("Item 2", maxPricesOfItems[1].ItemName);

                Assert.Equal(100, maxPricesOfItems[0].Cost);
                Assert.Equal(250, maxPricesOfItems[1].Cost);
            }
        }
    }
}


