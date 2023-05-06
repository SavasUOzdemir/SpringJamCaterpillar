using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The Service Locator is the central hub from where we take all IGameServices. It is supposed to be (together with the GameManager) the only singleton in the architecture.
/// </summary>
public class ServiceLocator
{
    private ServiceLocator() { }

    private readonly Dictionary<string, IGameService> _services = new Dictionary<string, IGameService>();

    public static ServiceLocator Instance { get; private set; }

    public static void Setup()
    {
        Instance = new ServiceLocator();
    }

    public T Get<T>() where T : IGameService
    {
        string key = typeof(T).Name;
        if (!_services.ContainsKey(key))
        {
            Debug.LogError($"{key} not registered with {GetType().Name}");
            throw new InvalidOperationException();
        }

        return (T)_services[key];
    }

    public void Register<T>(T service) where T : IGameService
    {
        string key = typeof(T).Name;
        if (_services.ContainsKey(key))
        {
            Debug.LogError($"Attempted to register service of type {key} which is already registered with the {GetType().Name}.");
            return;
        }

        _services.Add(key, service);
    }

    public void Deregister<T>() where T : IGameService
    {
        string key = typeof(T).Name;
        if (!_services.ContainsKey(key))
        {
            Debug.LogError($"Attempted to unregister service of type {key} which is not registered with the {GetType().Name}.");
            return;
        }

        _services.Remove(key);
    }
}
