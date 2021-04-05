using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;


public class StageTwoManager : MonoBehaviour
{
    private Player thePlayer;
    float distance;
    float speed;
    float finalSpeed;
    string gender;
    string pronoun;
    float answer;
    Vector2 PlayerStartPoint;
    
    TimeSpan duration;
    private float gameTime = 0.0f;

   
    public TextMeshProUGUI timer;
    bool simulate;
    
    
    // Start is called before the first frame update
    void Start()
    {
        thePlayer = FindObjectOfType<Player>();
        gender = PlayerPrefs.GetString("Gender");
        PlayerStartPoint = thePlayer.transform.position;
        
       
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
        if (gender == "Male")
        {
            pronoun = ("he");
        }
        if (gender == "Female")
        {
            pronoun = ("she");
        }
        /*if(simulate)
        {
            duration = TimeSpan.FromMilliseconds(gameTime * 1000);

            int milliseconds = Convert.ToInt32(duration.ToString(@"ff"));
            int seconds = Convert.ToInt32(duration.ToString(@"ss"));

            timer.SetText($"{seconds}:{milliseconds} sec");
        }*/
        
    
       
    }
    public void generateProblem()
    {
        distance = UnityEngine.Random.Range(5, 10);
        speed = UnityEngine.Random.Range(2.0f, 5.0f);
        finalSpeed = (float)System.Math.Round(speed, 2);
        SimulationManager.question = (("The ceiling is still crumbling and the next safe spot is <b>")+ distance + ("</b> meter away from  <b>") + PlayerPrefs.GetString("Name") + ("</b>. If <b>") + PlayerPrefs.GetString("Name") + ("</b> will now run at exactly <b>")+ finalSpeed.ToString("F1")+ ("</b> m/s, how long should ")+ pronoun + (" run so that ")+ pronoun + (" will not get hit by the crumbling debris of the ceiling?"));
        
    }
    public void play()
    {
        //simulate = true;
        //answer = SimulationManager.playerAnswer;
        //thePlayer.moveSpeed = speed;

    }
    public void reset()
    {
        thePlayer.transform.position = PlayerStartPoint;
        thePlayer.moveSpeed = 0;
        generateProblem();
    }
}
