using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] public float _dangerMeter  { get; private set; }
    public bool _InDangerZone { get; set; }
    [SerializeField] private float _waitSeconds = 1f;
    private float _dangerZoneTickingDamage = 5f;
    private float _safeZoneTickingHeal = -2.5f;
    private Coroutine _tickingCoroutineReference;
    [SerializeField]lb_Bird_Game birdScript;

    const string SAFEZONETAG = "SafeZone";
    const float MAXDANGERMETER = 100f;

    public void DangerMeterChange(float dangerMeterChange)
    {
        _dangerMeter += dangerMeterChange;
        if (_dangerMeter > MAXDANGERMETER)
        {
            _dangerMeter = MAXDANGERMETER;
        }
        if (_dangerMeter < 0)
        {
            _dangerMeter = 0;
        }

        if (_dangerMeter >= 95f)
        {
            if(!birdScript.GetAggroToCharacter())
            {
                birdScript.SetAggroToCharacter(true);
            }
        }
        else if (birdScript.GetAggroToCharacter())
        {
            birdScript.SetAggroToCharacter(false);
        }

        return;
    }

    private void Start()
    {
        _InDangerZone = true;
        _dangerMeter = 0;
        _tickingCoroutineReference = StartCoroutine(DamageTick());
    }

    private void Die()
    {
        //some logic for antagonist to lock onto target and swoop in
        //animator.play("die");
        StartCoroutine(DeathTimer(.5f));
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
