using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveableObject : MonoBehaviour
{
    [SerializeField] Material m_Material;
    [SerializeField] float forceMagnitude = 3f;
    [SerializeField] float thresholdPlayerMass = 15f;
    Vector3 direction;
    Rigidbody rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        collision.gameObject.GetComponent<BoxCollider>().enabled = true;
    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.layer != LayerMask.NameToLayer("Ground"))
        {
            if (collision.gameObject.GetComponent<Rigidbody>().mass >= thresholdPlayerMass)
            {
                if (Input.GetKey(KeyCode.W))
                {
                    direction = -(collision.transform.position - transform.position).normalized;
                    float distance = (collision.transform.position - transform.position).magnitude;
                }
                else
                    direction = Vector3.zero;
                Vector3 targetPosition = transform.position + (direction * Time.deltaTime) * forceMagnitude;
                rb.MovePosition(targetPosition);
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        direction = Vector3.zero;
        collision.gameObject.GetComponent<BoxCollider>().enabled = false;
    }
}