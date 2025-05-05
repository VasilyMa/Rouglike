using System.Collections.Generic;
using UnityEngine;
using static UICurrencyData;

namespace Client
{
    struct CurrencyComponent
    {

        public List<Currency> currencies;
        public Currency Favour;   ///dell - hardCode
        public Currency Effigy;   ///dell - hardCode
        public Currency SkillShard;   ///dell - hardCode
        // add your data here.
        public void AddCurrency(string name, int amount)
        {
            ///SaveModule.Save(); TODO rework save
        }

        public bool SpendCurrency(string name, int amount)
        {
            Currency currency = currencies.Find(c => c.Name == name);
            if (currency != null)
            {
                var value = currency.SpendAmount(amount);
                ///SaveModule.Save(); TODO rework save
                return value;
            }
            
            return false;
        }

        public int GetCurrencyAmount(string name)
        {
            Currency currency = currencies.Find(c => c.Name == name);
            if (currency != null)
            {
                return currency.Amount;
            }
            
            return 0;
        }

        public void DisplayCurrencies()
        {
            foreach (var currency in currencies)
            {
                
            }
        }

    }
}