using AutoMapper;
using SimbiosWebMVC.Areas.Admin.Models.User;
using SimbiosWebMVC.Data.Entities.Identity;

namespace SimbiosWebMVC.Areas.Admin.Mapper
{
    public class UserMapper : Profile
    {
        public UserMapper()
        {
            CreateMap<UserEntity, UserItemViewModel>()
                .ForMember(dest => dest.Roles, opt => opt.Ignore());
        }
    }
}
