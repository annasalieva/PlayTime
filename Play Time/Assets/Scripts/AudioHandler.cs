using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class AudioHandler : MonoBehaviour
{
    public static AudioHandler StaticAudioHandler;
    public AudioSource Background;
    public AudioSource clickUI;
    public AudioLibrary[] Tracks;


    public bool LoopBackground = true;
    public string DefaultBackgroundClip;

    [Range(0, 1)] public float MasterVolume = 1;

    private float masterVolume;
    private bool mute = false;

    private AudioLibrary backgroundTrack = null;
    private AudioLibrary clicksound = null;

    public bool EnterLemonTest = false;

    void Start()
    {
        SetVolume(MasterVolume);
        if (StaticAudioHandler == null)
        {
            DontDestroyOnLoad(gameObject);
            StaticAudioHandler = this;
        }
        else Destroy(gameObject);

        StartBackground(DefaultBackgroundClip);

    }
    private void Update()
    {
        Debug.Log(SceneManager.GetActiveScene().name);
        CheckSceneMusic();
        
    }
    //MUTE
    public static void Mute()
    {
        StaticAudioHandler.Mute(!StaticAudioHandler.mute);
    }
    public void Mute(bool State)
    {
        mute = State;
        if (mute)
        {
            masterVolume = 0;
        }
        else masterVolume = MasterVolume;
        if (backgroundTrack != null)
        {
            Background.volume = masterVolume * backgroundTrack.volume;
        }
        if (clicksound != null)
        {
            clickUI.volume = masterVolume * clicksound.volume;
        }
    }

    //PLAY PAUSE MENU MUSIC WHEN PAUSE MENU ON
    public void PlayPauseMenuMusic()
    {
        StartBackground("pause");
    }

    //CHECK SCENE
    public void CheckSceneMusic()
    {
        if (SceneManager.GetActiveScene().name == "LemonTest")
        {
            PlayChestMusic();
        }
        
    }

    // CHANGE THE BACKGROUNDMUSIC OF LEMON TEST
    public void PlayChestMusic()
    {
        if (!EnterLemonTest)
        {
            StartBackground("chest");
            EnterLemonTest = true;
        }

    }

    //SET VOLUME
    public void SetVolume(float volume)
    {
        MasterVolume = volume;
        masterVolume = MasterVolume;
        if (backgroundTrack != null) Background.volume = backgroundTrack.volume * masterVolume;
    }

    //PLAY CLICK SOUND
    private void StartClickSound(string clipname)
    {
        AudioLibrary track = getTrack(clipname);
        clicksound = track;
        clickUI.clip = track.Track;
        clickUI.Play();
        clickUI.volume = masterVolume * track.volume;
    }

    public static void startClickSound(string clipname)
    {
        StaticAudioHandler.StartClickSound(clipname);
    }


    //PLAY BACKGROUND MUSIC
    private void StartBackground(string clipname)
    {
        AudioLibrary track = getTrack(clipname);
        backgroundTrack = track;
        Background.clip = track.Track;
        Background.Play();
        Background.loop = LoopBackground;
        Background.volume = masterVolume * track.volume;
    }
    public static void startBackground(string clipname)
    {
        StaticAudioHandler.StartBackground(clipname);
    }

    //GET TRACK FROM AUDIO LIBRARY
    public AudioLibrary getTrack(string name)
    {
        AudioClip clip = null;

        foreach (AudioLibrary track in Tracks)
        {
            if (name == track.Name)
            {
                return track;
            }

        }
        Debug.Log("Tracks not found");
        return null;
    }

}

[System.Serializable]
public class AudioLibrary
{
    public string Name = "Name";

    public AudioClip Track;

    [Range(0, 1)] public float volume = 1;

}