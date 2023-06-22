using UnityEngine;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;
public class AudiosSequence : MonoBehaviour
{

    public AudioSource audioSource;
    public List<AudioClip> audioClips;
    private int currentClipIndex = 0;

    void Start()
    {
        PlayNextClip();
    }

    void PlayNextClip()
    {
        if (currentClipIndex < audioClips.Count)
        {
            audioSource.clip = audioClips[currentClipIndex];
            audioSource.Play();
            currentClipIndex++;
        }
        else
        {
            audioSource.Stop();
            
            // Finalizar el programa Unity
            #if UNITY_EDITOR
                UnityEditor.EditorApplication.ExitPlaymode();
            #else
                Application.Quit();
            #endif
        }
    }
    void Update()
    {
        if (!audioSource.isPlaying)
        {
            PlayNextClip();
        }
    }   


}

