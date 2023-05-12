using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using Cinemachine;


public class BokehFocus : MonoBehaviour
{
    [SerializeField] Volume _vol;
    [SerializeField] CinemachineFreeLook _camera;
    [SerializeField] Component component;
    [SerializeField] VolumeProfile volumeProfile;
    [SerializeField] DepthOfField _dof;
    [SerializeField] Vector3 _vec = new Vector3(0.6f, .6f);

    private void Awake()
    {
    }
    private void Start()
    {
        volumeProfile = _vol.sharedProfile;

    }
    void ResetVec()
    {
        _vec = new Vector3(.6f, .6f);
    }
    private void LateUpdate()
    {
        if (volumeProfile.TryGet(out _dof))
        {
            _dof.focusDistance.value = Mathf.Lerp(_dof.focusDistance.value, GetDistance(),5*Time.deltaTime);
            _dof.aperture.value=Mathf.Lerp(_dof.aperture.value, GetAperture(),5*Time.deltaTime);
        } 
    }

    private float GetDistance()
    {
        switch (_camera.transform.position.y)
        {
            case > 10: return 15;
            case < 3: return 150;
            case >= 3: return 18;
                default: return 18;
        } 
    }

    private float GetAperture()
    {
        switch (_camera.transform.position.y)
        {
            case > 9: return 15;
            case < 3: return 32;
            case < 6: return 25;
            default: return 18;

        }
    }

}
