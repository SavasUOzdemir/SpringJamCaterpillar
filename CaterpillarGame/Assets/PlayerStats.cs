using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public float _DangerMeter { get; private set; }
    public bool _InDangerZone { get; set; }
    [SerializeField] private float _waitSeconds = 1;
    [SerializeField] private float _dangerZoneTickingDamage;
    private Coroutine _tickingCoroutineReference;

    const string SAFEZONETAG = "SafeZone";
    public void DangerMeterChange(float dangermeterchange)
    {
        _DangerMeter += dangermeterchange;
        return;
    }

    private void Start()
    {
        _tickingCoroutineReference = StartCoroutine(DamageTick());
    }

    private void Die()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(SAFEZONETAG))
            return;
        _InDangerZone = false;
    }

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag(SAFEZONETAG) || !_InDangerZone)
            return;
        _InDangerZone = false;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag(SAFEZONETAG))
            return;
        _InDangerZone = true;
    }

    IEnumerator DamageTick()
    {
        while (true)
        {
            while (!_InDangerZone) 
            { 
                yield return new WaitForSeconds(_waitSeconds);
            }
            yield return new WaitForSeconds(_waitSeconds);
            DangerMeterChange(_dangerZoneTickingDamage);
        }
    }
}
