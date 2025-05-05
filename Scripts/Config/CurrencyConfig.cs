using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
[CreateAssetMenu(fileName = "CurrencyConfig", menuName = "Configs/CurrencyConfig")]
public class CurrencyConfig : ScriptableObject
{
    public List<CurrencyRange> currencyRange;
    public InteractiveCurrencyObject GetInteractionCurrency(float amount)
    {
        return currencyRange.LastOrDefault(x => x.MinValue < amount).Currency;
    }
}
[System.Serializable]
public class CurrencyRange
{
    public InteractiveCurrencyObject Currency;
    public float MinValue;
}