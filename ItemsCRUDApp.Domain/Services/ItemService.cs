using ItemsCRUDApp.Shared.Requests;
using ItemsCRUDApp.Shared.Responses;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ItemsCRUDApp.Data.Entities;
using AutoMapper;
using ItemsCRUDApp.Data;
using System.Linq;

namespace ItemsCRUDApp.Domain.Services
{
    public class ItemService : IItemService
    {
        IUnitOfWork _unitOfWork;
        IMapper _mapper;
        public ItemService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<ItemResponse> AddAsync(ItemRequest request)
        {
            var entity = _mapper.Map<Item>(request);
            var result =  await _unitOfWork.ItemRepository.Add(entity);
            await _unitOfWork.ItemRepository.SaveChanges();
            return _mapper.Map<ItemResponse>(result);
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _unitOfWork.ItemRepository.Get(id);
            await _unitOfWork.ItemRepository.Delete(entity);
            await _unitOfWork.ItemRepository.SaveChanges();
        }

        public async Task<List<ItemResponse>> FindAsync(Expression<Func<Item, bool>> predicate)
        {
            var entity = await _unitOfWork.ItemRepository.Find(predicate);
            return _mapper.Map<List<ItemResponse>>(entity.ToList());
        }

        public async Task<List<ItemResponse>> GetAllAsync()
        {
            var entityList = await _unitOfWork.ItemRepository.All();
            return _mapper.Map<List<ItemResponse>>(entityList);
        }

        public async Task<ItemResponse> GetAsync(int id)
        {
            var entity = await _unitOfWork.ItemRepository.Get(id);
            if (entity is null) return null;
            return _mapper.Map<ItemResponse>(entity);
        }

        public async Task<decimal?> MaxPriceByItemName(string itemName)
        {
            var itemsByName = await MaxPricesOfItems();
            return itemsByName.FirstOrDefault(l => l.ItemName == itemName)?.Cost;
        }

        public async Task<List<ItemResponse>> MaxPricesOfItems()
        {
            var entityList = await _unitOfWork.ItemRepository.All();
            if (entityList == null) return new List<ItemResponse>();
            var maxPriceList = entityList.GroupBy(l => l.ItemName).Select(g => g.OrderByDescending(r => r.Cost).First());
            return _mapper.Map<List<ItemResponse>>(maxPriceList); 
        }

        public async Task<ItemResponse> UpdateAsync(ItemRequest request)
        {
            var existingItem = await _unitOfWork.ItemRepository.Get(request.Id);
            if (existingItem == null) return null;


            var entity = _mapper.Map(request, existingItem);
            var result = await _unitOfWork.ItemRepository.Update(entity);
            await _unitOfWork.ItemRepository.SaveChanges();

            return _mapper.Map<ItemResponse>(result);
        }
    }
}
