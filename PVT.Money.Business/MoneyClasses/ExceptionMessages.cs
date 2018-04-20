using PVT.Money.Business.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace PVT.Money.Business
{
    public static class ExceptionMessages
    {
        //AccountManager messages
        public static string SendComissionParamIsNull(Langs lang = Langs.ENG)
        {
            switch (lang)
            {
                case Langs.ENG:
                    return "It`s impossible to send null comission.";
                case Langs.RUS:
                    return "Невозможно отправить null комиссию.";
                default:
                    return "Unknown languege";
            }
        }

        public static string TransactionAccIsNull(Langs lang = Langs.ENG)
        {            
            switch(lang)
            {
                case Langs.ENG:
                    return "Impossible transaction. One of the accounts is null";
                case Langs.RUS:
                    return "Транзакция невозможна. Один из счетов null.";
                default:
                    return "Unknown languege";
            }
        }

        public static string TransactionInsufficientFunds(Langs lang = Langs.ENG)
        {
            switch (lang)
            {
                case Langs.ENG:
                    return "Insufficient funds on the account. It`s impossible to transact.";
                case Langs.RUS:
                    return "Недостаточно средств на счете. Невозможно произвести транзакцию.";
                default:
                    return "Unknown languege";
            }
        }

        public static string AccountManagerComissionNotPercent(Langs lang = Langs.ENG)
        {
            switch (lang)
            {
                case Langs.ENG:
                    return "Comission amount shouldn`t be less than zero or greater than one.";
                case Langs.RUS:
                    return "Размер комиссии не должен быть меньше нуля или больше единицы.";
                default:
                    return "Unknown languege";
            }
        }

        //Account messages
        public static string AccountMoneyArgumentIsNull(Langs lang = Langs.ENG)
        {
            switch (lang)
            {
                case Langs.ENG:
                    return "Money argument should be initialised before use.";
                case Langs.RUS:
                    return "Агрумент Money должен быть инициализирован перед использованием.";
                default:
                    return "Unknown languege";
            }
        }

        public static string AccountSubscribeNullParam(Langs lang = Langs.ENG)
        {
            switch (lang)
            {
                case Langs.ENG:
                    return "Can`t subscribe to commission of null AccountManager object.";
                case Langs.RUS:
                    return "Невозможно подписаться на комиссию объекта AccountManager равного null.";
                default:
                    return "Unknown languege";
            }
        }

        public static string AccountUnsubscribeNullParam(Langs lang = Langs.ENG)
        {
            switch (lang)
            {
                case Langs.ENG:
                    return "Can`t unsubscribe from commission of null AccountManager object.";
                case Langs.RUS:
                    return "Невозможно отказаться от подписки на комиссию объекта AccountManager равного null.";
                default:
                    return "Unknown languege";
            }
        }

        //ExchangeRates messages
        public static string ExchangeRatesNoFittedRate(Langs lang = Langs.ENG)
        {
            switch (lang)
            {
                case Langs.ENG:
                    return "There is no fitted exchange rate.";
                case Langs.RUS:
                    return "Не существует подходящего курса перевода.";
                default:
                    return "Unknown languege";
            }
        }

        //Money messages
        public static string MoneyAmountNotLessZero(Langs lang = Langs.ENG)
        {
            switch (lang)
            {
                case Langs.ENG:
                    return "Money amount shouldn`t be less than zero";
                case Langs.RUS:
                    return "Количество средств не должно быть меньше нуля.";
                default:
                    return "Unknown languege";
            }
        }

        public static string MoneyAdditionNullFail(Langs lang = Langs.ENG)
        {
            switch (lang)
            {
                case Langs.ENG:
                    return "The addition of null money object is impossible";
                case Langs.RUS:
                    return "Невозможно добавить money объект равный null.";
                default:
                    return "Unknown languege";
            }
        }

        public static string MoneySubtractionNullFail(Langs lang = Langs.ENG)
        {
            switch (lang)
            {
                case Langs.ENG:
                    return "The subtraction of null money object is impossible";
                case Langs.RUS:
                    return "Невозможно вычесть money объект равный null.";
                default:
                    return "Unknown languege";
            }
        }

        public static string MoneySubtractionLessThanSubtractedFail(Langs lang = Langs.ENG)
        {
            switch (lang)
            {
                case Langs.ENG:
                    return "Current amount of money is less than subtracted amount. It`s impossible to subtract.";
                case Langs.RUS:
                    return "Текущее количество средств меньше чем вычитаемое. Невозможно вычесть.";
                default:
                    return "Unknown languege";
            }
        }

        public static string MoneyComparisonWithNullFail(Langs lang = Langs.ENG)
        {
            switch (lang)
            {
                case Langs.ENG:
                    return "The comparsion with null money object is impossible";
                case Langs.RUS:
                    return "Сравнение с объектом равным null невозможно.";
                default:
                    return "Unknown languege";
            }
        }

        //Rate messages
        public static string RateNotLessZero(Langs lang = Langs.ENG)
        {
            switch (lang)
            {
                case Langs.ENG:
                    return "Rate should be greater than zero.";
                case Langs.RUS:
                    return "Обменный курс должен быть больше нуля";
                default:
                    return "Unknown languege";
            }
        }

        //CommissionAccountSerializer messages
        public static string CASerializerNullAccount(Langs lang = Langs.ENG)
        {
            switch (lang)
            {
                case Langs.ENG:
                    return "The serialization of null account object is impossible";
                case Langs.RUS:
                    return "Невозможно сериализовать account объект равный null.";
                default:
                    return "Unknown languege";
            }
        }

        public static string CASerializerFailPath(Langs lang = Langs.ENG)
        {
            switch (lang)
            {
                case Langs.ENG:
                    return "Wrong file path. It's impossible to deserialize.";
                case Langs.RUS:
                    return "Неверный путь к файлу. Невозможно десериализовать.";
                default:
                    return "Unknown languege";
            }
        }
    }
}
