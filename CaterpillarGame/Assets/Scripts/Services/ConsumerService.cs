using System.Collections.Generic;
using UnityEngine;

public class ConsumerService : IGameService
{
    private ConsumableSpawnpoint _lastConsumedSpawnpoint;

    public void ConsumeItem(IConsumable consumable, IConsumer consumer)
    {
        Rigidbody rigidBody = consumer.GetRigidbody();
        float consumerMass = rigidBody.mass;

        float requiredMass = consumable.GetRequiredMassForConsumableType();

        if (consumerMass < requiredMass)
        {
            return;
        }

        float massRegen = consumable.GetMassRegen();

        // play effects here
        AudioPlaybackService audioPlaybackService = ServiceLocator.Instance.Get<AudioPlaybackService>();
        audioPlaybackService.PlaySingleShot(AudioType.PlayerEating);

        // set weight
        consumer.SetWeight(rigidBody.mass += massRegen);

        ConsumabilityService consumabilityService = ServiceLocator.Instance.Get<ConsumabilityService>();
        consumabilityService.Deregister(consumable);
        consumabilityService.UpdateConsumability(rigidBody, consumerMass);

        ConsumableSpawnpoint consumableSpawnpoint = consumable.GetSpawnpoint();
        consumableSpawnpoint.SetConsumable(null);
        _lastConsumedSpawnpoint = consumableSpawnpoint;


        consumable.Destroy();
    }

    public ConsumableSpawnpoint GetLastConsumedSpawnpoint()
    {
        return _lastConsumedSpawnpoint;
    }
}
