using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using System.Collections.Generic;
using Leopotam.EcsLite.ExtendedSystems;
using UnityEngine;
using UI;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;
using UnityEngine.VFX;

namespace Client
{
    sealed class EcsStartup : MonoBehaviour
    {
        EcsWorld _world;
        EcsSystems _mapGenerationSystems;
        EcsSystems _commonSystems;
        EcsSystems _initSystems;
        EcsSystems _inputSystems;
        EcsSystems _actionSystems;
        EcsSystems _moveSystems;
        EcsSystems _fixedMoveSystems;
        EcsSystems _deadSystems;
        EcsSystems _enemySystems;
        EcsSystems _reactionSystems;
        EcsSystems _abilitySystems;
        EcsSystems _soundSystems;
        EcsSystems _abilityEndSystems;
        EcsSystems _delSystems;
        EcsSystems _oberverSystems;
        EcsSystems _resolveAbilitySystems;
        EcsSystems _toughnessSystems;
        EcsSystems _utilitySystems, _spawnWaveSystems, _validCheckAbilitySystems, _findAbilitySystems, _requestAbilityEventSystems, _resolveRequestAbilitySystems, _timerAbilitySystems, _resolveTimerSystems, _disposeAbilitySystems, _animationSystems, _relicSystems;
        EcsSystems _serviceSystems;
        EcsSystems _hardControlSystems;
        EcsSystems _modifiersSystems;
        EcsSystems _conditionSystems;
        EcsSystems _hitInervalSystems;
        GameState _state;
        [SerializeField] private bool _isMainScene;
        bool isMenuOpen;
        bool isTest;


        private void Awake()
        {
            // if (GameState.Instance == null) _state = new GameState();
            // else _state = GameState.Instance;

            // _state.InitGameplayState();
            ClearNahuyAnimationErrors(); // chtoby animation events ne srali oshibkami
                                         //UIManagerRitualist.GetUIManager.BaseInit();
            isTest = ConfigModule.GetConfig<GameConfig>().IS_TEST;
        }

