using Term7MovieCore.Entities;

namespace Term7MovieRepository.Repositories.Interfaces
{
    public interface IPaymentRequestRepository
    {
        Task<MomoPaymentCreateRequest> GetPaymentRequestByOrderIdAsync(string orderId);
        void InsertPaymentRequest(MomoPaymentCreateRequest request);
    }
}
