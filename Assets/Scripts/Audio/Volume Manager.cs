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
        // if previous settings are found, we'll load the previous volume settings
        if (GameData.Instance.totalGamesPlayed == 0)
        {
            masterVolumeSlider.value = 1;
            musicSlider.value = 1;
            soundSlider.value = 1;
        }
        else
        {
            LoadVolume();
        }
    }

    // master volume slider
    public void SetMasterVolume()
    {
        masterVolume = masterVolumeSlider.value;
        mixer.SetFloat("masterVolume", Mathf.Log10(masterVolume) * 20);
        GameData.Instance.masterVolume = masterVolume;
    }

    // music volume slider
    public void SetMusicVolume()
    {
        musicVolume = musicSlider.value;
        mixer.SetFloat("music", Mathf.Log10(musicVolume) * 20);
        GameData.Instance.musicVolume = musicVolume;
    }

    // sound volume slider
    public void SetSoundVolume()
    {
        soundVolume = soundSlider.value;
        mixer.SetFloat("sfx", Mathf.Log10(soundVolume) * 20);
        GameData.Instance.soundVolume = soundVolume;
    }

    // when player releases mouse button,
    // volume settings will save
    public void OnMouseUp()
    {
        GameData.Instance.masterVolume = masterVolume;
        GameData.Instance.musicVolume = musicVolume;
        GameData.Instance.soundVolume = soundVolume;
    }

    // loads volume settings
    private void LoadVolume()
    {
        GameData.Instance.Load();

        masterVolumeSlider.value = GameData.Instance.masterVolume;
        musicSlider.value = GameData.Instance.musicVolume;
        soundSlider.value = GameData.Instance.soundVolume;
    }
}
