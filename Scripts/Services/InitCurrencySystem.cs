using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UICurrencyData;
using Statement;

namespace Client
{
    sealed class InitCurrencySystem : MainEcsSystem
    {
        readonly EcsWorldInject _world = default;
        readonly EcsSharedInject<GameState> _state = default;
        readonly EcsPoolInject<CurrencyComponent> _pool = default;

        public override MainEcsSystem Clone()
        {
            return new InitCurrencySystem();
        }

        public override void Init(IEcsSystems systems)
        {
            /*_pool.Value.Add(State.Instance.GetEntity("PlayerEntity")).currencies = PlayerEntity.Instance.Currency.Currencies; //TODO Vasya => currencies = Data.currencies;

            UICurrencyData uiCurrencyData = new UICurrencyData
            {
                Currency1Type = CurrencyType.Favour,
                Currency1Value = PlayerEntity.Instance.Currency.Currencies[0].Amount.ToString(),
                Currency2Type = CurrencyType.Effigy,
                Currency2Value = PlayerEntity.Instance.Currency.Currencies[1].Amount.ToString()
            };
            if (SceneManager.GetActiveScene().name != "TestScene")
            {
                UIManagerRitualist.GetUIManager.InGameCurrencies.SetCurrencies(uiCurrencyData);
            }*/
        }
    }
}

[Serializable]
public class Currency
{
    public string Name;
    private int _amount;
    public int Amount { get => _amount; }

    public Currency(string name, int amount)
    {
        Name = name;
        _amount = amount;
    }

    public void AddAmount(int amountToAdd)
    {
        _amount += amountToAdd;
        
    }

    public bool SpendAmount(int amountToSpend)
    {
        if (_amount >= amountToSpend)
        {
            _amount -= amountToSpend;
            return true;
        }
        return false; // no money
    } 
    public override string ToString()
    {
        return $"{Name}: {_amount}";
    }
}

