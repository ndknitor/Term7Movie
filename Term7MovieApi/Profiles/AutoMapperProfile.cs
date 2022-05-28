using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Term7MovieCore.Data.Dto;
using Term7MovieCore.Entities;

namespace Term7MovieApi.Profiles
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<UserInfo, User>()
                .BeforeMap((src, des) => des.EmailConfirmed = true)
                .ForMember(
                    des => des.FullName,
                    option => option.MapFrom(src => src.DisplayName)
                )
                .ForMember(
                    des => des.PictureUrl,
                    option => option.MapFrom(src => src.PhotoUrl)
                )
                .ForMember(
                    des => des.UserName,
                    option => option.MapFrom(src => src.Email)
                );
        }          
    }
}
