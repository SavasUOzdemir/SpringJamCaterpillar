using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour, IConsumer
{
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private Light _light;
    private IEnumerator _myCoroutine;
    private MassChanger _massChanger;

    const float _metamorphosisThresholdWeight = 40;

    public void Start()
    {
        _massChanger = new MassChanger(this);
        _myCoroutine = _massChanger.MassChangeCoroutine();

        StartMassChangeCoroutine();
    }

    private void StartMassChangeCoroutine()
    {
        StartCoroutine(_myCoroutine);
    }

    public void SetWeight(float newWeight)
    {
        if(newWeight > _metamorphosisThresholdWeight)
        {
            BecomeButterfly();
        }
        else
        {
            _rigidbody.mass = newWeight;
        }
    }

    private void BecomeButterfly()
    {
        Debug.LogWarning("I became a butterfly!");
    }

    public Rigidbody GetRigidbody()
    {
        return _rigidbody;
    }

    public Light GetLight()
    {
        return _light;
    }
}
