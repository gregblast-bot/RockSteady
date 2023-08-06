using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicList : MonoBehaviour
{
    public AudioClip[] clips;
    private AudioClip nextClip;
    private AudioSource audioSource;
    public bool randomPlay = false;
    private bool pause = false;
    private int currentClipIndex = 0;
    public float volume;

    private void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }

    void Start()
    {
        audioSource = FindObjectOfType<AudioSource>();
        audioSource.loop = false;
    }

    void Update()
    {
        if (audioSource != null)
        {
            if (!audioSource.isPlaying && pause == false)
            {
                if (randomPlay)
                {
                    nextClip = GetRandomClip();
                }
                else
                {
                    nextClip = GetNextClip();
                }
                SetAudio();
            }

            // Handle switching between tracks
            if (Input.GetKeyDown(KeyCode.N) && pause == false)
            {
                if (randomPlay)
                {
                    nextClip = GetRandomClip();
                }
                else
                {
                    nextClip = GetNextClip();
                }
                SetAudio();
            }
            else if (Input.GetKeyDown(KeyCode.B) && pause == false)
            {
                if (randomPlay)
                {
                    nextClip = GetRandomClip();
                }
                else
                {
                    nextClip = GetLastClip();
                }
                SetAudio();
            }

            if (Input.GetKeyDown(KeyCode.P))
            {
                PauseAudio();
            }
            else if (Input.GetKeyDown(KeyCode.O))
            {
                UnPauseAudio();
            }
        }
    }

    private AudioClip GetRandomClip()
    {
        return clips[UnityEngine.Random.Range(0, clips.Length)];
    }

    private AudioClip GetNextClip()
    {
        return clips[(currentClipIndex + 1) % clips.Length];
    }

    private AudioClip GetLastClip()
    {
        return clips[(currentClipIndex - 1) % clips.Length];
    }

    private void SetAudio()
    {
        currentClipIndex = Array.IndexOf(clips, nextClip);
        audioSource.clip = nextClip;
        audioSource.volume = volume;
        audioSource.Play();
    }

    private void PauseAudio()
    {
        pause = true;
        audioSource.Pause();
    }

    private void UnPauseAudio()
    {
        pause = false;
        audioSource.UnPause();
    }
}
