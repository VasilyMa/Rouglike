using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "ObserverConfig", menuName = "Config/ObserverConfig")]
public class ObserverConfig : Config
{
    public PlayerObserver PlayerObserver;
    public AbilityObserver AbilityObserver;
    public BossObserver BossObserver;
    public EnemyObserver EnemyObserver;

    public override IEnumerator Init()
    {
        yield return null;
    }
}
