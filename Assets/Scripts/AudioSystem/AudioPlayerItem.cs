using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(AudioSource))]
public class AudioPlayerItem : MonoBehaviour
{
    private AudioSource audioSource;
    private IObjectPool<AudioPlayerItem> objectPool;

    [Inject]
    private void Construct(IObjectPool<AudioPlayerItem> objectPool)
    {
        this.objectPool = objectPool;
    }

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        this.audioSource = GetComponent<AudioSource>();        
    }


    public async void Play(AudioClipSO clipSO)
    {
        Debug.Log("Played");
        AudioClipSO.Apply(clipSO, audioSource);
        audioSource.clip = clipSO.Clip;
        audioSource.Play();
        await Task.Delay((int)(clipSO.Clip.length * 1100));
        objectPool.ReleaseObject(this);
    }
}
