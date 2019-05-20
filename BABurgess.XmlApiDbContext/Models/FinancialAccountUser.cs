using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace BABurgess.XmlApiDbContext.Models
{
    [XmlRoot("Account", Namespace = "FinancialAccount", IsNullable = false)]
    public class FinancialAccountUser
    {
        private string userName;
        [XmlElement("UserName")]
        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }

        private string id;
        [XmlElement("Id")]
        public string Id
        {
            get { return id; }
            set { id = value; }
        }

        private string passwordHash;
        [XmlElement("PasswordHash")]
        public string PasswordHash
        {
            get { return passwordHash; }
            set { passwordHash = value; }
        }

        private string email;
        [XmlElement("Email")]
        public string Email
        {
            get { return email; }
            set { email = value; }
        }

        private string availableBalance;
        [XmlElement("AvailableBalance")]
        public string AvailableBalance
        {
            get { return availableBalance; }
            set { availableBalance = value; }
        }
        private List<Stock> stocks;
        [XmlArray(ElementName = "Stocks")]
        public List<Stock> Stocks
        {
            get { return stocks; }
            set { stocks = value; }
        }
        private List<Transaction> transactions;
        [XmlArray(ElementName = "Transactions")]
        public List<Transaction> Transactions
        {
            get { return transactions; }
            set { transactions = value; }
        }
    }
}
