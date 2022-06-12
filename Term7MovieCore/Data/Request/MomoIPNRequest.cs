
namespace Term7MovieCore.Data.Request
{
    public class MomoIPNRequest
    {
        public string PartnerCode { set; get; }
        public string OrderId { set; get; }
        public string RequestId { set; get; }
        public string Amount { set; get; }
        public string OrderInfo { set; get; }
        public string OrderType { set; get; }
        public long TransId { set; get; }
        public int ResultCode { set; get; }
        public string Message { set; get; }
        public string PayType { set; get; }
        public long ResponseTime { set; get; }
        public string ExtraData { set; get; }
        public string Signature { set; get; }

    }
}
