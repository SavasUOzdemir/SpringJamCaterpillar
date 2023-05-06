using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour, IConsumer
{
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private Light _light;

    [SerializeField] private GameObject _caterpillarMesh;
    [SerializeField] private GameObject _butterflyMesh;
    
    private IEnumerator _massChangeCoroutine;
    private MassChanger _massChanger;

    const float METAMORPHOSIS_THRESHOLD_WEIGHT = 40;

    public void Start()
    {
        _massChanger = new MassChanger(this);
        _massChangeCoroutine = _massChanger.MassChangeCoroutine();

        StartMassChangeCoroutine();
    }

    private void StartMassChangeCoroutine()
    {
        StartCoroutine(_massChangeCoroutine);
    }

    public void SetWeight(float newWeight)
    {
        if(newWeight > METAMORPHOSIS_THRESHOLD_WEIGHT)
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

        _caterpillarMesh.gameObject.SetActive(false);
        _butterflyMesh.gameObject.SetActive(true);
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
