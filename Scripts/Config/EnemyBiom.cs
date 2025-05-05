using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyBiom", menuName = "EnemyBiom/New")]
public class EnemyBiom : ScriptableObject
{
    public RoomTier[] EnemyTiers;
    public RoomTier[] BossTiers;

    

    public RoomBase GetRandomRoom(int tier, bool IsBoss)
    {
        List<RoomBase> rooms = new List<RoomBase>();

        if (IsBoss)
        {
            rooms = BossTiers[Random.Range(0, BossTiers.Length)].Rooms;
        }
        else
        {
            var value = Mathf.Clamp(tier, 0, EnemyTiers.Length - 1);
            rooms = EnemyTiers[value].Rooms;
        }
        
        int randomIndex = Random.Range(0, rooms.Count);
        return rooms[randomIndex];
    }
}

[System.Serializable]
public class RoomTier
{
    public List<RoomBase> Rooms;
}