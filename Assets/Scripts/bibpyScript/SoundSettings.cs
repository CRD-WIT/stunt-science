using UnityEngine.UI;
using UnityEngine;

public class SoundSettings : MonoBehaviour
{
   private static readonly string FirsPlay = "FirstPlay";
    private static readonly string bgmPrefs = "bgmPrefs";
    private static readonly string sfxPrefs = "sfxPrefs";
    private int firstPlayInt;
    public Slider bgmSlider, sfxSlider;
    private float bgmfloat, sfxFloat;
    public AudioSource bgmAudio;
    public AudioSource[] sfxAudio;
    // Start is called before the first frame update
    void Start()
    {
        firstPlayInt = PlayerPrefs.GetInt(FirsPlay);

        if(firstPlayInt == 0)
        {
            bgmfloat = .125f;
            sfxFloat = .75f;
            bgmSlider.value = bgmfloat;
            sfxSlider.value = sfxFloat;
            PlayerPrefs.SetFloat(bgmPrefs, bgmfloat);
            PlayerPrefs.SetFloat(sfxPrefs, sfxFloat);
            PlayerPrefs.SetInt(FirsPlay, -1);
        }
        else
        {
            bgmfloat = PlayerPrefs.GetFloat(bgmPrefs);
            bgmSlider.value = bgmfloat;
            sfxFloat = PlayerPrefs.GetFloat(sfxPrefs);
            sfxSlider.value = sfxFloat;
        }
    }

    
    public void SaveSoundSettings()

    {
        PlayerPrefs.SetFloat(bgmPrefs, bgmSlider.value);
        PlayerPrefs.SetFloat(sfxPrefs, sfxSlider.value);
    }
    void OnApplicationFocus(bool inFocus) 
     {
         if(!inFocus)
         {
             SaveSoundSettings();
         }
     }
     public void UpdateSound()
     {
         bgmAudio.volume = bgmSlider.value;
         
         for(int i = 0; i < sfxAudio.Length; i++)
         {
             sfxAudio[i].volume = sfxSlider.value;
         }
     }  
}
