using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Term7MovieCore.Data.Dto;
using Term7MovieCore.Data.Request;
using Term7MovieCore.Data.Response;

namespace Term7MovieService.Services.Interface
{
    public interface ITransactionService
    {
        TransactionCreateResponse CreateTransaction(TransactionCreateRequest request, UserDTO user);
        Task ProcessPaymentAsync(MomoIPNRequest ipn);
        Task<ParentResultResponse> GetTransactionByIdAsync(Guid transactionId);
        Task<ParentResponse> CheckPaymentStatus(Guid transactionId);
        Task<ParentResultResponse> GetAllTransactionAsync(TransactionFilterRequest request, long userId, string roleId);
    }
}
