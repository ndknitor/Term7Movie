namespace Term7MovieCore.Data.Response
{
    public class MomoPaymentCreateResponse
    {
        public string partnerCode { get; set; }
        public string RequestId { set; get; }
        public string OrderId { set; get; }
        public long Amount { set; get; }
        public long ResponseTime { set; get; }
        public string Message { set; get; }
        public int ResultCode { set; get; }
        public string PayUrl { set; get; }
        public string Deeplink { set; get; }
        public string QrCodeUrl { set; get; }
        public string DeeplinkMiniApp { set; get; }
    }
}
