using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    void Awake()
    {
        ServiceLocator.Setup();
        ServiceLocator.Instance.Register<ConsumerService>(new ConsumerService());
        ServiceLocator.Instance.Register<ConsumabilityService>(new ConsumabilityService());
        ServiceLocator.Instance.Register<ConsumableSpawningService>(new ConsumableSpawningService());
    }

    void Update()
    {
        
    }
}
