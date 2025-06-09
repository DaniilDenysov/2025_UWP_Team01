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
    [SerializeField] private AudioClipSO background;

    [Header("Positional Sound")]
    [SerializeField] private AudioPlayerItem itemPrefab;
    [SerializeField] private IObjectPool<AudioPlayerItem> audioObjectPool;


    public delegate void AudioPlayer(AudioClipSO clipSO, Vector3 pos);

    private AudioPlayer player;

    [Inject]
    private void Construct(IObjectPool<AudioPlayerItem> audioObjectPool)
    {
        this.audioObjectPool = audioObjectPool;
    }

    private void Awake()
    {
        attachedSource.outputAudioMixerGroup = audioMixers;
        player = PlayOneShot;
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        PlayBackgroudMusic(background);
    }


    private void PlayBackgroudMusic(AudioClipSO clipSO)
    {
        AudioClipSO.Apply(clipSO, attachedSource);
        attachedSource.clip = clipSO.Clip;
        attachedSource.Play();
    }

    public void PlayOneShot(AudioClipSO clipSO)
    {
        AudioClipSO.Apply(clipSO, attachedSource);
        attachedSource.PlayOneShot(clipSO.Clip);
    }

    private void PlayOneShot(AudioClipSO clipSO, Vector3 pos)
    {
        var item = audioObjectPool.GetObject(itemPrefab);
        item.transform.position = pos;
        item.Play(clipSO);
    }
}