using System.Xml.Serialization;

namespace BABurgess.XmlApiDbContext.Models
{
    [XmlType(TypeName = "Transaction")]
    public class Transaction
    {
        private string userId;
        [XmlElement(ElementName = "UserId")]
        public string UserId
        {
            get { return userId; }
            set { userId = value; }
        }
        [XmlElement(ElementName = "TransactionId")]
        public string TransactionId { get; set; }
        public enum TransactionType
        {
            AddFunds,
            WithdrawFunds,
            PurchaseStock,
            SellStock
        }
        [XmlElement(ElementName = "TransactionType")]
        public TransactionType TypeTransaction { get; set; }
        [XmlElement(ElementName = "Timestamp")]
        public string Timestamp { get; set; }
        [XmlElement(ElementName = "Symbol")]
        public string Symbol { get; set; }
        [XmlElement(ElementName = "StockPrice")]
        public string StockPrice { get; set; }
        [XmlElement(ElementName = "Quantity")]
        public string Quantity { get; set; }
        [XmlElement(ElementName = "FundsChange")]
        public string FundsChange { get; set; }
    }
}