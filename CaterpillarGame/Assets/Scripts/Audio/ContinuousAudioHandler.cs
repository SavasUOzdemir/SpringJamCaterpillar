using System.Collections;
using UnityEngine;

public class ContinuousAudioHandler
{
    public AudioSource _audioSource { get; private set; }
    public AudioClip _currentClip { get; private set; }
    public AudioClip[] _clips { get; private set; }
    public int _currentClipIndex { get; private set; } = -1;
    private bool _isPlayingAudio = false;

    public ContinuousAudioHandler(AudioSource audioSource, AudioClip[] clips)
    {
        _audioSource = audioSource;
        _clips = clips;
    }

    public void Stop()
    {
        _audioSource.Stop();
    }

    public void Play(int clipIndex)
    {
        _currentClip = _clips[clipIndex];
        _audioSource.clip = _currentClip;
        _audioSource.Play();
    }

    public IEnumerator ContinuousRandomClipsCoroutine()
    {
        while (true)
        {
            yield return null;
            if (_audioSource.isPlaying == false)
            {
                int currentClipIndex = _currentClipIndex;
                int newClipIndex = PickRandomClip(_clips, currentClipIndex);
                Play(newClipIndex);
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
