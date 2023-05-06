using System.Collections.Generic;
using System.Linq;

public class ConsumableSpawningService : IGameService
{
    private List<ConsumableSpawnpoint> _consumableSpawnpoints = new List<ConsumableSpawnpoint>();

    public void Register(ConsumableSpawnpoint consumableSpawnpoint)
    {
        _consumableSpawnpoints.Add(consumableSpawnpoint);
    }

    public void SpawnRandom(ConsumableType consumableType)
    {
        List<ConsumableSpawnpoint> freeSpawnpoints = _consumableSpawnpoints.Where(s => s.GetConsumable() == null).ToList();

        if(freeSpawnpoints.Count == 0)
        {
            return;
        }

        int randomIndex = UnityEngine.Random.Range(0, freeSpawnpoints.Count);
        ConsumableSpawnpoint randomSpawnpoint = freeSpawnpoints[randomIndex];

        ConsumableFactory.Create(randomSpawnpoint, consumableType);
    }

    public void Spawn(ConsumableSpawnpoint consumableSpawnpoint, ConsumableType consumableType)
    {
        ConsumableFactory.Create(consumableSpawnpoint, consumableType);
    }
}
