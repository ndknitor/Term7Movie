
using Term7MovieCore.Data.Dto.Analyst;
using Term7MovieCore.Data.Response.Analyst;
using Term7MovieRepository.Repositories.Interfaces;
using Term7MovieService.Services.Interface;

namespace Term7MovieService.Services.Implement
{
    public class AnalystService : IAnalystService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IShowtimeRepository showRepository;
        private readonly ITicketRepository ticketRepository;
        private readonly ITransactionHistoryRepository tranHisRepository;

        public AnalystService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            showRepository = _unitOfWork.ShowtimeRepository;
            ticketRepository = _unitOfWork.TicketRepository;
            tranHisRepository = _unitOfWork.TransactionHistoryRepository;
        }

        public Task<DashboardResponse> GetQuickAnalystForDashboard(int companyid)
        {
            throw new NotImplementedException();
        }

        /* --------- START PRIVATE FUNCTION ------- */
        private Task<DashboardDTO> GettingAnalystForOneWeek(int companyid)
        {
            return null;
        }
        /* --------- END PRIVATE FUNCTION ----------*/
    }
}
