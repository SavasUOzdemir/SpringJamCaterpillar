using System.Collections.Generic;
using UnityEngine;


public class ConsumableObject : MonoBehaviour, IConsumable
{
    [SerializeField] private float massRegen = 15f;
    [SerializeField] private ConsumableType _consumableType;
    [SerializeField] private Collider _collder;
    private ConsumabilityService _consumabilityService;
    private ConsumableSpawnpoint _consumableSpawnpoint;

    public void Initialise(ConsumableSpawnpoint consumableSpawnpoint)
    {
        _consumableSpawnpoint = consumableSpawnpoint;

        _consumabilityService = ServiceLocator.Instance.Get<ConsumabilityService>();
        _consumabilityService.Register(this);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<IConsumer>(out IConsumer consumer))
        {
            ConsumerService consumerService = ServiceLocator.Instance.Get<ConsumerService>();
            consumerService.ConsumeItem(this, consumer);
        }
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }

    public float GetMassRegen()
    {
        return massRegen;
    }

    public ConsumableType GetConsumableType()
    {
        return _consumableType;
    }

    public ConsumableSpawnpoint GetSpawnpoint()
    {
        return _consumableSpawnpoint;
    }

    public void SetIsTriggered(bool isTriggered)
    {
        _collder.isTrigger = isTriggered;
    }

    //Move to somewhere else
    public float GetRequiredMassForConsumableType()
    {
        switch (_consumableType)
        {
            case ConsumableType.Leaf:
                return 1;
            case ConsumableType.Apple:
                return _consumabilityService.MassThreshold2;
            case ConsumableType.Honey:
                return _consumabilityService.MassThreshold3;
            case ConsumableType.Catkin:
                return _consumabilityService.MassThreshold4;
            default:
                Debug.LogWarning($"No implementation for type {_consumableType}. Return default");
                return 1;
        }
    }
}
