using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateVolume : MonoBehaviour
{
    [SerializeField] SoundMixersManager soundMixersManager;
    [SerializeField] Slider soundFXSlider;
    [SerializeField] Slider musicSlider;

    private float musicVolume;
    private float soundFXVolume;
    private void Start()
    {
        soundMixersManager = GameObject.FindWithTag("SoundMixers").GetComponent<SoundMixersManager>();

        soundMixersManager.audioMixer.GetFloat("Music", out musicVolume);
        soundMixersManager.audioMixer.GetFloat("SoundFX", out  soundFXVolume);

        soundFXSlider.value = Mathf.Pow(10, soundFXVolume / 20);
        musicSlider.value = Mathf.Pow(10, musicVolume / 20);
        
    }

    public void OnMusicChange()
    {
        soundMixersManager.SetMusic(musicSlider.value);
    }

    public void OnSoundFXChange()
    {
        soundMixersManager.SetSoundFX(soundFXSlider.value);
    }
}
