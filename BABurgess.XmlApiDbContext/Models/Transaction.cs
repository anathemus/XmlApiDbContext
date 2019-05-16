using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Serialization;

namespace BABurgess.XmlApiDbContext.Models
{
    public class Transaction
    {
        private string userId;
        [XmlElement(ElementName = "user_id")]
        public string UserId
        {
            get { return userId; }
            set { userId = value; }
        }
        [XmlElement(ElementName = "transaction_id")]
        public string TransactionId { get; set; }
        public enum TransactionType
        {
            AddFunds,
            WithdrawFunds,
            PurchaseStock,
            SellStock
        }
        [XmlElement(ElementName = "transaction_type")]
        public TransactionType TypeTransaction { get; set; }
        [XmlElement(ElementName = "timestamp")]
        public DateTime Timestamp { get; set; }
        [XmlElement(ElementName = "symbol")]
        public string Symbol { get; set; }
        [XmlElement(ElementName = "stock_price")]
        public decimal StockPrice { get; set; }
        [XmlElement(ElementName = "quantity")]
        public long Quantity { get; set; }
        [XmlElement(ElementName = "funds_change")]
        public decimal FundsChange { get; set; }
    }
}