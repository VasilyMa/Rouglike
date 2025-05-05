using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using System.Collections;
using System.Linq;

[CreateAssetMenu(fileName = "SoundConfig", menuName = "Configs/SoundConfig")]
public class SoundConfig : Config
{
    [ReadOnlyInspector] public SoundBoundle[] SoundBoundle;

    private Dictionary<string, SoundBoundle> _dictionary = new Dictionary<string, SoundBoundle>();

    public List<EventReference> UISounds;
    public EventReference _lobbyAmbient;
    public EventReference _mainMenuAmbient;
    public override IEnumerator Init()
    {

        // ��������� ��� ������� �� ����� Resources
        SoundBoundle = Resources.LoadAll<SoundBoundle>("");

        if (SoundBoundle.Length == 0)
        {
            
            yield return null;
        }
        else
        {
            foreach (var boundle in SoundBoundle)
            {
                if (string.IsNullOrEmpty(boundle.KEY_ID)) boundle.KEY_ID = boundle.name;

                _dictionary.Add(boundle.KEY_ID, boundle);
            }
        }

        yield return null;
    }

    public SoundBoundle GetBoundle(string key)
    {
        return _dictionary[key];
    }
}
