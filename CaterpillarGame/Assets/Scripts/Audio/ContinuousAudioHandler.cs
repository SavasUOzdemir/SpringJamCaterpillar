using System.Collections;
using UnityEngine;

public class ContinuousAudioHandler : IAudioHandler
{
    public AudioSource AudioSource { get; private set; }
    public AudioClip CurrentClip { get; private set; }
    public AudioClip[] Clips { get; private set; }
    public int CurrentClipIndex { get; private set; } = -1;

    public ContinuousAudioHandler(AudioSource audioSource, AudioClip[] clips)
    {
        AudioSource = audioSource;
        Clips = clips;
    }

    public void Stop()
    {
        AudioSource.Stop();
    }

    public void Play()
    {
        CurrentClip = Clips[CurrentClipIndex];
        AudioSource.clip = CurrentClip;
        AudioSource.Play();
    }

    public IEnumerator ContinuousRandomClipsCoroutine()
    {
        while (true)
        {
            yield return null;
            if (AudioSource.isPlaying == false)
            {
                int currentClipIndex = CurrentClipIndex;
                CurrentClipIndex = PickRandomClip(Clips, currentClipIndex);
                Play();
            }
        }
    }

    private int PickRandomClip(AudioClip[] clips, int currentClipIndex)
    {
        if (clips.Length == 0)
        {
            Debug.LogError("clip list is empty!");
        }

        if (clips.Length == 1)
        {
            return 0;
        }

        int randomClipIndex = currentClipIndex;
        while (randomClipIndex == currentClipIndex)
        {
            randomClipIndex = UnityEngine.Random.Range(0, clips.Length);
        }
        currentClipIndex = randomClipIndex;
        return currentClipIndex;
    }
}
