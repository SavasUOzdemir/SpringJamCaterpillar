
using UnityEngine;

public class ConsumerService : IGameService
{
    public void ConsumeItem(IConsumable consumable, IConsumer consumer)
    {
        Rigidbody rb = consumer.GetRigidbody();
        float massRegen = consumable.GetMassRegen();

        // play effect
        consumer.SetWeight(rb.mass += massRegen); 
    }
}
