using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EcsFeature", menuName = "Ecs/NewFeature")]
public class EcsFeature : ScriptableObject
{
    [Tooltip("Only Inited Ecs System")]
    public List<EcsGroupFeature> InitGroupSystems;
    [Tooltip("Init and run in update")]
    public List<EcsGroupFeature> RunGroupSystems;
    [Tooltip("Init and run in fixed update")]
    public List<EcsGroupFeature> FixedGroupSystems;
    [Tooltip("Init and run in late update")]
    public List<EcsGroupFeature> LateGroupSystems;
}
