using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Volume : MonoBehaviour
{
    public AudioMixer audioMix;

    public float autoValue;

    private void Awake()
    {
        SetMasterVol(autoValue);
        SetMusicVol(autoValue);
        SetSFXVol(autoValue);
    }

    public void SetMasterVol(float sliderValue)
    {
        audioMix.SetFloat("MasterVol", Mathf.Log10(sliderValue) * 20);
    }

    public void SetMusicVol(float sliderValue)
    {
        audioMix.SetFloat("MusicVol", Mathf.Log10(sliderValue) * 20);
    }

    public void SetSFXVol(float sliderValue)
    {
        audioMix.SetFloat("SFXVol", Mathf.Log10(sliderValue) * 20);
    }
}
