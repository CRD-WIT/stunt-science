using UnityEngine;
using System.Linq;

public class LevelManager : MonoBehaviour
{
    [Header("Backgrounds")]
    public GameObject[] levelBackgrounds;

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

    public int[] levelScoreCollection;

    public int levelScoreTotal = 0;

    public int activeBackgroundIndex = 0;

    void Start()
    {
        //unlockedCount = PlayerPrefs.GetInt("unlockedCount", 2);

        activeBackgroundIndex = Random.Range(0, levelBackgrounds.Length);

        levelVelocityEasy = PlayerPrefs.GetInt("levelVelocityEasy", 0);
        levelVelocityMedium = PlayerPrefs.GetInt("levelVelocityMedium", 0);
        levelVelocityHard = PlayerPrefs.GetInt("levelVelocityHard", 0);

        levelCards[0].locked = false;
        levelCards[1].locked = PlayerPrefs.GetInt("level1MediumLocked", 1) == 1 ? true : false;
        levelCards[2].locked = PlayerPrefs.GetInt("level1HardLocked", 1) == 1 ? true : false;

        levelAccelerationEasy = PlayerPrefs.GetInt("levelAccelerationEasy", 0);
        levelAccelerationMedium = PlayerPrefs.GetInt("levelAccelerationMedium", 0);
        levelAccelerationHard = PlayerPrefs.GetInt("levelAccelerationHard", 0);

        //levelCards[3].locked = PlayerPrefs.GetInt("level2EasyLocked", 1) == 1 ? true : false;
        levelCards[3].locked = false;
        levelCards[4].locked = PlayerPrefs.GetInt("level2MediumLocked", 1) == 1 ? true : false;
        levelCards[5].locked = PlayerPrefs.GetInt("level2HardLocked", 1) == 1 ? true : false;

        levelFreeFallProjectileEasy = PlayerPrefs.GetInt("levelFreeFallProjectileEasy", 0);
        levelFreeFallProjectileMedium = PlayerPrefs.GetInt("levelFreeFallProjectileMedium", 0);
        levelFreeFallProjectileHard = PlayerPrefs.GetInt("levelFreeFallProjectileHard", 0);

        //levelCards[6].locked = PlayerPrefs.GetInt("level3EasyLocked", 1) == 1 ? true : false;
        levelCards[6].locked = false;
        levelCards[7].locked = PlayerPrefs.GetInt("level3MediumLocked", 1) == 1 ? true : false;
        levelCards[8].locked = PlayerPrefs.GetInt("level3HardLocked", 1) == 1 ? true : false;

        levelCircularMotionEasy = PlayerPrefs.GetInt("levelCircularMotionEasy", 0);
        levelCircularMotionMedium = PlayerPrefs.GetInt("levelCircularMotionMedium", 0);
        levelCircularMotionHard = PlayerPrefs.GetInt("levelCircularMotionHard", 0);

        // levelCards[9].locked = PlayerPrefs.GetInt("level4EasyLocked", 1) == 1 ? true : false;        
        levelCards[9].locked = true;
        levelCards[10].locked = PlayerPrefs.GetInt("level4MediumLocked", 1) == 1 ? true : false;
        levelCards[11].locked = PlayerPrefs.GetInt("level4HardLocked", 1) == 1 ? true : false;

        levelForcesEasy = PlayerPrefs.GetInt("levelForcesEasy", 0);
        levelForcesMedium = PlayerPrefs.GetInt("levelForcesMedium", 0);
        levelForcesHard = PlayerPrefs.GetInt("levelForcesHard", 0);

        // levelCards[12].locked = PlayerPrefs.GetInt("level5EasyLocked", 1) == 1 ? true : false;
        levelCards[12].locked = true;
        levelCards[13].locked = PlayerPrefs.GetInt("level5MediumLocked", 1) == 1 ? true : false;
        levelCards[14].locked = PlayerPrefs.GetInt("level5HardLocked", 1) == 1 ? true : false;

        levelScoreCollection = new int[15] {
            levelVelocityEasy,
            levelVelocityMedium,
            levelVelocityHard,
            levelAccelerationEasy,
            levelAccelerationMedium,
            levelAccelerationHard,
            levelFreeFallProjectileEasy,
            levelFreeFallProjectileMedium,
            levelFreeFallProjectileHard,
            levelCircularMotionEasy,
            levelCircularMotionMedium,
            levelCircularMotionHard,
            levelForcesEasy,
            levelForcesMedium,
            levelForcesHard
        };


    }

    // Update is called once per frame
    void Update()
    {
        levelScoreTotal = levelScoreCollection.Sum();
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

        for (int i = 0; i < levelBackgrounds.Length; i++)
        {
            if (i == activeBackgroundIndex)
            {
                levelBackgrounds[i].SetActive(true);
            }
            else
            {
                levelBackgrounds[i].SetActive(false);
            }
        }
    }
}
