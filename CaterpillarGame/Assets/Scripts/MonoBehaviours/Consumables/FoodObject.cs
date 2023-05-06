using System.Collections.Generic;
using UnityEngine;


public class FoodObject : MonoBehaviour, IConsumable
{
    private float hpRegen;
    private float massRegen = 15f;
    private Rigidbody rb;

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<IConsumer>(out IConsumer consumer))
        {
            ConsumerService consumerService = ServiceLocator.Instance.Get<ConsumerService>();
            consumerService.ConsumeItem(this, consumer);
            Destroy(gameObject);
        }
    }

    public float GetMassRegen()
    {
        return massRegen;
    }
}

