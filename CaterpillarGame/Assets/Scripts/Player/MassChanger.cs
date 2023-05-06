using System.Collections;
using UnityEngine;

public class MassChanger
{
    private float massReductionFactor = 2;

    const float LIGHT_MAX = 2.1f;
    const float LIGHT_MIN = 0.5f;
    const float SCALE_MAX = 1.5f;
    const float SCALE_MIN = 0.6f;

    private PlayerCharacter _playerCharacter;
    private Rigidbody _rigidbody;

    public MassChanger(PlayerCharacter playerCharacter)
    {
        _playerCharacter = playerCharacter;
        _rigidbody = playerCharacter.GetRigidbody();
    }

    public IEnumerator MassChangeCoroutine()
    {
        while (_rigidbody.mass >= 1)
        {
            yield return new WaitForFixedUpdate();

            _rigidbody.mass -= Time.fixedDeltaTime / massReductionFactor;
            DoScaleCalculation();
            DoLightIntensityCalculation();

        }
        if (_rigidbody.mass < 1)
            _rigidbody.mass = 1;
    }

    private void DoScaleCalculation()
    {
        float targetScale = _rigidbody.mass / 30f > SCALE_MIN ? _rigidbody.mass / 30 : SCALE_MIN;
        float scaleFactor = (Mathf.Clamp(targetScale, SCALE_MIN, SCALE_MAX));
        _playerCharacter.transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);
    }

    private void DoLightIntensityCalculation()
    {
        float mass = _rigidbody.mass;
        Light light = _playerCharacter.GetLight();

        float targetIntensity = mass / 10f < LIGHT_MAX ? mass/10f: LIGHT_MAX;
        float clampedIntensity = Mathf.Clamp(targetIntensity, LIGHT_MIN, LIGHT_MAX);
        light.intensity = clampedIntensity;
    }
}
