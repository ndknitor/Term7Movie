using Term7MovieCore.Data;
using Term7MovieCore.Data.Dto;
using Term7MovieCore.Data.Exceptions;
using Term7MovieCore.Data.Request;
using Term7MovieCore.Data.Response;
using Term7MovieRepository.Repositories.Interfaces;
using Term7MovieService.Services.Interface;

namespace Term7MovieService.Services.Implement
{
    public class TicketTypeService : ITicketTypeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITicketTypeRepository ticketTypeRepository;

        public TicketTypeService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            ticketTypeRepository = _unitOfWork.TicketTypeRepository;
        }

        public async Task<ParentResultResponse> GetAllTicketTypeAsync(long managerId)
        {
            IEnumerable<TicketTypeDto> list = await ticketTypeRepository.GetAllTicketTypeByManagerIdAsync(managerId);

            return new ParentResultResponse
            {
                Result = list,
                Message = Constants.MESSAGE_SUCCESS
            };
        }

        public async Task<ParentResponse> CreateTicketType(TicketTypeCreateRequest request, long managerId)
        {
            int count = await ticketTypeRepository.CreateAsync(request, managerId);

            if (count == 0) throw new DbOperationException();

            return new ParentResponse
            {
                Message = Constants.MESSAGE_SUCCESS
            };
        }
    }
}
