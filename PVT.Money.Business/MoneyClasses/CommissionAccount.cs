using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using PVT.Money.Business.MoneyClasses;
using PVT.Money.Business.MoneyClasses.ForGlobalRates;
using System.Threading.Tasks;
using PVT.Money.Data;
using PVT.Money.Business.Enums;

namespace PVT.Money.Business
{
    [Serializable]
    public class CommissionAccount : Account, IXmlSerializable
    {
        public CommissionAccount(AccountEntity accEntity, params AccountCreationOptions[] options) : base(accEntity, options)
        {

        }

        public CommissionAccount(string id, decimal nominal, Currency curr) : base(id,nominal,curr)
        {

        }

        public CommissionAccount(decimal nominal, Currency curr) : base(nominal, curr)
        {

        }

        public CommissionAccount(Currency curr) : base(curr)
        {

        }

        public CommissionAccount(string id, Currency curr) : base(id,curr)
        {

        }

        public CommissionAccount(): base("default",0m,Currency.USD)
        {

        }

        private void CommissionGet(object obj, Money m)
        {
            IGlobalRates gr = new GlobalRates();
            Task<decimal> task = gr.GetRateAsync(m.Curr, Money.Curr);
            Money.AdjustExchangeRate(m.Curr, Money.Curr, task.Result);
            Money.AddMoney(m);
            m.SubtractMoney(m);
        }

        public void SubscribeToCommission(AccountManager accountManager)
        {
            if (accountManager == null)
                throw new ArgumentNullException(nameof(accountManager), ExceptionMessages.AccountSubscribeNullParam());
            accountManager.CommissionSended += CommissionGet;
        }

        public void SubscribeToCommission(IAccountManager accountManager)
        {
            if (accountManager == null)
                throw new ArgumentNullException(nameof(accountManager), ExceptionMessages.AccountSubscribeNullParam());
            accountManager.CommissionSended += CommissionGet;
        }

        public void UnsubscribeFromCommission(AccountManager accountManager)
        {
            if (accountManager == null)
                throw new ArgumentNullException(nameof(accountManager), ExceptionMessages.AccountUnsubscribeNullParam());
            accountManager.CommissionSended -= CommissionGet;
        }

        public void UnsubscribeFromCommission(IAccountManager accountManager)
        {
            if (accountManager == null)
                throw new ArgumentNullException(nameof(accountManager), ExceptionMessages.AccountUnsubscribeNullParam());
            accountManager.CommissionSended -= CommissionGet;
        }

        public override string ToString()
        {
            return AccountID;
        }

        public XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            decimal nominal;
            Currency currency;
            
            AccountID = reader.GetAttribute("AccountID", string.Empty);
            reader.ReadStartElement();
            nominal = reader.ReadElementContentAsDecimal("Amount", string.Empty);
            currency = (reader.ReadElementContentAsString("Currency", string.Empty)).StrToCurrency();

            Money.SubtractMoney(Money);
            Money.Convert(currency);
            Money.AddMoney(new Money(nominal, currency));
        }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteStartAttribute("AccountID");
            writer.WriteValue(AccountID);
            writer.WriteStartElement("Amount");
            writer.WriteValue(Money.Amount);
            writer.WriteEndElement();
            writer.WriteStartElement("Currency");
            writer.WriteValue(Money.Curr.CurrToString());
            writer.WriteEndElement();
        }
    }
}
