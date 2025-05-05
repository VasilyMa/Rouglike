using AbilitySystem;
using UnityEngine;
using Client;
using System.Collections.Generic;
using System.Collections;

[CreateAssetMenu(fileName = "GameConfig", menuName = "Configs/GameConfig")]
public class GameConfig : Config
{
    public bool IS_TEST;

    public float StaminaRecoveryRate;

    public float StaminabarTimeAnimation;
    public int TierStep;
    public int MetaLevelStep;
    public float HealAfterRoom;
    public float DistanceToSpawn;
    public float PushForce = 2;
    public float TimeOfDeath;

    public GameObject PlayerGO;
    public float MaxHealthPlayer = 100;
    public float PlayerSpeed;
    public float GlobalDamageCD = 0.5f;
    public float SourceDamageCD = 0.5f;

    public float TimeAnimationHit = 0.6f;
    public float PartOneHit = 0.3f;
    public float PartTwoHit = 0.3f;

    public override IEnumerator Init()
    {
        yield return null;  
    }
}
