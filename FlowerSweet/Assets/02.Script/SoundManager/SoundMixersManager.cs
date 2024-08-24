using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundMixersManager : MonoBehaviour
{
    [SerializeField] public AudioMixer audioMixer;


    public void SetSoundFX(float level)
    {
        audioMixer.SetFloat("SoundFX", Mathf.Log10(level) * 20f);
    }


    public void SetMusic(float level)
    {
        audioMixer.SetFloat("Music", Mathf.Log10(level) * 20f);
    }

}
