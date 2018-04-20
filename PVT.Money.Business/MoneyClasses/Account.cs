using PVT.Money.Business.Enums;
using PVT.Money.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PVT.Money.Business
{
    [Serializable]
    public class Account
    {
        public string AccountID { get; protected set; }
        public Money Money { get; }

        public Account( AccountEntity accEntity, params AccountCreationOptions[] options)
        {
            if (accEntity == null)
                throw new ArgumentNullException(nameof(accEntity));

            if(!options.Any( o => o == AccountCreationOptions.CurrencyOnly))
            {                
                if(options.Any(o => o == AccountCreationOptions.ZeroAmount))
                    Money = new Money(0, accEntity.Currency.StrToCurrency());
                else
                    Money = new Money(accEntity.Amount, accEntity.Currency.StrToCurrency());

                if (options.Any(o => o == AccountCreationOptions.EmptyName))
                    AccountID = "";
                else
                    AccountID = accEntity.AccountName;
            }
            else
            {
                AccountID = "";
                Money = new Money(0, accEntity.Currency.StrToCurrency());
            }
        }

        public Account(string id, decimal nominal ,Currency curr)
        {
            AccountID = id;
            Money = new Money(nominal, curr);
        }

        public Account(string id, Currency curr)
        {
            AccountID = id;
            Money = new Money(0, curr);
        }

        public Account(decimal nominal, Currency curr)
        {
            Money = new Money(nominal, curr);
        }

        public Account(Currency curr)
        {
            Money = new Money(0, curr);
        }

        public void AdjuctExchangeRate(Currency currFrom, Currency currTo, decimal coefficient)
        {
            if(currFrom != currTo)
                Money.AdjustExchangeRate(currFrom,currTo,coefficient);
        }

        public decimal GetExchangeRate(Currency currFrom, Currency currTo)
        {
            if (currFrom == currTo)
                return 1;
            else
                return Money.GetExchangeRate(currFrom, currTo);
        }
    }
}
