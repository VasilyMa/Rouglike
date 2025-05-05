using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

[CreateAssetMenu(fileName = "UIConfig", menuName = "Config/UI")]
public class UIConfig : Config
{
    public override IEnumerator Init()
    {
        yield return null;
    }

    public void Initialize()
    {
        if (EntryPoint.Instance.EnableUI)
        {
            var allUIManager = Resources.LoadAll<UIManagerRitualist>("");

            if (allUIManager.Length == 0) return;

            var selectedUIManager = allUIManager.FirstOrDefault();

            if (selectedUIManager != null)
            {
                var instance = Object.Instantiate(selectedUIManager);
                instance.BaseInit();
            }
        }
    }
}
