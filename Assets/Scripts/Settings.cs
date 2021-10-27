using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class Settings : MonoBehaviour
{
    public GameObject settingsPanel;
    public GameObject levelFinishedPanel;
    public GameObject stuntGuidePanel;
    bool leveFinishedIsOpen;
    bool settingsPanelIsOpen;

    public GameObject flashCard;
    public GameObject flashCardEnd;

    public bool flashCardIsOpen = false;
    public bool flashCardEndIsOpen;

    bool stuntGuidePanelIsOpen;
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
    public Sprite[] soundButtonImages;
    public Image soundButtonImage;
    int level5EasyPoints;
    int level5MediumPoints;
    int level5HardPoints;
    bool level5Locked;
    public AudioSource backgroundAudio;

    bool soundOn = true;

    bool musicIsOn = true;

    public bool muteIntroSoundOnStart;

    public bool isFirstStart = false;

    public AudioSource[] sfxAudios;

    public FirebaseManager firebaseManager;

    public string[] gameLevelNames = { "", "Velocity", "Acceleration", "FreeFallProjectile", "CircularMotion", "Forces", "Work", "Energy", "Power", "Momemtum" };

    public string[] GetLevelNames()
    {
        return gameLevelNames;
    }
    void Start()
    {
        ToggleFlashCard();
        // if (!muteIntroSoundOnStart)
        // {
        //     ToggleIntroMusic();
        // }

        // Sound
        LoadVolumes();
        if (settingsPanel)
        {
            settingsPanel.SetActive(settingsPanelIsOpen);
        }

        if (levelFinishedPanel)
        {
            levelFinishedPanel.SetActive(leveFinishedIsOpen);
        }
        if (soundValue)
        {
            soundValue.text = $"{Mathf.RoundToInt(soundLevel * 100)}%";
        }
        if (musicValue)
        {
            musicValue.text = $"{Mathf.RoundToInt(musicLevel * 100)}%";
        }
        if (soundSlider)
        {
            soundSlider.value = soundLevel;
        }
        if (musicSlider)
        {
            musicSlider.value = musicLevel;
        }


        if (versionCodeText)
        {
            versionCodeText.text = $"Version: {versionCode}";
        }

        // Set global gameplay stats for data logging.
        int levelNumber = 0;
        string difficulty = null;
        int stage = 0;

        if (isFirstStart)
        {
            firebaseManager.GameLogMutation(levelNumber, stage, difficulty, Actions.StartedGame, 0);
        }

    }

    public void GotoNextLevel(string LevelToUnlock)
    {
        PlayerPrefs.SetInt(LevelToUnlock, 0);
        SceneManager.LoadScene("LevelSelectV2");
    }

    public void ToggleFlashCard()
    {
        flashCardIsOpen = !flashCardIsOpen;
    }

    public void ToggleFlashCardEnd()
    {
        flashCardEndIsOpen = !flashCardEndIsOpen;
        if (!flashCardEndIsOpen == true)
        {
            ToggleLevelFinished();
        }
    }

    public void ReloadLevel()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    // Update is called once per frame
    void Update()
    {
        if (fps)
        {
            fps.text = $"Frame Rate: {(Mathf.RoundToInt(1.0f / Time.smoothDeltaTime)).ToString()}f/s";
        }
        if (settingsPanel)
        {
            settingsPanel.SetActive(settingsPanelIsOpen);
        }
        if (levelFinishedPanel)
        {
            levelFinishedPanel.SetActive(leveFinishedIsOpen);
        }
        if (stuntGuidePanel)
        {
            stuntGuidePanel.SetActive(stuntGuidePanelIsOpen);
        }
        if (soundValue)
        {
            soundValue.text = $"{Mathf.RoundToInt(soundLevel * 100)}%";
        }
        if (musicValue)
        {
            musicValue.text = $"{Mathf.RoundToInt(musicLevel * 100)}%";
        }
        if (soundIconOff)
        {
            soundIconOff.SetActive(soundLevel == 0);
        }
        if (musicIconOff)
        {
            musicIconOff.SetActive(musicLevel == 0);
        }
        if (soundIconOn)
        {
            soundIconOn.SetActive(soundLevel != 0);
        }
        if (musicIconOn)
        {
            musicIconOn.SetActive(musicLevel != 0);
        }
        if (flashCard)
        {
            flashCard.SetActive(flashCardIsOpen);
        }
        if (flashCardEnd)
        {
            flashCardEnd.SetActive(flashCardEndIsOpen);
        }
        AudioListener.volume = soundLevel;
        if (AudioListener.volume == 0)
        {
            if (soundButtonImage)
            {
                soundButtonImage.sprite = soundButtonImages[1];
            }
        }
        else
        {
            if (soundButtonImage)
            {
                soundButtonImage.sprite = soundButtonImages[0];
            }
        }
    }
    public void ToggleSettings()
    {
        settingsPanelIsOpen = !settingsPanelIsOpen;
    }

    public void ToggleAssistance()
    {
        // Set global gameplay stats for data logging.
        int levelNumber = int.Parse(PlayerPrefs.GetString("LevelNumber"));
        string difficulty = PlayerPrefs.GetString("DifficultyName");
        int stage = int.Parse(PlayerPrefs.GetString("Stage"));

        stuntGuidePanelIsOpen = !stuntGuidePanelIsOpen;
        if (stuntGuidePanelIsOpen)
        {
            firebaseManager.GameLogMutation(levelNumber, stage, difficulty, Actions.OpenedStuntGuide, 0);
        }
        else
        {
            firebaseManager.GameLogMutation(levelNumber, stage, difficulty, Actions.ClosedStuntGuide, 0);
        }
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
        for (int i = 0; i < sfxAudios.Length; i++)
        {
            sfxAudios[i].volume = musicLevel;
        }
    }

    public void ResetLife()
    {
        PlayerPrefs.SetInt("Life", 3);
    }

    public void ToggleIntroMusic()
    {
        if (musicIsOn)
        {
            backgroundAudio.volume = 0;
        }
        else
        {
            backgroundAudio.volume = PlayerPrefs.GetFloat("MusicVolume", musicLevel);

        }
        musicIsOn = !musicIsOn;
        
    }

    public void ToggleVolume()
    {
        // Set global gameplay stats for data logging.
        int levelNumber = int.Parse(PlayerPrefs.GetString("LevelNumber", "1"));
        string difficulty = PlayerPrefs.GetString("DifficultyName");
        int stage = int.Parse(PlayerPrefs.GetString("Stage", "1"));


        if (soundOn)
        {
            firebaseManager.GameLogMutation(levelNumber, stage, difficulty, Actions.MutedSound, 0);
            soundLevel = 0;
        }
        else
        {
            soundLevel = PlayerPrefs.GetFloat("SoundVolume");
            if (soundLevel == 0)
            {
                soundLevel = 1;
            }
            firebaseManager.GameLogMutation(levelNumber, stage, difficulty, Actions.UnmutedSound, soundLevel);
        }
        soundOn = !soundOn;
        soundSlider.value = soundLevel;
    }

    public void SaveVolume()
    {
        PlayerPrefs.SetFloat("MusicVolume", musicLevel);
        PlayerPrefs.SetFloat("SoundVolume", soundLevel);
        LoadVolumes();
    }

    public void LoadVolumes()
    {
        soundLevel = PlayerPrefs.GetFloat("SoundVolume", 1f);
        musicLevel = PlayerPrefs.GetFloat("MusicVolume", 1f);
    }

    public void QuitApp()
    {
        // Set global gameplay stats for data logging.
        int levelNumber = int.Parse(PlayerPrefs.GetString("LevelNumber"));
        string difficulty = PlayerPrefs.GetString("DifficultyName");
        int stage = int.Parse(PlayerPrefs.GetString("Stage"));
        firebaseManager.GameLogMutation(levelNumber, stage, difficulty, Actions.ExitedGame, 0);
        Application.Quit();
    }

    public void ResetSettings(FirebaseManager fm)
    {
        // Set global gameplay stats for data logging.
        int levelNumber = int.Parse(PlayerPrefs.GetString("LevelNumber", "0"));
        string difficulty = PlayerPrefs.GetString("DifficultyName", null);
        Debug.Log($"Difficulty: {difficulty}");
        int stage = int.Parse(PlayerPrefs.GetString("Stage", "0"));
        fm.GameLogMutation(levelNumber, stage, difficulty.Length > 1 ? difficulty : null, Actions.NewGame, 0);
        PlayerPrefs.DeleteAll();
    }
    public void ProjEasyReloadScene()
    {
        PlayerPrefs.SetInt("Life", 3);
        SceneManager.LoadScene("LevelThreeStage1");
    }
    public void ProjMedReloadScene()
    {
        PlayerPrefs.SetInt("Life", 3);
        SceneManager.LoadScene("LevelThreeStage1Medium");
    }

}
