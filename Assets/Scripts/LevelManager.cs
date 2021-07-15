using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    void Start()
    {

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
