using System.Collections;
using System.Collections.Generic;

using Leopotam.EcsLite;

using UnityEngine;

using Client;

[CreateAssetMenu(fileName = "EcsGroup", menuName = "Ecs/NewEcsGroup")]
public class EcsGroupFeature : ScriptableObject
{
    [SerializeReference] public List<MainEcsSystem> EcsSystems;
}
