using ItemsCRUDApp.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ItemsCRUDApp.Data.Repositories
{
    public class ItemRepository : GenericRepository<Item>
    {
        public ItemRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
