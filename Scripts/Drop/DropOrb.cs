using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DropOrb : IDrop
{
    public OrbResource Orb;
    public InteractiveObject DropItem(Vector3 DropPosition, Vector3 EndPosition)
    {
        var orbObject = PoolModule.Instance.GetFromPool<InteractiveOrbObject>(Orb.interactiveOrbObject, false);
        orbObject.SetValue(Orb);
        orbObject.ThisGameObject.transform.position = DropPosition;
        orbObject.ThisGameObject.transform.gameObject.SetActive(true);
        return orbObject;
    }
}
