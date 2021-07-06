using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    public GameObject settingsPanel;
    bool settingsPanelIsOpen;
    public float soundLevel = 1f;
    public float musicLevel = 1f;
    public Slider soundSlider;
    public Slider musicSlider;
    public Text soundValue;
    public Text musicValue;

    public GameObject musicIconOn;
    public GameObject musicIconOff;

    public GameObject soundIconOn;
    public GameObject soundIconOff;
    void Start()
    {
        settingsPanel.SetActive(settingsPanelIsOpen);
        soundValue.text = $"{Mathf.RoundToInt(soundLevel * 100)}%";
        musicValue.text = $"{Mathf.RoundToInt(musicLevel * 100)}%";
        soundSlider.value = soundLevel;
        musicSlider.value = musicLevel;
    }

    // Update is called once per frame
    void Update()
    {
        settingsPanel.SetActive(settingsPanelIsOpen);
        soundValue.text = $"{Mathf.RoundToInt(soundLevel * 100)}%";
        musicValue.text = $"{Mathf.RoundToInt(musicLevel * 100)}%";
        soundIconOff.SetActive(soundLevel == 0);
        musicIconOff.SetActive(musicLevel == 0);
        soundIconOn.SetActive(soundLevel != 0);
        musicIconOn.SetActive(musicLevel != 0);
        // AudioListener.volume = musicLevel;
    }
    public void ToggleSettings()
    {
        settingsPanelIsOpen = !settingsPanelIsOpen;
    }
    public void UpdateSoundValue()
    {
        soundLevel = soundSlider.value;
    }
    public void UpdateMusicValue()
    {
        musicLevel = musicSlider.value;
    }
}
