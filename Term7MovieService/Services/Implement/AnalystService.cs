
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

        public async Task<DashboardResponse> GetDashboardManager(long? managerid, string time)
        {
            if (managerid == null)
                throw new DbForbiddenException();
            if(time.Equals(Constants.WEEK))
            {
                var result = await GettingAnalystForTwoWeek(managerid.Value);
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
            else//eventually there will be another function for year calculation but we
                //dont have time so i will put the code below
                //like it was nothing :v
            {
                var result = await GettingAnalystForTwoMonth(managerid.Value);
                bool S1mple = IsItLastMonthYet(DateTime.UtcNow);
                return new DashboardResponse
                {
                    ShowtimeDashboard = result.Item1,
                    TicketSoldDashboard = result.Item2,
                    IncomeDashboard = result.Item3,
                    IsItStatistical = S1mple,
                    Message = Constants.MESSAGE_SUCCESS
                };
            }
        }

        public async Task<DashboardResponse> GetDashboardForAdmin(string time)
        {
            if(time.Equals(Constants.WEEK))
            {
                var result = await GettingAnalystForTwoWeek();
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
            else
            {
                var result = await GettingAnalystForTwoMonth();
                bool Analysable = IsItLastMonthYet(DateTime.UtcNow);
                return new DashboardResponse
                {
                    ShowtimeDashboard = result.Item1,
                    TicketSoldDashboard = result.Item2,
                    IncomeDashboard = result.Item3,
                    IsItStatistical = Analysable,
                    Message = Constants.MESSAGE_SUCCESS
                };
            }
        }

        public async Task<YearlyIncomeResponse> GetYearlyIncomeForManager(int year, long? managerid)
        {
            if (managerid == null)
                throw new DbForbiddenException();
            var result = await tranHisRepository.GetIncomeForAYear(year, managerid.Value);
            return new YearlyIncomeResponse
            {
                Result = result,
                HighestIncome = result.Max(a => a.Income),
                LowestIncome = result.Min(a => a.Income),
                Message = Constants.MESSAGE_SUCCESS
            };
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
        private async Task<Tuple<ShowtimeQuanityDTO, TicketSoldDTO, IncomeDTO>> GettingAnalystForTwoWeek(long managerid)
        {
            DateTime RightNow = DateTime.UtcNow;
            DateTime MondayThisWeek = HowManyDaysUntilMonday(RightNow);
            DateTime MondayPreviousWeek = BiteTheDustPreviousWeekMonday(RightNow);
            DateTime SundayPreviousWeek = BiteTheDustPreviousWeekSunday(RightNow);
            var showtimeQuanity = await showRepository.GetShowtimeQuanityInTwoRecentWeek(managerid
                , MondayThisWeek, MondayPreviousWeek, SundayPreviousWeek);
            var ticketSold = await tranHisRepository.GetTicketSoldInTwoRecentWeek(managerid
                , MondayThisWeek, MondayPreviousWeek, SundayPreviousWeek);
            var Income = await tranHisRepository.GetTicketStonkOrStinkInTwoRecentWeek(managerid
                , MondayThisWeek, MondayPreviousWeek, SundayPreviousWeek);
            //trả 404 nếu 1 trong những thứ trên có vấn đề hence
            return Tuple.Create(showtimeQuanity, ticketSold, Income);
        }
        private async Task<Tuple<ShowtimeQuanityDTO, TicketSoldDTO, IncomeDTO>> GettingAnalystForTwoWeek()
        {
            DateTime RightNow = DateTime.UtcNow;
            DateTime MondayThisWeek = HowManyDaysUntilMonday(RightNow);
            DateTime MondayPreviousWeek = BiteTheDustPreviousWeekMonday(RightNow);
            DateTime SundayPreviousWeek = BiteTheDustPreviousWeekSunday(RightNow);
            var showtimeQuanity = await showRepository.GetShowtimeQuanityInTwoRecentWeek(MondayThisWeek,
                MondayPreviousWeek, SundayPreviousWeek);
            var ticketSold = await tranHisRepository.GetTicketSoldInTwoRecentWeek(MondayThisWeek,
                MondayPreviousWeek, SundayPreviousWeek);
            var Income = await tranHisRepository.GetTicketStonkOrStinkInTwoRecentWeek(MondayThisWeek,
                MondayPreviousWeek, SundayPreviousWeek);
            //trả 404 nếu 1 trong những thứ trên có vấn đề hence
            return Tuple.Create(showtimeQuanity, ticketSold, Income);
        }
        private async Task<Tuple<ShowtimeQuanityDTO, TicketSoldDTO, IncomeDTO>> GettingAnalystForTwoMonth(long managerid)
        {
            DateTime RightNow = DateTime.UtcNow;
            DateTime FirstDateOfTheMonth = HowManyDaysUntilFirstMonth(RightNow);
            DateTime FirstDateOfPreviousMonth = FirstPreviousMonthDate(RightNow);
            DateTime LastDateOfPreviousMonth = LastPreviousMonthDate(RightNow);
            var showtimeQuanity = await showRepository.GetShowtimeQuanityInTwoRecentMonth(managerid
                , FirstDateOfTheMonth, FirstDateOfPreviousMonth, LastDateOfPreviousMonth);
            var ticketSold = await tranHisRepository.GetTicketSoldInTwoRecentMonth(managerid
                , FirstDateOfTheMonth, FirstDateOfPreviousMonth, LastDateOfPreviousMonth);
            var Income = await tranHisRepository.GetTicketStonkOrStinkInTwoRecentMonth(managerid
                , FirstDateOfTheMonth, FirstDateOfPreviousMonth, LastDateOfPreviousMonth);
            //trả 404 nếu 1 trong những thứ trên có vấn đề hence
            return Tuple.Create(showtimeQuanity, ticketSold, Income);
        }
        private async Task<Tuple<ShowtimeQuanityDTO, TicketSoldDTO, IncomeDTO>> GettingAnalystForTwoMonth()
        {
            DateTime RightNow = DateTime.UtcNow;
            DateTime FirstDateOfTheMonth = HowManyDaysUntilFirstMonth(RightNow);
            DateTime FirstDateOfPreviousMonth = FirstPreviousMonthDate(RightNow);
            DateTime LastDateOfPreviousMonth = LastPreviousMonthDate(RightNow);
            var showtimeQuanity = await showRepository.GetShowtimeQuanityInTwoRecentMonth(FirstDateOfTheMonth,
                FirstDateOfPreviousMonth, LastDateOfPreviousMonth);
            var ticketSold = await tranHisRepository.GetTicketSoldInTwoRecentMonth(FirstDateOfTheMonth,
                FirstDateOfPreviousMonth, LastDateOfPreviousMonth);
            var Income = await tranHisRepository.GetTicketStonkOrStinkInTwoRecentMonth(FirstDateOfTheMonth,
                FirstDateOfPreviousMonth, LastDateOfPreviousMonth);
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
            {//.adddays(1).addticks(-1) mean get the datetime of the last in that specific date
                case 0: return InTheMeantime.AddDays(0 - 7).AddDays(1).AddTicks(-1);
                case 1: return InTheMeantime.AddDays(6 - 7).AddDays(1).AddTicks(-1);
                case 2: return InTheMeantime.AddDays(5 - 7).AddDays(1).AddTicks(-1);
                case 3: return InTheMeantime.AddDays(4 - 7).AddDays(1).AddTicks(-1);
                case 4: return InTheMeantime.AddDays(3 - 7).AddDays(1).AddTicks(-1);
                case 5: return InTheMeantime.AddDays(2 - 7).AddDays(1).AddTicks(-1);
                case 6: return InTheMeantime.AddDays(1 - 7).AddDays(1).AddTicks(-1);
                default: throw new NotSupportedException();
            }
        }

        private DateTime HowManyDaysUntilFirstMonth(DateTime InTheMeantime)
        {
            if (InTheMeantime.Day == 1) return InTheMeantime.Date;
            return new DateTime(InTheMeantime.Year, InTheMeantime.Month, 1);
        }
        private DateTime FirstPreviousMonthDate(DateTime InTheMeantime)
        {
            if (InTheMeantime.Month == 1)
                return new DateTime(InTheMeantime.Year - 1, 12, 1);
            return new DateTime(InTheMeantime.Year, InTheMeantime.Month - 1, 1);
        }
        private DateTime LastPreviousMonthDate(DateTime InTheMeantime)
        {
            if (InTheMeantime.Month == 1)
                return new DateTime(InTheMeantime.Year - 1, 12, 31);
            DateTime firsttime = new DateTime(InTheMeantime.Year, InTheMeantime.Month - 1, 1);
            return firsttime.AddMonths(1).AddTicks(-1);
        }

        private bool IsItSundayYet(DateTime WhichDayIsIt)
        {
            return (int)WhichDayIsIt.DayOfWeek == 0 ? true : false;
        }

        private bool IsItLastMonthYet(DateTime WhichMonthIsIt)
        {
            //my brain is limitation
            switch(WhichMonthIsIt.Month)
            {
                case 1:
                    return WhichMonthIsIt.Day == 31;
                case 2:
                    return WhichMonthIsIt.Day == 28 || WhichMonthIsIt.Day == 29;
                case 3:
                    return WhichMonthIsIt.Day == 31;
                case 4:
                    return WhichMonthIsIt.Day == 30;
                case 5:
                    return WhichMonthIsIt.Day == 31;
                case 6:
                    return WhichMonthIsIt.Day == 30;
                case 7:
                    return WhichMonthIsIt.Day == 31;
                case 8:
                    return WhichMonthIsIt.Day == 31;
                case 9:
                    return WhichMonthIsIt.Day == 30;
                case 10:
                    return WhichMonthIsIt.Day == 31;
                case 11:
                    return WhichMonthIsIt.Day == 30;
                case 12:
                    return WhichMonthIsIt.Day == 31;
                default:
                    return false;
            }
        }
        /* ------------------------------------- END PRIVATE FUNCTION --------------------------------- */
    }
}
