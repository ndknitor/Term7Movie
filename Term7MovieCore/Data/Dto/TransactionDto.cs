﻿namespace Term7MovieCore.Data.Dto
{
    public class TransactionDto
    {
        public Guid Id { set; get; }
        public long CustomerId { set; get; }
        public UserDTO Customer { set; get; }
        public DateTime PurchasedDate { set; get; }
        public DateTime ValidUntil { set; get; }
        public decimal Total { set; get; }
        public string QRCodeUrl { set; get; }
        public int StatusId { get; set; }
        public TransactionStatusDto Status { set; get; }
        public int? MomoResultCode { set; get; }
        public IEnumerable<TicketDto> Tickets { get; set; }
    }
}