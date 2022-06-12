using Term7MovieCore.Data.Dto;
using Term7MovieCore.Data.Request;
using Term7MovieCore.Data.Response;
using Term7MovieCore.Entities;

namespace Term7MovieService.Services.Interface
{
    public interface IPaymentService
    {
        Task<MomoPaymentCreateResponse> CreateMomoPaymentRequestAynsc(Transaction transaction, UserDTO user);
        Task ProcessMomoIPNRequestAsync(MomoIPNRequest ipn);
    }
}
