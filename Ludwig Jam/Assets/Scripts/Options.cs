using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class Options : MonoBehaviour
{

    public AudioMixer audioMix;

    public Slider master;
    public Slider music;
    public Slider sfx;

    private const float DEFAULT_VOLUME = 0.8f;

    private void Start()
    {
        if (PlayerPrefs.GetFloat("timeSoFar") +
            PlayerPrefs.GetFloat("MasterVol") +
            PlayerPrefs.GetFloat("MusicVol") +
            PlayerPrefs.GetFloat("SFXVol") <= 0 && PlayerPrefs.GetInt("VFX") == 0)
        {

            SetMasterVol(DEFAULT_VOLUME);
            master.value = DEFAULT_VOLUME;

            SetMusicVol(DEFAULT_VOLUME);
            music.value = DEFAULT_VOLUME;

            SetSFXVol(DEFAULT_VOLUME);
            sfx.value = DEFAULT_VOLUME;

        }
        else
        {
            SetMasterVol(PlayerPrefs.GetFloat("MasterVol"));
            master.value = PlayerPrefs.GetFloat("MasterVol");

            SetMusicVol(PlayerPrefs.GetFloat("MusicVol"));
            music.value = PlayerPrefs.GetFloat("MusicVol");

            SetSFXVol(PlayerPrefs.GetFloat("SFXVol"));
            sfx.value = PlayerPrefs.GetFloat("SFXVol");
        }

    }

    public void SetMasterVol(float sliderValue)
    {
        PlayerPrefs.SetFloat("MasterVol", sliderValue);
        audioMix.SetFloat("MasterVol", Mathf.Log10(sliderValue) * 20);
    }

    public void SetMusicVol(float sliderValue)
    {
        PlayerPrefs.SetFloat("MusicVol", sliderValue);
        audioMix.SetFloat("MusicVol", Mathf.Log10(sliderValue) * 20);
    }

    public void SetSFXVol(float sliderValue)
    {
        PlayerPrefs.SetFloat("SFXVol", sliderValue);
        audioMix.SetFloat("SFXVol", Mathf.Log10(sliderValue) * 20);
    }
}

