using System.Collections;
using UnityEngine;

public class MassChanger
{
    private float massReductionFactor = 1f;

    const float LIGHT_MAX = 2.1f;
    const float LIGHT_MIN = 0.5f;
    const float SCALE_MAX = 1.5f;
    const float SCALE_MIN = 0.6f;

    private PlayerCharacter _playerCharacter;
    private Rigidbody _rigidbody;
    private ConsumabilityService _consumabilityService;

    public bool IsCalculating = false;

    public MassChanger(PlayerCharacter playerCharacter)
    {
        _playerCharacter = playerCharacter;
        _rigidbody = playerCharacter.GetRigidbody();
    }

    public IEnumerator MassChangeCoroutine()
    {
        if (_consumabilityService == null)
        {
            _consumabilityService = ServiceLocator.Instance.Get<ConsumabilityService>();
        }

        while (_rigidbody.mass >= 1)
        {
            yield return new WaitForFixedUpdate();

            float oldMass = _rigidbody.mass;
            float newMass = _rigidbody.mass - Time.fixedDeltaTime / massReductionFactor;
            _playerCharacter.SetWeight(newMass);

            float desiredScale = CalculateScale();
            SetPlayerScale(desiredScale);

            float desiredLightIntensity = CalculateLightIntensity();
            SetLightIntensity(desiredLightIntensity);

            float desiredSpeed = CalculateSpeed();
            _playerCharacter.SetSpeed(desiredSpeed);

            _consumabilityService.UpdateConsumability(_rigidbody, oldMass);
        }
    }   

    private float CalculateScale()
    {
        float targetScale = _rigidbody.mass / 30f > SCALE_MIN ? _rigidbody.mass / 30 : SCALE_MIN;
        float scaleFactor = (Mathf.Clamp(targetScale, SCALE_MIN, SCALE_MAX));

        return scaleFactor;
    }

    private void SetPlayerScale(float scaleFactor)
    {
        _playerCharacter.transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);
    }

    private float CalculateLightIntensity()
    {
        float mass = _rigidbody.mass;

        float targetIntensity = mass / 10f < LIGHT_MAX ? mass/10f: LIGHT_MAX;
        float clampedIntensity = Mathf.Clamp(targetIntensity, LIGHT_MIN, LIGHT_MAX);

        return clampedIntensity;
    }

    private void SetLightIntensity(float newIntensity)
    {
        Light light = _playerCharacter.GetLight();
        light.intensity = newIntensity;
    }

    private float CalculateSpeed()
    {
        float mass = _rigidbody.mass;
        float massMax = PlayerCharacter.METAMORPHOSIS_THRESHOLD_WEIGHT;

        float maxSpeed = 7;
        float minSpeed = 4;

        float desiredSpeed = maxSpeed - ((maxSpeed - minSpeed) * ((mass - 1) / (massMax - 1)));

        return desiredSpeed;
    }
}
    