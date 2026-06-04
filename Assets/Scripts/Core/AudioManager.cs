using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Manages all audio: background music, sound effects, and voice lines.
/// Implements pooling for efficient SFX management.
/// </summary>
public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private int sfxPoolSize = 20;
    [SerializeField] private float masterVolume = 1f;
    [SerializeField] private float musicVolume = 0.7f;
    [SerializeField] private float sfxVolume = 0.8f;
    
    private Queue<AudioSource> sfxPool = new Queue<AudioSource>();
    private static AudioManager instance;
    
    public static AudioManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<AudioManager>();
                if (instance == null)
                {
                    GameObject go = new GameObject("AudioManager");
                    instance = go.AddComponent<AudioManager>();
                }
            }
            return instance;
        }
    }
    
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
        
        InitializeSFXPool();
    }
    
    private void InitializeSFXPool()
    {
        for (int i = 0; i < sfxPoolSize; i++)
        {
            GameObject sfxObj = new GameObject($"SFX_{i}");
            sfxObj.transform.SetParent(transform);
            AudioSource source = sfxObj.AddComponent<AudioSource>();
            source.playOnAwake = false;
            sfxPool.Enqueue(source);
        }
    }
    
    public void PlayMusic(AudioClip clip, bool loop = true)
    {
        if (musicSource != null)
        {
            musicSource.clip = clip;
            musicSource.loop = loop;
            musicSource.volume = musicVolume * masterVolume;
            musicSource.Play();
        }
    }
    
    public void StopMusic()
    {
        if (musicSource != null)
        {
            musicSource.Stop();
        }
    }
    
    public void PlaySFX(AudioClip clip, Vector3 position = default, float volume = 1f)
    {
        if (clip == null)
            return;
        
        AudioSource source = GetSFXSource();
        if (source != null)
        {
            source.transform.position = position;
            source.volume = volume * sfxVolume * masterVolume;
            source.PlayOneShot(clip);
        }
    }
    
    private AudioSource GetSFXSource()
    {
        if (sfxPool.Count > 0)
        {
            return sfxPool.Dequeue();
        }
        
        // Create new if pool exhausted
        GameObject sfxObj = new GameObject("SFX");
        sfxObj.transform.SetParent(transform);
        AudioSource source = sfxObj.AddComponent<AudioSource>();
        source.playOnAwake = false;
        return source;
    }
    
    public void SetMasterVolume(float volume)
    {
        masterVolume = Mathf.Clamp01(volume);
        UpdateVolumes();
    }
    
    public void SetMusicVolume(float volume)
    {
        musicVolume = Mathf.Clamp01(volume);
        UpdateVolumes();
    }
    
    public void SetSFXVolume(float volume)
    {
        sfxVolume = Mathf.Clamp01(volume);
    }
    
    private void UpdateVolumes()
    {
        if (musicSource != null)
        {
            musicSource.volume = musicVolume * masterVolume;
        }
    }
    
    public void PauseMusic()
    {
        if (musicSource != null)
        {
            musicSource.Pause();
        }
    }
    
    public void ResumeMusic()
    {
        if (musicSource != null)
        {
            musicSource.UnPause();
        }
    }
}
