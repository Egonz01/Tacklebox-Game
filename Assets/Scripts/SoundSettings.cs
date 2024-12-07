using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundSettings : MonoBehaviour
{
    [SerializeField] AudioMixer audioMixer;
    [SerializeField] Slider masterVolumeSlider;
    [SerializeField] Slider sfxVolumeSlider;
    [SerializeField] Slider ambienceVolumeSlider;
    string master = "MasterVolume";
    string sfx = "SFXVolume";
    string ambience = "AmbienceVolume";

    void Start()
    {
        masterVolumeSlider.value = PlayerPrefs.GetFloat(master, .5f);
        sfxVolumeSlider.value = PlayerPrefs.GetFloat(sfx, .5f);
        ambienceVolumeSlider.value = PlayerPrefs.GetFloat(ambience, .5f);
        SetMasterVolume();
        SetSFXVolume();
        SetAmbienceVolume();
    }

    public void SetMasterVolume() {
        SetVolume(master, masterVolumeSlider.value);
        PlayerPrefs.SetFloat(master, masterVolumeSlider.value);
    }

    public void SetSFXVolume() {
        SetVolume(sfx, sfxVolumeSlider.value);
        PlayerPrefs.SetFloat(sfx, sfxVolumeSlider.value);
    }

    public void SetAmbienceVolume() {
        SetVolume(ambience, ambienceVolumeSlider.value);
        PlayerPrefs.SetFloat(ambience, ambienceVolumeSlider.value);
    }

    void SetVolume(string groupName, float value) {
        float adjustedVolume = Mathf.Log10(value) * 20;
        if (value == 0) {
            adjustedVolume = -80;
        }
        audioMixer.SetFloat(groupName, adjustedVolume);
    }
}
