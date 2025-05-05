using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "AIProfile", menuName = "Configs/AIProfile")]
public class AIProfile : ScriptableObject, ISerializationCallbackReceiver
{
    public string KEY_ID;

    public AnimationCurve AggressionScoreByHealth;
    public AnimationCurve AggressionScoreByDistance;
    public AnimationCurve CowardiceScoreByHealth;
    public AnimationCurve CowardiceScoreByDistance;
    public AnimationCurve DefenceScoreByHealth;

    [Tooltip("in seconds")]public float ReactionToAction = 0.25f;

    public bool IsBoss;

    [ShowIf("IsBoss", true)]public BossStage[] BossStages;  // maybe any class with different ability on any stage, stage changes automatic by health, how much stages => health / BossStages.Count

    public void OnAfterDeserialize()
    {

    }

    public void OnBeforeSerialize()
    {
        if (string.IsNullOrEmpty(name)) return;

        KEY_ID = name;
    }
}
[System.Serializable]
public class BossStage
{
    public AbilityBase[] Abilities;
}