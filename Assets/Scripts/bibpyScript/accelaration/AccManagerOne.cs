using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccManagerOne : MonoBehaviour
{

    public float Vi;
    public float Vf;
    float timer;
    public float time;
    float generateTime;
    public float accelaration;
    private Player thePlayer;
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
    // Start is called before the first frame update
    void Start()
    {
        //thePlayer = FindObjectOfType<Player>();
        theBike = FindObjectOfType<BikeManager>();
        gender = PlayerPrefs.GetString("Gender");
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
        accelaration = accSimulation.playerAnswer;
        if (accSimulation.simulate)
        {
            if (gas)
            {
                theBike.moveSpeed += accelaration * Time.fixedDeltaTime;
            }
            timer += Time.fixedDeltaTime;
            if (timer >= time)
            {
                gas = false;
                accSimulation.simulate = false;
                //theBike.moveSpeed = theBike.myRigidbody.velocity.x;
                if (accelaration != correctAns)
                {
                     walls.SetActive(true);
                     
                    if (accelaration < correctAns & accelaration > correctAns - 1)
                    {
                        if (currentPos >= playerDistance)
                        {
                            //theBike.moveSpeed = Vf - 1;
                            
                            
                        }
                    }
                    if (accelaration > correctAns & accelaration < correctAns + 1)
                    {
                        if (currentPos >= playerDistance)
                        {
                            //theBike.moveSpeed = Vf + 1;
                            
                           
                        }
                    }
                }
                if (accelaration == correctAns)
                {
                    theBike.moveSpeed = Vf;
                }
            }


        }




    }
    public void generateProblem()
    {
        generateTime = Random.Range(3.0f, 3.5f);
        time = (float)System.Math.Round(generateTime, 2);
        accSimulation.question = (("In order for <b>") + PlayerPrefs.GetString("Name") + ("</b> to enter the tunnel on the otherside of the platform where  <b>") + pronoun + ("</b>is in, <b>") + pronoun + ("</b> must drive his motorcycle from a complete standstill to a speed of <b>") + Vf.ToString("F1") + ("</b> m/s, after ") + time.ToString("F1") + ("seconds. What should be ") + pronoun + (" accelaration in order to achieve the final velocity?"));

        //theHeart.losslife = false;

    }
}
