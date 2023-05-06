using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public float _DangerMeter { get; private set; }
    public bool _InDangerZone { get; set; }
    [SerializeField] private float _waitSeconds = 1;
    [SerializeField] private float _dangerZoneTickingDamage;
    public void DangerMeterChange(float dangermeterchange)
    {
        _DangerMeter += dangermeterchange;
        return;
    }

    private void Die() 
    {
        
    }

    IEnumerator DamageTick()
    {
        while (true)
        {
            yield return new WaitForSeconds(_waitSeconds);
            DangerMeterChange(_dangerZoneTickingDamage);
        }
    }
}
