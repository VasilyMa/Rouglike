using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "GeneralBiomsConfig", menuName = "Config/GeneralBiomsConfig")]
public class BiomConfig : Config
{
    public RoomGOConfig[] Bioms;
    public GameObject Lobby;

    public override IEnumerator Init()
    {
        yield return null;
    }
    public int GetRandomBiomType(int currentType)
    {
        int value = currentType;
        if(Bioms.Length == 1) return 0;
        while (value == currentType)
        {
            value = Random.Range(0, Bioms.Length);
        }
        return value;
    }
    public RoomGOConfig GetRoomGOConfigByBiomType(int index)
    {
        return Bioms[index];
    }
}
