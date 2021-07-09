using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using GameConfig;

public class LevelCard : MonoBehaviour
{
    public TMP_Text levelNumberObject;
    public TMP_Text levelDifficultyObject;
    public LevelStar[] levelStars;
    [Range(0, 3)]
    public int starActiveCount;
    [Range(0, 100)]
    public int levelNumberText;
    public bool locked;
    public GameObject cardUnlocked;
    public GameObject cardLocked;
    public GameObject levelDetails;
    public Difficulty levelDifficultyName;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        levelNumberObject.SetText(levelNumberText.ToString());
        levelStars[0].ToggleStar(starActiveCount>=1);
        levelStars[1].ToggleStar(starActiveCount>=2);
        levelStars[2].ToggleStar(starActiveCount>=3);
        cardUnlocked.SetActive(!locked);
        levelDetails.SetActive(!locked);
        cardLocked.SetActive(locked);
        switch (levelDifficultyName)
        {
            case Difficulty.Easy:
                levelDifficultyObject.SetText("Easy");
                break;
            case Difficulty.Medium:
                levelDifficultyObject.SetText("Medium");
                break;
            case Difficulty.Hard:
                levelDifficultyObject.SetText("Hard");
                break;
        }        
    }
}
