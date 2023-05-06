using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsumableSpawnpointContainer : MonoBehaviour
{
    [SerializeField] private List<ConsumableSpawnpoint> _consumableSpawnpoints;

    public void Start()
    {
        for (int i = 0; i < _consumableSpawnpoints.Count; i++)
        {
            _consumableSpawnpoints[i].Setup();
            _consumableSpawnpoints[i].Initialise();
        }    
    }
}
