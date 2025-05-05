using Leopotam.EcsLite;
using UnityEngine;
using Leopotam.EcsLite.Di;

namespace Client
{
    sealed class InitEnemySystem : MainEcsSystem
    {  
        readonly EcsSharedInject<GameState> _state = default;
        readonly EcsFilterInject<Inc<CreateEnemyEvent>> _filter = default;
        readonly EcsPoolInject<CreateEnemyEvent> _spawnPool = default;
        readonly EcsPoolInject<EnemyComponent> _enemyPool = default;
        readonly EcsPoolInject<ViewComponent> _viewPool = default;
        readonly EcsPoolInject<BossComponent> _bossPool = default;

        //Poehali naxyi
        readonly EcsPoolInject<WaveIndex> _waveIndex = default;
        readonly EcsPoolInject<InitUnitEvent> _initUnitPool = default;
        readonly EcsPoolInject<SpawnAbilityEvent> _spawnAbilityEvent = default;
        readonly EcsPoolInject<DropComponent> _dropPool;

        public override MainEcsSystem Clone()
        {
            return new InitEnemySystem();
        }

        public override void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter.Value)
            {
                ref var spawnComp = ref _spawnPool.Value.Get(entity);
                ref var viewComp = ref _viewPool.Value.Add(entity);
                ref var initComp = ref _initUnitPool.Value.Add(entity);

                initComp.EnemyMetaConfig = spawnComp.EnemyUnitMetaConfig;

                var GO = PoolModule.Instance.GetFromPool<UnitMB>(spawnComp.UnitConfig.Unit.GetComponent<UnitMB>()).ThisGameObject;
                GO.GetComponent<AbilityUnitMB>().WeaponConfig = spawnComp.UnitConfig.WeaponConfig;
                GO.GetComponentInChildren<SkinnedMeshRenderer>().material = spawnComp.UnitConfig.Material;
                GO.GetComponentInChildren<MainSkinnedMeshRendererMarker>().GetComponent<SkinnedMeshRenderer>().sharedMesh = spawnComp.UnitConfig.MeshEnemy;//??
                GO.transform.position = spawnComp.SpawnPos;
                GO.transform.rotation = Quaternion.identity;
                GO.transform.position.Set(GO.transform.position.x, 0, GO.transform.position.z);
                viewComp.GameObject = GO;
                _waveIndex.Value.Add(entity);
                ref var enemyComp = ref _enemyPool.Value.Add(entity);


                if (spawnComp.UnitConfig.AIProfile.IsBoss)
                {
                    ref var bossComp = ref _bossPool.Value.Add(entity);
                    bossComp.CurrentStage = 0;
                    bossComp.StageCount = spawnComp.UnitConfig.AIProfile.BossStages.Length;
                    bossComp.BossStages = spawnComp.UnitConfig.AIProfile.BossStages;
                    var bossSpawnPoint = GameObject.Find("BossSpawnPoint");
                    if (bossSpawnPoint)
                    {
                        GO.transform.position = bossSpawnPoint.transform.position;
                        GO.transform.rotation = bossSpawnPoint.transform.localRotation;
                    }
                    _spawnAbilityEvent.Value.Add(entity);
                }
                ref var dropComp = ref _dropPool.Value.Add(entity);
                dropComp.dropConfig = spawnComp.UnitConfig.DropConfig;

                GO.SetActive(true);
            }
        }
    }
}
