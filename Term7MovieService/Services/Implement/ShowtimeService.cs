using AutoMapper;
using Term7MovieCore.Data;
using Term7MovieCore.Data.Collections;
using Term7MovieCore.Data.Dto;
using Term7MovieCore.Data.Exceptions;
using Term7MovieCore.Data.Request;
using Term7MovieCore.Data.Response;
using Term7MovieCore.Entities;
using Term7MovieRepository.Repositories.Interfaces;
using Term7MovieService.Services.Interface;

namespace Term7MovieService.Services.Implement
{
    public class ShowtimeService : IShowtimeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IShowtimeRepository showtimeRepo;
        private readonly IMapper _mapper;

        public ShowtimeService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            showtimeRepo = _unitOfWork.ShowtimeRepository;
            _mapper = mapper;
        }

        public async Task<ShowtimeListResponse> GetShowtimesAsync(ShowtimeFilterRequest request, long userId)
        {
            PagingList<ShowtimeDto> list;

            if (request.TheaterId != null)
            {
                list = await showtimeRepo.GetShowtimesByTheaterIdAsync(request);
            } else
            {
                list = await showtimeRepo.GetShowtimesByManagerIdAsync(request, userId);
            }

            return new ShowtimeListResponse
            {
                Message = Constants.MESSAGE_SUCCESS,
                Showtimes = list
            };
        }

        public async Task<ShowtimeResponse> GetShowtimeByIdAsync(long id)
        {
            ShowtimeDto showtime = await showtimeRepo.GetShowtimeByIdAsync(id);

            if (showtime == null) throw new DbNotFoundException();

            var seats = await _unitOfWork.SeatRepository.GetRoomSeats(showtime.RoomId);

            showtime.Room.SeatDtos = seats;

            return new ShowtimeResponse
            {
                Message = Constants.MESSAGE_SUCCESS,
                Showtime = showtime
            };
        }

        public async Task<ShowtimeCreateResponse> CreateShowtimeAsync(ShowtimeCreateRequest request, long managerId)
        {

            Showtime showtime = _mapper.Map<Showtime>(request);

            bool notOverlap = await showtimeRepo.IsShowtimeNotOverlap(request);

            if (!notOverlap) throw new BadRequestException(ErrorMessageConstants.ERROR_MESSAGE_SHOWTIME_OVERLAP);

            long scopeIdentity = await showtimeRepo.CreateShowtimeAsync(showtime);

            if (scopeIdentity == 0) throw new DbOperationException();

            return new ShowtimeCreateResponse {
                Message = Constants.MESSAGE_SUCCESS,
                CreatedShowtimeId = scopeIdentity
            };
        }

        public async Task<ParentResponse> UpdateShowtimeAsync(ShowtimeUpdateRequest request)
        {
            Showtime showtime = _mapper.Map<Showtime>(request);

            await showtimeRepo.UpdateShowtimeAsync(showtime);

            if (_unitOfWork.HasChange())
            {
                await _unitOfWork.CompleteAsync();
                return new ParentResponse { Message = Constants.MESSAGE_SUCCESS };
            }

            throw new DbOperationException();
        }
    }
}
