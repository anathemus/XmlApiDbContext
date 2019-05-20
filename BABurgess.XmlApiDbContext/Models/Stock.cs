using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace BABurgess.XmlApiDbContext.Models
{
    [XmlType(TypeName = "Stock")]
    public class Stock
    {
        private string userId;
        [XmlElement(ElementName = "UserId")]
        public string UserId
        {
            get { return userId; }
            set { userId = value; }
        }
        [XmlElement(ElementName = "Symbol")]
        public string Symbol { get; set; }
        [XmlElement(ElementName = "PurchasePrice")]
        public string PurchasePrice { get; set; }
        [XmlElement(ElementName = "Quantity")]
        public string Quantity { get; set; }
    }
}
