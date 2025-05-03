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

            CreateMap<UserEntity, UserEditViewModel>()
                .ForMember(dest => dest.Roles, opt => opt.Ignore())
                .ForMember(dest => dest.ImageName, opt => opt
                .MapFrom(x => string.IsNullOrEmpty(x.Image) ? "/StandartImages/default.webp" : $"/images/200_{x.Image}"));

            CreateMap<UserEditViewModel, UserEntity>()
            .ForMember(dest => dest.Image, opt => opt.Ignore())
            .ForMember(dest => dest.UserRoles, opt => opt.Ignore())
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.SecurityStamp, opt => opt.Ignore())
            .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());

        }
    }
}
