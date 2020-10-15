using ItemsCRUDApp.Data.Entities;
using ItemsCRUDApp.Shared.Requests;
using ItemsCRUDApp.Shared.Responses;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ItemsCRUDApp.Domain.Services
{
    public interface IItemService
    {
        Task<ItemResponse> GetAsync(int id);

        Task<List<ItemResponse>> GetAllAsync();

        Task<ItemResponse> FindAsync(Expression<Func<Item, bool>> predicate);
        Task<ItemResponse> AddAsync(ItemRequest request);
        Task<ItemResponse> UpdateAsync(ItemRequest request);

        Task DeleteAsync(int id);
    }
}
