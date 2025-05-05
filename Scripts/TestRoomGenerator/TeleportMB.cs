using Client;
using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportMB : MonoBehaviour
{
    public int TeleportIndex;
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<UnitMB>(out UnitMB unitMB))
        {
            //todo dispose ability
            // ref var abilityComp = ref _abilityContainer.Get(unitMB._entity);
            // //abilityComp.CurrentAbility.Dispose(unitMB._entity, GameState.Instance.World);
            // abilityComp.HardDispose();
            //TODO GENERATION
            // if(unitMB._entity == GameState.Instance.PlayerEntity)
            // {
            //     GameState.Instance.SetRoomPosition(gameObject.transform.position);
            //     GameState.Instance.CreateNextRoom(TeleportIndex);
            // }
        }
    }
}
