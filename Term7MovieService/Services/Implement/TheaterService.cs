using AutoMapper;
using System.Threading.Tasks;
using Term7MovieCore.Data;
using Term7MovieCore.Data.Collections;
using Term7MovieCore.Data.Dto;
using Term7MovieCore.Data.Request;
using Term7MovieCore.Data.Response;
using Term7MovieCore.Entities;
using Term7MovieRepository.Repositories.Interfaces;
using Term7MovieService.Services.Interface;

namespace Term7MovieService.Services.Implement
{
    public class TheaterService : ITheaterService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITheaterRepository theaterRepo;
        private readonly IMapper _mapper;
        private readonly ILocationService _locationService;

        public TheaterService(IUnitOfWork unitOfWork, IMapper mapper, ILocationService locationService)
        {
            _unitOfWork = unitOfWork;
            theaterRepo = _unitOfWork.TheaterRepository;
            _mapper = mapper;
            _locationService = locationService;
        }

        public async Task<TheaterListResponse> GetTheatersAsync(TheaterFilterRequest request)
        {
            PagingList<TheaterDto> theaters = await theaterRepo.GetAllTheaterAsync(request);
            return new TheaterListResponse
            {
                Message = Constants.MESSAGE_SUCCESS,
                Theaters = theaters
            };
        }

        public async Task<TheaterResponse> GetTheaterByIdAsync(int id)
        {
            TheaterDto theater = await theaterRepo.GetTheaterByIdAsync(id);

            if (theater == null) return null;

            return new TheaterResponse
            {
                Theater = theater,
                Message = Constants.MESSAGE_SUCCESS
            };
        }

        public async Task<ParentResponse> CreateTheaterAsync(TheaterCreateRequest request, long managerId)
        {
            Theater theater = _mapper.Map<Theater>(request);
            
            theater.ManagerId = managerId;
            theater.CompanyId = await _unitOfWork.UserRepository.GetCompanyIdByManagerId(managerId);

            Location location = await GetLocationByAddressAsync(theater.Address);

            if (location != null)
            {
                theater.Latitude = location.Lat;
                theater.Longitude = location.Lng;
            }

            await theaterRepo.CreateTheaterAsync(theater);

            if (_unitOfWork.HasChange())
            {
                await _unitOfWork.CompleteAsync();
                return new ParentResponse { Message = Constants.MESSAGE_SUCCESS };
            }

            return null;
        }

        public async Task<ParentResponse> UpdateTheaterAsync(TheaterUpdateRequest request)
        {
            Theater theater = _mapper.Map<Theater>(request);

            Location location = await GetLocationByAddressAsync(theater.Address);

            if (location != null)
            {
                theater.Latitude = location.Lat;
                theater.Longitude = location.Lng;
            }

            await theaterRepo.UpdateTheaterAsync(theater);

            if (_unitOfWork.HasChange())
            {
                await _unitOfWork.CompleteAsync();
                return new ParentResponse { Message = Constants.MESSAGE_SUCCESS };
            }

            return null;
        }

        public async Task<ParentResponse> DeleteTheaterAsync(int id)
        {
            await theaterRepo.DeleteTheaterAsync(id);

            if (_unitOfWork.HasChange())
            {
                await _unitOfWork.CompleteAsync();
                return new ParentResponse { Message = Constants.MESSAGE_SUCCESS };
            }

            return null;
        }

        public async Task<Location> GetLocationByAddressAsync(string address)
        {
            Location  location = await _locationService.GetLocationByAddressAsync(address);

            if (location != null)
            {
                location.Lat = location.Lat.Length > 20 ? location.Lat.Substring(0, 20) : location.Lat;
                location.Lng = location.Lng.Length > 20 ? location.Lng.Substring(0, 20) : location.Lng;
            }
            return location;
        }
    }
}
