using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    void Awake()
    {
        ServiceLocator.Setup();
        ServiceLocator.Instance.Register<ConsumerService>(new ConsumerService());
    }

    void Update()
    {
        
    }
}
