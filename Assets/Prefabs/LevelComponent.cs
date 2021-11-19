using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelComponent : MonoBehaviour
{
    public string levelName;
    public TMP_Text levelNameObject;
    void Update()
    {
        if (levelName.Length<1)
        {
            levelNameObject.text = "No Title";
        }else{
            levelNameObject.text = levelName;
        }
    }
}
