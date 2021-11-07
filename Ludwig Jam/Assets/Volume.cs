using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Volume : MonoBehaviour
{
    public AudioMixer audioMix;

    public Slider master;
    public Slider music;
    public Slider sfx;

    private void Start()
    {
        SetMasterVol(PlayerPrefs.GetFloat("MasterVol"));
        master.value = PlayerPrefs.GetFloat("MasterVol");

        SetMusicVol(PlayerPrefs.GetFloat("MusicVol"));
        music.value = PlayerPrefs.GetFloat("MusicVol");

        SetSFXVol(PlayerPrefs.GetFloat("SFXVol"));
        sfx.value = PlayerPrefs.GetFloat("SFXVol");
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
