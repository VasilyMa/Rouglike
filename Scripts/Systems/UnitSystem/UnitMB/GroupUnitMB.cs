
using UnityEngine;

public class GroupUnitMB : MonoBehaviour
{
    [HideInInspector] public UnitMB[] _unitMBs;
    private void Awake()
    {
        _unitMBs = gameObject.GetComponents<UnitMB>();
        foreach (var unitMB in _unitMBs)
        {
            unitMB.SetGroupUnitMB = this;
        }
    }
}