        void Start()
        {
            //_world = new EcsWorld();
            // _state.World = _world;
            // _state.IsMainScene = _isMainScene;
            // GameState.ResetTime();

            //SHURIK SHURA TI NE VER SLEZAM VSE VERNETSA
            _mapGenerationSystems = new EcsSystems(_world, _state); //done

            _modifiersSystems = new EcsSystems(_world, _state);
            _abilitySystems = new EcsSystems(_world, _state);
            _animationSystems = new EcsSystems(_world, _state);
            _soundSystems = new EcsSystems(_world, _state);
            _commonSystems = new EcsSystems(_world, _state);
            _initSystems = new EcsSystems(_world, _state);
            _inputSystems = new EcsSystems(_world, _state);
            _reactionSystems = new EcsSystems(_world, _state);
            _hardControlSystems = new EcsSystems(_world, _state);


            //VASIA
            _deadSystems = new EcsSystems(_world, _state);
            _actionSystems = new EcsSystems(_world, _state);
            _moveSystems = new EcsSystems(_world, _state);
            _fixedMoveSystems = new EcsSystems(_world, _state);
            _enemySystems = new EcsSystems(_world, _state);
            _serviceSystems = new EcsSystems(_world, _state);
            _oberverSystems = new EcsSystems(_world, _state);
            _relicSystems = new EcsSystems(_world, _state);




            //MISHA
            _toughnessSystems = new EcsSystems(_world, _state);//pi
            _delSystems = new EcsSystems(_world, _state);//pi
            _abilityEndSystems = new EcsSystems(_world, _state);//pi
            _resolveAbilitySystems = new EcsSystems(_world, _state);//pi
            _utilitySystems = new EcsSystems(_world, _state);//pi
            _conditionSystems = new EcsSystems(_world, _state);
            _hitInervalSystems = new EcsSystems(_world, _state);

            //AnalniyPotroshitel
            _validCheckAbilitySystems = new EcsSystems(_world, _state);
            _findAbilitySystems = new EcsSystems(_world, _state);
            _requestAbilityEventSystems = new EcsSystems(_world, _state);
            _resolveRequestAbilitySystems = new EcsSystems(_world, _state);
            _timerAbilitySystems = new EcsSystems(_world, _state);
            _resolveTimerSystems = new EcsSystems(_world, _state);
            _disposeAbilitySystems = new EcsSystems(_world, _state);
            _spawnWaveSystems = new EcsSystems(_world, _state);



            _mapGenerationSystems
                .Add(new InitGlobalMapSystem())
                .Add(new CreateGlobalMapSystem())
                .Add(new InitLocalMapSystem())
                .Add(new GenerateLocalMapSystem())
                .Add(new UpdateSplineInstancesSystem())
                .Add(new CreateLocalMapSystem())
                // .Add(new SpawnLocalRoomSystem())
                .Add(new GenerateCompleteGlobalMapSystem())
                .Add(new ShowGlobalMapSystem())

            // .DelHere<CreateGlobalMapEvent>()
            // .DelHere<GenerateLocalMapEvent>()
            // .DelHere<CreateCurrentLocalMapPointEvent>()
            // .DelHere<SpawnLocalRoomEvent>()
            // .DelHere<ShowGlobalMapEvent>()
            ;
            _initSystems
                .Add(new StartSceneTESTSystem())
                .Add(new InitInputSystem())
                .Add(new InitPlayerSystem()) //poka tut initModifiers

                .Add(new InitInputHolder())
                .Add(new InitCurrencySystem())

                .Add(new InitAttackRoomSystem())
                //.Add(new InitRoomSystem())
                .Add(new InitEnemyPoolSystem())
                .Add(new ParticleSpawnEnemySystem())
                .Add(new TimerSpawnSystem())

                .Add(new SpawnAbilitySystem())

                .Add(new InitEnemySystem())
                .Add(new InitStaticUnitSystem())

                .Add(new InitUnitMBSystem())
                .Add(new InitUnitCommonSystem())
                .Add(new InitUnitAbilitySystem())
                .Add(new InitAbilitySystem())

                .Add(new InitDescriptionAbilitySystem())
                .Add(new InitTagAbilitySystem())
                .Add(new InitTimeLineBlocksAbilitySystem()) //TODO LAST POINT
                .Add(new InitResolveBlocksAbilitySystem())

                .Add(new AddAbilityToOwnerList())

                .Add(new InitAbilityCanvasSystem())
                .DelHere<InitAbilityEvent>()


                .DelHere<InitAIEvent>()                  //  ↓ инициализация ИИ
                .Add(new SpecialStaticUnitInit())
                .Add(new SpecialEnemyInit())
                .Add(new SpecialPlayerInit())
                .Add(new NextRoomPlayerSystem())

                .Add(new InitUnitBrainSystem())

                .Add(new InitSupportContextSystem())
                .Add(new InitFromPlaceContextSystem())
                .Add(new InitAlliesContextSystem())

                .Add(new InitAttacksContextSystem())
                .Add(new InitDesynchronizationSystem())
                .Add(new InitTargetsContextSystem())
                .Add(new InitTerrorizeContextSystem())


                .Add(new InitSelfContextSystem())

                .Add(new InitDefenseContextSystem())
                .Add(new InitThreatContextSystem())

                .Add(new InitEvaluationHelpersData())
                .Add(new FillAlliesListSystem())
                .Add(new InitRelicSystem())

                .Add(new InitInterface())
                .Add(new InitObserver())
                .Add(new InitBossObserverSystem())
                .Add(new InitEnemyObserverSystem())

                //.DelHere<SpawnUnitWithDelay>()
                .DelHere<CreateEnemyEvent>()
                .DelHere<InitUnitEvent>()
                .DelHere<InitContextEvent>()
                .DelHere<SpawnStaticUnitEvent>()
                .DelHere<InitAttackRoomEvent>()
            ;
            _modifiersSystems //группа систем ТОЛЬКО для безусловных модификаторов, которые влияют на циферки
                .Add(new InitModifiersSystem()) //навешивание на игрока компонента со всеми модификаторами
                                                //todo системы инита мофдификаторв/добавления/удаления вызывают работу системы пересчета абилок на основании модификаторов
                .Add(new AddModifierSystem())
                .Add(new DelModifierSystem())



                .Add(new ModifierAbilitiesSystem()) //поиск нужныч модификаторов для абилок
                .Add(new ClearModifiersSystem()) //default ability
                                                 //damage modifiers
                .Add(new ModifierAdditiveSystem())
                .Add(new ModifierMultiplicativeSystem())
                //todo projectile modifiers
                //todo attackZone modifiers
                //todo some modifiers
                //todo chargePoint modifiers

                .DelHere<InitModifiersEvent>()
                .DelHere<RequestAddModifier>()
                .DelHere<RequestDelModifier>()
                .DelHere<ModifiersContainerChangesEvent>()
                .DelHere<DamageModifier>()
                .DelHere<AdditiveModifier>()
                .DelHere<MultiplicativeModifier>()
            ;

            _inputSystems
                .Add(new InputSystem())
            ;
            _enemySystems
                .Add(new DeleteDeadBrainSystem()) //TODO LAST POINT
                .Add(new TemporaryCirclingCheckSystem())
                .Add(new ChangeAbilityBossStageSystem())
                .Add(new CheckBossStageSystem()) // инитим все абилки босса, потом надо смотреть на стейдж, отдельной системой смотреть здоровье? кидать ивент на изменение стейджа, exc inAction, 
                .Add(new AIDesynchronizationSystem())

                .Add(new UpdateSelfContextSystem())   //  ↓ проверка на возможность использования контекста ИИ
                .Add(new UpdateTargetsContextSystem())
                .Add(new UpdateFromPlaceContextSystem())
                .Add(new UpdateAlliesContextSystem())
                .Add(new UpdateAttacksContextSystem())
                .Add(new UpdateTerrorizeContextSystem())
                .Add(new UpdateDefenseContextSystem())
                .Add(new UpdateSupportContextSystem())
                .Add(new UpdateThreatsContextSystem())


                .Add(new EvaluateIdleActionScoreSystem())  //  ↓ если проверка на возможность использования контекста прошла, считаем рейтинг контекста ИИ
                .Add(new EvaluateAttackIntentionSystem())   // проверку надо что не все атаки говорят атаковать
                .Add(new EvaluateFromPlaceScoreSystem())
                .Add(new EvaluateSupportIntentionSystem())
                .Add(new EvaluateAttackActionScoreSystem())
                .Add(new EvaluateTerrorizeActionScoreSystem())
                .Add(new EvaluateSupportActionSystem())
                .Add(new EvaluateDefendActionScoreSystem())
                .Add(new EvaluateKeepAtRangeActionScoreSystem())
                .Add(new EvaluateInActionRotateSystem())

                .DelHere<AttackRequest>()
                .DelHere<TerrorizeRequest>()
                .DelHere<DefendRequest>()
                .DelHere<IdleRequest>()
                .DelHere<KeepAtRangeRequest>()
                .DelHere<MoveToTargetRequest>()
                .DelHere<SupportRequest>()


                .Add(new DecisionMakingSystem())         //  используем контекст с большим рейтингом и кидаем реквест на него
                .Add(new IdleRequestProcessingSystem())
                .Add(new MoveToTargetRequestProcessingSystem())
                .Add(new KeepAtRangeRequestProcessingSystem())
                .Add(new AttackRequestProcessingSystem())
                .Add(new TerrorizeRequestProcessingSystem())
                .Add(new DefendRequestProcessingSystem())
                .Add(new SupportRequestProcessingSystem())

                .Add(new StartAINavigationSystem())
                .Add(new StopAINavigationSystem())

                .DelHere<StartNavigationRequest>()
                .DelHere<StopNavigationRequest>()

                .Add(new AttackAISystem())
                .Add(new TerrorizeAISystem())
                .Add(new FromPlaceAISystem())
                .Add(new DefenseAISystem())
                .Add(new KeepAtRangeSystem())
                .Add(new AINavigationSystem())
                .Add(new AIRotationSystem())
                .Add(new CoerciveAbilitySystem())
            ;
            //нахождение абилок в листе
            _findAbilitySystems
                .Add(new FindAbilityByInputReferenceSystem())
                .DelHere<AbilityInputEvent>()


            ;
            //проверки валидности
            _validCheckAbilitySystems
                .Add(new AbilityPressedSystem())
                .Add(new AbilityReleasedSystem())

                .Add(new CheckOwnerBusyAbilitySystem())
                .Add(new CheckCoolDownAbilitySystem())
                .Add(new CheckCostAbilitySystem())

                .Add(new CheckPreRequisiteSystem())
                .Add(new CheckWithOutPreRequisiteSystem())

                .Add(new DeleteCheckAbilityToUseSystem())
                .DelHere<DeleteCheckAbilityToUseEvent>()

                .Add(new UseAbilitySystem())
                .Add(new ActiveTimerPressedAbilitySystem())
                .Add(new ActiveTimerReleasedAbilitySystem())
                .Add(new ActiveChargeTimerSystem())
                .DelHere<CheckAbilityToUse>()
                .DelHere<AbilityPressedEvent>()
                .DelHere<AbilityReleasedEvent>()
                .DelHere<FindPreRequisiteAbilityComponent>()
            ;
            _timerAbilitySystems
                .Add(new AttackEffectSystem())
                .Add(new InitTimerAbilitySystem())
                .Add(new InitChargePointTimerAbilitySystem())
                .Add(new InitCooldownTimerAbilitySystem())

                .DelHere<InitTimerAbilityEvent>()

                .Add(new CheckTimerAbilitySystem())
                .Add(new TimerAbilitySystem()) //прибавление таймера


                .Add(new TimerChargeAbilitySystem())
                .Add(new CooldownTimerAbilitySystem())
                .Add(new CheckCooldownTimerAbilitySystem())
                .Add(new CheckCooldownTimerWithChargePointAbilitySystem())

                .Add(new EndChargeAbilitySystem())
                .Add(new FullChargeAbilitySystem())

                .Add(new VFXFullChargeSystem())
                //todo sound after full charge

                .DelHere<FullChargeAbilityEvent>()
                .DelHere<EndChargeAbilityEvent>()
                .DelHere<VFXFullChargeComponent>()
            ;
            _disposeAbilitySystems
                .Add(new DisposeAllAbilityOnUnitSystem())
                .Add(new DisposeLockAbilitySystem())
                .Add(new AddWaitClickTimerSystem())
                .Add(new DisposeAbilitySystem())
                .Add(new DestroyAbilitySystem())

                .DelHere<DisposeAllAbilityOnUnitEvent>()
                .DelHere<DisposeAbilityEvent>()
                .DelHere<DestroyAbilityEvent>()
            ;

            _resolveTimerSystems
                .Add(new ResolveAbilityAfterTimerSystem())
                .DelHere<ResolveAbilityAfterTimerEvent>()
            ;
            _requestAbilityEventSystems //системы обработки реквестов
                .Add(new RequestAbilityChildSystem()) //TODO Last point
                .Add(new RequestInActionEventSystem())
                .Add(new RequestLockMoveEventSystem())
                .Add(new RequestLockRotationEventSystem())
                .Add(new RequestChangeAnimationEventSystem())
                .Add(new RequestAttackZoneEventSystem())
                .Add(new RequestVFXEventSystem())
                .Add(new RequestRedirectExternalMoveSystem())
                .Add(new RequestExternalMoveEventSystem())
                .Add(new RepeatingAbilitySystem())
                .Add(new RequestMissileAbilitySystem()) // инвокает InvokeMissileIvent его компоненты
                .Add(new RequestShieldSystem())
                .Add(new RequestAttackRoomSystem())

                .Add(new RequestDashSystem())
                .Add(new RequestInvulnerabilitySystem())

                .DelHere<RequestConditionEvent>()
                .DelHere<RequestAttackRoomEvent>()
                .DelHere<RequestAbilityChildEvent>()
                .DelHere<RequestShieldEvent>()
                .DelHere<RequestInActionEvent>()
                .DelHere<RequestLockMoveEvent>()
                .DelHere<RequestLockRotationEvent>()
                .DelHere<RequestChangeAnimationEvent>()
                .DelHere<RequestAttackZoneEvent>()
                .DelHere<RequestVFXEvent>()
                .DelHere<RequestExternalMoveEvent>()
                .DelHere<RequestMissileAbilityEvent>()
                .DelHere<RequestInvulnerability>()
                .DelHere<RequestDash>()
            ;
            _resolveRequestAbilitySystems
                .Add(new ChangeAnimatorUnitSystem())
                .Add(new ChangeAnimationUnitSystem())
                .Add(new TimerAttackRoomSystem())
                .Add(new InvokeVisualEffectSystem())

                .Add(new UnitCollisionSystem()) //ловит ивент колизии  и вешает ивент DoResolveBlockEvent  игрока с врагом
                .Add(new ResolveBlockSystem()) //ловит ивент и резолвит блок эффектов на энтити
                .Add(new InvokeMissileSystem())
                .Add(new InvokeAttackZoneSystem())
                .Add(new DisableAttackZoneSystem())

                .Add(new RecalculateResolveBlockSystem())

                .DelHere<AnimationStateComponent>()
                .DelHere<InvokeAttackZoneEvent>()
                .DelHere<InvokeVisualEffectEvent>()
                //.DelHere<VisualEffectsComponent>()
                .DelHere<UnitCollisionEvent>()
                .DelHere<DoResolveBlockEvent>()
                .DelHere<InvokeMissileEvent>()
                .DelHere<RecalculateResolveBlockEvent>()

            ;
            _abilityEndSystems //сменит нэйминг этой хуйни
                .Add(new WaitClickTimerSystem())
                .Add(new DelWaitClickSystem())
                .Add(new DelLockInActionSystem())
                .Add(new RequestActiveControlSystem())
                .Add(new ActiveControlSystem())


                .DelHere<DelLockInActionEvent>()
                .DelHere<RequestActiveControlEvent>()
                .DelHere<ActiveControlEvent>()
                .DelHere<DelRotateComponent>()
                .DelHere<EnemyRushComponent>()
                .DelHere<RamComponent>()
                .DelHere<MoveEvent>()
                .DelHere<DelWaitClick>()
            ;
            _resolveAbilitySystems
                //PermanentEffects//
                //.Add(new VampirismSystem())
                //.Add(new DamageOverTimeSystem())
                //.Add(new ExplosionOnHitSystem())
                //PermanentEffects//


                .Add(new AbilityDamageSystem())
                .Add(new AbilityHitSystem())
                .Add(new StunSystem())
                .Add(new KnockBackSystem()) //TODO HIT ANIM
                .Add(new PushSystem())

               .DelHere<DamageEffect>()
               .DelHere<HitEffect>()
               .DelHere<PushEffect>()
           ;


            //использование абилок

           
                        //_actionSystems
                        //;
            

            _abilitySystems
            .Add(new PermanentMissileSystem())
            .Add(new ChangeMissileTargetSystem())
            .Add(new RunMissileDirectionSystem())
            .Add(new RunMissileToPointSystem())
            .Add(new RunMissilePursueSystem())
            .Add(new RunMissileTrajectorySystem())
            .Add(new ChangeViewMissileSystem())
            .Add(new RunMissileBackToCasterSystem())
            .Add(new RunMissileArcYSystem())
            .Add(new RunMissileTrailSystem())

            .Add(new UpdateTrailSystem())
            .Add(new TrailResolveSystem())
            .Add(new DestroyTrailFromHeadSystem())
            .Add(new DestroyTrailFromTailSystem())
            .Add(new FinishTrailSystem())


            .Add(new LightningMissileSystem())

            .Add(new UpdateMissileSystem())
            .Add(new MissileManagerSystem())

            .Add(new ChangeResolveMissileSystem())
            .Add(new TelegraphyOfMissileTargetSystem())
             .Add(new FinishMissileSystem())
             .Add(new InvulnerabilitySystem())
             .Add(new DashingSystem())
             .DelHere<InvokeMissileEvent>()
                ;

            _relicSystems
                .Add(new InvokeRelicEffectSystem())
                .DelHere<InvokeRelicEvent>()
                ;

            _fixedMoveSystems
                .Add(new AddMovePlayerSystem())
                .Add(new ExternalMoveSystem())
                .Add(new PushAfterRagDollSystem())
            ;
            _hardControlSystems
                .Add(new RequestAddHardControlSystem())
                .Add(new AddHardControlSystem())
                .Add(new HardControlTimerCheckSystem())
                .Add(new DelHardControlSystem())
                .Add(new HardControlStopAINavigationSystem())
                .DelHere<RequestAddHardControlEvent>()
                .DelHere<AddHardControlEvent>()
                .DelHere<DelHardControlEvent>()
            ;
            _moveSystems
                .Add(new InputMoveHolderSystem())
                //.Add(new PursuitSystem())
                .Add(new AddMovePursuerSystem())
                .Add(new AddMoveShelterSystem())
                .Add(new AddRotationPlayerSystem())
                .Add(new AddRotationSystem())
                .Add(new CirclingSystem())
                //.Add(new MoveUnitSystem())
                .Add(new RotationUnitSystem())
                //.Add(new StopNavMeshSystem())
                .Add(new StopMoveSystem())

                //.Add(new CheckNavMeshSytem())
                .DelHere<MoveComponent>()
                .DelHere<RotationComponent>()
            ;



            _commonSystems
                .Add(new HealSystem())
                .Add(new StaminaSystem())
                .Add(new ChangeWeaponTestSystem())
                .Add(new UpdateEffectSystem())
                .Add(new DeadSlowSystem())
                .Add(new SlowSystem())
                .Add(new LightSettingsChangeRequestProcessingSystem())
                .Add(new LightSettingsChangeSystem())
                .Add(new DropSystem())

                .DelHere<HealEvent>()
                .DelHere<LightSettingChangeRequest>()
            ;
            _spawnWaveSystems

               .Add(new ManagerFightSystem())
               .Add(new SpawnWaveSystem())
               .Add(new StopFightSystem())

               .DelHere<SpawnWaveEvent>()

               .DelHere<StartFightEvent>()
               .DelHere<MomentDeadEvent>()
               .DelHere<StopFightEvent>()

           ;

            _utilitySystems
                .DelHere<ShieldDestructionEvent>()
                .Add(new TimerShieldSystem())
                //.Add(new ShieldSystem())
                .Add(new ExplosiveShieldSystem())
                .Add(new ShieldDestructionSystem())
            ;
            _reactionSystems
                //todo rewrite takeDamage seq 
                .Add(new GlobalDamageCDSystem())
                .Add(new SourceDamageCDSystem())

                .Add(new AddAllowedComponentsToTakeDamageEntitySystem())
                .Add(new CheckDeathBeforeDamageSystem())
                .Add(new ClearAllAllowedComponentsOnTakeDamageEntitySystem())


                //todo система выдачи разрешения урона по щиту
                .Add(new CheckShieldDamageAllowedSystem())
                .Add(new CheckDamageAllowedSystem())
                .Add(new CheckSourceDamageCDSystem())
                .Add(new CheckToughnessDamageAllowedSystem())

                //todo перенести сюда систему шилда(получение урона по шилду и его преобразование)
                .Add(new ShieldSystem())
                .Add(new TakeDamageSystem())
                .Add(new AddGlobalDamageCDSystem())
                .Add(new AddSourceDamageCDSystem())


                .Add(new ToughnessSystem())
                .Add(new RemoveHighToughnessSystem())



                .Add(new CheckHealthAfterDamageSystem())
                .Add(new CheckVisualDamageSystem())
                .Add(new CheckNumberDamageAllowedSystem())
                .Add(new CheckSoundDamageAllowedSystem())
                .Add(new CheckHitAnimationAllowedSystem())
                .Add(new CheckShakeCameraAllowedSystem())


                .Add(new StartTimerViewDamageSystem())
                .Add(new VisualDamageSystem())

                .Add(new NumberDamageSystem())
                .Add(new SoundTakeDamageSystem())
                .Add(new CheckSideHitSystem())

                .Add(new HitAddHardControlSystem())
                .Add(new HitSetAnimationSystem())
                .Add(new CheckAddHitIntervalSystem())
                .Add(new HitDisposeAbilitySystem())

                    .Add(new DelayRecoveryToughnessSystem()) //это надо удалить? или ШО?
                    .Add(new RemoveDelayRecoveryToughnessSystem())
                    .Add(new RecoveryToughnessSystem())
                    .Add(new RemoveRecoveryToughnessSystem())



                .Add(new DeleteAbilitiesAfterDeadSystem())
                .Add(new TiredSystem())
                .Add(new TeleportSystem())

                .Add(new ShakeDamageSystem())
                    .Add(new ShakeCameraSystem())

                .Add(new RelicDropSystem())

                .Add(new DelTakeDamageEntitySystem())
                .Add(new ClearAllAllowedComponentsOnTakeDamageEntitySystem())

                .DelHere<ReactionEvent>()
                .DelHere<TakeDamageComponent>()
                .DelHere<ClearAllAllowedComponents>()
                .DelHere<CheckSideHitEvent>()
                .DelHere<EndToughnessImmunityEvent>()
                .DelHere<RemoveHighToughnessEvent>()
                .DelHere<RemoveDelayRecoveryToughnessEvent>()
                .DelHere<RemoveRecoveryToughnessEvent>()
            ;
            _hitInervalSystems
                .Add(new AddHitIntervalSystem())
                .Add(new DisposeHitIntervalSystem())
                .Add(new CalculationOfTimeIntervalsSystem())
                .Add(new TimerHardHitSystem() )
                .Add(new TimerBeforApprovedDashSystem())
                .Add(new TimerApprovedDashSystem())
                .Add(new DelHereAddHitIntervalEvent())
                .Add(new DelHereDisposeHitIntervalEvent())
                .Add(new DelHereCalculationHitIntervalEvent())
                ;
            _conditionSystems
                .Add(new RequestConditionSystem())
                .Add(new RequestConditionOverlaySystem())
                .Add(new ConditionOverlayToUnitSystem())
                .Add(new AddPointConditionSystem())
                .Add(new UpdatingDeletionTimerAfterAddPointSystem())

                .Add(new ResolveOverlayToUnitSystem())
                .Add(new ResolveAfterMaxPointSystem())
                .Add(new ResolveAfterAddPointSystem())
                .Add(new ResolveOfAddingToMaxPointsSystem())

                .Add(new RecalculatingTimerAfterAddPointSystem())
                .Add(new TimerForResolveSystem())

                .Add(new NotifyOnConditionSystem())
                .Add(new ListenerConditionSystem())

                .Add(new RemovePointSystem())

                .Add(new MaxPointConditionSystem())
                .Add(new ResolveConditionSystem())
                .Add(new ChangeViewConditionSystem())
                .Add(new DestroyViewConditionSystem())
                .Add(new DestroyConditionSystem())

                .DelHere<ChangeViewConditionEvent>()
                .DelHere<AddPointConditionEvent>()
                .DelHere<MaxPointConditionEvent>()
                .DelHere<ResolveConditionEvent>()
                .DelHere<RequestConditionOverlayEvent>()
                .DelHere<ConditionOverlayToUnitEvent>()
                .DelHere<ListenOnConditionEvent>()
                ;
            _deadSystems
                .Add(new MomentDeadSystem())
                .Add(new DisposeAbilityAfterMomentDeadSystem())
                .Add(new UnitStaticDeadSystem())
                .Add(new EnemyDeadSystem())
                .Add(new PlayerDeadSystem())

                .Add(new DisableVisualEffectSystem())
                .Add(new DisableConditionAfterDeadUnitSystem())
                .Add(new DisableNavMeshSystem())
                .Add(new DeadRagDollSystem())
                .Add(new SoundOfDeathSystem())
                .Add(new TimerDeadSystem())
                .Add(new DissolutionSystem())

                .Add(new RecoveryViewAfterDeathSystem())
                .Add(new DelEntitySystem())
                .DelHere<MomentDeadEvent>()
            ;
            _soundSystems
                .Add(new SoundSystem())
            ;
            _delSystems
                .DelHere<VIEWDIRECTIONInputEvent>()
                .DelHere<MousePositionInputEvent>()
                .DelHere<WASDInputEvent>()
                .DelHere<AbilityInputEvent>()
                ;

            _animationSystems
                .Add(new SpawnAnimationSystem())
                .Add(new InActionAnimationSystem())
                .Add(new DashAnimationSystem())
                .Add(new RecoveryAnimationSystem())
                .Add(new HitAnimationSystem())
                .Add(new KnockbackAnimationSystem())
                .Add(new CastAnimationSystem())
                .Add(new ToughnessAnimationSystem())
                .Add(new AttackAnimationSystem())
                .Add(new MoveAnimationSystem())
                .Add(new IdleAnimationSystem())

                .DelHere<IdleAnimationState>()
                .DelHere<MoveAnimationState>()
                .DelHere<DashAnimationState>()
                .DelHere<AttackAnimationState>()
                .DelHere<HitAnimationState>()
                .DelHere<KnockbackAnimationState>()
                .DelHere<ToughnessAnimationState>()
                .DelHere<CastAnimationState>()
                .DelHere<RecoveryAnimationState>()
                .DelHere<SpawnAnimationState>()
                .DelHere<InActionAnimationState>()
                ;

            _oberverSystems
                .Add(new PlayerObserverSystem())
                .Add(new AbilityObserverSystem())
                .Add(new EnemyObserverSystem())
                .Add(new BossObserverSystem())
                ;
            _serviceSystems
               .Add(new RequestSwitchControllerPresetSystem())
                // .DelHere<DisposeAbility>()
                //.DelHere<InvokeCastAbility>()
                //.DelHere<PreCastAbility>()
                //.DelHere<DoCastAbility>()
                //.DelHere<ResolveAbility>()
                //.DelHere<FinishAbility>()



                // register your systems here, for example:
                // .Add (new TestSystem1 ())
                // .Add (new TestSystem2 ())

                // register additional worlds here, for example:
                // .AddWorld (new EcsWorld (), "events")
                // add debug systems for custom worlds here, for example:
                // .Add (new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem ("events"))
#if !UNITY_EDITOR
 ;
#endif
#if UNITY_EDITOR
                // add debug systems for custom worlds here, for example:
                // .Add (new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem ("events"))
                .Add(new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem());
#endif

            //if (SceneManager.GetActiveScene().name == "True Lobby") SubscribeUIElements();
            InjectAllSystems(_mapGenerationSystems, _initSystems, _modifiersSystems, _timerAbilitySystems, _disposeAbilitySystems, _resolveTimerSystems, _requestAbilityEventSystems, _resolveRequestAbilitySystems, _findAbilitySystems, _validCheckAbilitySystems, _resolveAbilitySystems, _relicSystems, _inputSystems, _enemySystems, _actionSystems, _moveSystems, _fixedMoveSystems, _commonSystems, _spawnWaveSystems, _reactionSystems, _hitInervalSystems, _deadSystems, _delSystems, _abilityEndSystems, _abilitySystems, _soundSystems, _utilitySystems, _hardControlSystems, _toughnessSystems, _animationSystems, _serviceSystems, _oberverSystems);
            InitAllSystems(_mapGenerationSystems, _initSystems, _modifiersSystems, _timerAbilitySystems, _disposeAbilitySystems, _resolveTimerSystems, _requestAbilityEventSystems, _resolveRequestAbilitySystems, _findAbilitySystems, _validCheckAbilitySystems, _resolveAbilitySystems, _relicSystems, _inputSystems, _enemySystems, _actionSystems, _moveSystems, _fixedMoveSystems, _commonSystems, _spawnWaveSystems, _reactionSystems, _hitInervalSystems, _deadSystems, _delSystems, _abilityEndSystems, _abilitySystems, _soundSystems, _utilitySystems, _hardControlSystems, _toughnessSystems, _animationSystems, _serviceSystems, _oberverSystems);

            //if (SceneManager.GetActiveScene().name != "True Lobby");
            //{
            //    if (!_world.GetPool<RequestSwithControllerEvent>().Has(GameState.Instance.PlayerEntity))
            //    {
            //        _world.GetPool<RequestSwithControllerEvent>().Add(GameState.Instance.PlayerEntity).InputActionPreset = InputActionPreset.FullControl;
            //    } //TODO VASYA GENIUOS NAPISAL HUINU VASYA MIGULIN ALEXSEEVICH EPTA BLYA
            //}
        }

