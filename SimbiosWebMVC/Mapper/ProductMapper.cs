using AutoMapper;
using SimbiosWebMVC.Data.Entities;
using SimbiosWebMVC.Models.Category;
using SimbiosWebMVC.Models.Product;

namespace SimbiosWebMVC.Mapper
{
    public class ProductMapper : Profile
    {
        public ProductMapper()
        {
            CreateMap<ProductEntity, ProductItemViewModel>()
                .ForMember(x => x.CategoryName, opt => opt.MapFrom(x => x.Category.Name))
                .ForMember(x => x.Images, opt => opt.MapFrom(x => x.ProductImages.Select(x => x.Name)));

            }
    }
}
