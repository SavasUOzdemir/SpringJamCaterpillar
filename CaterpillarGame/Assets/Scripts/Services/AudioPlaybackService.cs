using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlaybackService : IGameService
{
    private AudioSourceContainer _audioSourceContainer;
    private ContinuousAudioHandler _playerMovementAudioHandler;
    private RandomSingleShotAudioHandler _playerEatingAudioHandler;

    public void Initialise()
    {
        _audioSourceContainer = MonoBehaviourLocator.Instance.Get<AudioSourceContainer>();

        AudioSource playerMovementSource = _audioSourceContainer.GetAudioSource(AudioType.PlayerWalking);
        AudioClip[] playerMovementClips = _audioSourceContainer.GetPlayerMovementClips();

        _playerMovementAudioHandler = new ContinuousAudioHandler(playerMovementSource, playerMovementClips);

        AudioSource playerEatingSource = _audioSourceContainer.GetAudioSource(AudioType.PlayerEating);
        AudioClip[] playerEatingClips = _audioSourceContainer.GetPlayerEatinglips();

        _playerEatingAudioHandler = new RandomSingleShotAudioHandler(playerEatingSource, playerEatingClips);
    }

    public IEnumerator GetPlayerMovementAudioCoroutine()
    {
        return _playerMovementAudioHandler.ContinuousRandomClipsCoroutine();
    }

    public void PlaySingleShot(AudioType audioType)
    {
        switch (audioType)
        {
            case AudioType.PlayerEating:
                _playerEatingAudioHandler.Play();
                break;
            default:
                throw new NotImplementedException();
        }
    }

    public void StopAudio(AudioType audioType)
    {
        switch (audioType)
        {
            case AudioType.PlayerWalking:
                _playerMovementAudioHandler.Stop();
                break;
            case AudioType.PlayerEating:
                _playerEatingAudioHandler.Stop();
                break;
            default:
                throw new NotImplementedException();
        }
    }
}
