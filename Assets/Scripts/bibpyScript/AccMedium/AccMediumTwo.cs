using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AccMediumTwo : MonoBehaviour
{
    public GameObject hangingRagdoll1, hangingRagdoll2, ropeTip1, ropeTip2, ragdollPrefab, stickmanPoint, windshield, driver, glass, glassPos, vanCollider;
    public GameObject carInitial, chopperInitial, carArrow, chopperArrow, ragdollPause,distanceMeter;
    public PlayerB thePlayer;
    public Hellicopter theChopper;
    public SubHellicopter theSubChopper;
    public SubSuv theSubSuv;
    public Suv theVan;
    private AccMidSimulation theSimulate;
    public DistanceMeter[] theMeter;
    public HeartManager theHeart;
    float A, B, C, D;
    float generateViH, Vih, generateAccH, accH, generateViV, Viv, generateAccV, accV, generateDistance, distance, checkDistance;
    float chopperCurrentPos, vanCurrentPos, kickpointTimeA, kickpointTimeB, timer, generatekickDistance, kickDistance, kickpointTimeC, playerKickDistance;
    float chopperAccPos, vanAccPos, vanTime, chopperDistance, correctAnswer, playerTime, generateplayerVanDistance, playerVanDistance;
    bool reposition = true;
    bool kickReady, follow;
    public QuestionControllerC theQuestion;
    string gender, pronoun;
    public TMP_Text vivTxt, vihTxt, accvTxt, acchTxt, actiontxt,timertxt;
    public TMP_Text debugAnswer;
    // Start is called before the first frame update
    void Start()
    {
        theHeart.startbgentrance();
        //theQuestion.stageNumber = 2;
        theSimulate = FindObjectOfType<AccMidSimulation>();
        gender = PlayerPrefs.GetString("Gender");
        if (gender == "Male")
        {
            pronoun = ("his");
        }
        if (gender == "Female")
        {
            pronoun = ("her");
        }
        thePlayer.gameObject.SetActive(true);
        theSimulate.stage = 2;
        generateProblem();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        debugAnswer.SetText($"Answer: {correctAnswer}");
        chopperCurrentPos = theChopper.transform.position.x;
        vanCurrentPos = theVan.transform.position.x;
        hangingRagdoll1.transform.position = ropeTip1.transform.position;
        playerTime = (-Vih + Mathf.Sqrt((Vih*Vih) + (4*((accH/2)* AccMidSimulation.playerAnswer)))) / accH;
        checkDistance = ((Vih * kickpointTimeA) + ((accH * (kickpointTimeA * kickpointTimeA)) / 2)) + ((Viv * kickpointTimeA) + ((accV * (kickpointTimeA * kickpointTimeA)) / 2));
        generatekickDistance = ((Vih * kickpointTimeA) + ((accH * (kickpointTimeA * kickpointTimeA)) / 2)) + chopperAccPos;
        correctAnswer = (float)System.Math.Round(generatekickDistance, 2) - chopperAccPos;
        kickDistance = (float)System.Math.Round(generatekickDistance, 2);
        playerKickDistance = AccMidSimulation.playerAnswer + chopperAccPos;
        generateplayerVanDistance = ((Viv * playerTime) + ((accV * (playerTime * playerTime)) / 2));
        playerVanDistance = (vanAccPos - generateplayerVanDistance) + chopperAccPos;
        if (follow)
        {
            carInitial.transform.position = theVan.transform.position;
            chopperInitial.transform.position = theChopper.transform.position;
            vihTxt.text = ("vi = ") + theChopper.flySpeed.ToString("F2") + (" m/s");
            accvTxt.text = ("a = ") + accV.ToString("F2") + (" m/s");
            vivTxt.text = ("vi = ") + (-theVan.moveSpeed).ToString("F2") + (" m/s²");
            acchTxt.text = ("a = ") + accH.ToString("F2") + (" m/s²");
        }


        if (theQuestion.isSimulating)
        {
            timertxt.text = timer.ToString("F2") + ("s");

            if (playerKickDistance == kickDistance)
            {
                if (chopperCurrentPos < playerKickDistance)
                {
                    hangingRagdoll2.transform.position =  new Vector2(ropeTip2.transform.position.x ,ropeTip2.transform.position.y-.7f);
                }
            }
            else
            {
                 hangingRagdoll2.transform.position =  new Vector2(ropeTip2.transform.position.x ,ropeTip2.transform.position.y-.7f);
            }
            if (reposition)
            {
                carInitial.transform.position = theVan.transform.position;
                chopperInitial.transform.position = theChopper.transform.position;
                vihTxt.text = ("v = ") + Vih.ToString("F2") + (" m/s");
                accvTxt.text = ("a = ") + ("0 m/s");
                vivTxt.text = ("v = ") + Viv.ToString("F2") + (" m/s²");
                acchTxt.text = ("a = ") + ("0 m/s²");
                theChopper.flySpeed = Vih;
                theVan.moveSpeed = -Viv;
                if (chopperCurrentPos >= chopperAccPos & vanCurrentPos <= vanAccPos)
                {
                    theChopper.accelaration = accH;
                    theVan.accelaration = accV;
                    theChopper.accelarating = true;
                    theVan.accelarating = true;
                    reposition = false;
                    theSubChopper.fade = true;
                    theSubSuv.fade = true;
                    carArrow.SetActive(false);
                    chopperArrow.SetActive(false);
                    follow = true;
                    hangingRagdoll1.SetActive(false);





                }
            }
            if (reposition == false)
            {
                timer += Time.fixedDeltaTime;
                distanceMeter.SetActive(true);
               

                if (playerKickDistance == kickDistance)
                {
                    actiontxt.text = "Next";
                    theQuestion.answerIsCorrect = true;
                    theMeter[0].distance = chopperCurrentPos;
                    //theQuestion.SetModalTitle("Stunt Success");
                    //theQuestion.SetModalText(PlayerPrefs.GetString("Name") + (" has successfully jumped into the van"));
                    if (chopperCurrentPos >= playerKickDistance - 1.2f)
                    {
                        timer = kickpointTimeA;
                        timertxt.text = timer.ToString("F2") + ("s");
                        if (kickReady)
                        {
                            
                            thePlayer.gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
                            StartCoroutine(kick());
                            theQuestion.isSimulating = false;

                        }
                    }


                }
                if (playerKickDistance > kickDistance)
                {
                    actiontxt.text = "Retry";
                    if (chopperCurrentPos < kickDistance)
                    {
                        theMeter[0].distance = chopperCurrentPos;
                    }
                    //theQuestion.SetModalTitle("Stunt Failed");
                    //theQuestion.SetModalText(PlayerPrefs.GetString("Name") + (" got hit by the van before he could kick the windshield and was unable to enter the van! The correct answer is <b>") + correctAnswer.ToString("F2") + (" meters</b>."));
                    if (chopperCurrentPos >= kickDistance)
                    {
                        timer = kickpointTimeA;
                        timertxt.text = timer.ToString("F2") + ("s");
                        theMeter[0].distance = kickDistance - chopperAccPos;
                        thePlayer.gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
                        thePlayer.ropeHang = false;
                        thePlayer.standup = true;
                        if (kickReady)
                        {
                            theHeart.ReduceLife();
                            StartCoroutine(StuntResult());
                            theQuestion.isSimulating = false;

                        }
                    }
                    vanCollider.SetActive(true);
                    windshield.SetActive(true);
                }
                if (playerKickDistance < kickDistance)
                {
                    actiontxt.text = "Retry";
                    if (chopperCurrentPos < playerKickDistance)
                    {
                        theMeter[0].distance = chopperCurrentPos;
                    }
                    //theQuestion.SetModalTitle("Stunt Failed");
                    //theQuestion.SetModalText(PlayerPrefs.GetString("Name") + (" kick before touching  the windshield and was unable to enter the van! The correct answer is <b>") + correctAnswer.ToString("F2") + (" meters</b>."));
                    vanCollider.SetActive(true);
                    windshield.SetActive(true);
                    if (chopperCurrentPos >= playerKickDistance - 2f)
                    {
                        timer = playerTime;
                        timertxt.text = timer.ToString("F2") + ("s");
                        theMeter[0].distance = playerKickDistance - chopperAccPos;
                        ragdollPause.SetActive(true);
                        ragdollPause.transform.position = new Vector2(playerKickDistance, thePlayer.transform.position.y);
                        theSubSuv.gameObject.SetActive(true);
                        theSubSuv.transform.position = new Vector2(playerVanDistance, theSubSuv.transform.position.y);
                        if (kickReady)
                        {
                            StartCoroutine(kick());
                        }
                        if (chopperCurrentPos >= kickDistance)
                        {
                            thePlayer.gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
                            thePlayer.ropeHang = false;
                            thePlayer.standup = true;
                            theHeart.ReduceLife();
                           theQuestion.isSimulating = false;
                        }
                    }
                }



            }

        }
    }
    public void generateProblem()
    {
        theHeart.losslife = false;
        vanCollider.SetActive(false);
        windshield.SetActive(false);
        thePlayer.gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        theSubChopper.gameObject.SetActive(true);
        theSubSuv.gameObject.SetActive(true);
        distanceMeter.SetActive(false);
        theChopper.accelarating = false;
        theChopper.flySpeed = 0;
        theVan.accelarating = false;
        theVan.moveSpeed = 0;
        follow = false;
        carArrow.SetActive(true);
        chopperArrow.SetActive(true);
        thePlayer.ropeHang = true;
        kickReady = true;
        generateViH = Random.Range(5f, 6f);
        Vih = (float)System.Math.Round(generateViH, 2);
        generateAccH = Random.Range(3f, 5f);
        accH = (float)System.Math.Round(generateAccH, 2);
        generateViV = Random.Range(7f, 8f);
        Viv = (float)System.Math.Round(generateViV, 2);
        generateAccV = Random.Range(3f, 4f);
        accV = (float)System.Math.Round(generateAccV, 2);
        generateDistance = Random.Range(38, 40);
        distance = (float)System.Math.Round(generateDistance, 2);
        B = ((accH + accV) / 2);
        A = -(Vih + Viv);
        C = -distance;
        D = Mathf.Sqrt((B * B) - (4 * A * C));
        kickpointTimeA = (A + Mathf.Sqrt((A * A) - (4 * B * C))) / (2 * B);
        kickpointTimeB = (B - Mathf.Sqrt((B * B) - (4 * A * C))) / (2 * A);
      
        vanTime = 30 / Viv;
        chopperDistance = Vih * vanTime;
        chopperAccPos = 20 - (distance / 2);
        vanAccPos = 20 + (distance / 2);
        theChopper.transform.position = new Vector2(chopperAccPos - chopperDistance, theChopper.transform.position.y);
        theVan.transform.position = new Vector2(20 + (distance / 2 + 30), theVan.transform.position.y);
        theSubChopper.transform.position = new Vector2(chopperAccPos, theSubChopper.transform.position.y);
        theSubSuv.transform.position = new Vector2(vanAccPos, theSubSuv.transform.position.y);
        theQuestion.SetQuestion((PlayerPrefs.GetString("Name") + (" is now instructed to hang from a helicopter and must need to take over an incoming van moving fast without a driver. To get in the van, ") + PlayerPrefs.GetString("Name") + (" must break its windshield by kicking it the exact moment ") + pronoun + (" feet touches it, If the initial velocity of helicopter is <b>") + Vih.ToString("F2") + ("</b> m/s and accelerating at <b>") + accH.ToString("F2") + ("</b> m/s², at what distance from the initial positon should ") + PlayerPrefs.GetString("Name") + (" do the kicking to successfully performed the stunts if the initial velocity of the incoming Van <b>") + Viv.ToString("F2") + ("</b> m/s and acelerating at <b>")+accV.ToString("F2")+("</b> m/s² when the van is <b>")+distance.ToString("F2")+("</b> meters away from ")+PlayerPrefs.GetString("Name")+("?")));
        hangingRagdoll2.transform.position = ropeTip2.transform.position;
        carInitial.transform.position = theSubSuv.transform.position;
        chopperInitial.transform.position = theSubChopper.transform.position;
        vihTxt.text = ("vi = ") + Vih.ToString("F2") + (" m/s");
        accvTxt.text = ("a = ") + accV.ToString("F2") + (" m/s");
        vivTxt.text = ("vi = ") + Viv.ToString("F2") + (" m/s²");
        acchTxt.text = ("a = ") + accH.ToString("F2") + (" m/s²");
        theMeter[0].positionX = chopperAccPos;
        theMeter[0].distance = 0f;
        theSubChopper.fade = false;
        ragdollPause.SetActive(false);
        reposition = true;
        theMeter[1].positionX = chopperAccPos;
        theMeter[1].distance = distance;
        hangingRagdoll1.SetActive(true);
        kickReady = true;
        timer = 0;



    }
    public IEnumerator errorMesage()
    {
        theQuestion.popupVisible = true;
        yield return new WaitForSeconds(3);
        theQuestion.popupVisible = false;
    }
    IEnumerator kick()
    {
        thePlayer.hangkick = true;
        kickReady = false;
        windshield.SetActive(true);
        StartCoroutine(StuntResult());
        if (playerKickDistance == kickDistance)
        {
            theMeter[0].distance = kickDistance - chopperAccPos;
            glassSpawn();
        }

        yield return new WaitForSeconds(.1f);

        thePlayer.hangkick = false;
        if (playerKickDistance == kickDistance)
        {
            thePlayer.gameObject.SetActive(false);
            driver.SetActive(true);
            thePlayer.ropeHang = false;
            thePlayer.transform.localScale = new Vector2(-thePlayer.transform.localScale.x, thePlayer.transform.localScale.y);
        }
    }
    IEnumerator StuntResult()
    {
        if (playerKickDistance > kickDistance)
        {
            kickReady = false;
        }
        yield return new WaitForSeconds(3);
        StartCoroutine(theSimulate.DirectorsCall());
        if (playerKickDistance == kickDistance)
        {
            theQuestion.ActivateResult((PlayerPrefs.GetString("Name") + (" has successfully performed the stunt and able to jumped into the van")),true, false);
        }
        if (playerKickDistance < kickDistance)
        {
            theQuestion.ActivateResult((PlayerPrefs.GetString("Name") + (" has failed to jump into the van ")),false, false);
        }
        /*if (playerKickDistance > kickDistance)
        {
            theQuestion.ActivateResult((PlayerPrefs.GetString("Name") + (" got hit by the van before he could kick the windshield and was unable to enter the van! The correct answer is <b>") + correctAnswer.ToString("F2") + (" meters</b>.")),false, false);
        }*/
        
        //theQuestion.ToggleModal();
        yield return new WaitForSeconds(2);
        theChopper.accelarating = false;
        theChopper.flySpeed = 0;
    }
    public void ragdollSpawn()
    {
        GameObject stick = Instantiate(ragdollPrefab);
        stick.transform.position = stickmanPoint.transform.position;
    }
    public void glassSpawn()
    {
        GameObject glassDebri = Instantiate(glass);
        glassDebri.transform.position = glassPos.transform.position;
    }
     public void action()
    {
        //theQuestion.ToggleModal();
        if(theQuestion.answerIsCorrect == false)
        {
            theSimulate.retry();
            
        }
        else
        {
            theSimulate.next();
        }
    }
}
