using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class DropRelic : IDrop
{
    public RelicResource Relic;
    public InteractiveObject DropItem(Vector3 DropPosition, Vector3 EndPosition)
    {
        var relicView = ConfigModule.GetConfig<ViewConfig>().RelicView;
        var interactiveRelicObject = relicView.FirstOrDefault(x => x.Rarity == Relic.Rarity).InteractiveRelicObject;

        if (interactiveRelicObject is null) return null;

        if (Relic.SourceRelic == null) return null;

        var relicObject = PoolModule.Instance.GetFromPool<InteractiveRelicObject>(interactiveRelicObject, false);
        relicObject.ThisGameObject.transform.position = DropPosition;
        relicObject.ThisGameObject.transform.gameObject.SetActive(true);
        relicObject.SetData(Relic);
        return relicObject;
    }
}
