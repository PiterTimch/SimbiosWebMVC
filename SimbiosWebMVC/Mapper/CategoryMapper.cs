﻿using AutoMapper;
using SimbiosWebMVC.Data.Entities;
using SimbiosWebMVC.Models.Category;
using SimbiosWebMVC.Models.Helpers;

namespace SimbiosWebMVC.Mapper
{
    public class CategoryMapper : Profile
    {
        public CategoryMapper()
        {
            CreateMap<CategoryEntity, CategoryItemViewModel>()
                .ForMember(x => x.Image, opt => opt.MapFrom(x => x.ImageUrl));

            CreateMap<CategoryCreateViewModel, CategoryEntity>()
                .ForMember(x => x.ImageUrl, opt => opt.Ignore());

            CreateMap<CategoryEntity, CategoryEditViewModel>()
                .ForMember(x => x.ViewImage, opt => opt.MapFrom(x =>
                    string.IsNullOrEmpty(x.ImageUrl) ? "/pictures/default.jpg" : $"/images/400_{x.ImageUrl}"))
                .ForMember(x => x.ImageFile, opt => opt.Ignore())
                .ReverseMap();

            CreateMap<CategoryEntity, SelectItemViewModel>();
        }
    }
}
