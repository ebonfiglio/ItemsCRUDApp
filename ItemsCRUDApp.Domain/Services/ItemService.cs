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

        public async Task<ItemResponse> FindAsync(Expression<Func<Item, bool>> predicate)
        {
            var entity = await _unitOfWork.ItemRepository.Find(predicate);
            return _mapper.Map<ItemResponse>(entity);
        }

        public async Task<List<ItemResponse>> GetAllAsync()
        {
            var entityList = await _unitOfWork.ItemRepository.All();
            return _mapper.Map<IEnumerable<ItemResponse>>(entityList).ToList();
        }

        public async Task<ItemResponse> GetAsync(int id)
        {
            var entity = await _unitOfWork.ItemRepository.Get(id);
            return _mapper.Map<ItemResponse>(entity);
        }

        public async Task<ItemResponse> UpdateAsync(ItemRequest request)
        {
            var existingItem = await _unitOfWork.ItemRepository.Get(request.Id);
            if (existingItem == null)
            {
                throw new ArgumentException($"Entity with {request.Id} is not present");
            }

            var entity = _mapper.Map(request, existingItem);
            var result = await _unitOfWork.ItemRepository.Update(entity);
            await _unitOfWork.ItemRepository.SaveChanges();

            return _mapper.Map<ItemResponse>(result);
        }
    }
}
