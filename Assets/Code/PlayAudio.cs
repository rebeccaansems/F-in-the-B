using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAudio : MonoBehaviour {

    public int CurrentAudioClip = 0;
    public AudioClip[] AudioClips;
    
    public void Play()
    {
        if(AudioClips.Length > 0)
        {
            AudioSource.PlayClipAtPoint(AudioClips[CurrentAudioClip], Camera.main.transform.position, 0.75f);
        }
    }
}
