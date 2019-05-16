using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace BABurgess.XmlApiDbContext.Models
{
    [XmlRoot(DataType = "AccountModel", ElementName = "account", IsNullable = true, Namespace = "account")]
    public class AccountModel
    {
        private string userId;

        [XmlElement(ElementName = "user_id")]
        public string UserId
        {
            get { return userId; }
            set { userId = value; }
        }

        private string userName;

        [XmlElement(ElementName = "username")]
        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }

        private string passwordHash;

        [XmlElement(ElementName = "password_hash")]
        public string PasswordHash
        {
            get { return passwordHash; }
            set { passwordHash = value; }
        }

        [XmlElement(ElementName = "stocks")]
        public List<Stock> Stocks { get; set; }

        [XmlElement(ElementName = "transactions")]
        public List<Transaction> Transactions { get; set; }

        [XmlElement(ElementName = "available_balance")]
        public decimal AvailableBalance { get; set; }
    }
}