using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UIElements;
using Zenject;

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioMixerGroup audioMixers;

    [Header("UI")]
    [SerializeField] private AudioSource attachedSource;

    [Header("Background")]
    [SerializeField] private AudioSource backgroundSource;
    [SerializeField] private AudioClipSO background;


    private void Awake()
    {
        attachedSource.outputAudioMixerGroup = audioMixers;
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        PlayBackgroudMusic(background);
    }


    private void PlayBackgroudMusic(AudioClipSO clipSO)
    {
        AudioClipSO.Apply(clipSO, backgroundSource);
        backgroundSource.clip = clipSO.Clip;
        backgroundSource.Play();
    }

    public void PlayOneShot(AudioClipSO clipSO)
    {
        AudioClipSO.Apply(clipSO, attachedSource);
        attachedSource.PlayOneShot(clipSO.Clip);
    }

    
}