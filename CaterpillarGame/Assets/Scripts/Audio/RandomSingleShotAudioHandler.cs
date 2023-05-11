using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSingleShotAudioHandler : IAudioHandler
{
    public AudioSource AudioSource { get; private set; }
    public AudioClip[] Clips { get; private set; }

    public RandomSingleShotAudioHandler(AudioSource audioSource, AudioClip[] audioClips)
    {
        AudioSource = audioSource;
        Clips = audioClips;
    }

    public void Play()
    {
        AudioClip clip = PickRandomClip(Clips);
        AudioSource.clip = clip;
        AudioSource.Play();
    }

    public void Stop()
    {
        AudioSource.Stop();
    }

    private AudioClip PickRandomClip(AudioClip[] clips)
    {
        if (clips.Length == 0)
        {
            Debug.LogError("clip list is empty!");
        }

        if (clips.Length == 1)
        {
            return clips[0];
        }

        int randomClipIndex = UnityEngine.Random.Range(0, clips.Length);

        return clips[randomClipIndex];
    }
}
