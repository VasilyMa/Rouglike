using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Timeline;
[CreateAssetMenu(fileName = "DynamicMusicTemplate", menuName = "Configs/DynamicMusic")]
public class DynamicMusicTemplate : ScriptableObject
{
    public bool AutoCreateSignals;
    [ShowIf("AutoCreateSignals")]public int BPM;
    [ShowIf("AutoCreateSignals")]public float SegmentationDrumByTact;
    [Space(10)]
    public float StartTimeKey;
    public float MinCombatTimeKey;
    public float MaxCombatTimeKey;
    public float EndCombatTimeKey;
    public TimelineAsset TimelineAsset;
}
