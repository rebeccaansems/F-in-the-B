using UnityEngine;

public class PlayAudio : MonoBehaviour
{
    public AudioClip[] AudioClips;

    private int currentAudioClip = 0;

    public void Play()
    {
        if (AudioClips.Length > 0)
        {
            AudioSource.PlayClipAtPoint(AudioClips[currentAudioClip], Camera.main.transform.position, PlayerStats.s_SFXAudio);
        }
        currentAudioClip = 0;
    }

    public void ChangeAudio(int num)
    {
        currentAudioClip = num;
    }

    public void PlayRandom()
    {
        if (AudioClips.Length > 0)
        {
            AudioSource.PlayClipAtPoint(AudioClips[Random.Range(0, AudioClips.Length)], Camera.main.transform.position, PlayerStats.s_SFXAudio);
        }
    }
}
