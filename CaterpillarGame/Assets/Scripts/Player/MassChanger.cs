using System.Collections;
using UnityEngine;

public class MassChanger
{
    private float massReductionFactor = .2f;

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
            DoScaleCalculation();
            DoLightIntensityCalculation();

            _consumabilityService.UpdateConsumability(_rigidbody, oldMass);
        }
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
