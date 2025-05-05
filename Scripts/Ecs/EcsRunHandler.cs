using System;
using System.Collections;
using System.Collections.Generic;

using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
using Statement;

public class EcsRunHandler
{
    private EcsWorld _world;
    public EcsWorld World => _world;
    private List<EcsSystems> _allSystems;

    private EcsSystems _runSystems;
    private EcsSystems _initSystems;
    private EcsSystems _fixedSystems;
    private EcsSystems _lateSystems;


    public EcsRunHandler(EcsFeature ecsFeature)
    {
        _world = new EcsWorld();

        _initSystems = new EcsSystems(_world, State.Instance);
        _runSystems = new EcsSystems(_world, State.Instance);
        _fixedSystems = new EcsSystems(_world, State.Instance);
        _lateSystems = new EcsSystems(_world, State.Instance);

        _allSystems = new List<EcsSystems>() { _initSystems, _runSystems, _fixedSystems, _lateSystems };

        try
        {
            foreach (var groupSystem in ecsFeature.InitGroupSystems)
            {
                foreach (var system in groupSystem.EcsSystems)
                {
                    try
                    {
                        _initSystems.Add(system.Clone());
                    }
                    catch (Exception ex)
                    {
                        
                    }
                }
            }

            foreach (var groupSystem in ecsFeature.RunGroupSystems)
            {
                foreach (var system in groupSystem.EcsSystems)
                {
                    try
                    {
                        _runSystems.Add(system.Clone());
                    }
                    catch (Exception ex)
                    {
                        
                    }
                }
            }

            foreach (var groupSystem in ecsFeature.FixedGroupSystems)
            {
                foreach (var system in groupSystem.EcsSystems)
                {
                    try
                    {
                        _fixedSystems.Add(system.Clone());
                    }
                    catch (Exception ex)
                    {
                        
                    }
                }
            }

            foreach (var groupSystem in ecsFeature.LateGroupSystems)
            {
                foreach (var system in groupSystem.EcsSystems)
                {
                    try
                    {
                        _lateSystems.Add(system.Clone());
                    }
                    catch (Exception ex)
                    {
                        
                    }
                }
            }

            try
            {
                _allSystems.ForEach(groupSystem => groupSystem.Inject());
            }
            catch (Exception ex)
            {
                
            }
        }
        catch (Exception ex)
        {
            
        }

#if UNITY_EDITOR
        _runSystems.Add(new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem());
#endif
    }

    public void PreInit()
    {
        foreach (var groupSystem in _allSystems)
        {
            if (groupSystem is IEcsPreInitSystem preInitGroup)
            {
                preInitGroup.PreInit(groupSystem);
            }
        }
    }

    public void Init()
    {
        foreach (var groupSystem in _allSystems) groupSystem.Init(); 
    }

    public void Run()
    {
        _runSystems?.Run();
    }

    public void LateRun()
    {
        _lateSystems?.Run();
    }

    public void FixedRun()
    {
        _fixedSystems?.Run();
    }

    public void Destroy()
    {
        foreach (var groupSystem in _allSystems)
        {
            groupSystem.Destroy();
        }

        if (_world != null) _world.Destroy();
    }
}
