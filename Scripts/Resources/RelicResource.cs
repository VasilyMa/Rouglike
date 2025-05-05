using UnityEngine;

[CreateAssetMenu(fileName = "Relic", menuName = "Resources/NewRelic")]
[System.Serializable]
public class RelicResource : ScriptableObject, ISerializationCallbackReceiver
{
    [ReadOnlyInspector] public string KEY_ID;
    public Rarity Rarity;
    public string Name;
    public string Description;  
    public Sprite Icon;
    [SerializeReference] public SourceRelic SourceRelic;
    public void InvokeRelic()
    {
        PlayerEntity.Instance.RelicCollectionData.AddTemporaryRelic(new CurrentRelicData(KEY_ID));
        SourceRelic.InvokeRelic();
    }

    public void LoadRelic()
    {
        SourceRelic.InvokeRelic();
    }

    public void OnAfterDeserialize()
    {

    }

    public void OnBeforeSerialize()
    {
        if (!string.IsNullOrEmpty(name))
        {
            KEY_ID = name;
        }
    }
}
