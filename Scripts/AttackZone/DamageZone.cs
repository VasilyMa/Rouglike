using Client;
using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Statement;
public class DamageZone : MonoBehaviour
{
    List<UnitMB> _effectedUnits = new List<UnitMB>();
    [HideInInspector] public int SenderEntity;
    [HideInInspector] public int Damage;

    public void Start()
    {
        Invoke("SetActive", 0.2f);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            if (other.TryGetComponent<UnitMB>(out var unit))
            {
                if (!_effectedUnits.Contains(unit))
                {
                    _effectedUnits.Add(unit);
                    
                    ref var takeDamageComp = ref State.Instance.EcsRunHandler.World.GetPool<TakeDamageComponent>().Add(State.Instance.EcsRunHandler.World.NewEntity());
                    takeDamageComp.Damage = Damage;
                    takeDamageComp.KillerEntity = State.Instance.EcsRunHandler.World.PackEntity(SenderEntity);
                    takeDamageComp.TargetEntity = State.Instance.EcsRunHandler.World.PackEntity(unit._entity);
                }
            }
        }
    }
    public void SetActive()
    {
        gameObject.SetActive(false);
        _effectedUnits.Clear();
    }
}
