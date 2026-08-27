using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AudioService : MonoBehaviour
{
    public static AudioService Instance { get; private set; }
    
    [Header("SFX Clips")]
    [SerializeField] private AudioClip sfxTentacleLaunch;
    [SerializeField] private AudioClip sfxTentacleLaunch02;
    [SerializeField] private AudioClip sfxCapture;
    [SerializeField] private AudioClip sfxCapture02;
    [SerializeField] private AudioClip sfxGrow;
    [SerializeField] private AudioClip sfxGrow02;
    [SerializeField] private AudioClip sfxVictory;
    [SerializeField] private AudioClip sfxDefeat;
    
    [Header("Music")]
    [SerializeField] private AudioClip bgmBattle;
    [SerializeField] private AudioClip bgmMenu;
    
    [Header("Audio Sources")]
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource musicSource;
    
    [Header("Settings")]
    [SerializeField] private float growThrottle = 0.12f;
    [SerializeField] private float bgmVolume = 0.398f;
    [SerializeField] private float growVolume = 0.631f;
    [SerializeField] private float captureVolume = 0.891f;
    [SerializeField] private float defeatVolume = 0.891f;
    
    private float lastGrowTime = 0f;
    private Coroutine fadeCoroutine;
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    
    private void Start()
    {
        LoadAudioClips();
        SetupAudioSources();
        PlayBattleMusic();
    }
    
    private void LoadAudioClips()
    {
        sfxGrow = LoadAudioClip("1a2b3c4d5e6f7a8b9c0d1e2f3a4b5c6d");
        sfxGrow02 = LoadAudioClip("2b3c4d5e6f7a8b9c0d1e2f3a4b5c6d7e");
        sfxTentacleLaunch = LoadAudioClip("3c4d5e6f7a8b9c0d1e2f3a4b5c6d7e8f");
        sfxTentacleLaunch02 = LoadAudioClip("4d5e6f7a8b9c0d1e2f3a4b5c6d7e8f9a");
        sfxCapture = LoadAudioClip("5e6f7a8b9c0d1e2f3a4b5c6d7e8f9a0b");
        sfxCapture02 = LoadAudioClip("6f7a8b9c0d1e2f3a4b5c6d7e8f9a0b1c");
        sfxVictory = LoadAudioClip("7a8b9c0d1e2f3a4b5c6d7e8f9a0b1c2d");
        sfxDefeat = LoadAudioClip("8b9c0d1e2f3a4b5c6d7e8f9a0b1c2d3e");
        bgmBattle = LoadAudioClip("9c0d1e2f3a4b5c6d7e8f9a0b1c2d3e4f");
        bgmMenu = LoadAudioClip("0d1e2f3a4b5c6d7e8f9a0b1c2d3e4f5a");
    }
    
    private AudioClip LoadAudioClip(string guid)
    {
#if UNITY_EDITOR
        string assetPath = UnityEditor.AssetDatabase.GUIDToAssetPath(guid);
        if (!string.IsNullOrEmpty(assetPath))
        {
            return UnityEditor.AssetDatabase.LoadAssetAtPath<AudioClip>(assetPath);
        }
#endif
        return null;
    }
    
    private void SetupAudioSources()
    {
        if (sfxSource == null)
        {
            sfxSource = gameObject.AddComponent<AudioSource>();
        }
        
        if (musicSource == null)
        {
            musicSource = gameObject.AddComponent<AudioSource>();
        }
        
        sfxSource.spatialBlend = 0f;
        sfxSource.spatialize = false;
        sfxSource.dopplerLevel = 0f;
        sfxSource.volume = 1f;
        
        musicSource.spatialBlend = 0f;
        musicSource.spatialize = false;
        musicSource.dopplerLevel = 0f;
        musicSource.loop = true;
        musicSource.volume = bgmVolume;
    }
    
    public void PlayTentacleLaunch()
    {
        if (sfxTentacleLaunch == null && sfxTentacleLaunch02 == null)
            return;
        
        AudioClip clipToPlay = Random.value > 0.5f && sfxTentacleLaunch02 != null 
            ? sfxTentacleLaunch02 
            : sfxTentacleLaunch;
        
        if (clipToPlay != null)
        {
            float pitch = Random.Range(0.97f, 1.03f);
            sfxSource.pitch = pitch;
            sfxSource.PlayOneShot(clipToPlay);
            sfxSource.pitch = 1f;
        }
    }
    
    public void PlayCapture()
    {
        AudioClip clipToPlay = Random.value > 0.5f && sfxCapture02 != null 
            ? sfxCapture02 
            : sfxCapture;
        
        if (clipToPlay != null)
        {
            sfxSource.PlayOneShot(clipToPlay, captureVolume);
        }
    }
    
    public void PlayGrow()
    {
        float currentTime = Time.time;
        if (currentTime - lastGrowTime < growThrottle)
            return;
        
        lastGrowTime = currentTime;
        
        AudioClip clipToPlay = Random.value > 0.5f && sfxGrow02 != null 
            ? sfxGrow02 
            : sfxGrow;
        
        if (clipToPlay != null)
        {
            float pitch = Random.Range(0.97f, 1.03f);
            sfxSource.pitch = pitch;
            sfxSource.PlayOneShot(clipToPlay, growVolume);
            sfxSource.pitch = 1f;
        }
    }
    
    public void PlayVictory()
    {
        if (sfxVictory != null)
        {
            sfxSource.PlayOneShot(sfxVictory);
        }
        FadeOutMusic(0.4f);
    }
    
    public void PlayDefeat()
    {
        if (sfxDefeat != null)
        {
            sfxSource.PlayOneShot(sfxDefeat, defeatVolume);
        }
        FadeOutMusic(0.4f);
    }
    
    private void PlayBattleMusic()
    {
        if (bgmBattle != null && musicSource != null)
        {
            musicSource.clip = bgmBattle;
            musicSource.volume = 0f;
            musicSource.Play();
            FadeInMusic(0.8f, bgmVolume);
        }
    }
    
    public void PlayMenuMusic()
    {
        if (bgmMenu != null && musicSource != null)
        {
            CrossfadeMusic(bgmMenu, 0.8f);
        }
    }
    
    private void FadeInMusic(float duration, float targetVolume)
    {
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }
        fadeCoroutine = StartCoroutine(FadeInCoroutine(duration, targetVolume));
    }
    
    private void FadeOutMusic(float duration)
    {
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }
        fadeCoroutine = StartCoroutine(FadeOutCoroutine(duration));
    }
    
    private void CrossfadeMusic(AudioClip newClip, float duration)
    {
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }
        fadeCoroutine = StartCoroutine(CrossfadeCoroutine(newClip, duration));
    }
    
    private IEnumerator FadeInCoroutine(float duration, float targetVolume)
    {
        float elapsed = 0f;
        float startVolume = musicSource.volume;
        
        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            musicSource.volume = Mathf.Lerp(startVolume, targetVolume, elapsed / duration);
            yield return null;
        }
        
        musicSource.volume = targetVolume;
    }
    
    private IEnumerator FadeOutCoroutine(float duration)
    {
        float elapsed = 0f;
        float startVolume = musicSource.volume;
        
        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            musicSource.volume = Mathf.Lerp(startVolume, 0f, elapsed / duration);
            yield return null;
        }
        
        musicSource.volume = 0f;
        musicSource.Stop();
    }
    
    private IEnumerator CrossfadeCoroutine(AudioClip newClip, float duration)
    {
        float elapsed = 0f;
        float startVolume = musicSource.volume;
        
        while (elapsed < duration / 2f)
        {
            elapsed += Time.unscaledDeltaTime;
            musicSource.volume = Mathf.Lerp(startVolume, 0f, elapsed / (duration / 2f));
            yield return null;
        }
        
        musicSource.Stop();
        musicSource.clip = newClip;
        musicSource.Play();
        
        elapsed = 0f;
        while (elapsed < duration / 2f)
        {
            elapsed += Time.unscaledDeltaTime;
            musicSource.volume = Mathf.Lerp(0f, bgmVolume, elapsed / (duration / 2f));
            yield return null;
        }
        
        musicSource.volume = bgmVolume;
    }
}
