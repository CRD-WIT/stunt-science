using UnityEngine;
using GameConfig;

public class LevelManager : MonoBehaviour
{

    [Range(0, 16)]
    public int unlockedCount = 0;
    [Header("Velocity")]

    [Range(0, 3)] public int levelVelocityEasy = 0;
    [Range(0, 3)] public int levelVelocityMedium = 0;
    [Range(0, 3)] public int levelVelocityHard = 0;

    [Header("Acceleration")]
    [Range(0, 3)] public int levelAccelerationEasy = 0;
    [Range(0, 3)] public int levelAccelerationMedium = 0;
    [Range(0, 3)] public int levelAccelerationHard = 0;
    [Header("Free Fall and Projectile")]
    [Range(0, 3)] public int levelFreeFallProjectileEasy = 0;
    [Range(0, 3)] public int levelFreeFallProjectileMedium = 0;
    [Range(0, 3)] public int levelFreeFallProjectileHard = 0;
    [Header("Circular Motion")]
    [Range(0, 3)] public int levelCircularMotionEasy = 0;
    [Range(0, 3)] public int levelCircularMotionMedium = 0;
    [Range(0, 3)] public int levelCircularMotionHard = 0;
    [Header("Forces")]
    [Range(0, 3)] public int levelForcesEasy = 0;
    [Range(0, 3)] public int levelForcesMedium = 0;
    [Range(0, 3)] public int levelForcesHard = 0;
    public LevelCard[] levelCards;

    bool level1Locked;

    bool level2Locked;

    bool level3Locked;

    bool level4Locked;

    bool level5Locked;

    void Start()
    {
        levelVelocityEasy = PlayerPrefs.GetInt("levelVelocityEasy", 0);
        levelVelocityMedium = PlayerPrefs.GetInt("levelVelocityMedium", 0);
        levelVelocityHard = PlayerPrefs.GetInt("levelVelocityHard", 0);
        level1Locked = PlayerPrefs.GetInt("level1Locked", 0) == 0 ? false : true;

        levelAccelerationEasy = PlayerPrefs.GetInt("levelAccelerationEasy", 0);
        levelAccelerationMedium = PlayerPrefs.GetInt("levelAccelerationMedium", 0);
        levelAccelerationHard = PlayerPrefs.GetInt("levelAccelerationHard", 0);
        level2Locked = PlayerPrefs.GetInt("level2Locked", 0) == 0 ? false : true;

        levelFreeFallProjectileEasy = PlayerPrefs.GetInt("levelFreeFallProjectileEasy", 0);
        levelFreeFallProjectileMedium = PlayerPrefs.GetInt("levelFreeFallProjectileMedium", 0);
        levelFreeFallProjectileHard = PlayerPrefs.GetInt("levelFreeFallProjectileHard", 0);
        level3Locked = PlayerPrefs.GetInt("level3Locked", 0) == 0 ? false : true;

        levelCircularMotionEasy = PlayerPrefs.GetInt("levelCircularMotionEasy", 0);
        levelCircularMotionMedium = PlayerPrefs.GetInt("levelCircularMotionMedium", 0);
        levelCircularMotionHard = PlayerPrefs.GetInt("levelCircularMotionHard", 0);
        level4Locked = PlayerPrefs.GetInt("level4Locked", 0) == 0 ? false : true;

        levelForcesEasy = PlayerPrefs.GetInt("levelForcesEasy", 0);
        levelForcesMedium = PlayerPrefs.GetInt("levelForcesMedium", 0);
        levelForcesHard = PlayerPrefs.GetInt("levelForcesHard", 0);
        level5Locked = PlayerPrefs.GetInt("level5Locked", 0) == 0 ? false : true;
    }

    public LevelData GetLevelData(int levelNumber, Difficulty difficulty)
    {
        // level 1
        if (levelNumber == 1 && difficulty == Difficulty.Easy)
        {
            return new LevelData(levelVelocityEasy, level1Locked);
        }
        if (levelNumber == 1 && difficulty == Difficulty.Medium)
        {
            return new LevelData(levelVelocityMedium, level1Locked);
        }
        if (levelNumber == 1 && difficulty == Difficulty.Hard)
        {
            return new LevelData(levelVelocityHard, level1Locked);
        }

        // level 2
        if (levelNumber == 2 && difficulty == Difficulty.Easy)
        {
            return new LevelData(levelAccelerationEasy, level2Locked);
        }
        if (levelNumber == 2 && difficulty == Difficulty.Medium)
        {
            return new LevelData(levelAccelerationMedium, level2Locked);
        }
        if (levelNumber == 2 && difficulty == Difficulty.Hard)
        {
            return new LevelData(levelAccelerationHard, level2Locked);
        }

        // level 3
        if (levelNumber == 3 && difficulty == Difficulty.Easy)
        {
            return new LevelData(levelFreeFallProjectileEasy, level3Locked);
        }
        if (levelNumber == 3 && difficulty == Difficulty.Medium)
        {
            return new LevelData(levelFreeFallProjectileMedium, level3Locked);
        }
        if (levelNumber == 3 && difficulty == Difficulty.Hard)
        {
            return new LevelData(levelCircularMotionHard, level3Locked);
        }


        // level 4
        if (levelNumber == 4 && difficulty == Difficulty.Easy)
        {
            return new LevelData(levelCircularMotionEasy, level4Locked);
        }
        if (levelNumber == 4 && difficulty == Difficulty.Medium)
        {
            return new LevelData(levelCircularMotionMedium, level4Locked);
        }
        if (levelNumber == 4 && difficulty == Difficulty.Hard)
        {
            return new LevelData(levelCircularMotionHard, level4Locked);
        }

        // level 5
        if (levelNumber == 5 && difficulty == Difficulty.Easy)
        {
            return new LevelData(levelForcesEasy, level5Locked);
        }
        if (levelNumber == 5 && difficulty == Difficulty.Medium)
        {
            return new LevelData(levelForcesMedium, level5Locked);
        }
        if (levelNumber == 5 && difficulty == Difficulty.Hard)
        {
            return new LevelData(levelForcesHard, level5Locked);
        }

        return new LevelData(0, true);

    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < levelCards.Length; i++)
        {
            levelCards[i].locked = unlockedCount <= i + 1;
        }
        levelCards[0].starActiveCount = levelVelocityEasy;
        levelCards[1].starActiveCount = levelVelocityMedium;
        levelCards[2].starActiveCount = levelVelocityHard;
        levelCards[3].starActiveCount = levelAccelerationEasy;
        levelCards[4].starActiveCount = levelAccelerationMedium;
        levelCards[5].starActiveCount = levelAccelerationHard;
        levelCards[6].starActiveCount = levelFreeFallProjectileEasy;
        levelCards[7].starActiveCount = levelFreeFallProjectileMedium;
        levelCards[8].starActiveCount = levelFreeFallProjectileHard;
        levelCards[9].starActiveCount = levelCircularMotionEasy;
        levelCards[10].starActiveCount = levelCircularMotionMedium;
        levelCards[11].starActiveCount = levelCircularMotionHard;
        levelCards[12].starActiveCount = levelForcesEasy;
        levelCards[13].starActiveCount = levelForcesMedium;
        levelCards[14].starActiveCount = levelForcesHard;

        

    }
}
