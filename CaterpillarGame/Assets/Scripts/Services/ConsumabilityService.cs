using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsumabilityService : IGameService
{
    public float MassThreshold2 = 30;
    public float MassThreshold3 = 45;

    private Dictionary<float, List<IConsumable>> _consumablesByLevel = new Dictionary<float, List<IConsumable>>();

    public ConsumabilityService()
    {
        _consumablesByLevel.Add(MassThreshold2, new List<IConsumable>());
        _consumablesByLevel.Add(MassThreshold3, new List<IConsumable>());
    }

    public void Register(IConsumable consumable)
    {
        float requiredMass = consumable.GetRequiredMassForConsumableType();
        if (_consumablesByLevel.TryGetValue(requiredMass, out List<IConsumable> consumables))
        {
            consumables.Add(consumable);
        }
        else
        {
            _consumablesByLevel.Add(requiredMass, new List<IConsumable>());
            _consumablesByLevel[requiredMass].Add(consumable);
        }
    }

    public void Deregister(IConsumable consumable)
    {
        float requiredMass = consumable.GetRequiredMassForConsumableType();
        _consumablesByLevel[requiredMass].Remove(consumable);
    }

    public void UpdateConsumability(Rigidbody rigidBody, float oldMass)
    {
        float newMass = rigidBody.mass;

        if (newMass == PlayerCharacter.METAMORPHOSIS_THRESHOLD_WEIGHT)
            return;

        // we grew
        if (oldMass < newMass)
        {
            CheckForIncreasingMass(oldMass, newMass);
        }
        else if (oldMass > newMass) // we shrank
        {
            CheckForDecreasingMass(oldMass, newMass);
        }
    }

    private void CheckForIncreasingMass(float oldMass, float newMass)
    {
        // unlock level 2
        if (oldMass < MassThreshold2 && newMass > MassThreshold2)
        {
            for (int i = 0; i < _consumablesByLevel[MassThreshold2].Count; i++)
            {
                _consumablesByLevel[MassThreshold2][i].SetIsTriggered(true);
            }
        }
        // unlock level 3
        if (oldMass < MassThreshold3 && newMass > MassThreshold3)
        {
            for (int i = 0; i < _consumablesByLevel[MassThreshold3].Count; i++)
            {
                _consumablesByLevel[MassThreshold3][i].SetIsTriggered(true);
            }
        }
    }

    private void CheckForDecreasingMass(float oldMass, float newMass)
    {
        // lock level 2
        if (oldMass < MassThreshold2 && newMass < MassThreshold2)
        {
            for (int i = 0; i < _consumablesByLevel[MassThreshold2].Count; i++)
            {
                _consumablesByLevel[MassThreshold2][i].SetIsTriggered(false);
            }
        }
        // lock level 3
        if (oldMass > MassThreshold3 && newMass < MassThreshold3)
        {
            for (int i = 0; i < _consumablesByLevel[MassThreshold3].Count; i++)
            {
                _consumablesByLevel[MassThreshold3][i].SetIsTriggered(false);
            }
        }
    }
}
