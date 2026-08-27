using UnityEngine;
using System.Collections.Generic;

public class AudioService : MonoBehaviour
{
    public static AudioService Instance { get; private set; }
    
    [Header("SFX Clips")]
    [SerializeField] private AudioClip sfxTentacleLaunch;
    [SerializeField] private AudioClip sfxTentacleLaunch02;
    [SerializeField] private AudioClip sfxCapture;
    [SerializeField] private AudioClip sfxGrow;
    [SerializeField] private AudioClip sfxVictory;
    [SerializeField] private AudioClip sfxDefeat;
    
    [Header("Music")]
    [SerializeField] private AudioClip bgmBattle;
    
    [Header("Audio Sources")]
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource musicSource;
    
    [Header("Settings")]
    [SerializeField] private float growThrottle = 0.12f;
    
    private float lastGrowTime = 0f;
    
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
        SetupAudioSources();
        PlayBattleMusic();
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
        musicSource.spatialBlend = 0f;
        musicSource.loop = true;
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
            sfxSource.PlayOneShot(clipToPlay);
        }
    }
    
    public void PlayCapture()
    {
        if (sfxCapture != null)
        {
            sfxSource.PlayOneShot(sfxCapture);
        }
    }
    
    public void PlayGrow()
    {
        if (sfxGrow == null)
            return;
        
        float currentTime = Time.time;
        if (currentTime - lastGrowTime >= growThrottle)
        {
            lastGrowTime = currentTime;
            sfxSource.PlayOneShot(sfxGrow);
        }
    }
    
    public void PlayVictory()
    {
        if (sfxVictory != null)
        {
            sfxSource.PlayOneShot(sfxVictory);
        }
    }
    
    public void PlayDefeat()
    {
        if (sfxDefeat != null)
        {
            sfxSource.PlayOneShot(sfxDefeat);
        }
    }
    
    private void PlayBattleMusic()
    {
        if (bgmBattle != null && musicSource != null)
        {
            musicSource.clip = bgmBattle;
            musicSource.Play();
        }
    }
}
