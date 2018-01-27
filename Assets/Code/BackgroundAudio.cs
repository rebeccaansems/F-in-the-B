using System.Collections;
using UnityEngine;

public class BackgroundAudio : MonoBehaviour
{
    public AudioClip[] BackgroundMusic;

    private AudioSource aud;
    private float currVolume;
    private int currSong;

    private static BackgroundAudio bgAudioObject;

    void Awake()
    {
        DontDestroyOnLoad(this);

        if (bgAudioObject == null)
        {
            bgAudioObject = this;
        }
        else
        {
            DestroyObject(gameObject);
        }
    }

    void Start()
    {
        currSong = Random.Range(0, BackgroundMusic.Length);
        aud = GetComponent<AudioSource>();

        if (!aud.isPlaying)
        {
            aud.clip = BackgroundMusic[currSong];
            aud.playOnAwake = true;
            aud.loop = true;
            aud.Play();
            aud.volume = PlayerStats.s_MusicAudio;
            currVolume = PlayerStats.s_MusicAudio;

            StartCoroutine(PlayBackgroundMusic());
        }
    }

    IEnumerator PlayBackgroundMusic()
    {
        yield return new WaitForSeconds(aud.clip.length);
        currSong = Random.Range(0, BackgroundMusic.Length);
        aud.clip = BackgroundMusic[currSong];
        aud.Play();
        PlayBackgroundMusic();
    }

    public void Update()
    {
        if (currVolume != PlayerStats.s_MusicAudio)
        {
            aud.volume = PlayerStats.s_MusicAudio;
            currVolume = PlayerStats.s_MusicAudio;
        }
    }
}