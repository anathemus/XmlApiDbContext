using System.Xml.Serialization;

namespace BABurgess.XmlApiDbContext.Models
{
    public class Stock
    {
        private string userId;
        [XmlElement(ElementName = "user_id")]
        public string UserId
        {
            get { return userId; }
            set { userId = value; }
        }
        [XmlElement(ElementName = "symbol")]
        public string Symbol { get; set; }
        [XmlElement(ElementName = "purchase_price")]
        public decimal PurchasePrice { get; set; }
        [XmlElement(ElementName = "quantity")]
        public long Quantity { get; set; }
    }
}