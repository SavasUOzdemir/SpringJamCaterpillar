using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [field: SerializeField] public float _DangerMeter  { get; private set; }
    public bool _InDangerZone { get; set; }
    [SerializeField] private float _waitSeconds = 1f;
    [SerializeField] private float _dangerZoneTickingDamage = 15f;
    [SerializeField] private float _safeZoneTickingHeal = -5f;
    private Coroutine _tickingCoroutineReference;
    [SerializeField] private Light _light;
    [SerializeField] GameObject safeZoneObject;
    const string SAFEZONETAG = "SafeZone";
    const float MAXDANGERMETER = 150f;
    public delegate void PlayerBelowThreshold(bool isTrue);
    public event PlayerBelowThreshold PlayerBelowThresholdEvent;

    public delegate void PlayerDeath();
    public event PlayerDeath PlayerDeathEvent;
    public void DangerMeterChange(float dangermeterchange)
    {
        _DangerMeter += dangermeterchange;
        if (_DangerMeter > MAXDANGERMETER)
            _DangerMeter = MAXDANGERMETER;
        if (_DangerMeter <0.05f)
            _DangerMeter = 0;
        if (MAXDANGERMETER - _DangerMeter <= 0.05f)
            PlayerBelowThresholdEvent?.Invoke(true);
        else
            PlayerBelowThresholdEvent?.Invoke(false);
    }

    private void FixedUpdate()
    {
        if (_light.intensity > .4f)
            safeZoneObject.SetActive(true);
        else 
        { 
            safeZoneObject.SetActive(false);
            _InDangerZone = true;
        }
    }

    private void Start()
    {
        _InDangerZone = false;
        _DangerMeter = 0f;
        _tickingCoroutineReference = StartCoroutine(DamageTick());
    }

    private void Die()
    {
        Time.timeScale = 0f;
        PlayerDeathEvent?.Invoke();
        //some logic for antagonist to lock onto target and swoop in
        //animator.play("die");
     //   StartCoroutine(DeathTimer(.5f));
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
                DangerMeterChange(_safeZoneTickingHeal);
            }
            yield return new WaitForSeconds(_waitSeconds);
            DangerMeterChange(_dangerZoneTickingDamage);
        }
    }

    IEnumerator DeathTimer (float time)
    {
        yield return new WaitForSeconds(time);

        Time.timeScale = 0f;
        Destroy(gameObject);
        //some logic to popup a death UI indicator
        yield break;
    }
}
