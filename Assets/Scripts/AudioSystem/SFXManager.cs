using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class SFXManager : MonoBehaviour
{
    [Header("Positional Sound")]
    [SerializeField] private AudioPlayerItem itemPrefab;
    private IObjectPool<AudioPlayerItem> audioObjectPool;

    public delegate void AudioPlayer(AudioClipSO clipSO, Vector3 pos);

    private AudioPlayer player;

    [Inject]
    private void Construct(IObjectPool<AudioPlayerItem> audioObjectPool)
    {
        this.audioObjectPool = audioObjectPool;
    }

    private void Awake()
    {
        player = PlayOneShot;
    }

    private void PlayOneShot(AudioClipSO clipSO, Vector3 pos)
    {
        var item = audioObjectPool.GetObject(itemPrefab);
        item.transform.position = pos;
        item.Play(clipSO);
    }
}
