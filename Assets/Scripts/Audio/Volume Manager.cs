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
    [SerializeField] Slider soundSlider;

    // TODO (6/16/2025)
    // figure out why volume settings arent being saved
    // once player exits setting screen menu and closes game
    private void Start()
    {
        // if previous settings are found, we'll load the previous volume settings
        LoadVolume();
    }

    // master volume slider
    public void SetMasterVolume()
    {
        float volume = masterVolumeSlider.value;
        mixer.SetFloat("masterVolume", Mathf.Log10(volume) * 20);
        GameData.Instance.masterVolume = volume;
    }

    // music volume slider
    public void SetMusicVolume()
    {
        float volume = musicSlider.value;
        mixer.SetFloat("music", Mathf.Log10(volume) * 20);
        GameData.Instance.musicVolume = volume;
    }

    // sound volume slider
    public void SetSoundVolume()
    {
        float volume = soundSlider.value;
        mixer.SetFloat("sfx", Mathf.Log10(volume) * 20);
        GameData.Instance.soundVolume = volume;
    }

    // loads volume settings
    private void LoadVolume()
    {
        masterVolumeSlider.value = GameData.Instance.masterVolume;
        musicSlider.value = GameData.Instance.musicVolume;
        soundSlider.value = GameData.Instance.soundVolume;
    }
}
