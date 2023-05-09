using System.Collections.Generic;
using UnityEngine;

public class ConsumableSpawnpointContainer : MonoBehaviour
{
    [SerializeField] private List<ConsumableSpawnpoint> _consumableSpawnpoints;


    private void Awake()
    {
    }

    public void Start()
    {
        _consumableSpawnpoints.Clear();
        foreach (var item in GetComponentsInChildren<ConsumableSpawnpoint>())
        {
            _consumableSpawnpoints.Add(item);
        }

        for (int i = 0; i < _consumableSpawnpoints.Count; i++)
        {
            _consumableSpawnpoints[i].Setup();
            _consumableSpawnpoints[i].Initialise();
        }   
        
        if(_consumableSpawnpoints.Count == 0)
        {
            Debug.LogWarning("There are no spawn points registered. Make sure they are added to the list in the Unity Editor");
        }
    }
}
