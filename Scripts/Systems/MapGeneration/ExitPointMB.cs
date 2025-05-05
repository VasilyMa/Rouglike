using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using Client;
using Statement;
using UnityEngine.AI;

public class ExitPointMB : MonoBehaviour
{
    public LocalMapPoint CurrentLocalMapPoint;
    public Vector2Int ExitDirection;
    public GameObject LockExitGameObject;
    public NavMeshObstacle LockExitNavMeshObstacle;
    public GameObject LockEnterGameObject;
    public GameObject Wall;
    public bool IsExit;
    public BoxCollider _collider;
    public Material ExitMaterial;
    public VisualEffect ExitVisualEffect;
    private float _time = 0f;
    private float _maxTime = 3f;
    public void Awake()
    {
        try
        {
            if(this.transform.GetChild(0).gameObject.name.Contains("Gate"))
            {
                Wall = this.transform.GetChild(0).gameObject;
            }
        }
        catch
        {

        }
        
    }
    public void Init(bool isExit)
    {
        //ExitDirection = direction;
        
        ExitMaterial = LockExitGameObject.GetComponent<Renderer>().material;
        ExitVisualEffect = LockExitGameObject.GetComponent<VisualEffect>();
        LockExitNavMeshObstacle = LockExitGameObject.GetComponent<NavMeshObstacle>();
        IsExit = isExit;
        
    }
    public void LockPoint()
    {
        _collider.enabled = false;
        //todo Lock
        if(IsExit)
        {
            LockExitGameObject.SetActive(true);
        }
        else
        {
            if(LockEnterGameObject != null) LockEnterGameObject.SetActive(true);
        }
    }
    public void UnLockPoint()
    {
        //todo Unlock
        // if(IsExit)
        // {
           // DeleteWall();
        // }
        if(IsExit)
        {
            //LockExitGameObject.SetActive(false);
            
            StartCoroutine(VanishFireWall());
            // _collider.enabled = false;
        }
    }
    private void DeleteWall()
    {
        StartCoroutine(VanishFireWall());
        if(LockEnterGameObject != null) LockEnterGameObject.SetActive(false);
    }
    void OnTriggerEnter(Collider other)
    {
        if(!IsExit && other.gameObject.tag == "Player")
        {
            if(CurrentLocalMapPoint.RoomSpawnList.Count != 0)
            {
                _collider.enabled = false;
                var state = BattleState.Instance;
                ref var localMapComp = ref state.EcsRunHandler.World.GetPool<LocalMapComponent>().Get(state.GetEntity("LocalMapEntity"));
                localMapComp.CurrentLocalMapPoint = CurrentLocalMapPoint;
                state.IndexWave = 0;
                state.CurrentRoom = CurrentLocalMapPoint.RoomMB;
                state.EcsRunHandler.World.GetPool<StartFightEvent>().Add(state.EcsRunHandler.World.NewEntity());
                BattleState.Instance.CurrentRoom.CurrentNumberOfEnemies = 0;
                CurrentLocalMapPoint.LockPoint();
            }

        }
    }
    public void SetActiveFalseWall()
    {
        Wall?.SetActive(false);
    }
    public void SetActiveFalseEnter()
    {
        if(Wall != null && Wall.activeInHierarchy)
        {
            GameObject.Destroy(LockEnterGameObject);
        }
    }
    IEnumerator VanishFireWall()
    {
        while(_time < _maxTime)
        {
            float _t = (_time / _maxTime);
            float value = 1 - _t;
            if(LockExitNavMeshObstacle.isActiveAndEnabled && _t > 0.5f) LockExitNavMeshObstacle.enabled = false;
            ExitVisualEffect.SetFloat("Alpha-Control", value);
            _time += Time.deltaTime;
            yield return null;
        }
        LockExitGameObject.SetActive(false);

    }

}
