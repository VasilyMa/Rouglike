using System;
using System.Collections;
using System.Collections.Generic;

using UniRx;

using UnityEngine;

public class ShopObserver
{
    private ReactiveCollection<ReactiveProperty<WeaponPlayerData>> _weaponsData =
        new ReactiveCollection<ReactiveProperty<WeaponPlayerData>>();

    public IReadOnlyReactiveCollection<ReactiveProperty<WeaponPlayerData>> Weapons => _weaponsData;

    private ReactiveCollection<ReactiveProperty<AbilityPlayerData>> _abilitiesData =
        new ReactiveCollection<ReactiveProperty<AbilityPlayerData>>();

    public IReadOnlyReactiveCollection<ReactiveProperty<AbilityPlayerData>> Abilities => _abilitiesData;

    public ShopObserver()
    {
        // ������������� ��� �������������
    }

    // ����� ��� ���������� ������
    public void AddWeapon(WeaponPlayerData weapon)
    {
        _weaponsData.Add(new ReactiveProperty<WeaponPlayerData>(weapon));
    }

    // ����� ��� ��������� ����������
    public void AddWeaponsRange(IEnumerable<WeaponPlayerData> weapons)
    {
        foreach (var weapon in weapons)
        {
            _weaponsData.Add(new ReactiveProperty<WeaponPlayerData>(weapon));
        }
    }

    // ����� ��� ���������� ������
    public void AddAbility(AbilityPlayerData weapon)
    {
        _abilitiesData.Add(new ReactiveProperty<AbilityPlayerData>(weapon));
    }

    // ����� ��� ��������� ����������
    public void AddAbilityRange(IEnumerable<AbilityPlayerData> weapons)
    {
        foreach (var weapon in weapons)
        {
            _abilitiesData.Add(new ReactiveProperty<AbilityPlayerData>(weapon));
        }
    }

    public void SubscribeToWeaponsChanges(Action<ReactiveProperty<WeaponPlayerData>, bool> onChanged)
    {
        var addSubscription = _weaponsData.ObserveAdd()
            .Subscribe(addEvent => onChanged(addEvent.Value, true));

        var removeSubscription = _weaponsData.ObserveRemove()
            .Subscribe(removeEvent => onChanged(removeEvent.Value, false));
    }
    public void SubscribeToAbilitiesChanges(Action<ReactiveProperty<AbilityPlayerData>, bool> onChanged)
    {
        var addSubscription = _abilitiesData.ObserveAdd()
            .Subscribe(addEvent => onChanged(addEvent.Value, true));

        var removeSubscription = _abilitiesData.ObserveRemove()
            .Subscribe(removeEvent => onChanged(removeEvent.Value, false));
    }
}
