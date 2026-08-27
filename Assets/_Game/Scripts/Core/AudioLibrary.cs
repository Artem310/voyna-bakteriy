using UnityEngine;

[CreateAssetMenu(fileName = "AudioLibrary", menuName = "Game/Audio Library")]
public class AudioLibrary : ScriptableObject
{
    [Header("SFX Clips")]
    public AudioClip sfxGrow;
    public AudioClip sfxGrow02;
    public AudioClip sfxTentacleLaunch;
    public AudioClip sfxTentacleLaunch02;
    public AudioClip sfxCapture;
    public AudioClip sfxCapture02;
    public AudioClip sfxVictory;
    public AudioClip sfxDefeat;
    
    [Header("BGM Clips")]
    public AudioClip bgmBattle;
    public AudioClip bgmMenu;
}
