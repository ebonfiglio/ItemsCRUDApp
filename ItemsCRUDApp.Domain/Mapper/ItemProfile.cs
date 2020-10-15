using AutoMapper;
using ItemsCRUDApp.Data.Entities;
using ItemsCRUDApp.Shared.Requests;
using ItemsCRUDApp.Shared.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace ItemsCRUDApp.Domain.Mapper
{
    public class ItemProfile : Profile
    {
        public ItemProfile()
        {
            CreateMap<ItemRequest, Item>().ReverseMap();
            CreateMap<ItemResponse, Item>().ReverseMap();
            CreateMap<List<ItemResponse>, List<Item>>().ReverseMap();
        }
    }
}
