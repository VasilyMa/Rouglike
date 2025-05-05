using System.Collections.Generic;

[System.Serializable]
public class SlotDataContainer : IDatable
{
    private List<SlotData> _slotDataList = new();
    private SlotData _currentSlotData;
    public SlotDataContainer()
    {
        _slotDataList = new List<SlotData>() { new SlotData(), new SlotData(), new SlotData() };

        _currentSlotData = _slotDataList[0];
    }

    public string DATA_ID => "SlotDataContainer_ID"; // name data

    public void ProcessUpdataData()
    {
        _currentSlotData.Currency = PlayerEntity.Instance.Currency;
        _currentSlotData.Abilities = PlayerEntity.Instance.Abilities;
        _currentSlotData.Weapons = PlayerEntity.Instance.Weapons;
        _currentSlotData.Map = PlayerEntity.Instance.Map;
        // TODO: Add data update logic
    }

    public void Dispose()
    {
        _slotDataList.Clear();
        _currentSlotData = null;
        // TODO: Remove data to default values, invokes where Clear Data
    }
    public void AddNewSlotData(SlotData slotData)
    {
        _slotDataList.Add(slotData);
    }
    public void SetCurrentSlotData(int index)
    {
        _currentSlotData = _slotDataList[index]; 
    }
    public SlotData GetCurrentData()
    {
        return _currentSlotData;
    }
}
[System.Serializable]
public class SlotData
{
    public GlobalMap Map;
    public PlayerWeapons Weapons;
    public PlayerAbilities Abilities;
    public PlayerCurrency Currency;
    public SlotData()
    {
        Map = new GlobalMap();
        Weapons = new PlayerWeapons();
        Abilities = new PlayerAbilities();
        Currency = new PlayerCurrency();
    }
}