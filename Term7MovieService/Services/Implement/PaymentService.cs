using AutoMapper;
using Microsoft.Extensions.Options;
using System.Net.Mime;
using System.Text;
using Term7MovieCore.Data;
using Term7MovieCore.Data.Dto;
using Term7MovieCore.Data.Enum;
using Term7MovieCore.Data.Options;
using Term7MovieCore.Data.Request;
using Term7MovieCore.Data.Response;
using Term7MovieCore.Entities;
using Term7MovieCore.Extensions;
using Term7MovieRepository.Repositories.Interfaces;
using Term7MovieService.Services.Interface;

namespace Term7MovieService.Services.Implement
{
    public class PaymentService : IPaymentService
    {
        private readonly MomoOption momoOption;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IPaymentRequestRepository paymentRequestRepo;

        private const string REQUEST_TYPE = "captureWallet";
        private const string PARTNER_NAME = "F Cinema";

        private const int MOMO_RESULT_CODE_SUCCESS = 0;

        public PaymentService(IOptions<MomoOption> option, IUnitOfWork unitOfWork, IMapper mapper)
        {
            momoOption = option.Value;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            paymentRequestRepo = _unitOfWork.PaymentRequestRepository;
        }

        public MomoPaymentCreateResponse CreateMomoPaymentRequest(TransactionDto transaction, UserDTO user)
        {
            MomoPaymentCreateResponse response = null;
            
            using (var client = new HttpClient())
            {
                MomoPaymentCreateRequest request = new MomoPaymentCreateRequest
                {
                    Amount = Convert.ToInt64(transaction.Total),
                    ExtraData = "",
                    IpnUrl = momoOption.IpnUrl,
                    Items = MapItemFromTicket(transaction).ToJson(),
                    Lang = Constants.LANG_EN,
                    OrderId = transaction.Id.ToString(),
                    OrderInfo = transaction.Id + "" + transaction.PurchasedDate,
                    PartnerCode = momoOption.PartnerCode,
                    PartnerName = PARTNER_NAME,
                    RedirectUrl = momoOption.RedirectUrl,
                    RequestId = transaction.Id.ToString(),
                    RequestType = REQUEST_TYPE,
                    UserInfo = _mapper.Map<MomoUserInfoModel>(user).ToJson()
                };

                request.Signature = CreateMomoSignature(request);

                StringContent content = new StringContent(request.ToJson(), Encoding.UTF8, MediaTypeNames.Application.Json);

                var postRequest = new HttpRequestMessage(HttpMethod.Post, momoOption.PayUrl);

                var res = client.Send(postRequest);

                if (res.IsSuccessStatusCode)
                {
                    _unitOfWork.PaymentRequestRepository.InsertPaymentRequest(request);

                    using(var reader = new StreamReader(res.Content.ReadAsStream()))
                    {
                        var message = reader.ReadToEnd();

                        response = message.ToObject<MomoPaymentCreateResponse>();
                    }
                }
            }

            return response;
        }

        public async Task ProcessMomoIPNRequestAsync(MomoIPNRequest ipn)
        {
            if (ipn.ResultCode != MOMO_RESULT_CODE_SUCCESS) return;

            MomoPaymentCreateRequest requestReceived = _mapper.Map<MomoPaymentCreateRequest>(ipn);
            requestReceived.IpnUrl = momoOption.IpnUrl;
            requestReceived.RedirectUrl = momoOption.RedirectUrl;
            requestReceived.RequestType = REQUEST_TYPE;

            if (CreateMomoSignature(requestReceived) == ipn.Signature)
            {
                MomoPaymentCreateRequest dbRequest = await paymentRequestRepo.GetPaymentRequestByOrderIdAsync(ipn.OrderId);

                if (!ipn.Signature.Equals(CreateMomoSignature(dbRequest))) return;

                IEnumerable<long> idList = dbRequest.Items.ToObject<IEnumerable<MomoItemModel>>().Select(i => i.Id);

                Guid transactionId = Guid.Parse(dbRequest.OrderId);

                bool success = await _unitOfWork.TicketRepository.BuyTicket(transactionId, idList);
                
                if (success)
                {
                    await _unitOfWork.TransactionRepository.UpdateTransaction(transactionId, (int) TransactionStatusEnum.Successful, MOMO_RESULT_CODE_SUCCESS);
                    await _unitOfWork.TransactionHistoryRepository.CreateTransactionHistory(idList);
                    await _unitOfWork.CompleteAsync();
                }
            }
        }

        private IEnumerable<MomoItemModel> MapItemFromTicket(TransactionDto transaction)
        {
            return _mapper.Map<IEnumerable<MomoItemModel>>(transaction.Tickets);
        }

        private string CreateMomoSignature(MomoPaymentCreateRequest request)
        {
            if (request == null) return String.Empty;

            string signature = $"accessKey={momoOption.AccessKey}&amount={request.Amount}" +
                    $"&extraData={request.ExtraData}&ipnUrl={request.IpnUrl}" +
                    $"&orderId={request.OrderId}&orderInfo={request.OrderInfo}" +
                    $"&partnerCode={request.PartnerCode}&redirectUrl={request.RedirectUrl}" +
                    $"&requestId={request.RequestId}&requestType={request.RequestType}";

            return signature.ToHS256String(momoOption.SecretKey);
        }
    }
}
