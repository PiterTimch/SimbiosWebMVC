using AutoMapper;
using SimbiosWebMVC.Data.Entities;
using SimbiosWebMVC.Models.Seeder;

namespace SimbiosWebMVC.Mapper
{
    public class SeederMapper : Profile
    {
        public SeederMapper()
        {
            CreateMap<SeederCategoryModel, CategoryEntity>()
                .ForMember(x => x.ImageUrl, opt => opt.MapFrom(x => x.Image));
        }
    }
}
