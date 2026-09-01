using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class AudioService : MonoBehaviour
{
    public static AudioService Instance { get; private set; }
    
    private AudioClip sfxTentacleLaunch;
    private AudioClip sfxTentacleLaunch02;
    private AudioClip sfxCapture;
    private AudioClip sfxCapture02;
    private AudioClip sfxGrow;
    private AudioClip sfxGrow02;
    private AudioClip sfxVictory;
    private AudioClip sfxDefeat;
    private AudioClip bgmBattle;
    private AudioClip bgmMenu;
    
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
    private string currentSceneName;
    
    private AudioSource tentacleLaunchSource1;
    private AudioSource tentacleLaunchSource2;
    private AudioSource captureSource;
    private float tentacle1EndTime;
    private float tentacle2EndTime;
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    
    private void Start()
    {
        LoadAudioClips();
        SetupAudioSources();
        currentSceneName = SceneManager.GetActiveScene().name;
        PlaySceneMusic();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    
    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        string newSceneName = scene.name;
        if (newSceneName != currentSceneName)
        {
            currentSceneName = newSceneName;
            PlaySceneMusic();
        }
    }
    
    private void LoadAudioClips()
    {
        AudioLibrary library = Resources.Load<AudioLibrary>("AudioLibrary");
        if (library == null)
        {
            Debug.LogWarning("AudioLibrary not found in Resources folder");
            return;
        }
        
        sfxGrow = library.sfxGrow;
        sfxGrow02 = library.sfxGrow02;
        sfxTentacleLaunch = library.sfxTentacleLaunch;
        sfxTentacleLaunch02 = library.sfxTentacleLaunch02;
        sfxCapture = library.sfxCapture;
        sfxCapture02 = library.sfxCapture02;
        sfxVictory = library.sfxVictory;
        sfxDefeat = library.sfxDefeat;
        bgmBattle = library.bgmBattle;
        bgmMenu = library.bgmMenu;
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
        
        tentacleLaunchSource1 = gameObject.AddComponent<AudioSource>();
        tentacleLaunchSource2 = gameObject.AddComponent<AudioSource>();
        captureSource = gameObject.AddComponent<AudioSource>();
        
        sfxSource.spatialBlend = 0f;
        sfxSource.spatialize = false;
        sfxSource.dopplerLevel = 0f;
        sfxSource.volume = 1f;
        
        musicSource.spatialBlend = 0f;
        musicSource.spatialize = false;
        musicSource.dopplerLevel = 0f;
        musicSource.loop = true;
        musicSource.volume = bgmVolume;
        
        ConfigureAudioSource(tentacleLaunchSource1);
        ConfigureAudioSource(tentacleLaunchSource2);
        ConfigureAudioSource(captureSource);
    }
    
    private void ConfigureAudioSource(AudioSource source)
    {
        source.spatialBlend = 0f;
        source.spatialize = false;
        source.dopplerLevel = 0f;
        source.volume = 1f;
        source.playOnAwake = false;
    }
    
    public void PlayTentacleLaunch()
    {
        if (sfxTentacleLaunch == null && sfxTentacleLaunch02 == null)
            return;
        
        AudioClip clipToPlay = Random.value > 0.5f && sfxTentacleLaunch02 != null 
            ? sfxTentacleLaunch02 
            : sfxTentacleLaunch;
        
        if (clipToPlay == null)
            return;
        
        float currentTime = Time.time;
        bool source1Playing = tentacle1EndTime > currentTime;
        bool source2Playing = tentacle2EndTime > currentTime;
        
        if (source1Playing && source2Playing)
        {
            return;
        }
        
        AudioSource sourceToUse = !source1Playing ? tentacleLaunchSource1 : tentacleLaunchSource2;
        float pitch = Random.Range(0.97f, 1.03f);
        sourceToUse.pitch = pitch;
        sourceToUse.PlayOneShot(clipToPlay);
        sourceToUse.pitch = 1f;
        
        float endTime = currentTime + clipToPlay.length;
        if (sourceToUse == tentacleLaunchSource1)
            tentacle1EndTime = endTime;
        else
            tentacle2EndTime = endTime;
    }
    
    public void PlayCapture()
    {
        AudioClip clipToPlay = Random.value > 0.5f && sfxCapture02 != null 
            ? sfxCapture02 
            : sfxCapture;
        
        if (clipToPlay == null)
            return;
        
        if (captureSource.isPlaying)
        {
            captureSource.Stop();
        }
        
        captureSource.volume = captureVolume;
        captureSource.PlayOneShot(clipToPlay);
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
    
    private void PlaySceneMusic()
    {
        if (currentSceneName == "Battle")
        {
            PlayBattleMusic();
        }
        else
        {
            PlayMenuMusic();
        }
    }
    
    private void PlayBattleMusic()
    {
        if (bgmBattle == null || musicSource == null)
            return;
        
        if (musicSource.isPlaying && musicSource.clip == bgmBattle)
            return;
        
        if (musicSource.isPlaying && musicSource.clip != bgmBattle)
        {
            CrossfadeMusic(bgmBattle, 0.8f);
        }
        else
        {
            musicSource.clip = bgmBattle;
            musicSource.volume = 0f;
            musicSource.Play();
            FadeInMusic(0.8f, bgmVolume);
        }
    }
    
    private void PlayMenuMusic()
    {
        if (bgmMenu == null || musicSource == null)
            return;
        
        if (musicSource.isPlaying && musicSource.clip == bgmMenu)
            return;
        
        if (musicSource.isPlaying && musicSource.clip != bgmMenu)
        {
            CrossfadeMusic(bgmMenu, 0.8f);
        }
        else
        {
            musicSource.clip = bgmMenu;
            musicSource.volume = 0f;
            musicSource.Play();
            FadeInMusic(0.8f, bgmVolume);
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
