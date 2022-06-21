using Term7MovieCore.Data;
using Term7MovieCore.Data.Dto;
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
    }
}
