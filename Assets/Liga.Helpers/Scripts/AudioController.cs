using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    #region Editor Attributes

    public int SoundEffectChannel = 1;

    #endregion

    #region Singleton

    static AudioController _instance;

    /// <summary>
    /// Controller to handle Audio functions
    /// </summary>
    public static AudioController Instance
    {
        get
        {
            if (_instance != null)
            {
                return _instance;
            }
            else
            {
                var audioManager = GameObject.FindObjectOfType<AudioController>();

                if (audioManager != null)
                {
                    _instance = audioManager;
                }
                else
                {
                    GameObject go = new GameObject();
                    go.name = "AudioManager";
                    _instance = go.AddComponent<AudioController>();
                }

                return _instance;
            }
        }
    }

    #endregion

    #region Enum

    public enum SoundType { Music, SoundEffect2D, Speech, SoundEffect3D, Narration}

    #endregion

    #region Constants

    const string musicSettings = "musicsettings";
    const string soundSettings = "soundsettings";
    const string unMuteLabel = "unmute";
    const string muteLabel = "mute";
    const string soundVolume = "soundVolume";
    const string musicVolume = "musicVolume";
    const string narrationVolume = "narrationVolume";

    #endregion

    #region Variables
    Dictionary<string, AudioClip> m_cachedSounds;

    private float m_soundVolume;
    private float m_narrationVolume;
    private float m_musicVolume;

    #endregion

    #region Awake

    public void Awake()
    {
        
        m_cachedSounds = new Dictionary<string, AudioClip>();
        _soundEffectChannels = new List<AudioSource>();

        _musicChannel = gameObject.AddComponent<AudioSource>();

        _exclusiveSounds = new List<PlayAndRemove>();

        for (int i = 0; i < SoundEffectChannel; i++)
            _soundEffectChannels.Add(gameObject.AddComponent<AudioSource>());

        LoadGameSettings();

        UnMuteSounds();
        UnMuteMusic();

        DontDestroyOnLoad(this);

    }

    private void LoadGameSettings()
    {
        string music = "unmute";
        string audio = "unmute";

        if (PlayerPrefs.HasKey(musicSettings))
            music = PlayerPrefs.GetString(musicSettings).ToLower();

        if (PlayerPrefs.HasKey(soundSettings))
            audio = PlayerPrefs.GetString(soundSettings).ToLower();
        
        m_soundVolume = SoundVolume();
        m_musicVolume = MusicVolume();
        m_narrationVolume = NarrationVolume();

        if (music.Equals(muteLabel))
            MuteMusic();
        else
            UnMuteMusic();

        if (audio.Equals(muteLabel))
            MuteSounds();
        else
            UnMuteSounds();
    }

    #endregion

    #region Attributes

    /// <summary>
    /// Controller that handle sounds effects channels 
    /// </summary>
    private List<AudioSource> _soundEffectChannels;

    /// <summary>
    /// Controller that handle music channel
    /// </summary>
    private AudioSource _musicChannel;

    /// <summary>
    /// Handle used channel
    /// </summary>
    private int _nextChannel;

    private bool _muteMusic;

    private bool _muteAudio;

    #endregion

    #region Properties

    public bool IsMusicMuted
    {
        get { return _muteMusic; }
    }

    public bool IsSoundMuted
    {
        get { return _muteAudio; }
    }

    List<PlayAndRemove> _exclusiveSounds;

    #endregion
    
    #region Play

    /// <summary>
    /// Play any sound 
    /// </summary>
    /// <param name="audio">Audio to be played</param>
    /// <param name="type">Type of sound</param>
    public void Play(string resourcePath, SoundType type, float volume = 1f, bool loop = false, bool exclusiveSound = false, float duration = 0, float pitch = 1f)
    {
        if (m_cachedSounds.ContainsKey(resourcePath))
        {
            Play(m_cachedSounds[resourcePath], type, volume, loop, exclusiveSound, duration, pitch);
        }
        else
        {
            AudioClip clip = Resources.Load<AudioClip>(resourcePath);
            m_cachedSounds.Add(resourcePath, clip);
            Play(m_cachedSounds[resourcePath], type, volume, loop, exclusiveSound, duration, pitch);
        }
    }


    /// <summary>
    /// Play any sound 
    /// </summary>
    /// <param name="audio">Audio to be played</param>
    /// <param name="type">Type of sound</param>
    public void Play(AudioClip audioClip, SoundType type, float volume = 1f, bool loop = false, bool exclusiveSound = false, float duration = 0, float pitch = 1f)
    {
        if (audioClip == null) return;

        switch (type)
        {
            case SoundType.Music:

                if (_musicChannel != null && _musicChannel.clip != null && _musicChannel.clip.name.Equals(audioClip.name)) return;

                volume = m_musicVolume;
                _musicChannel.clip = audioClip;
                _musicChannel.loop = true;
                _musicChannel.volume = volume;

                if (_muteMusic) return;

                _musicChannel.Play();
                break;
            case SoundType.Narration:
            case SoundType.SoundEffect2D:

                if (_muteAudio) return;

                volume = type == SoundType.SoundEffect2D ? m_soundVolume : m_narrationVolume;

                if (exclusiveSound)
                {
                    for (int i = 0; i < _exclusiveSounds.Count; i++)
                    {
                        if (!_exclusiveSounds[i].IsPlaying)
                        {
                            _exclusiveSounds[i].Play(audioClip, loop, volume, duration, pitch);
                            return;
                        }
                    }

                    GameObject go = new GameObject("ExclusiveSound");
                    var removeAfterPlay = go.AddComponent<PlayAndRemove>();
                    removeAfterPlay.Play(audioClip, loop, volume, duration,pitch);
                    _exclusiveSounds.Add(removeAfterPlay);
                    removeAfterPlay.transform.SetParent(transform);
                    return;
                }

                bool _freeChannel = false;

                for (int i = 0; i < SoundEffectChannel; i++)
                {
                    if (_soundEffectChannels[i].clip == null || !_soundEffectChannels[i].isPlaying)
                    {
                        _soundEffectChannels[i].clip = audioClip;
                        _soundEffectChannels[i].loop = loop;
                        _soundEffectChannels[i].volume = volume;
                        _soundEffectChannels[i].Play();

                        _freeChannel = true;
                        break;
                    }
                }

                if (!_freeChannel)
                {
                    //Check last index used

                    if (_soundEffectChannels.Any(x => !x.loop))
                    {
                        var channel = _soundEffectChannels.FirstOrDefault(x => !x.loop);
                        _nextChannel = _soundEffectChannels.IndexOf(channel);
                    }

                    _soundEffectChannels[_nextChannel].clip = audioClip;
                    _soundEffectChannels[_nextChannel].loop = loop;
                    _soundEffectChannels[_nextChannel].volume = volume;
                    _soundEffectChannels[_nextChannel].Play();

                    _nextChannel++;
                    if (_nextChannel > _soundEffectChannels.Count - 1)
                        _nextChannel = 0;
                }

                break;
            case SoundType.Speech:
                break;
            case SoundType.SoundEffect3D:
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Play sound or music attached to object!
    /// </summary>
    /// <param name="audio">Audio to be played</param>
    /// <param name="type">Type of audio</param>
    /// <param name="attachedObject">Object to attche audio</param>
    public void Play(AudioClip audioClip, SoundType type, GameObject attachedObject)
    {
        Play(audioClip, type);
    }

    #endregion

    #region Stop

    public void Stop(SoundType type = SoundType.Music)
    {
        switch (type)
        {
            case SoundType.Music:
                _musicChannel.Stop();
                break;
            case SoundType.SoundEffect2D:
                break;
            case SoundType.Speech:
                break;
            case SoundType.SoundEffect3D:
                break;
            default:
                break;
        }
    }

    public void StopSound(AudioClip sound)
    {
        AudioSource currentSoud = _soundEffectChannels.FirstOrDefault(x => x.clip == sound);
        if (currentSoud != null)
            currentSoud.Stop();
    }

    #endregion

    #region Pause

    public void Pause(SoundType type = SoundType.Music)
    {
        switch (type)
        {
            case SoundType.Music:
                _musicChannel.Pause();
                break;
            case SoundType.SoundEffect2D:
                break;
            case SoundType.Speech:
                break;
            case SoundType.SoundEffect3D:
                break;
            default:
                break;
        }
    }

    #endregion

    #region Resume

    public void Resume(SoundType type = SoundType.Music)
    {
        switch (type)
        {
            case SoundType.Music:
                _musicChannel.Play();
                break;
            case SoundType.SoundEffect2D:
                break;
            case SoundType.Speech:
                break;
            case SoundType.SoundEffect3D:
                break;
            default:
                break;
        }
    }
    #endregion

    #region Mute

    public void MuteMusic()
    {
        //AnalyticsManager.Instance.Music(true);

        _muteMusic = true;

        ChangeMusicState(muteLabel);

        Pause();
    }

    public void MuteSounds()
    {
        //AnalyticsManager.Instance.Music(true);

        ChangeSoundState(muteLabel);

        _muteAudio = true;
    }

    public void MuteAll()
    {
        //AnalyticsManager.Instance.Music(true);

        MuteSounds();
        MuteMusic();
    }

    public bool isSoundPlaying()
    {
       var audio = _soundEffectChannels.Where(x => x.isPlaying == true).FirstOrDefault();
       return audio == null ? false : true; 
    }

    #endregion

    #region UnMute

    public void UnMuteMusic()
    {
        //AnalyticsManager.Instance.Music(false);

        ChangeMusicState(unMuteLabel);

        _muteMusic = false;
        Resume();

    }

    public void UnMuteSounds()
    {
        //AnalyticsManager.Instance.Music(false);

        ChangeSoundState(unMuteLabel);
        _muteAudio = false;
    }

    public void UnMuteAll()
    {
        //AnalyticsManager.Instance.Music(false);

        UnMuteMusic();
        UnMuteSounds();
    }


    public float MusicVolume()
    {
        if (PlayerPrefs.HasKey(musicVolume))
            return PlayerPrefs.GetFloat(musicVolume);

        return 1;
    }

    public float SoundVolume()
    {
        if (PlayerPrefs.HasKey(soundVolume))
            return PlayerPrefs.GetFloat(soundVolume);

        return 1;
    }

    public float NarrationVolume()
    {
        if (PlayerPrefs.HasKey(narrationVolume))
            return PlayerPrefs.GetFloat(narrationVolume);

        return 1;
    }

    public IEnumerator ClearExclusiveSounds()
    {
        int remainingSounds = _exclusiveSounds.Count;
        int finalQuantity = remainingSounds / 3;
        for (int i = remainingSounds-1; i > finalQuantity; i--)
        {
            Destroy(_exclusiveSounds[i].gameObject);
            _exclusiveSounds.RemoveAt(i);
            yield return null;
        }

        m_cachedSounds.Clear();
    }

    #endregion

    #region Private Methods

    private void ChangeMusicState(string newState)
    {
        PlayerPrefs.SetString(musicSettings, newState);
        PlayerPrefs.SetString(soundSettings, _muteAudio ? muteLabel : unMuteLabel);

    }

    private void ChangeSoundState(string newState)
    {
        PlayerPrefs.SetString(soundSettings, newState);
        PlayerPrefs.SetString(musicSettings, _muteMusic ? muteLabel : unMuteLabel);

    }

    void OnDestroy()
    {
        //PlayerPrefs.Save();
    }

    public void ChangeVolume(SoundType currentType, float volume)
    {
        switch (currentType)
        {
            case SoundType.Music:
                 m_musicVolume = volume;
                 _musicChannel.volume = m_musicVolume;
                PlayerPrefs.SetFloat(musicVolume, m_musicVolume);

                break;
            case SoundType.SoundEffect2D:
                m_soundVolume = volume;

                foreach (AudioSource item in _soundEffectChannels)
                {
                    item.clip = null;
                }

                PlayerPrefs.SetFloat(soundVolume, m_soundVolume);
                break;
            case SoundType.Narration:
                m_narrationVolume = volume;
                PlayerPrefs.SetFloat(narrationVolume, m_narrationVolume);
                break;
        }
    }

    #endregion
}
