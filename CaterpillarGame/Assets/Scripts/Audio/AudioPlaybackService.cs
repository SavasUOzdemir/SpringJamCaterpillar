using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlaybackService : IGameService
{
    private AudioSourceContainer _audioSourceContainer;

    public void Initialise()
    {
        _audioSourceContainer = MonoBehaviourLocator.Instance.Get<AudioSourceContainer>();
    }
}
