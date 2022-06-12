using System.ComponentModel.DataAnnotations.Schema;

namespace Term7MovieCore.Entities
{
    [Table("PaymentRequests")]
    public class MomoPaymentCreateRequest
    {
        public long Id { set; get; }
        public string PartnerCode { set; get; }
        public string PartnerName { set; get; }
        public string RequestId { set; get; }
        public long Amount { set; get; }
        public string OrderId { set; get; }
        public string OrderInfo { set; get; }
        public string RedirectUrl { set; get; }
        public string IpnUrl { set; get; }
        public string RequestType { set; get; }
        public string ExtraData { set; get; }
        public string Items { set; get; }
        public string UserInfo { set; get; }
        public string Lang { set; get; }
        public string Signature { set; get; }
    }
}
