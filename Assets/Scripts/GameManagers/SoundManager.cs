using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using Random = UnityEngine.Random;

public class SoundManager : GameManager<SoundManager>
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private AudioSource backgroundAudioSource;

    [SerializeField] private GameObject soundEffectAudioSourcesContainer;
    [SerializeField] private AudioMixerGroup soundEffectAudioMixerGroup;
    [SerializeField] private int initialSoundEffectAudioSourcePoolCount;
    [SerializeField] private float defaultSoundEffectRandomPitchRange;
    private List<AudioSource> soundEffectAudioSourcePool = new List<AudioSource>();

    protected override void Awake()
    {
        base.Awake();

        for (int i = 0; i < initialSoundEffectAudioSourcePoolCount; i++)
        {
            CreateSFXSource();
        }
    }

    public void SetMasterVolume(float volume)
    {
        audioMixer.SetFloat("MasterVolume", volume);
    }

    public void SetBackgroundVolume(float volume)
    {
        audioMixer.SetFloat("BackgroundVolume", volume);
    }

    public void SetSoundEffectVolume(float volume)
    {
        audioMixer.SetFloat("SoundEffectVolume", volume);
    }

    public void PlayBackgroundMusic(AudioClip audioClip)
    {
        backgroundAudioSource.Pause();

        backgroundAudioSource.clip = audioClip;
        backgroundAudioSource.Play();
    }

    public void PlayDelayedBackgroundMusic(AudioClip audioClip, float delay)
    {
        backgroundAudioSource.Pause();

        backgroundAudioSource.clip = audioClip;
        backgroundAudioSource.PlayDelayed(delay);
    }

    public void PlaySoundEffect(AudioClip audioClip)
    {
        AudioSource audioSource = GetSoundEffectAudioSource();

        audioSource.PlayOneShot(audioClip);
    }

    public void PlaySoundEffectWithRandomPitch(AudioClip audioClip, float randomPitchRange)
    {
        AudioSource audioSource = GetSoundEffectAudioSource();

        PlayWithPitch(audioSource, audioClip, randomPitchRange);
    }
    
    public void PlaySoundEffectWithRandomPitch(AudioClip audioClip)
    {
        PlaySoundEffectWithRandomPitch(audioClip, defaultSoundEffectRandomPitchRange);
    }

    private AudioSource GetSoundEffectAudioSource()
    {
        foreach (AudioSource audioSource in soundEffectAudioSourcePool)
        {
            if (audioSource.isPlaying) continue;

            return audioSource;
        }

        return CreateSFXSource();
    }

    private AudioSource CreateSFXSource()
    {
        AudioSource audioSource = soundEffectAudioSourcesContainer.AddComponent<AudioSource>();

        audioSource.outputAudioMixerGroup = soundEffectAudioMixerGroup;
        soundEffectAudioSourcePool.Add(audioSource);

        return audioSource;
    }
    
    private IEnumerator PlayWithPitch(AudioSource audioSource, AudioClip clip, float pitchRange)
    {   
        float originalPitch = audioSource.pitch;
        audioSource.pitch = 1.0f + Random.Range(-pitchRange, pitchRange);
        audioSource.PlayOneShot(clip);

        yield return new WaitForSeconds(clip.length);
        audioSource.pitch = originalPitch;
    }
}
