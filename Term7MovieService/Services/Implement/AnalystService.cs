
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

        /* -------- START GETTING QUICK ANALYST ------ */
        private async Task<TicketQuanityDTO> GetQuickTicketVolumeForACompany(int companyid)
        {
            DateTime RightNow = DateTime.UtcNow;
            DateTime MondayThisWeek = HowManyDaysUntilMonday(RightNow);
            DateTime MondayPreviousWeek = BiteTheDustPreviousWeekMonday(RightNow);
            DateTime SundayPreviousWeek = BiteTheDustPreviousWeekSunday(RightNow);
            var quanity = await ticketRepository.GetQuickTicketQuanityInTwoWeek(companyid, 
                MondayThisWeek, MondayPreviousWeek, SundayPreviousWeek);
            return quanity;
        }
        /* -------- END GETTING QUICK ANALYST ------ */

        /// <summary>
        /// Below are function to convert right damn now into 3 different time so we can get a part of 
        /// the result in database :v
        /// </summary>
        /// <param name="InTheMeantime"></param>
        /// <returns></returns>
        /// <exception cref="NotSupportedException"></exception>
        private DateTime HowManyDaysUntilMonday(DateTime InTheMeantime)
        {
            int DayOfTheWeek = (int)InTheMeantime.DayOfWeek;
            switch (DayOfTheWeek)
            {
                case 0: return InTheMeantime.AddDays(-6).Date;
                case 1: return InTheMeantime.AddDays(1).Date; //so it wont block the constraint
                case 2: return InTheMeantime.AddDays(-1).Date;
                case 3: return InTheMeantime.AddDays(-2).Date;
                case 4: return InTheMeantime.AddDays(-3).Date;
                case 5: return InTheMeantime.AddDays(-4).Date;
                case 6: return InTheMeantime.AddDays(-5).Date;
                default: throw new NotSupportedException();
            }
        }
        private DateTime BiteTheDustPreviousWeekMonday(DateTime InTheMeantime)
        {
            int DayOfTheWeek = (int)InTheMeantime.DayOfWeek;
            switch (DayOfTheWeek)
            {
                case 0: return InTheMeantime.AddDays(-6 - 7).Date;
                case 1: return InTheMeantime.AddDays(0 - 7).Date; //so it wont block the constraint
                case 2: return InTheMeantime.AddDays(-1 - 7).Date;
                case 3: return InTheMeantime.AddDays(-2 - 7).Date;
                case 4: return InTheMeantime.AddDays(-3 - 7).Date;
                case 5: return InTheMeantime.AddDays(-4 - 7).Date;
                case 6: return InTheMeantime.AddDays(-5 - 7).Date;
                default: throw new NotSupportedException();
            }
        }
        private DateTime BiteTheDustPreviousWeekSunday(DateTime InTheMeantime)
        {
            int DayOfTheWeek = (int)InTheMeantime.DayOfWeek;
            switch (DayOfTheWeek)
            {
                case 0: return InTheMeantime.AddDays(0 - 7).Date;
                case 1: return InTheMeantime.AddDays(6 - 7).Date; //so it wont block the constraint
                case 2: return InTheMeantime.AddDays(5 - 7).Date;
                case 3: return InTheMeantime.AddDays(4 - 7).Date;
                case 4: return InTheMeantime.AddDays(3 - 7).Date;
                case 5: return InTheMeantime.AddDays(2 - 7).Date;
                case 6: return InTheMeantime.AddDays(1 - 7).Date;
                default: throw new NotSupportedException();
            }
        }


        /* --------- END PRIVATE FUNCTION ----------*/
    }
}
