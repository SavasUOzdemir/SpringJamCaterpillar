using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsumableSpawnpointContainer : MonoBehaviour
{
    [SerializeField] private List<ConsumableSpawnpoint> consumableSpawnpoints;

    public void Start()
    {
        for (int i = 0; i < consumableSpawnpoints.Count; i++)
        {
            consumableSpawnpoints[i].Setup();
            consumableSpawnpoints[i].Initialise();
        }    
    }
}
