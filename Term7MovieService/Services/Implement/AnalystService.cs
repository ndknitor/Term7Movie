
using Term7MovieCore.Data;
using Term7MovieCore.Data.Dto.Analyst;
using Term7MovieCore.Data.Exceptions;
using Term7MovieCore.Data.Response.Analyst;
using Term7MovieRepository.Repositories.Interfaces;
using Term7MovieService.Services.Interface;

namespace Term7MovieService.Services.Implement
{
    public class AnalystService : IAnalystService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IShowtimeRepository showRepository;
        //private readonly ITicketRepository ticketRepository;
        private readonly ICompanyRepository companyRepository;
        private readonly ITransactionHistoryRepository tranHisRepository;

        public AnalystService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            showRepository = _unitOfWork.ShowtimeRepository;
            //ticketRepository = _unitOfWork.TicketRepository;
            tranHisRepository = _unitOfWork.TransactionHistoryRepository;
            companyRepository = _unitOfWork.CompanyRepository;
        }

        public async Task<DashboardResponse> GetQuickAnalystDashboardForManager(int companyid, long? managerid)
        {
            if (managerid == null)
                throw new DbForbiddenException();
            if (await companyRepository.GetManagerIdFromCompanyId(companyid) != managerid)
                throw new DbForbiddenException();
            var result = await GettingAnalystForOneWeek(companyid);
            bool S1mple = IsItSundayYet(DateTime.UtcNow);
            return new DashboardResponse
            {
                ShowtimeDashboard = result.Item1,
                TicketSoldDashboard = result.Item2,
                IncomeDashboard = result.Item3,
                IsItStatistical = S1mple,
                Message = Constants.MESSAGE_SUCCESS
            };
        }

        public async Task<DashboardResponse> GetQuickAnalystDashboardForAdmin()
        {
            var result = await GettingAnalystForOneWeek();
            bool Analysable = IsItSundayYet(DateTime.UtcNow);
            return new DashboardResponse
            {
                ShowtimeDashboard = result.Item1,
                TicketSoldDashboard = result.Item2,
                IncomeDashboard = result.Item3,
                IsItStatistical = Analysable,
                Message = Constants.MESSAGE_SUCCESS
            };
        }

        public Task<YearlyIncomeResponse> GetYearlyIncomeForManager(int companyid, int year)
        {
            throw new NotImplementedException();
        }

        public async Task<YearlyIncomeResponse> GetYearlyIncomeForAdmin(int year)
        {
            var result = await tranHisRepository.GetIncomeForAYear(year);
            return new YearlyIncomeResponse
            {
                Result = result,
                HighestIncome = result.Max(a => a.Income),
                LowestIncome = result.Min(a => a.Income),
                Message = Constants.MESSAGE_SUCCESS
            };
        }




        /* ------------------------------------- START PRIVATE FUNCTION --------------------------------- */

        //______ START GETTING QUICK ANALYST
        private async Task<Tuple<ShowtimeQuanityDTO, TicketSoldDTO, IncomeDTO>> GettingAnalystForOneWeek(int companyid)
        {
            DateTime RightNow = DateTime.UtcNow;
            DateTime MondayThisWeek = HowManyDaysUntilMonday(RightNow);
            DateTime MondayPreviousWeek = BiteTheDustPreviousWeekMonday(RightNow);
            DateTime SundayPreviousWeek = BiteTheDustPreviousWeekSunday(RightNow);
            var showtimeQuanity = await showRepository.GetQuickShowtimeQuanity(companyid
                , MondayThisWeek, MondayPreviousWeek, SundayPreviousWeek);
            var ticketSold = await tranHisRepository.GetQuickTicketSoldInTwoRecentWeek(companyid
                , MondayThisWeek, MondayPreviousWeek, SundayPreviousWeek);
            var Income = await tranHisRepository.GetQuickTicketStonkOrStinkInTwoRecentWeek(companyid
                , MondayThisWeek, MondayPreviousWeek, SundayPreviousWeek);
            //trả 404 nếu 1 trong những thứ trên có vấn đề hence
            return Tuple.Create(showtimeQuanity, ticketSold, Income);
        }
        private async Task<Tuple<ShowtimeQuanityDTO, TicketSoldDTO, IncomeDTO>> GettingAnalystForOneWeek()
        {
            DateTime RightNow = DateTime.UtcNow;
            DateTime MondayThisWeek = HowManyDaysUntilMonday(RightNow);
            DateTime MondayPreviousWeek = BiteTheDustPreviousWeekMonday(RightNow);
            DateTime SundayPreviousWeek = BiteTheDustPreviousWeekSunday(RightNow);
            var showtimeQuanity = await showRepository.GetQuickShowtimeQuanity(MondayThisWeek, 
                MondayPreviousWeek, SundayPreviousWeek);
            var ticketSold = await tranHisRepository.GetQuickTicketSoldInTwoRecentWeek(MondayThisWeek, 
                MondayPreviousWeek, SundayPreviousWeek);
            var Income = await tranHisRepository.GetQuickTicketStonkOrStinkInTwoRecentWeek(MondayThisWeek, 
                MondayPreviousWeek, SundayPreviousWeek);
            //trả 404 nếu 1 trong những thứ trên có vấn đề hence
            return Tuple.Create(showtimeQuanity, ticketSold, Income);
        }
        //______ END GETTING QUICK ANALYST

        /// <summary>
        /// Below are function to convert right damn now into 3 different time so we can get a part of 
        /// the result in database :v There is some minor function to processing time logical
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
                case 1: return InTheMeantime.AddDays(0).Date;
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
                case 1: return InTheMeantime.AddDays(0 - 7).Date;
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
                case 1: return InTheMeantime.AddDays(6 - 7).Date;
                case 2: return InTheMeantime.AddDays(5 - 7).Date;
                case 3: return InTheMeantime.AddDays(4 - 7).Date;
                case 4: return InTheMeantime.AddDays(3 - 7).Date;
                case 5: return InTheMeantime.AddDays(2 - 7).Date;
                case 6: return InTheMeantime.AddDays(1 - 7).Date;
                default: throw new NotSupportedException();
            }
        }
        private bool IsItSundayYet(DateTime WhichDayIsIt)
        {
            return (int)WhichDayIsIt.DayOfWeek == 0 ? true : false;
        }
        /* ------------------------------------- END PRIVATE FUNCTION --------------------------------- */
    }
}
