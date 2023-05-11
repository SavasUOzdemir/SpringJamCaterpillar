using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAudioHandler
{
    AudioSource AudioSource { get; }

    void Play();
    void Stop();
}
