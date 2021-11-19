using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StarsCount : MonoBehaviour
{
    public LevelManager levelManager;

    public TMP_Text scoreText;

    void Start(){
        
    }
    void Update()
    {

        scoreText.text = $"{levelManager.levelScoreTotal}/45";
    }
}
