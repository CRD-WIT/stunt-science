using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class AccManagerTwo : MonoBehaviour
{

    public float Vi;
    public float Vf;
    public float timer;
    public float time;
    float generateAcceleration;
    public float deceleration;
    private Player thePlayer;
    private accSimulation theSimulation;
    private BikeManager theBike;
    public bool gas;
    float correctAns;
    float generateAns;
    float playerDistance;
    float playerVf;
    float currentPos;
    public float correctDistance;
    string pronoun;
    string gender;
    public GameObject walls;
    public GameObject AfterStuntMessage;
    public TMP_Text stuntMessageTxt;
    public Button retry, next;
    // Start is called before the first frame update
    void Start()
    {
        //thePlayer = FindObjectOfType<Player>();
        theBike = FindObjectOfType<BikeManager>();
        gender = PlayerPrefs.GetString("Gender");
        theSimulation = FindObjectOfType<accSimulation>();
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
        generateAns = (Vi - Vf) / deceleration;
        correctAns = (float)System.Math.Round(generateAns, 2);
        playerDistance = (time * time) * deceleration / 2;
        playerVf = (2 * playerDistance) / time;
        currentPos = theBike.transform.position.x;
        
        if (gas)
            {
                theBike.moveSpeed = Vi;
            }
        
        if (accSimulation.simulate)
        {
            gas = true;
            time = accSimulation.playerAnswer;
             if (time != correctAns)
                {
                     walls.SetActive(true);
                     retry.gameObject.SetActive(true);
                     
                    if (time < correctAns & time > correctAns - 0.5f)
                    {
                        time -= 0.5f;
                    }
                    if (time > correctAns & time < correctAns + 0.5f)
                    {
                        time += 0.5f;
                    }
                    if(time < correctAns)
                    {
                        stuntMessageTxt.text = "<b><color=red>Stunt Failed!!!</b>\n\n" + PlayerPrefs.GetString("Name") + " deccelerates the motorcycle too fast and overshot the tunnel entrance. The correct answer is </color>" + correctAns.ToString("F1") +"seconds.";
                    }
                    if(time > correctAns)
                    {
                        stuntMessageTxt.text = "<b><color=red>Stunt Failed!!!</b>\n\n" + PlayerPrefs.GetString("Name") + " deccelerates the motorcycle too slow and undershot the tunnel entrance. The correct answer is </color>" + correctAns.ToString("F1") +"seconds.";
                    }
                }
                if (time == correctAns)
                {
                    next.gameObject.SetActive(true);
                    stuntMessageTxt.text = "<b><color=green>Your Answer is Correct!!!</b>\n\n" + PlayerPrefs.GetString("Name") + " went through the tunnel</color>";
                    

                }          
            if (theBike.brake == true)
            {
                gas = false;
                timer += Time.fixedDeltaTime;
                if (timer < time)
                {
                    theBike.moveSpeed -= deceleration * Time.fixedDeltaTime;
                }
            }
            if (timer >= time)
            {
                theBike.moveSpeed = theBike.myRigidbody.velocity.x;
                StartCoroutine(StuntResult());
                accSimulation.simulate = false;
                //theBike.moveSpeed = theBike.myRigidbody.velocity.x;
               
            }


        }




    }
    public void generateProblem()
    {
         Vi = 20;
        //time = (float)System.Math.Round(generateTime, 2);
        accSimulation.question = (PlayerPrefs.GetString("Name") + ("</b> is initially speeding is motorcycle at <b>") + Vi.ToString("F1") + ("</b> m/s and must decelerate to a final velocity of 10 m/s right before dropping the ledge so it could safely enter the tunnel across it. How long should <b>") + pronoun + ("</b> apply brake if braking deccelerates the motorcycle by <b>") + deceleration.ToString("F1") + ("</b> m/s²?"));

        //theHeart.losslife = false;

    }
    IEnumerator StuntResult()
    {
        yield return new WaitForSeconds(2);
        StartCoroutine(theSimulation.DirectorsCall());
        yield return new WaitForSeconds(2);
        AfterStuntMessage.SetActive(true);
        walls.SetActive(false);
    }
}