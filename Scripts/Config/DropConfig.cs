using Client;
using Leopotam.EcsLite;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "DropConfig", menuName = "Configs/DropConfig")]
public class DropConfig : ScriptableObject
{
    public AnimationCurve NumberOfThrows;
    [SerializeReference] public List<IDrop> GuaranteedDrop;
    public List<DropItem> ProbabilisticDrop;
    public List<IDrop> GetDropLoot(int CountItem = -1)
    {
        List<IDrop> dropLoot = new(GuaranteedDrop);
        List<DropItem> buffProbabilisticDrop = new(ProbabilisticDrop);
        int countOfThrows = CountItem <= -1 ? (int)NumberOfThrows.Evaluate(UnityEngine.Random.Range(0f, 1f)) : CountItem;
        for(int cast = 0; cast < countOfThrows; cast++)
        {
            float sum = 0;
            buffProbabilisticDrop.ForEach(item => sum += item.Weight);
            if (sum <= 0) continue;
            float randomIndex = UnityEngine.Random.Range(0f, sum);
            foreach(var itemLoot in buffProbabilisticDrop)
            {
                randomIndex -= itemLoot.Weight;
                if (randomIndex > 0) continue;
                if (itemLoot.dropItem is DropEmpty) break;
                dropLoot.Add(itemLoot.dropItem);
                if (itemLoot.isOnce) buffProbabilisticDrop.Remove(itemLoot);
                break;
            }
        }
        return dropLoot;
    }
}
