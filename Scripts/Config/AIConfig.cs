using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AIConfig", menuName = "Config/AI")]
public class AIConfig : Config
{
    public List<AIProfile> AIProfiles;

    private Dictionary<string, AIProfile> AIProfileDictionary;

    public override IEnumerator Init()
    {
        foreach (var aiProfile in AIProfiles) AIProfileDictionary.Add(aiProfile.KEY_ID, aiProfile); 

        yield return null;
    }
}
