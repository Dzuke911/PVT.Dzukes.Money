using NUnit.Framework;
using System;
using System.IO;

namespace PVT.Money.Business.Tests.MoneyTests
{
    [TestFixture]
    class CommissionAccountSerializerTests
    {
        [Test]
        public void SerializeBinaryFail()
        {
            //Assert
            var ex = Assert.Throws<ArgumentNullException>(() => CommissionAccountSerializer.SerializeBinary(null));
            Assert.AreEqual(ExceptionMessages.CASerializerNullAccount()+Environment.NewLine+ "Parameter name: account",ex.Message);
        }

        [Test]
        public void SerializeBinaryOK()
        {
            //Arrange
            CommissionAccount acc = new CommissionAccount("myCommission", 10.234m, Currency.CHF);

            //Act
            CommissionAccountSerializer.SerializeBinary(acc);

            //Assert
            Assert.True(File.Exists("myCommission.txt"));
        }

        [Test]
        public void DeserislizeBinaryFail()
        {
            //Assert
            var ex = Assert.Throws<ArgumentException>(() => CommissionAccountSerializer.DeserializeBinary("nonexistent file path here"));
            Assert.AreEqual(ExceptionMessages.CASerializerFailPath(), ex.Message);
        }

        [Test]
        public void DeserializeBinaryOK()
        {
            //Arrange
            CommissionAccount baseAcc = new CommissionAccount("myCommissionBin_test", 10.234m, Currency.CHF);

            //Act
            CommissionAccountSerializer.SerializeBinary(baseAcc);
            CommissionAccount newAcc = CommissionAccountSerializer.DeserializeBinary("myCommissionBin_test.txt");

            //Assert
            Assert.AreEqual(baseAcc.AccountID, newAcc.AccountID);
            Assert.AreEqual(baseAcc.Money.Amount, newAcc.Money.Amount);
            Assert.AreEqual(baseAcc.Money.Curr, newAcc.Money.Curr);
        }

        [Test]
        public void SerializeXML_Fail()
        {
            //Assert
            var ex = Assert.Throws<ArgumentNullException>(() => CommissionAccountSerializer.SerializeXML(null));
            Assert.AreEqual(ExceptionMessages.CASerializerNullAccount() + Environment.NewLine + "Parameter name: account", ex.Message);
        }

        [Test]
        public void SerializeXML_OK()
        {
            //Arrange
            CommissionAccount acc = new CommissionAccount("myCommissionXML_SerializeTest", 10.234m, Currency.CHF);

            //Act
            CommissionAccountSerializer.SerializeXML(acc);

            //Assert
            Assert.True(File.Exists("myCommissionXML_SerializeTest.xml"));
        }

        [Test]
        public void DeserislizeXML_Fail()
        {
            //Assert
            var ex = Assert.Throws<ArgumentException>(() => CommissionAccountSerializer.DeserializeXML("nonexistent file path here"));
            Assert.AreEqual(ExceptionMessages.CASerializerFailPath(), ex.Message);
        }

        [Test]
        public void DeserializeXML_OK()
        {
            //Arrange
            CommissionAccount baseAcc = new CommissionAccount("myCommissionXML_DeserializeTest", 10.234m, Currency.CHF);

            //Act
            CommissionAccountSerializer.SerializeXML(baseAcc);
            CommissionAccount newAcc = CommissionAccountSerializer.DeserializeXML("myCommissionXML_DeserializeTest.xml");

            //Assert
            Assert.AreEqual(baseAcc.AccountID, newAcc.AccountID);
            Assert.AreEqual(baseAcc.Money.Amount, newAcc.Money.Amount);
            Assert.AreEqual(baseAcc.Money.Curr, newAcc.Money.Curr);
        }
    }
}
