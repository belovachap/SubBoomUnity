using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Sources")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource soundSource;

    [Header("Music Clips")]
    public AudioClip menuMusic;
    public AudioClip inGameMusic;

    [Header("Sound Effect Clips")]
    public AudioClip buttonPressSFX;
    public AudioClip submarineSFX;
    public AudioClip depthChargeSFX;
    public AudioClip torpedoSFX;
    public AudioClip explosionSFX;
    public AudioClip gameOverSFX;

    public void PlayMusic(AudioClip clip)
    {
        musicSource.clip = clip;
        musicSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        soundSource.PlayOneShot(clip);
    }
}
