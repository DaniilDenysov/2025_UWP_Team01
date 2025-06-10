using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusicChanger : MonoBehaviour
{
    [SerializeField] private AudioClipSO[] backgroundMusic;
    
    private SoundManager soundManager;
    private void Start()
    {
        soundManager = FindObjectOfType<SoundManager>();

        if (soundManager == null)
        {
            Debug.Log("Sound Manager not found");
            return;
        }
    }

    public void ChangeBackgroundMusic(int index)
    {
        if (index < backgroundMusic.Length)
        {
            soundManager.PlayBackgroudMusic(backgroundMusic[index]);
        }   
    }
}
