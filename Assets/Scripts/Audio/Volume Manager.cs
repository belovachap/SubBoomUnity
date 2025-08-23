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

    private float masterVolume, musicVolume, soundVolume;

    private void Start()
    {
        masterVolumeSlider.value = GameData.Instance.data.masterVolume;
        musicSlider.value = GameData.Instance.data.musicVolume;
        soundSlider.value = GameData.Instance.data.soundVolume;
    }

    // master volume slider
    public void SetMasterVolume()
    {
        masterVolume = masterVolumeSlider.value;
        mixer.SetFloat("masterVolume", Mathf.Log10(masterVolume) * 20);
        GameData.Instance.data.masterVolume = masterVolume;
    }

    // music volume slider
    public void SetMusicVolume()
    {
        musicVolume = musicSlider.value;
        mixer.SetFloat("music", Mathf.Log10(musicVolume) * 20);
        GameData.Instance.data.musicVolume = musicVolume;
    }

    // sound volume slider
    public void SetSoundVolume()
    {
        soundVolume = soundSlider.value;
        mixer.SetFloat("sfx", Mathf.Log10(soundVolume) * 20);
        GameData.Instance.data.soundVolume = soundVolume;
    }
}