        //void SubscribeUIElements()
        //{
        //    UIManagerRitualist.GetUIManager.UIShopManagerGlobal.OnWeaponBuy += PlayerEntity.Instance.UnlockWeapon;
        //    UIManagerRitualist.GetUIManager.UIShopManagerGlobal.OnWeaponSwitch += PlayerEntity.Instance.SetWeapon;
        //    UIManagerRitualist.GetUIManager.UIShopManagerGlobal.OnAbilityBuy += PlayerEntity.Instance.UnlockAbility;
        //    UIManagerRitualist.GetUIManager.UIShopManagerGlobal.OnAbilitySwitch += PlayerEntity.Instance.SetAbility;

        //}

        //void DescribeUIElements()
        //{
        //    UIManagerRitualist.GetUIManager.UIShopManagerGlobal.OnWeaponBuy -= PlayerEntity.Instance.UnlockWeapon;
        //    UIManagerRitualist.GetUIManager.UIShopManagerGlobal.OnWeaponSwitch -= PlayerEntity.Instance.SetWeapon;
        //    UIManagerRitualist.GetUIManager.UIShopManagerGlobal.OnAbilityBuy -= PlayerEntity.Instance.UnlockAbility;
        //    UIManagerRitualist.GetUIManager.UIShopManagerGlobal.OnAbilitySwitch -= PlayerEntity.Instance.SetAbility;
        //}

