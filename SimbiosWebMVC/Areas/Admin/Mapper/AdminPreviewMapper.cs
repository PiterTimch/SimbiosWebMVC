using AutoMapper;
using SimbiosWebMVC.Areas.Admin.Models.ViewComponentModels;
using SimbiosWebMVC.Data.Entities.Identity;

namespace SimbiosWebMVC.Areas.Admin.Mapper
{
    public class AdminPreviewMapper : Profile
    {
        public AdminPreviewMapper()
        {
            CreateMap<UserEntity, AdminPreviewModel>()
                .ForMember(x => x.FullName, opt => opt.MapFrom(x => $"{x.LastName} {x.FirstName}"));
        }
    }
}
