using AutoMapper;
using System.Threading.Tasks;
using Term7MovieCore.Data;
using Term7MovieCore.Data.Collections;
using Term7MovieCore.Data.Dto;
using Term7MovieCore.Data.Exceptions;
using Term7MovieCore.Data.Request;
using Term7MovieCore.Data.Response;
using Term7MovieCore.Data.Response.Theater;
using Term7MovieCore.Entities;
using Term7MovieRepository.Repositories.Interfaces;
using Term7MovieService.Services.Interface;

namespace Term7MovieService.Services.Implement
{
    public class TheaterService : ITheaterService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITheaterRepository theaterRepo;
        private readonly ICompanyRepository companyRepo;
        private readonly IMapper _mapper;
        private readonly ILocationService _locationService;

        public TheaterService(IUnitOfWork unitOfWork, IMapper mapper, ILocationService locationService)
        {
            _unitOfWork = unitOfWork;
            theaterRepo = _unitOfWork.TheaterRepository;
            companyRepo = _unitOfWork.CompanyRepository;
            _mapper = mapper;
            _locationService = locationService;
        }

        public async Task<TheaterListResponse> GetTheatersAsync(TheaterFilterRequest request, long userId, string role)
        {
            PagingList<TheaterDto> theaters;

            if (Constants.ROLE_MANAGER.Equals(role))
            {
                theaters = await theaterRepo.GetAllTheaterByManagerIdAsync(request, userId);
            } else
            {
                theaters = await theaterRepo.GetAllTheaterAsync(request);
            }
            
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

        public async Task<ParentResponse> UpdateTheaterDefaultPrice(long managerId, TheaterDefaultPriceUpdateRequest request)
        {
            await theaterRepo.UpdateDefaultPriceAsync(request, managerId);

            if (! await _unitOfWork.CompleteAsync())
            {
                throw new DbOperationException();
            }

            return new ParentResponse
            {
                Message = Constants.MESSAGE_SUCCESS
            };
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

        public async Task<TheaterNameResponse> GetTheaterNamesFromCompany(int companyId, long? managerid)
        {
            try
            {
                if (!managerid.HasValue)
                    throw new Exception("403");
                //khum phải manager
                long? ActualManager = await companyRepo.GetManagerIdFromCompanyId(companyId);
                if (ActualManager == null)
                    return new TheaterNameResponse { Message = "Can't find such company" };
                if (ActualManager.Value != managerid.Value)
                    throw new Exception("400");
                //thằng manager này đang ráng vào company thằng khác

                var rawData = await theaterRepo.GetAllTheaterByCompanyIdAsync(companyId);
                return new TheaterNameResponse { Message = Constants.MESSAGE_SUCCESS,
                                                    TheaterNames = rawData};
            }
            catch(Exception ex)
            {
                if (ex.Message == "EMPTYDATA")
                    return new TheaterNameResponse { Message = "There is no theater in this company." };
                if (ex.Message == "DBCONNECTION")
                    return new TheaterNameResponse { Message = "Database down" };
                throw new Exception(ex.Message);
            }
        }
    }
}