        void Update()
        {
            _mapGenerationSystems?.Run();
            _hardControlSystems?.Run();
            _initSystems?.Run();
            _inputSystems?.Run();
            _modifiersSystems?.Run();

            _findAbilitySystems?.Run();
            _validCheckAbilitySystems?.Run();
            _timerAbilitySystems?.Run();
            _disposeAbilitySystems?.Run();
            _resolveTimerSystems?.Run();

            _requestAbilityEventSystems?.Run();
            _resolveRequestAbilitySystems?.Run();
            _abilityEndSystems?.Run();
            _enemySystems?.Run();
            _actionSystems?.Run();

            _relicSystems?.Run();
            _moveSystems?.Run();
            _abilitySystems?.Run();
            _soundSystems?.Run();
            _commonSystems?.Run();

            _spawnWaveSystems?.Run();
            _resolveAbilitySystems?.Run();
            _utilitySystems?.Run();
            _reactionSystems?.Run();
            _hitInervalSystems?.Run();
            _conditionSystems?.Run();

            _toughnessSystems?.Run();
            _serviceSystems?.Run();
            _deadSystems?.Run();
            _animationSystems?.Run();
            _oberverSystems?.Run();

            _delSystems?.Run();
            //if (Input.GetKeyDown(KeyCode.Escape))
            //{
            //    if (!isMenuOpen)
            //    {
            //        UIManagerRitualist.GetUIManager.SusApp.OverlayState("menumodal");
            //        Time.timeScale = 0;
            //        isMenuOpen = true;
            //    }
            //    else
            //    {
            //        UIManagerRitualist.GetUIManager.SusApp.OverlayState("disabled");
            //        //BattleState.Instance.ResetTime();
            //        isMenuOpen = false;
            //    }
            //}
        }
        private void FixedUpdate()
        {
            _fixedMoveSystems?.Run();
        }


