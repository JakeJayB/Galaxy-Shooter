using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private AudioSource _audioSource;
    [SerializeField]
    private AudioClip _explosionSoundClip;
    [SerializeField]
    private AudioClip _powerUpSoundClip;

    void Start()
    {
        _audioSource = GetComponent<AudioSource>();

        if (_audioSource == null)
        {
            Debug.LogError("Audio Source is NULL");
        }
        
    }


    public void ExplosionAudioClip()
    {
        _audioSource.clip = _explosionSoundClip;
        _audioSource.Play();
    }

    public void PowerUpAudioClip()
    {
        _audioSource.clip = _powerUpSoundClip;
        _audioSource.Play();
    }
}
