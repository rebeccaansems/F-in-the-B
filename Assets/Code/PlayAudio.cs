using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAudio : MonoBehaviour {

    public AudioClip[] AudioClips;

    private int currentAudioClip = 0;

    public void Play()
    {
        if(AudioClips.Length > 0)
        { 
            AudioSource.PlayClipAtPoint(AudioClips[currentAudioClip], Camera.main.transform.position, 0.75f);
        }

        currentAudioClip = 0;
    }

    public void ChangeAudio(int num)
    {
        currentAudioClip = num;
    } 
}
