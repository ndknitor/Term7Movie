using AutoMapper;
using Term7MovieCore.Data.Dto;
using Term7MovieCore.Data.Request;
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

            CreateMap<RoomCreateRequest, Room>()
                .BeforeMap((src, des) => des.Status = true);

            CreateMap<RoomUpdateRequest, Room>();

            CreateMap<SeatCreateRequest, Seat>()
                .BeforeMap((src, des) => des.Status = true);

            CreateMap<SeatUpdateRequest, Seat>();

            CreateMap<SeatTypeUpdateRequest, SeatType>();

            CreateMap<TheaterCreateRequest, Theater>()
                .BeforeMap((src, des) => des.Status = true);

            CreateMap<TheaterUpdateRequest, Theater>();

            CreateMap<ShowtimeCreateRequest, Showtime>();

            CreateMap<ShowtimeUpdateRequest, Showtime>();
        }          
    }
}
