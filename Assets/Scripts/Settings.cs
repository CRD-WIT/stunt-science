using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
public class Settings : MonoBehaviour
{
    public GameObject settingsPanel;
    public GameObject levelFinishedPanel;
    bool leveFinishedIsOpen;
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
    public TMP_Text versionCodeText;
    public string versionCode;
    public TMP_Text fps;
    int level1EasyPoints;
    int level1MediumPoints;
    int level1HardPoints;
    bool level1Locked;


    int level2EasyPoints;
    int level2MediumPoints;
    int level2HardPoints;
    bool level2Locked;


    int level3EasyPoints;
    int level3MediumPoints;
    int level3HardPoints;
    bool level3Locked;


    int level4EasyPoints;
    int level4MediumPoints;
    int level4HardPoints;
    bool level4Locked;


    int level5EasyPoints;
    int level5MediumPoints;
    int level5HardPoints;
    bool level5Locked;

    void Start()
    {

        settingsPanel.SetActive(settingsPanelIsOpen);
        levelFinishedPanel.SetActive(leveFinishedIsOpen);
        soundValue.text = $"{Mathf.RoundToInt(soundLevel * 100)}%";
        musicValue.text = $"{Mathf.RoundToInt(musicLevel * 100)}%";
        soundSlider.value = soundLevel;
        musicSlider.value = musicLevel;
        versionCodeText.text = $"Version: {versionCode}";
    }

    public void GotoNextLevel(string LevelToUnlock){
        PlayerPrefs.SetInt(LevelToUnlock, 0);
        SceneManager.LoadScene("LevelSelectV2");
    }

     public void ReloadLevel(){
         Scene scene = SceneManager.GetActiveScene(); 
         SceneManager.LoadScene(scene.name);
    }


    // Update is called once per frame
    void Update()
    {
        fps.text = $"Frame Rate: {(Mathf.RoundToInt(1.0f / Time.smoothDeltaTime)).ToString()}f/s";
        settingsPanel.SetActive(settingsPanelIsOpen);
        levelFinishedPanel.SetActive(leveFinishedIsOpen);
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

    public void ToggleLevelFinished()
    {
        leveFinishedIsOpen = !leveFinishedIsOpen;
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
