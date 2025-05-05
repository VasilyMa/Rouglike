using Client;
using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Statement;

public class StaticUnit : MonoBehaviour
{
    public GameObject TrainingManeken;
    public AIProfile AIprofile;
    void Start()
    {
        var _world = State.Instance.EcsRunHandler.World;
        var newEntity = _world.NewEntity();
        ref var unit = ref _world.GetPool<SpawnStaticUnitEvent>().Add(newEntity);
        unit.GameObject = TrainingManeken;

        unit.abilities = new();
        unit.AIprofile = AIprofile;
        unit.Health = 100000000000;
        unit.isImmortal = false;
        unit.OwnnerEntity = _world.PackEntity(State.Instance.GetEntity("PlayerEntity"));
        unit.position = transform.position;
        unit.Parent = transform;
    }
}
