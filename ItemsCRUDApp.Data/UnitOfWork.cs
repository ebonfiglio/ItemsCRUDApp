using ItemsCRUDApp.Data.Repositories;
using ItemsCRUDApp.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ItemsCRUDApp.Data
{
    public interface IUnitOfWork
    {
        IRepository<Item> ItemRepository { get; }

        void SaveChanges();
    }
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext context;
        public UnitOfWork(ApplicationDbContext context)
        {
            this.context = context;
        }

        private IRepository<Item> itemRepository;
        public IRepository<Item> ItemRepository
        {
            get
            {
                if (itemRepository == null)
                {
                    itemRepository = new ItemRepository(context);
                }
                return itemRepository;
            }
        }

        public void SaveChanges()
        {
            context.SaveChanges();
        }
    }
}
