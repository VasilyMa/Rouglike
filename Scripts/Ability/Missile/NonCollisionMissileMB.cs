using Leopotam.EcsLite;
using UnityEngine;

public class NonCollisionMissileMB : MissileMB
{
    public override void Invoke(EcsWorld world, int entity, int layerMask)
    {
        base.Invoke(world, entity, layerMask);
    }
}
