using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralSoundSetting : MonoBehaviour
{
    private static readonly string bgmPrefs = "bgmPrefs";
   private static readonly string sfxPrefs = "sfxPrefs";
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
        
         for(int i = 0; i < sfxAudio.Length; i++)
         {
             sfxAudio[i].volume = sfxFloat;
         }
    }
}
