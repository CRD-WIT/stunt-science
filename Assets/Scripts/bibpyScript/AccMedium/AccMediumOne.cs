using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AccMediumOne : MonoBehaviour
{
    public GameObject hangingRagdoll, ropeTip, playerInTruck, ragdollPrefab, stickmanPoint, playerGrabline, carInitials, chopperInitials;
    public GameObject ragdollPause, ropePoint, carArrow, chopperArrow;
    public PlayerB thePlayer;
    public Hellicopter theChopper;
    public SubHellicopter theSubChopper;
    public TruckManager theTruck;
    private AccMidSimulation theSimulate;
    private HeartManager theHeart;
    public bool chase, togreen, tored;
    public float timer, correctAnswer;
    public float velocity, accelaration, vf, distanceH;
    public QuestionControllerB theQuestion;
    float generateVelocity, generateAccelaration, generateCorrectAnswer;
    public float chopperPos, truckPos, answer;
    public bool readyToJump, follow, timeOn;
    public TMP_Text timertxt, vHtxt, viTtxt, aTtxt, actiontxt, playTimertxt;
    float grabLineDistance, playerGrabLineDistance;
    private Vector2 subChopperStartPos, chopperStartPos;
    // Start is called before the first frame update
    void Start()
    {
        theHeart = FindObjectOfType<HeartManager>();
        subChopperStartPos = theSubChopper.transform.position;
        chopperStartPos = theChopper.transform.position;
        theSimulate = FindObjectOfType<AccMidSimulation>();
        generateProblem();

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        vHtxt.text = ("v = ") + velocity.ToString("F2") + ("m/s");
        vf = accelaration * AccMidSimulation.playerAnswer;
        aTtxt.text = ("a = ") + accelaration.ToString("F2") + ("m/s²");
        //playerGrabLineDistance = velocity * AccMidSimulation.playerAnswer;
        playerGrabLineDistance = (accelaration * (AccMidSimulation.playerAnswer * AccMidSimulation.playerAnswer)) / 2;
        truckPos = theTruck.transform.position.x;

    if(theTruck.transform.position.x >= 72)
    {
        theTruck.moveSpeed = 0;
        theTruck.accelerating = false;
    }


        if (AccMidSimulation.simulate == true)
        {

            answer = AccMidSimulation.playerAnswer;
            distanceH = answer * velocity + 0.67f;
            timertxt.gameObject.SetActive(true);

            
            chopperPos = theChopper.transform.position.x;
            hangingRagdoll.transform.position = ropeTip.transform.position;
            theChopper.flySpeed = velocity;
            playerGrabline.transform.position = new Vector2(playerGrabLineDistance + 2.67f, playerGrabline.transform.position.y);
            if (timeOn)
            {
                timer += Time.fixedDeltaTime;
                playTimertxt.text = timer.ToString("F2") + "s";
                timertxt.text = timer.ToString("F2") + "s";
                thePlayer.transform.position = playerInTruck.transform.position;
            }
            if (chase)
            {
                carArrow.SetActive(false);
                chopperArrow.SetActive(false);
                carInitials.transform.position = new Vector2(theTruck.transform.position.x + 2.1f, theTruck.transform.position.y);
                chopperInitials.transform.position = new Vector2(theChopper.transform.position.x + 2.1f, theChopper.transform.position.y);


                viTtxt.text = ("v= ") + theTruck.moveSpeed.ToString("F2") + ("m/s");
                timertxt.gameObject.transform.position = theTruck.transform.position;
                theTruck.accelerating = true;
                theTruck.accelaration = accelaration;
                


                if (answer == correctAnswer)
                {
                    actiontxt.text = "Next";
                    theQuestion.answerIsCorrect = true;
                    if (timer >= answer - .45f)
                    {

                        if (readyToJump)
                        {
                            StartCoroutine(StuntResult());
                            StartCoroutine(jump());
                            //grabline.SetActive(true);
                        }
                        //theQuestion.SetModalTitle("Stunt Success");
                        //theQuestion.SetModalText(PlayerPrefs.GetString("Name") + (" has grabbed the rope and is now succesfully hanging from a hellicopter</color>"));
                    }
                    if (truckPos >= 72)
                    {
                        theTruck.accelerating = false;
                        theTruck.moveSpeed = 0;
                        chase = false;
                    }
                }
                if (answer < correctAnswer)
                {
                    if (truckPos >= 68)
                    {
                        theTruck.accelerating = false;
                        theTruck.moveSpeed = 0;
                        chase = false;
                    }
                    actiontxt.text = "Retry";
                    AccMidSimulation.playerDead = true;
                    if (timer >= answer - .45f)
                    {

                        if (readyToJump)
                        {
                            StartCoroutine(StuntResult());
                            StartCoroutine(jump());
                            //grabline.SetActive(true);
                        }

                    }

                    //theQuestion.SetModalTitle("Stunt Failed");
                    //theQuestion.SetModalText(PlayerPrefs.GetString("Name") + " grab the rope too soon. The correct answer is </color>" + correctAnswer.ToString("F2") + "seconds.");
                }
                if (answer > correctAnswer)
                {

                    actiontxt.text = "Retry";
                    //theQuestion.SetModalTitle("Stunt Failed");
                    AccMidSimulation.playerDead = true;
                    if (playerGrabLineDistance < 56)
                    {
                        if (timer >= answer - .45f)
                        {
                            if (readyToJump)
                            {
                               
                                StartCoroutine(jump());
                                StartCoroutine(StuntResult());

                            }
                            //theQuestion.SetModalText(PlayerPrefs.GetString("Name") + (" grab the rope too late. The correct answer is </color>" + correctAnswer.ToString("F2") + "seconds."));
                        }
                        if (truckPos >= 70)
                        {
                            theTruck.accelerating = false;
                            theTruck.moveSpeed = 0;
                            chase = false;
                        }
                    }
                    if (playerGrabLineDistance > 56)
                    {

                        if (truckPos >= 72)
                        {
                            theHeart.losinglife();
                            theTruck.accelerating = false;
                            theTruck.moveSpeed = 0;
                            StartCoroutine(StuntResult());
                            velocity = 0;
                            chase = false;
                            timeOn = false;
                            
                        }
                        //theQuestion.SetModalText(PlayerPrefs.GetString("Name") + (" didnt get the chance to grab the rope. The correct answer is </color>" + correctAnswer.ToString("F2") + "seconds."));
                    }


                }

            }
            if (follow)
            {
                chopperInitials.transform.position = new Vector2(theChopper.transform.position.x + 2.1f, theChopper.transform.position.y);
                carInitials.transform.position = new Vector2(theTruck.transform.position.x + 2.1f, theTruck.transform.position.y);
                if (answer == correctAnswer)
                {
                    timertxt.gameObject.transform.position = new Vector2(theChopper.transform.position.x, timertxt.gameObject.transform.position.y);
                }
            }
        }
    }
    public void generateProblem()
    {
        theChopper.flySpeed = 0;
        theHeart.losslife = false;
        timeOn = false;
        carArrow.SetActive(true);
        chopperArrow.SetActive(true);
        togreen = false;
        tored = false;
        theTruck.moveSpeed = 0;
        timertxt.gameObject.SetActive(false);
        carInitials.SetActive(true);
        ropePoint.SetActive(false);
        ragdollPause.SetActive(false);
        theChopper.transform.position = chopperStartPos;
        theSubChopper.transform.position = subChopperStartPos;
        follow = false;
        timertxt.gameObject.transform.position = theTruck.transform.position;
        timertxt.gameObject.SetActive(false);
        theSubChopper.gameObject.SetActive(true);
        theSubChopper.fade = false;
        playerGrabline.SetActive(false);
        timer = 0;
        timertxt.text = timer.ToString("F2") + "s";
        playTimertxt.text = timer.ToString("F2") + "s";
        readyToJump = true;
        generateAccelaration = Random.Range(4f, 8f);
        generateVelocity = Random.Range(8f, 10f);
        accelaration = (float)System.Math.Round(generateAccelaration, 2);
        velocity = (float)System.Math.Round(generateVelocity, 2);
        generateCorrectAnswer = (2 * velocity) / accelaration;
        correctAnswer = (float)System.Math.Round(generateCorrectAnswer, 2);
        grabLineDistance = velocity * correctAnswer;
        carInitials.transform.position = new Vector2(theTruck.transform.position.x + 2f, theTruck.transform.position.y);
        chopperInitials.transform.position = new Vector2(theSubChopper.transform.position.x + 2.2f, theSubChopper.transform.position.y);
        viTtxt.text = ("vi = 0m/s ");
        theQuestion.SetQuestion((("<b>") + PlayerPrefs.GetString("Name") + ("</b> is standing in top of a non moving truck, waiting for the hellicopter to pass by, chase it with the truck, and grab the rope hanging from it, If the hellicopter flies forward at a constant speed of <b>") + velocity.ToString("F2") + ("</b> m/s and the truck follows to catch up with an accelaration of <b>") + accelaration.ToString("F2") + ("</b> m/s² the moment the rope passes by <b>") + PlayerPrefs.GetString("Name") + ("</b>, after how many seconds should <b>") + PlayerPrefs.GetString("Name") + ("</b> precisely grab the rope the moment the truck starts moving?")));
    }
    IEnumerator StuntResult()
    {
        if (playerGrabLineDistance < 56)
        {
            yield return new WaitForSeconds(6);
        }
        if (playerGrabLineDistance > 56)
        {
            yield return new WaitForSeconds(2);
        }
        velocity = 0;
        StartCoroutine(theSimulate.DirectorsCall());
        //theQuestion.ToggleModal();
        AccMidSimulation.simulate = false;
        timertxt.gameObject.SetActive(false);

    }
    IEnumerator jump()
    {
        readyToJump = false;
        if (playerGrabLineDistance > 55)
        {
            yield return new WaitForSeconds(2f);

            follow = true;

            playTimertxt.text = answer.ToString("F2") + "s";
            //viTtxt.text = ("v= ") + vf.ToString("F2") + ("m/s");
            velocity = 0;
            theTruck.moveSpeed = 0;
        }
        if (playerGrabLineDistance < 56)
        {
            thePlayer.toReach = true;

            yield return new WaitForSeconds(.5f);
            thePlayer.toReach = false;
            thePlayer.gameObject.SetActive(false);
            timeOn = false;

            if (answer == correctAnswer)
            {
                timertxt.color = new Color32(3, 63, 0, 255);
                hangingRagdoll.SetActive(true);

                playTimertxt.text = answer.ToString("F2") + "s";
            }
            thePlayer.standup = true;

            follow = true;
            timertxt.text = answer.ToString("F2") + "s";
            playTimertxt.text = answer.ToString("F2") + "s";
            //viTtxt.text = ("v= ") + vf.ToString("F2") + ("m/s");
            if (answer != correctAnswer)
            {
                
                timertxt.color = new Color32(188, 10, 0, 255);
                playerGrabline.SetActive(true);
                ropePoint.SetActive(true);
                ragdollPause.SetActive(true);
                ragdollPause.transform.position = new Vector2(playerGrabLineDistance + 2.67f, ragdollPrefab.transform.position.y);
                theSubChopper.gameObject.SetActive(true);
                theSubChopper.transform.position = new Vector2(distanceH, theSubChopper.transform.position.y);
                ragdollSpawn();
                yield return new WaitForSeconds(1.5f);
                theHeart.losinglife();
            }

        }

        //AccMidSimulation.simulate = false;

    }
    public void ragdollSpawn()
    {
        GameObject stick = Instantiate(ragdollPrefab);
        stick.transform.position = stickmanPoint.transform.position;
    }
    public IEnumerator errorMesage()
    {
        theQuestion.popupVisible = true;
        yield return new WaitForSeconds(3);
        theQuestion.popupVisible = false;
    }
    public void action()
    {
        //theQuestion.ToggleModal();
        if (theQuestion.answerIsCorrect == false)
        {
            theSimulate.retry();

        }
        else
        {
            theSimulate.next();
        }
    }
}
