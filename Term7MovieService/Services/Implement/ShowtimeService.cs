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

        public async Task<ShowtimeListResponse> GetShowtimesByTheaterIdAsync(ShowtimeFilterRequest request)
        {
            PagingList<ShowtimeDto> list = await showtimeRepo.GetShowtimesByTheaterIdAsync(request);
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

            return new ShowtimeResponse
            {
                Message = Constants.MESSAGE_SUCCESS,
                Showtime = showtime
            };
        }

        public async Task<ParentResponse> CreateShowtimeAsync(ShowtimeCreateRequest request, long managerId)
        {

            Showtime showtime = _mapper.Map<Showtime>(request);

            bool notOverlap = await showtimeRepo.IsShowtimeNotOverlap(request);

            if (!notOverlap) throw new BadRequestException(ErrorMessageConstants.ERROR_MESSAGE_SHOWTIME_OVERLAP);

            await showtimeRepo.CreateShowtimeAsync(showtime);

            if (!_unitOfWork.HasChange()) throw new DbOperationException();

            await _unitOfWork.CompleteAsync();

            return new ParentResponse { Message = Constants.MESSAGE_SUCCESS };
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
