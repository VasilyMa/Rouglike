using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropHeal : IDrop
{
    public float Amount;
    [SerializeReference] public InteractiveHealObject InteractiveHealObject;
    public InteractiveObject DropItem(Vector3 DropPosition, Vector3 EndPosition)
    {
        var healObject = PoolModule.Instance.GetFromPool<InteractiveHealObject>(InteractiveHealObject, false);
        healObject.ThisGameObject.transform.position = DropPosition;
        healObject.ThisGameObject.transform.gameObject.SetActive(true);
        healObject.Amount = Amount;
        healObject.Invoke(healObject.transform.parent);
        return healObject;
    }
}
