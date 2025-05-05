using FMODUnity;

using UnityEngine;

[CreateAssetMenu(fileName = "SoundBoundle", menuName = "Sound/SoundBoundle")]
public class SoundBoundle : ScriptableObject, ISerializationCallbackReceiver
{
    public string KEY_ID;
    public EventReference EventReference;
    public EventReference Ambient;
    public EventReference PlaceholderSound;

    public void OnAfterDeserialize()
    {

    }

    public void OnBeforeSerialize()
    {
        if (this == null) return;

        if (!string.IsNullOrEmpty(name))
        {
            KEY_ID = name;
        }
    }
}