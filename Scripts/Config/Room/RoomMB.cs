using UnityEngine;
using Leopotam.EcsLite.Di;
using Client;
using System.Collections.Generic;
using Leopotam.EcsLite;
using Statement;

public class RoomMB : MonoBehaviour
{
    public GameObject CenterOfRoom;
    public RoomBase RoomConfig;
    public Transform[] SpawnPoints;
    public List<AttackRoomMB> roomAttackZones;
    public int CurrentNumberOfEnemies = 0;
    public void Init(RoomGOConfig biom,int tier, bool isBoss)
    {
        //TODO GENERATION TIER ENEMY
        //todo TIER
        RoomConfig = biom.EnemyBiom.GetRandomRoom(tier, isBoss);  
    }
    public bool CheckIndexAttackZone(int index, int attackEntity,EcsWorld world)
    {
        bool flag = false;
        foreach(var attackZone in roomAttackZones)
        {
            if(attackZone.IndexAttackZone == index)
            {
                attackZone.Invoke(world, attackEntity);
                flag = true;
            }
        }
        return flag;
    }
}
