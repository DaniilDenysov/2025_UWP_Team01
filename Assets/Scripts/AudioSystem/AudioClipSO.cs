using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(fileName = "create new SoundConfig", menuName = "SoundConfig")]
public class AudioClipSO : ScriptableObject
{
    [SerializeField] private AudioClip audioClip;
    public AudioClip Clip { get => audioClip; }

    [SerializeField] private AudioMixerGroup audioMixer;
    public AudioMixerGroup AudioMixer
    {
        get => audioMixer;
    }

    [Header("Core Audio")]
    [SerializeField, Range(0, 1)] private float volume = 1 ;
    public float Volume { get => volume; }

    [SerializeField, Range(0f, 1f)] private float spatialBlend = 0f;
    public float SpatialBlend { get => spatialBlend; }

    [Header("Playback Settings")]
    [SerializeField] private bool playOnAwake;
    public bool PlayOnAwake { get => playOnAwake; }

    [SerializeField] private bool loop = false;
    public bool Loop { get => loop; }


    [Header("Pitch & Variation")]
    [SerializeField, Range(-3f, 3f)] private float pitch = 1f;
    public float Pitch { get => pitch; }

    [Header("Advanced")]
    [SerializeField] private bool bypassEffects = false;
    public bool BypassEffects { get => bypassEffects; }

    [SerializeField] private bool reverbBypassEffect = false;
    public bool ReverbBypassEffect { get => reverbBypassEffect; }

    public static void Apply(AudioClipSO configSO, AudioSource source)
    {
        if (source == null || configSO == null) return;
        source.outputAudioMixerGroup = configSO.audioMixer;
        source.volume = configSO.Volume;
        source.bypassEffects = configSO.BypassEffects;
        source.bypassReverbZones = configSO.ReverbBypassEffect;
        source.spatialBlend = configSO.SpatialBlend;
        source.loop = configSO.Loop;
        source.playOnAwake = configSO.PlayOnAwake;
    }
}
