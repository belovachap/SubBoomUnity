using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeManager : MonoBehaviour
{
    [Header("Mixer")]
    [SerializeField] AudioMixer mixer;

    [Header("Sliders")]
    [SerializeField] Slider masterVolumeSlider;
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider sfxSlider;

    private void Start()
    {
        // if previous settings are found, we'll load the previous volume settings
        if (PlayerPrefs.HasKey("masterVolume") || PlayerPrefs.HasKey("music") || PlayerPrefs.HasKey("sfx"))
        {
            LoadVolume();
        }
        else
        {
            SetMusicVolume();
            SetMasterVolume();
            SetSoundVolume();
        }
    }

    // master volume slider
    public void SetMasterVolume()
    {
        float volume = masterVolumeSlider.value;
        mixer.SetFloat("masterVolume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("masterVolume", volume);
    }

    // music volume slider
    public void SetMusicVolume()
    {
        float volume = musicSlider.value;
        mixer.SetFloat("music", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("music", volume);
    }

    // sound volume slider
    public void SetSoundVolume()
    {
        float volume = sfxSlider.value;
        mixer.SetFloat("sfx", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("sfx", volume);
    }

    // loads volume settings
    private void LoadVolume()
    {
        masterVolumeSlider.value = PlayerPrefs.GetFloat("masterVolume");
        musicSlider.value = PlayerPrefs.GetFloat("music");
        sfxSlider.value = PlayerPrefs.GetFloat("sfx");

        SetMasterVolume();
        SetMusicVolume();
        SetSoundVolume();
    }
}
