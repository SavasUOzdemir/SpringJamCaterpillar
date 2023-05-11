using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSourceContainer : MonoBehaviour, IMonoBehaviourSingleton
{
    [SerializeField] private AudioSource _playerMovementSource;

    public void Setup()
    {
        if(_playerMovementSource == null)
        {
            Debug.Log("Cannot find _playerMovementSource");
        }
    }
}
