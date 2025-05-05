using Client;

using System.Collections.Generic;
[System.Serializable]
public class PlayerData : IDatable
{
    public PlayerCurrency Currency = new PlayerCurrency();
    public PlayerWeapons Weapons = new PlayerWeapons();
    public PlayerAbilities Abilities = new PlayerAbilities();
    public RelicCollectionData RelicCollection = new RelicCollectionData();
    public PlayerStatusData PlayerStatus = new PlayerStatusData();
    public GlobalMap Map = new GlobalMap();

    public PlayerData()
    {

    }

    public string DATA_ID => "PlayerData_ID"; // name data

    public void ProcessUpdataData()
    {
        Currency = PlayerEntity.Instance.Currency;
        Weapons = PlayerEntity.Instance.Weapons;
        Abilities = PlayerEntity.Instance.Abilities;
        RelicCollection = PlayerEntity.Instance.RelicCollectionData;
        PlayerStatus = PlayerEntity.Instance.PlayerData;
        Map = PlayerEntity.Instance.Map;
        /*foreach (var weapon in Weapons.Weapons)
        {
            UnityEngine.
        }
        foreach (var ability in Abilities.Abilities)
        {
            UnityEngine.
        }
        
        foreach (var relic in RelicCollection.Relics)
        {
            UnityEngine.
        }*/

        // TODO: Add data update logic
    }

    public void Dispose()
    {
        // TODO: Remove data to default values, invokes where Clear Data
    }
}
