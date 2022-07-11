namespace Term7MovieCore.Data.Request
{
    public class MomoPaymentQueryRequest
    {
        public string PartnerCode { set; get; }
        public string RequestId { set; get; }
        public string OrderId { set; get; }
        public string Lang { set; get; } = "en";
        public string Signature { set; get; }
    }
}
