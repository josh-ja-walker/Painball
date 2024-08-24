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

        if (PlayerPrefs.GetFloat("timeSoFar") <= 0
            && PlayerPrefs.GetFloat("MasterVol") <= 0
            && PlayerPrefs.GetFloat("MusicVol") <= 0 
            && PlayerPrefs.GetFloat("SFXVol") <= 0)
        {
            SetMasterVol(0.8f);
            master.value = 0.8f;

            SetMusicVol(0.8f);
            music.value = 0.8f;

            SetSFXVol(0.8f);
            sfx.value = 0.8f;
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
