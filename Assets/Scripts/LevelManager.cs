using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [Range(0, 100)]
    public int unlockedCount = 0;
    public LevelCard[] levelCards;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < levelCards.Length; i++)
        {
            levelCards[i].locked = unlockedCount <=i+1;
        }
        
    }
}
