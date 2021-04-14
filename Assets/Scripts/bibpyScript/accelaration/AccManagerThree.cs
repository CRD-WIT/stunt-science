using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class AccManagerThree : MonoBehaviour
{
    public float Vi;
    public float Vf;
    public float timer;
    public float time;
    float generateTime;
    float generateAcceleration;
    public float deacceleration;
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
        generateAns = 60 / time;
        correctAns = (float)System.Math.Round(generateAns, 2);
        playerDistance = (time * time) * deacceleration / 2;
        playerVf = (2 * playerDistance) / time;
        currentPos = theBike.transform.position.x;
        deacceleration = generateAns / time;
        Vi = accSimulation.playerAnswer;
        if (gas)
            {
                theBike.moveSpeed = Vi;
            }
        
        if (accSimulation.simulate)
        {
            gas = true;
            
             if (Vi != correctAns)
                {
                     walls.SetActive(true);
                     retry.gameObject.SetActive(true);
                     
                    if (Vi < correctAns & Vi > correctAns - 1f)
                    {
                        Vi -= 1f;
                    }
                    if (Vi > correctAns & Vi < correctAns +1f)
                    {
                        Vi += 1f;
                    }
                    if(Vi < correctAns)
                    {
                        stuntMessageTxt.text = "<b><color=red>Stunt Failed!!!</b>\n\n" + PlayerPrefs.GetString("Name") + " deccelerates the motorcycle too fast and overshot the tunnel entrance. The correct answer is </color>" + correctAns.ToString("F1") +"seconds.";
                    }
                    if(Vi > correctAns)
                    {
                        stuntMessageTxt.text = "<b><color=red>Stunt Failed!!!</b>\n\n" + PlayerPrefs.GetString("Name") + " deccelerates the motorcycle too slow and undershot the tunnel entrance. The correct answer is </color>" + correctAns.ToString("F1") +"seconds.";
                    }
                }
                if (Vi == correctAns)
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
                    theBike.moveSpeed -= deacceleration * Time.fixedDeltaTime;
                    if(theBike.moveSpeed <= 0)
                    {
                        theBike.moveSpeed = 0;
                    }
                }
            }
            if (timer >= time)
            {
                theBike.moveSpeed = 0;
                StartCoroutine(StuntResult());
                accSimulation.simulate = false;
                //theBike.moveSpeed = theBike.myRigidbody.velocity.x;
               
            }


        }




    }
    public void generateProblem()
    {
        generateTime = Random.Range(2.5f, 4.5f);
        theBike.transform.position = new Vector2(-10, theBike.transform.position.y);
        time = (float)System.Math.Round(generateTime, 2);
        
        accSimulation.question = (PlayerPrefs.GetString("Name") + ("</b> must park his motorcycle perfectly at the back of truck. If braking the motorcycle deaccelerates it by <b>") + deacceleration.ToString("F1") + ("</b> m/s, what should be its initial velocity(Vi) so it will come into complete stop after braking it for exactly  <b>") + time.ToString("F2") + ("</b> seconds?"));
        
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
