using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class SFXManager : MonoBehaviour
{
    public static SFXManager Instance { get; private set; }

    [Header("Positional Sound")]
    [SerializeField] private AudioPlayerItem itemPrefab;
    private IObjectPool<AudioPlayerItem> audioObjectPool;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    [Inject]
    private void Construct(IObjectPool<AudioPlayerItem> audioObjectPool)
    {
        this.audioObjectPool = audioObjectPool;
    }


    public void PlayOneShot(AudioClipSO clipSO, Vector3 pos)
    {
        var item = audioObjectPool.GetObject(itemPrefab);
        item.transform.position = pos;
        item.Play(clipSO);
    }
}
