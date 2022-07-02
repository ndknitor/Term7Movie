using Dapper;
using Microsoft.Data.SqlClient;
using Term7MovieCore.Data.Options;
using Term7MovieCore.Entities;
using Term7MovieRepository.Repositories.Interfaces;

namespace Term7MovieRepository.Repositories.Implement
{
    public class PaymentRequestRepository : IPaymentRequestRepository
    {
        private readonly AppDbContext _context;
        private readonly ConnectionOption _connectionOption;

        public PaymentRequestRepository(AppDbContext context, ConnectionOption connectionOption)
        {
            _context = context;
            _connectionOption = connectionOption;
        }

        public async Task<MomoPaymentCreateRequest> GetPaymentRequestByOrderIdAsync(string orderId)
        {
            MomoPaymentCreateRequest req = null;

            using(SqlConnection con = new SqlConnection(_connectionOption.FCinemaConnection))
            {
                string sql =
                    " SELECT * " +
                    " FROM PaymentRequests " +
                    " WHERE OrderId = @orderId ";
                object param = new { orderId };
                req = await con.QueryFirstOrDefaultAsync<MomoPaymentCreateRequest>(sql, param);
            }

            return req;
        }

        public void InsertPaymentRequest(MomoPaymentCreateRequest req)
        {
           _context.PaymentRequests.Add(req);
           _context.SaveChanges();
        }
    }
}
