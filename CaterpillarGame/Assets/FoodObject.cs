using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodObject : MonoBehaviour
{
    float hpRegen;
    float massRegen = 15f;
    Rigidbody rb;
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;
        rb = other.GetComponent<Rigidbody>();
        if ((rb.mass+massRegen)>30)
            rb.mass = 30f;
        else
            rb.mass += massRegen;
        Destroy(gameObject);
    }
}
