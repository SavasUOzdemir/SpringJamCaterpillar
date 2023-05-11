using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSourceContainer : MonoBehaviour, IMonoBehaviourSingleton
{
    [SerializeField] private AudioSource _playerMovementSource;

    [SerializeField] private AudioClip[] _movementAudioClips;

    public void Awake()
    {
        if (_playerMovementSource == null)
        {
            Debug.LogError("Cannot find _playerMovementSource");
        }
        if (_movementAudioClips.Length == 0)
        {
            Debug.LogError("Cannot find any audio clips for movement");
        }

        MonoBehaviourLocator.Instance.Register<AudioSourceContainer>(this);
    }

    public AudioSource GetPlayerMovementSource()
    {
        return _playerMovementSource;
    }

    public AudioClip[] GetPlayerMovementClips()
    {
        return _movementAudioClips;
    }
}
