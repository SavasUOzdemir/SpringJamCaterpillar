using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Any mono behaviour that occurs only once in the scene should be registered at the MonoBehaviourLocator.
// Like the ServiceLocator, this MonoBehaviourLocator can be used to pick up singletons. Make sure any IMonoBehaviourSingleton is registered on Awake and is deregistered when unloading the scene

public class MonoBehaviourLocator
{
    private MonoBehaviourLocator() { }

    private readonly Dictionary<string, IMonoBehaviourSingleton> _services = new Dictionary<string, IMonoBehaviourSingleton>();

    public static MonoBehaviourLocator Instance { get; private set; }

    public static void Setup()
    {
        Instance = new MonoBehaviourLocator();
    }

    public T Get<T>() where T : IMonoBehaviourSingleton
    {
        string key = typeof(T).Name;
        if (!_services.ContainsKey(key))
        {
            Debug.LogError($"{key} not registered with {GetType().Name}");
            throw new InvalidOperationException();
        }

        return (T)_services[key];
    }

    public void Register<T>(T service) where T : IMonoBehaviourSingleton
    {
        string key = typeof(T).Name;
        if (_services.ContainsKey(key))
        {
            Debug.LogError($"Attempted to register monobehaviour of type {key} which is already registered with the {GetType().Name}.");
            return;
        }

        _services.Add(key, service);
    }

    public void Deregister<T>() where T : IMonoBehaviourSingleton
    {
        string key = typeof(T).Name;
        if (!_services.ContainsKey(key))
        {
            Debug.LogError($"Attempted to unregister monobehaviour of type {key} which is not registered with the {GetType().Name}.");
            return;
        }

        _services.Remove(key);
    }
}
