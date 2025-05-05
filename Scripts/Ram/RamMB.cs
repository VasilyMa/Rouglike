using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(CapsuleCollider))]
public class RamMB : MonoBehaviour
{
    [HideInInspector] AbilityUnitMB UnitMB;
    void Start()
    {
        UnitMB= gameObject.transform.parent.gameObject.GetComponent<AbilityUnitMB>();
    }
    private void OnTriggerEnter(Collider other)
    {
        //UnitMB.CheckRam(other);
    }
}
