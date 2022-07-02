using Term7MovieCore.Data.Dto;
using Term7MovieCore.Data.Request;
using Term7MovieCore.Data.Response;
using Term7MovieCore.Entities;

namespace Term7MovieService.Services.Interface
{
    public interface IPaymentService
    {
        MomoPaymentCreateResponse CreateMomoPaymentRequest(TransactionDto transaction, UserDTO user);
        Task ProcessMomoIPNRequestAsync(MomoIPNRequest ipn);
    }
}
