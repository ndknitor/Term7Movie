
using Newtonsoft.Json;
using StackExchange.Redis;
using Term7MovieCore.Data;
using Term7MovieCore.Data.Dto.Analyst;
using Term7MovieCore.Data.Exceptions;
using Term7MovieCore.Data.Response.Analyst;
using Term7MovieRepository.Cache.Interface;
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

        private readonly ICacheProvider cacheProvider;

        public AnalystService(IUnitOfWork unitOfWork, ICacheProvider cacheProvider)
        {
            _unitOfWork = unitOfWork;
            showRepository = _unitOfWork.ShowtimeRepository;
            //ticketRepository = _unitOfWork.TicketRepository;
            tranHisRepository = _unitOfWork.TransactionHistoryRepository;
            companyRepository = _unitOfWork.CompanyRepository;

            this.cacheProvider = cacheProvider;
        }

        public async Task<DashboardResponse> GetQuickAnalystDashboardForManager(long? managerid)
        {
            if (managerid == null)
                throw new DbForbiddenException();
            var result = await GettingAnalystForOneWeek(managerid.Value);
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
            var result = await cacheProvider.GetValueAsync<DashboardDTO>(Constants.REDIS_VALUE_ADMIN_DASHBOARD);
            bool Analysable = IsItSundayYet(DateTime.UtcNow);
            if (result != null)
            {
                return new DashboardResponse
                {
                    ShowtimeDashboard = result.Showtime,
                    TicketSoldDashboard = result.TicketSold,
                    IncomeDashboard = result.Income,
                    IsItStatistical = Analysable,
                    Message = Constants.MESSAGE_SUCCESS
                };
            }
            var DbResult = await GettingAnalystForOneWeek();

            return new DashboardResponse
            {
                ShowtimeDashboard = DbResult.Item1,
                TicketSoldDashboard = DbResult.Item2,
                IncomeDashboard = DbResult.Item3,
                IsItStatistical = Analysable,
                Message = Constants.MESSAGE_SUCCESS
            };
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
        private async Task<Tuple<ShowtimeQuanityDTO, TicketSoldDTO, IncomeDTO>> GettingAnalystForOneWeek(long managerid)
        {
            DateTime RightNow = DateTime.UtcNow;
            DateTime MondayThisWeek = HowManyDaysUntilMonday(RightNow);
            DateTime MondayPreviousWeek = BiteTheDustPreviousWeekMonday(RightNow);
            DateTime SundayPreviousWeek = BiteTheDustPreviousWeekSunday(RightNow);
            var showtimeQuanity = await showRepository.GetQuickShowtimeQuanityAsync(managerid
                , MondayThisWeek, MondayPreviousWeek, SundayPreviousWeek);
            var ticketSold = await tranHisRepository.GetQuickTicketSoldInTwoRecentWeekAsync(managerid
                , MondayThisWeek, MondayPreviousWeek, SundayPreviousWeek);
            var Income = await tranHisRepository.GetQuickTicketStonkOrStinkInTwoRecentWeekAsync(managerid
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
            var showtimeQuanity = await showRepository.GetQuickShowtimeQuanityAsync(MondayThisWeek, 
                MondayPreviousWeek, SundayPreviousWeek);
            var ticketSold = await tranHisRepository.GetQuickTicketSoldInTwoRecentWeekAsync(MondayThisWeek, 
                MondayPreviousWeek, SundayPreviousWeek);
            var Income = await tranHisRepository.GetQuickTicketStonkOrStinkInTwoRecentWeekAsync(MondayThisWeek, 
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

        private void SetAnalystOnRedis(DashboardDTO adminvalue
            , DashboardDTO managervalue, long? ManagerId)
        {
            //easy admin caching
            cacheProvider.SetValue(Constants.REDIS_VALUE_ADMIN_DASHBOARD, adminvalue);
            //so damn complex manager caching :(
            HashEntry[] rawdataCollection = new HashEntry[managervalue.Count];
            int i = 0;
            foreach(var item in managervalue)
            {
                HashEntry rawdata = new HashEntry(item.Key, JsonConvert.SerializeObject(item.Value));
                rawdataCollection[i] = rawdata;
                i++;
            }
            cacheProvider.PutHashMapAsync(Constants.REDIS_KEY_DASHBOARD, rawdataCollection);
        }
        /* ------------------------------------- END PRIVATE FUNCTION --------------------------------- */
    }
}
