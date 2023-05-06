using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MassChanger : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField] float massReductionFactor = 5;
    [SerializeField] Light _light;

    const float LIGHT_MAX = 2.1f;
    const float LIGHT_MIN = 0.5f;
    const float SCALE_MAX = 1.5f;
    const float SCALE_MIN = 0.6f;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    void Start()
    {
        MassChangeCoroutineStarter();
    }

    void MassChangeCoroutineStarter()
    {
        StartCoroutine(MassChangeCoroutine());
    }

    IEnumerator MassChangeCoroutine()
    {
        while (rb.mass >= 1)
        {
            yield return new WaitForFixedUpdate();
            rb.mass -= Time.fixedDeltaTime/ massReductionFactor;
            DoScaleCalculation(rb.mass);
            DoLightIntensityCalculation(rb.mass);

        }
        if (rb.mass < 1)
            rb.mass = 1;
    }

    void DoScaleCalculation(float mass)
    {
        float targetScale = rb.mass / 30f > SCALE_MIN ? rb.mass / 30 : SCALE_MIN;
        float scaleFactor = (Mathf.Clamp(targetScale, SCALE_MIN, SCALE_MAX));
        transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);
    }
    void DoLightIntensityCalculation(float mass)
    {
        float targetIntensity = mass / 10f < LIGHT_MAX ? mass/10f: LIGHT_MAX;
        float clampedIntensity = Mathf.Clamp(targetIntensity, LIGHT_MIN, LIGHT_MAX);
        _light.intensity = clampedIntensity;
    }
}
