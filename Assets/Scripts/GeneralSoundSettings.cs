using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralSoundSettings : MonoBehaviour
{
    private static readonly string bgmPrefs = "MusicVolume";
    private static readonly string sfxPrefs = "SoundVolume";
    private float bgmfloat, sfxFloat;
    public AudioSource bgmAudio;
    public AudioSource[] sfxAudio;

    void Awake()
    {
        ContinueSettings();
    }
    private void ContinueSettings()
    {
        bgmfloat = PlayerPrefs.GetFloat(bgmPrefs);
        sfxFloat = PlayerPrefs.GetFloat(sfxPrefs);

        bgmAudio.volume = bgmfloat;

        for (int i = 0; i < sfxAudio.Length; i++)
        {
            sfxAudio[i].volume = sfxFloat;
        }
    }
}