using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ConsumableSpawningService : IGameService
{
    private List<ConsumableSpawnpoint> _consumableSpawnpoints = new List<ConsumableSpawnpoint>();

    private const float RESPAWN_INTERFAL = 12f;

    public void Register(ConsumableSpawnpoint consumableSpawnpoint)
    {
        _consumableSpawnpoints.Add(consumableSpawnpoint);
    }

    public void Spawn(ConsumableSpawnpoint consumableSpawnpoint, ConsumableType consumableType)
    {
        ConsumableFactory.Create(consumableSpawnpoint, consumableType);
    }

    public IEnumerator ReplenishConsumablesCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(RESPAWN_INTERFAL);
            SpawnRandom(ConsumableType.Leaf);
        }
    }

    public void SpawnRandom(ConsumableType consumableType)
    {
        List<ConsumableSpawnpoint> freeSpawnpoints = _consumableSpawnpoints.Where(s => s.GetConsumable() == null).ToList();

        if(freeSpawnpoints.Count  < 2)
        {
            return;
        }

        int randomIndex = UnityEngine.Random.Range(0, freeSpawnpoints.Count);
        ConsumableSpawnpoint randomSpawnpoint = freeSpawnpoints[randomIndex];

        ConsumableFactory.Create(randomSpawnpoint, consumableType);
    }
}