        void OnDestroy()
        {
            //if (SceneManager.GetActiveScene().name == "True Lobby") DescribeUIElements();

            if (_mapGenerationSystems != null)
            {
                _mapGenerationSystems.Destroy();
                _mapGenerationSystems = null;
            }
            if (_conditionSystems != null)
            {
                _conditionSystems.Destroy();
                _conditionSystems = null;
            }
            if (_commonSystems != null)
            {
                _commonSystems.Destroy();
                _commonSystems = null;
            }
            if (_inputSystems != null)
            {
                _inputSystems.Destroy();
                _inputSystems = null;
            }
            if (_initSystems != null)
            {
                _initSystems.Destroy();
                _initSystems = null;
            }
            if (_modifiersSystems != null)
            {
                _modifiersSystems.Destroy();
                _modifiersSystems = null;
            }
            if (_deadSystems != null)
            {
                _deadSystems.Destroy();
                _deadSystems = null;
            }
            if (_actionSystems != null)
            {
                _actionSystems.Destroy();
                _actionSystems = null;
            }
            if (_moveSystems != null)
            {
                _moveSystems.Destroy();
                _moveSystems = null;
            }
            if (_fixedMoveSystems != null)
            {
                _fixedMoveSystems.Destroy();
                _fixedMoveSystems = null;
            }

            if (_enemySystems != null)
            {
                _enemySystems.Destroy();
                _enemySystems = null;
            }
            if (_toughnessSystems != null)
            {
                _toughnessSystems.Destroy();
                _toughnessSystems = null;
            }
            if (_reactionSystems != null)
            {
                _reactionSystems.Destroy();
                _reactionSystems = null;
            }            
            if (_hitInervalSystems != null)
            {
                _hitInervalSystems.Destroy();
                _hitInervalSystems = null;
            }
            if (_abilitySystems != null)
            {
                _abilitySystems.Destroy();
                _abilitySystems = null;
            }
            if (_soundSystems != null)
            {
                _soundSystems.Destroy();
                _soundSystems = null;
            }
            if (_abilityEndSystems != null)
            {
                _abilityEndSystems.Destroy();
                _abilityEndSystems = null;
            }
            if (_delSystems != null)
            {
                _delSystems.Destroy();
                _delSystems = null;
            }
            if (_resolveAbilitySystems != null)
            {
                _resolveAbilitySystems.Destroy();
                _resolveAbilitySystems = null;
            }
            if (_utilitySystems != null)
            {
                _utilitySystems.Destroy();
                _utilitySystems = null;
            }
            if (_spawnWaveSystems != null)
            {
                _spawnWaveSystems.Destroy();
                _spawnWaveSystems = null;
            }
            if (_validCheckAbilitySystems != null)
            {
                _validCheckAbilitySystems.Destroy();
                _validCheckAbilitySystems = null;
            }
            if (_findAbilitySystems != null)
            {
                _findAbilitySystems.Destroy();
                _findAbilitySystems = null;
            }
            if (_requestAbilityEventSystems != null)
            {
                _requestAbilityEventSystems.Destroy();
                _requestAbilityEventSystems = null;
            }
            if (_resolveRequestAbilitySystems != null)
            {
                _resolveRequestAbilitySystems.Destroy();
                _resolveRequestAbilitySystems = null;
            }
            if (_timerAbilitySystems != null)
            {
                _timerAbilitySystems.Destroy();
                _timerAbilitySystems = null;
            }
            if (_serviceSystems != null)
            {
                _serviceSystems.Destroy();
                _serviceSystems = null;
            }
            if (_resolveTimerSystems != null)
            {
                _resolveTimerSystems.Destroy();
                _resolveTimerSystems = null;
            }
            if (_disposeAbilitySystems != null)
            {
                _disposeAbilitySystems.Destroy();
                _disposeAbilitySystems = null;
            }
            if (_hardControlSystems != null)
            {
                _hardControlSystems.Destroy();
                _hardControlSystems = null;
            }

            if (_animationSystems != null)
            {
                _animationSystems.Destroy();
                _animationSystems = null;
            }
            // cleanup custom worlds here.

            // cleanup default world.
            if (_world != null)
            {
                _world.Destroy();
                _world = null;
            }

            //_state.MainDisposable.Dispose();
            PoolModule.Instance.Dispose();
            //SaveModule.Save();
        }


        private void InjectAllSystems(params EcsSystems[] systems)
        {
            foreach (var system in systems)
            {
                system.Inject();
            }
        }

        private void InitAllSystems(params EcsSystems[] systems)
        {
            foreach (var system in systems)
            {
                system.Init();
            }
        }

        private void DelSystems(params EcsSystems[] systems)
        {
            foreach (var system in systems)
            {
                system.Destroy();
            }
        }

        private void ClearNahuyAnimationErrors()
        {
            List<AnimationEvent> lstEvent = new List<AnimationEvent>();
            Animator[] animators = Object.FindObjectsOfType<Animator>();
            foreach (Animator animator in animators)
            {
                foreach (AnimationClip ac in animator.runtimeAnimatorController.animationClips)
                {
                    foreach (AnimationEvent ev in ac.events)
                    {
                        ev.messageOptions = SendMessageOptions.DontRequireReceiver;
                        lstEvent.Add(ev);
                    }
                    ac.events = lstEvent.ToArray();
                    lstEvent.Clear();
                }
            }
        }
    }
}
