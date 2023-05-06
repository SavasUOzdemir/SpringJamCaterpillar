
using UnityEngine;

public class ConsumerService : IGameService
{
    public void ConsumeItem(IConsumable consumable, IConsumer consumer)
    {
        Rigidbody rb = consumer.GetRigidbody();
        float massRegen = consumable.GetMassRegen();

        if ((rb.mass + massRegen) > 30)
            rb.mass = 30f;
        else
            rb.mass += massRegen;
    }
}
