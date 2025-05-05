using System.Collections.Generic;
using System;
using UnityEngine;

public static class EntityModule
{
    public static bool IsInit;

    private static Dictionary<Type, SourceEntity> _dictionary = new Dictionary<Type, SourceEntity>();

    public static void Initialize()
    {
        IsInit = false;
        _dictionary = new Dictionary<Type, SourceEntity>() { [typeof(PlayerEntity)] = new PlayerEntity(), [typeof(SoundEntity)] = new SoundEntity(), [typeof(ObserverEntity)] = new ObserverEntity() };
        foreach (var entity in _dictionary.Values)
        {
            entity.Init();
        }
        /*foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            foreach (var type in assembly.GetTypes())
            {
                if (typeof(SourceEntity).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract)
                {
                    try
                    {
                        if (Activator.CreateInstance(type) is SourceEntity instance)
                        {
                            _dictionary.Add(type, instance.Init());
                        }
                    }
                    catch (Exception ex)
                    {
                        
                    }
                }
            }
        }*/

        IsInit = true;
    }

    public static T GetEntity<T>() where T : SourceEntity
    {
        if (_dictionary.TryGetValue(typeof(T), out SourceEntity entity))
        {
            return entity as T;
        }

        

        return null;
    }
}

public abstract class SourceEntity
{
    public abstract SourceEntity Init();
}