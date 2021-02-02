using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SoundUtils
{
   
    // this is for objects with an audioSource. If the object is going to be destroyed, use AudioSource.PlayClipAtPoint
    public static void PlaySound(AudioSource audioSource, AudioClip audioClip)
    {
        if (audioClip == null)
        {
            Debug.LogWarning("audioClip is null!");
            return;
        }
        
        if (audioSource == null)
        {
            Debug.LogWarning("audioSource is null!");
            return;
        }

        audioSource.clip = audioClip;
        audioSource.Play();
    }
}
