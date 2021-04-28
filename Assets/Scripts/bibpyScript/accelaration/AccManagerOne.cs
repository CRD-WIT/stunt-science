using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class AccManagerOne : MonoBehaviour
{

    public float Vi;
    public float Vf;
    public float timer;
    public float time;
    float generateTime;
    public float accelaration;
    private Player thePlayer;
    private accSimulation theSimulation;
    private HeartManager theHeart;
    private BikeManager theBike;
    public bool gas = true;
    float correctAns;
    float generateAns;
    float playerDistance;
    float playerVf;
    float currentPos;
    float correctDistance;
    string pronoun;
    string gender;
    public GameObject walls;
    public GameObject AfterStuntMessage;
    public TMP_Text stuntMessageTxt;
    public Button retry, next;
    bool tooSlow;
    // Start is called before the first frame update
    void Start()
    {
        AfterStuntMessage.SetActive(false);
        //thePlayer = FindObjectOfType<Player>();
        theBike = FindObjectOfType<BikeManager>();
        gender = PlayerPrefs.GetString("Gender");
        theSimulation = FindObjectOfType<accSimulation>();
        theHeart = FindObjectOfType<HeartManager>();
        theBike.moveSpeed = 0;
        if (gender == "Male")
        {
            pronoun = ("he");
        }
        if (gender == "Female")
        {
            pronoun = ("she");
        }
        generateProblem();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        generateAns = (Vf - Vi) / time;
        correctAns = (float)System.Math.Round(generateAns, 2);
        playerDistance = (time * time) * accelaration / 2;
        correctDistance = (time * time) * correctAns / 2;
        playerVf = (2 * playerDistance) / time;
        currentPos = theBike.transform.position.x;
        if(tooSlow)
        {
            theBike.moveSpeed -= 2 * Time.fixedDeltaTime;
            if(theBike.moveSpeed <= 0)
            {
                tooSlow = false;
                theBike.moveSpeed = 0;
                StartCoroutine(StuntResult());
                theHeart.losinglife();
            }
        }
        
        if (accSimulation.simulate)
        {
            accelaration = accSimulation.playerAnswer;
            
            if (accelaration != correctAns)
                {
                     walls.SetActive(true);
                     retry.gameObject.SetActive(true);
                     accSimulation.playerDead = true;
                     if (accelaration < correctAns & accelaration > correctAns - .5f)
                     {
                         accelaration -= .5f;
                     }
                     if (accelaration > correctAns & accelaration < correctAns + 0.5f)
                     {
                         accelaration += 1f;
                     }
                    if(accelaration < correctAns)
                    {
                        stuntMessageTxt.text = "<b><color=red>Stunt Failed!!!</b>\n\n" + PlayerPrefs.GetString("Name") + " accelerated the motorcycle too slow and undershot the tunnel entrance. The correct answer is </color>" + correctAns.ToString("F1") +"m/s².";
                    }
                    if(accelaration > correctAns)
                    {
                        stuntMessageTxt.text = "<b><color=red>Stunt Failed!!!</b>\n\n" + PlayerPrefs.GetString("Name") + " accelerated the motorcycle too fast and undershot the tunnel entrance. The correct answer is </color>" + correctAns.ToString("F1") +"m/s².";
                    }
                }
                if (accelaration == correctAns)
                {
                    next.gameObject.SetActive(true);
                    stuntMessageTxt.text = "<b><color=green>Your Answer is Correct!!!</b>\n\n" + PlayerPrefs.GetString("Name") + " went through the tunnel</color>";
                    theBike.moveSpeed = Vf;
                    
                    

                }
            if (gas)
            {
                theBike.moveSpeed += accelaration * Time.fixedDeltaTime;
            }
            timer += Time.fixedDeltaTime;
            if (timer >= time)
            {
                if (accelaration == correctAns)
                {
                    StartCoroutine(StuntResult());
                }
                gas = false;
                accSimulation.simulate = false;
                
                if(currentPos < 10)
                {  
                   tooSlow = true;   
                }
                
            }


        }




    }
    public void generateProblem()
    {
        generateTime = Random.Range(3.0f, 3.5f);
        time = (float)System.Math.Round(generateTime, 2);
        accSimulation.question = (("In order for <b>") + PlayerPrefs.GetString("Name") + ("</b> to enter the tunnel on the otherside of the platform where  <b>") + pronoun + ("</b>is in, <b>") + pronoun + ("</b> must drive his motorcycle from a complete standstill to a speed of <b>") + Vf.ToString("F1") + ("</b> m/s, after ") + time.ToString("F1") + ("seconds. What should be ") + pronoun + (" accelaration in order to achieve the final velocity?"));
        theHeart.losslife = false;
        theBike.moveSpeed = 0;

    }
    IEnumerator StuntResult()
    {
        
        StartCoroutine(theSimulation.DirectorsCall());
        yield return new WaitForSeconds(1);
        AfterStuntMessage.SetActive(true);
        walls.SetActive(false);
    }
    
}
