using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AudioPlaybackService : IGameService
{
    private AudioSourceContainer _audioSourceContainer;
    private ContinuousAudioHandler _playerMovementAudioHandler;

    public void Initialise()
    {
        _audioSourceContainer = MonoBehaviourLocator.Instance.Get<AudioSourceContainer>();

        AudioSource playerMovementSource = _audioSourceContainer.GetPlayerMovementSource();
        AudioClip[] playerMovementClips = _audioSourceContainer.GetPlayerMovementClips();

        _playerMovementAudioHandler = new ContinuousAudioHandler(playerMovementSource, playerMovementClips);
    }

    public IEnumerator GetPlayerMovementAudioCoroutine()
    {
        return _playerMovementAudioHandler.ContinuousRandomClipsCoroutine();
    }

    public void StopAudio(AudioType audioType)
    {
        switch (audioType)
        {
            case AudioType.PlayerWalking:
                _playerMovementAudioHandler.Stop();
                break;
            default:
                throw new NotImplementedException();
        }
    }
}

public enum AudioType
{
    PlayerWalking
}