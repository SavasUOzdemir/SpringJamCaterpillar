using UnityEngine;

public class GameManager : MonoBehaviour
{
    void Awake()
    {
        ServiceLocator.Setup();
        ServiceLocator.Instance.Register<ConsumerService>(new ConsumerService());
        ServiceLocator.Instance.Register<ConsumabilityService>(new ConsumabilityService());
        ServiceLocator.Instance.Register<ConsumableSpawningService>(new ConsumableSpawningService());
        ServiceLocator.Instance.Register<AudioPlaybackService>(new AudioPlaybackService());
    }

    public void Start()
    {
        ConsumableSpawningService consumableSpawningService = ServiceLocator.Instance.Get<ConsumableSpawningService>();
        StartCoroutine(consumableSpawningService.ReplenishConsumablesCoroutine());
    }

}
