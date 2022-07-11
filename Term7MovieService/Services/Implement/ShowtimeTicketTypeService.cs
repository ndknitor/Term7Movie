using Term7MovieCore.Data;
using Term7MovieCore.Data.Dto;
using Term7MovieCore.Data.Exceptions;
using Term7MovieCore.Data.Request;
using Term7MovieCore.Data.Response;
using Term7MovieRepository.Repositories.Interfaces;
using Term7MovieService.Services.Interface;

namespace Term7MovieService.Services.Implement
{
    public class ShowtimeTicketTypeService : IShowtimeTicketTypeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IShowtimeTicketTypeRepository showtimeTicketTypeRepository;

        public ShowtimeTicketTypeService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            showtimeTicketTypeRepository = _unitOfWork.ShowtimeTicketTypeRepository;
        }

        public async Task<ParentResultResponse> GetShowtimeTicketTypeByShowtimeIdAsync(long showtimeId)
        {
            IEnumerable<ShowtimeTicketTypeDto> list = await showtimeTicketTypeRepository.GetShowtimeTicketTypeByShowtimeId(showtimeId);

            return new ParentResultResponse
            {
                Message = Constants.MESSAGE_SUCCESS,
                Result = list
            };
        }

        public async Task<ParentResponse> CreateShowtimeTicketTypeAsync(ShowtimeTicketTypeCreateRequest request)
        {
            int count = await showtimeTicketTypeRepository.InsertShowtimeTicketType(request);

            if (count == 0) throw new BadRequestException("Showtime's start time is due or ticket type is duplicate");

            return new ParentResponse
            {
                Message = Constants.MESSAGE_SUCCESS
            };
        }
    }
}
