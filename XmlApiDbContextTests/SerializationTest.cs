using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Serialization;
using BABurgess.XmlApiDbContext.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BABurgess.XmlApiDbContext.Helpers;

namespace XmlApiDbContextTests
{
    [TestClass]
    public class SerializationTest
    {
        public FinancialAccountUser SetupTestModel()
        {
            FinancialAccountUser model = new FinancialAccountUser();

            model.UserName = "BABurgess";
            model.Id = "12345";
            model.PasswordHash = GetHashString("reallybigpassword");
            model.AvailableBalance = "1000000000.00";
            model.Stocks = new List<Stock>();
            model.Transactions = new List<Transaction>();
            Stock stock = new Stock();
            stock.Symbol = "BZZ";
            stock.UserId = "12345";
            stock.PurchasePrice = "12.97";
            stock.Quantity = "10000";
            model.Stocks.Add(stock);
            Transaction transaction = new Transaction();
            transaction.Timestamp = DateTime.Now.ToString();
            transaction.UserId = "12345";
            transaction.TypeTransaction = Transaction.TransactionType.PurchaseStock;
            transaction.TransactionId = new Guid().ToString();
            transaction.Symbol = "BZZ";
            transaction.StockPrice = "12.97";
            transaction.Quantity = "10000";
            transaction.FundsChange = "-12970.00";
            model.Transactions.Add(transaction);

            return model;
        }
        [TestMethod]
        public void TestMethod1()
        {
            FinancialAccountUser model = SetupTestModel();
            XmlSerializer serializer = new XmlSerializer(typeof(FinancialAccountUser));
            string path = @"D:\Source\BABurgess.XmlApiDbContext\"
                    + @"BABurgess.XmlApiDbContext\App_Data\"
                    + "TestAccount.xml";
            Stream stream = new FileStream(path, FileMode.Create);

            serializer.Serialize(stream, model);

            Assert.IsTrue(File.Exists(path));
        }

        [TestMethod]
        public void TestMethod2()
        {
            string postBody = "";

            XmlSerializer serializer = new XmlSerializer(typeof(FinancialAccountUser));
            using (StringWriter writer = new StringWriter())
            {
                serializer.Serialize(writer, SetupTestModel());
                postBody = writer.ToString();
            }
            Console.WriteLine(postBody);
            Assert.IsNotNull(postBody);
        }

        [TestMethod]
        public void EncryptedStorageTest1()
        {
            FinancialAccountUser user = SetupTestModel();
            string postBody = String.Empty;

            XmlSerializer serializer = new XmlSerializer(typeof(FinancialAccountUser));
            using (StringWriter writer = new StringWriter())
            {
                serializer.Serialize(writer, user);
                postBody = writer.ToString();
            }
            string encryptedFile = StringEncryption.EncryptString(postBody, user.PasswordHash);
            string finalFile = StringEncryption.EmbedUserString(encryptedFile, user.PasswordHash);

            Console.WriteLine(finalFile);
            Assert.IsNotNull(finalFile);
        }

        public static byte[] GetHash(string inputString)
        {
            HashAlgorithm algorithm = SHA256.Create();
            return algorithm.ComputeHash(Encoding.UTF8.GetBytes(inputString));
        }

        public static string GetHashString(string inputString)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in GetHash(inputString))
                sb.Append(b.ToString("X2"));

            return sb.ToString();
        }
    }
}
