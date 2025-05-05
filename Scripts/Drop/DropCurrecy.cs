using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropCurrecy : IDrop
{
    public CurrencyConfig CurrencyConfig;
    public AnimationCurve Amount;

    public InteractiveObject DropItem(Vector3 DropPosition, Vector3 EndPosition)
    {
        var valueAmount = Amount.Evaluate(UnityEngine.Random.Range(0f, 1f));
        var currencyView = CurrencyConfig.GetInteractionCurrency(valueAmount);
        var currencyObj = PoolModule.Instance.GetFromPool<InteractiveCurrencyObject>(currencyView, false);
        currencyObj.ThisGameObject.transform.position = DropPosition;
        currencyObj.Amount = (int)valueAmount;
        currencyObj.Invoke(currencyObj.transform.parent);
        currencyObj.ThisGameObject.transform.gameObject.SetActive(true);
        return currencyObj;
    }
}
