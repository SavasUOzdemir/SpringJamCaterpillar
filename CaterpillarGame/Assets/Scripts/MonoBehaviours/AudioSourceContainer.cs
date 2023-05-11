using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSourceContainer : MonoBehaviour, IMonoBehaviourSingleton
{
    [SerializeField] private AudioSource _playerEatingSource;
    [SerializeField] private AudioSource _playerMovementSource;

    [SerializeField] private AudioClip[] _playerMovementAudioClips;
    [SerializeField] private AudioClip[] _playerEatingAudioClips;

    public void Awake()
    {
        if (_playerEatingSource == null)
        {
            Debug.LogError("Cannot find _playerEatingSource");
        }
        if (_playerMovementSource == null)
        {
            Debug.LogError("Cannot find _playerMovementSource");
        }
        if (_playerMovementAudioClips.Length == 0)
        {
            Debug.LogError("Cannot find any audio clips for movement");
        }
        if (_playerEatingAudioClips.Length == 0)
        {
            Debug.LogError("Cannot find any audio clips for eating");
        }

        MonoBehaviourLocator.Instance.Register<AudioSourceContainer>(this);
    }

    public AudioSource GetAudioSource(AudioType audioType)
    {
        switch (audioType)
        {
            case AudioType.PlayerEating:
                return _playerEatingSource;
            case AudioType.PlayerWalking:
                return _playerMovementSource;
            default:
                throw new NotImplementedException();
        }
    }

    public AudioClip[] GetPlayerEatinglips()
    {
        return _playerEatingAudioClips;
    }

    public AudioClip[] GetPlayerMovementClips()
    {
        return _playerMovementAudioClips;
    }
}
