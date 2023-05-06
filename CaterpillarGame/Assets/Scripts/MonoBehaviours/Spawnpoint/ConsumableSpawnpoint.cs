using System.Collections.Generic;
using UnityEngine;

public class ConsumableSpawnpoint : MonoBehaviour
{
    [SerializeField] private ConsumableType _defaultConsumableType;

    private ConsumableSpawningService _consumableSpawningService;
    private IConsumable _consumable;

    public void Setup()
    {
        _consumableSpawningService = ServiceLocator.Instance.Get<ConsumableSpawningService>();
        _consumableSpawningService.Register(this);
    }

    public void Initialise()
    {
        _consumableSpawningService.Spawn(this, _defaultConsumableType);
    }

    public void SetConsumable(IConsumable consumable)
    {
        _consumable = consumable;
    }

    public IConsumable GetConsumable()
    {
        return _consumable;
    }
}
