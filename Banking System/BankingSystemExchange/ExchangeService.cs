using BankingSystem.ApplicationLogic.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace BankingSystemExchange
{
    public class ExchangeService
    {
        private const string BASE_URL = "https://min-api.cryptocompare.com";

        public List<CurrencyRate> GetConversionRate(Currency from, Currency[] to)
        {
            if (to == null || to.Length == 0)
            {
                throw new ArgumentException("to");
            }
            string url = $"{BASE_URL}/data/price?fsym={from}&tsyms={string.Join(",", to)}";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";

            List<CurrencyRate> returnValues = new List<CurrencyRate>();

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                using (Stream dataStream = response.GetResponseStream())
                {
                    using (StreamReader reader = new StreamReader(dataStream))
                    {
                        string content = reader.ReadToEnd();

                        Dictionary<string, decimal> conversionRates = JsonConvert.DeserializeObject<Dictionary<string, decimal>>(content);

                        foreach (KeyValuePair<string, decimal> rate in conversionRates)
                        {
                            returnValues.Add(new CurrencyRate
                            {
                                Rate = rate.Value,
                                Currency = (Currency)Enum.Parse(typeof(Currency), rate.Key)
                            });
                        }
                        return returnValues;
                    }

                }
            }
        }

        public decimal Convert(Currency fromCurrency, Currency toCurrency, decimal fromAmmount , decimal toAmmount ,
            UserBankAccounts fromAccount ,UserBankAccounts toAccount, decimal viewModelAmmount ,decimal viewModelRate)
        {
            
            if (fromAccount.AccountId == toAccount.AccountId)
            {
                
                throw new Exception("Selected currencies are the same.");
            }

            if (fromAccount.Amount < viewModelAmmount)
            {
                throw new Exception ("Insufficient funds.");
                
            }

            List<CurrencyRate> rates = GetConversionRate(fromCurrency, new Currency[] { toCurrency });

            viewModelRate = rates[0].Rate;

            fromAccount.Amount -= viewModelAmmount;
            toAccount.Amount += (viewModelAmmount * viewModelRate);

            return toAccount.Amount;
        }
    }
}
