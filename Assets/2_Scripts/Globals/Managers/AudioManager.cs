
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour,IDataPersistence
{
    public static AudioManager Instance { get; private set; }
    
    [SerializeField] Audio[] _music, _sfx;
    public AudioSource musicSource;
    public AudioSource sfxSource;

    

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    public void LoadData(GameData data)
    {
        musicSource.mute = data.muteMusic;
        musicSource.volume = data.musicVolume;
        sfxSource.volume = data.sfxVolume;
        //UIManager.instance._musicMute.isOn = data.muteMusic;
    }

    public void SaveData(ref GameData data)
    {
        data.muteMusic = musicSource.mute;
        data.musicVolume = musicSource.volume;
        data.sfxVolume = sfxSource.volume;
    }

    public void UpdateGameVolume()
    {
        UIManager.instance.UpdateVolume(musicSource.volume, sfxSource.volume, musicSource.mute);
    }

    #region Preferences

    public void MuteMusic()
    {
        musicSource.mute = !musicSource.mute;
    }

    public void MusicVolume(float volume)
    {
        musicSource.volume = volume;
    }

    public void SFXVolume(float volume)
    {
        sfxSource.volume = volume;
    }

    #endregion

    #region Methods
    public void PlayMusic(string name)
    {
        Audio audio = Array.Find(_music, x => x.name == name);

        if(audio == null)
        {
            Debug.LogWarning($"Music named {name} has not been found, try another name");
            return;
        }

        musicSource.clip = audio.clip;
        musicSource.Play();
    }

    public void PlaySFX(string name)
    {
        Audio audio = Array.Find(_sfx, x => x.name == name);

        if (audio == null)
        {
            Debug.LogWarning($"SFX named {name} has not been found, try another name");
            return;
        }

        sfxSource.PlayOneShot(audio.clip);
    }

    #endregion

}
