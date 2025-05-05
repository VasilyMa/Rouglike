using Client;
using Leopotam.EcsLite;
using Statement;
using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class SettingSceneCanvas : MonoBehaviour
{
    public TestRoomMB testRoomMb;
    RoomMB roomMB;
    EcsWorld _world;
    Dictionary<int, UnitBrain> _unitBrainContainer = new();
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            AddNewEnemy();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            DelAI();
        }
        if(Input.GetKeyDown(KeyCode.Alpha3))
        {
            AddAI();
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            AllDead();
        }
        if(Input.GetKeyDown(KeyCode.Alpha9))
        {
            int player = BattleState.Instance.GetEntity("PlayerEntity");
            // ref var animator = ref BattleState.Instance.EcsRunHandler.World.GetPool<TakeDamageComponent>().Add(BattleState.Instance.EcsRunHandler.World.NewEntity());
            // animator.TargetEntity = BattleState.Instance.EcsRunHandler.World.PackEntity(player);
            // animator.Damage = 8;
            // animator.KillerEntity = BattleState.Instance.EcsRunHandler.World.PackEntity(player);
            ref var animator = ref BattleState.Instance.EcsRunHandler.World.GetPool<AnimatorComponent>().Get(BattleState.Instance.GetEntity("PlayerEntity"));
            animator.Animator.SetTrigger(AnimatorComponent.HitLeft);
        }
    }
    private void Start()
    {

        BattleState.Instance.CurrentRoom = roomMB = FindObjectsOfType<RoomMB>()[0];
        var navMeshSurface = FindAnyObjectByType<NavMeshSurface>();
        navMeshSurface.BuildNavMesh();
        navMeshSurface.UpdateNavMesh(navMeshSurface.navMeshData);
    }
    public void AddNewEnemy()
    {

        _world = BattleState.Instance.EcsRunHandler.World;
        EcsPool<SpawnUnitWithDelay> _createPool = _world.GetPool<SpawnUnitWithDelay>();
        foreach(var enemy in testRoomMb.SpawnUnit.UnitConfigMetas)
        {
            ref var _createEnemyComp = ref _createPool.Add(_world.NewEntity());
            _createEnemyComp.UnitConfig = enemy.GetUnit();
            _createEnemyComp.SpawnPos = new Vector3(0, 0, 10);
            _createEnemyComp.SpawnPos = roomMB.SpawnPoints[0].position;
        }
    }
    public void DelAI()
    {
        _world = BattleState.Instance.EcsRunHandler.World;
        EcsFilter filter = _world.Filter<UnitBrain>().End();
        EcsPool<UnitBrain> poolBrain = _world.GetPool<UnitBrain>();
        foreach(var entity in filter)
        {
            if (_unitBrainContainer.ContainsKey(entity))
                _unitBrainContainer[entity] = poolBrain.Get(entity);
            else
                _unitBrainContainer.Add(entity, poolBrain.Get(entity));
            poolBrain.Del(entity);
        }
    }
    public void AddAI()
    {
        _world = BattleState.Instance.EcsRunHandler.World;
        EcsFilter filter = _world.Filter<EnemyComponent>().Exc<UnitBrain>().End();
        EcsPool<UnitBrain> poolBrain = _world.GetPool<UnitBrain>();
        foreach (var entity in filter)
        {
            ref var unitBrain = ref poolBrain.Add(entity);
            unitBrain = _unitBrainContainer[entity];

        }
    }
    public void AllDead()
    {
        _world = BattleState.Instance.EcsRunHandler.World;
        EcsFilter filter = _world.Filter<EnemyComponent>().Exc<DeadComponent>().End();
        EcsPool<DeadComponent> deadPool = _world.GetPool<DeadComponent>();
        foreach(var entity in filter)
        {
            deadPool.Add(entity);
        }
    }
}
