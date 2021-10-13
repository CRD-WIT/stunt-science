using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AccMediumThree : MonoBehaviour
{
    public GameObject edge, hangingRagdoll, ropeTip, ragdollPrefab, stickmanPoint, playerPos, carInitials, chopperInitials, wordedBoard, edgeline, ropehere,carArrow, chopperArrow;
    public SubSuv theSubVan;
    public DistanceMeter theDistance;
    public SubHellicopter theSubChopper;
    private AccMidSimulation theSimulate;
    public Suv theSuv;
    public PlayerB thePlayer;
    public Hellicopter theChopper;
    public HeartManager theHeart;
    private ScoreManager theScorer;
    float correctAnswer, accH, accV, velocity, dv, dx, dh = 40, ropeDistance, distanceH, vanTime;
    float time, suvPos, chopperPos, generateDv, generateVelocity, generateAccH, generateCorrectAnswer, playerTime;
    bool repos, ragdollReady, follow, pausePos, resultReady;
    public TMP_Text viVtxt, viHtxt, aVtxt, aHtxt, actiontxt;
    string pronoun, gender;
    private Vector2 chopperStartPos, vanStartPos;
    private Quaternion vanstartRot;
    public QuestionControllerC theQuestion;
    public GameObject actionButton, navigator;
    int currentLevel;
    int currentStar;
    public TMP_Text debugAnswer;

    // Start is called before the first frame update
    void Start()
    {
        theScorer = FindObjectOfType<ScoreManager>();
        theHeart.startbgentrance();
        //theQuestion.stageNumber = 3;
        currentLevel = PlayerPrefs.GetInt("level");
        currentStar = PlayerPrefs.GetInt("AcstarM");
        //thePlayer.transform.localScale = new Vector2(-thePlayer.transform.localScale.x, thePlayer.transform.localScale.y);
        chopperStartPos = theChopper.transform.position;
        vanStartPos = theSuv.transform.position;
        vanstartRot = theSuv.transform.rotation;
        theSimulate = FindObjectOfType<AccMidSimulation>();
        theSimulate.stage = 3;
        wordedBoard.transform.position = new Vector2(wordedBoard.transform.position.x + 19, wordedBoard.transform.position.y);
        gender = PlayerPrefs.GetString("Gender");
        if (gender == "Male")
        {
            pronoun = ("him");
        }
        if (gender == "Female")
        {
            pronoun = ("her");
        }
         generateProblem();


    }

    // Update is called once per frame
    void Update()
    {
        debugAnswer.SetText($"Answer: {correctAnswer}");
        thePlayer.transform.position = playerPos.transform.position;
        thePlayer.myRigidbody.mass = 0;
        suvPos = theSuv.transform.position.x;
        chopperPos = theChopper.transform.position.x;
        accV = AccMidSimulation.playerAnswer;
        theDistance.distance = dv;
        if (theQuestion.isSimulating == true)
        {
            viHtxt.text = ("v = ") + (-theChopper.flySpeed).ToString("F2") + ("m/s");
            viVtxt.text = ("v = ") + (-theSuv.moveSpeed).ToString("F2") + ("m/s");
            aVtxt.text = ("a = ") + accV.ToString("F2") + ("m/s²");
            playerTime = (-velocity + Mathf.Sqrt((velocity * velocity) - (4 * (accV / 2) * (-dv)))) / (2 * (accV / 2));
            ropeDistance = (dh - ((velocity * playerTime) + ((accH * (playerTime * playerTime)) / 2))) + 15;
            edgeline.SetActive(false);



            if (follow)
            {
                carInitials.gameObject.transform.position = theSuv.transform.position;
                chopperInitials.gameObject.transform.position = theChopper.transform.position;
                carArrow.SetActive(false);
                chopperArrow.SetActive(false);
            }

            if (repos)
            {
                theChopper.flySpeed = -velocity;
                theSuv.moveSpeed = -velocity;
                if (suvPos <= theSubVan.transform.position.x)
                {
                    theChopper.deaccelaration = true;
                    theSuv.accelarating = true;
                    theChopper.accelaration = accH;
                    theSuv.accelaration = accV;
                    theSubVan.fade = true;
                    theSubChopper.fade = true;
                    repos = false;
                    follow = true;
                }
            }
            if (chopperPos <= 15)
            {
                theChopper.deaccelaration = false;
                theChopper.accelaration = accH * 3;
                theChopper.accelarating = true;
                theChopper.myRigidbody.constraints = RigidbodyConstraints2D.None;
                theChopper.flyUp = 2;
                if (theChopper.flySpeed >= 0)
                {
                    theChopper.accelarating = false;
                    theChopper.flySpeed = 0;
                    theChopper.flyUp = 0;
                    theChopper.myRigidbody.constraints = RigidbodyConstraints2D.FreezePositionY;
                }
            }
            if (suvPos <= 15)
            {
                viVtxt.text = ("v = 0m/s");
                follow = false;
                if(resultReady)
                {
                    StartCoroutine(StuntResult());
                }
                
                if (accV == correctAnswer)
                {
                    aVtxt.color = new Color32(107, 0, 176, 255);
                    //actiontxt.text = "Next";
                    theQuestion.answerIsCorrect = true;
                    //theQuestion.SetModalTitle("Stunt Success");
                    hangingRagdoll.SetActive(true);
                    hangingRagdoll.transform.position = ropeTip.transform.position;
                    //theQuestion.SetModalText(PlayerPrefs.GetString("Name") + " grabbed the rope before the van fell on the water");
                    //actionButton.SetActive(false);

                }


                //Time.timeScale = 0;
                theSuv.myCollider.enabled = false;
                thePlayer.gameObject.SetActive(false);
                theSuv.accelarating = false;
                theSuv.deaccelarating = true;
                theSuv.accelaration = accV * 3;
                if (accV != correctAnswer)
                {
                    theHeart.ReduceLife();
                    //theQuestion.SetModalTitle("Stunt Failed");
                    if (ropeDistance > 15)
                    {
                        theSubChopper.transform.position = new Vector2(ropeDistance, theSubChopper.transform.position.y);
                    }
                    if (ropeDistance < 15)
                    {
                        if (pausePos)
                        {
                            chopperPause();
                        }
                    }
                    theSubChopper.fade = false;
                    theSubChopper.gameObject.SetActive(true);
                    ropehere.SetActive(true);
                    if (ragdollReady)
                    {
                        ragdollSpawn();
                        ragdollReady = false;
                    }
                    if (accV > correctAnswer & accV < correctAnswer + .5f)
                    {
                        ropeDistance -= .2f;
                    }
                    if (accV < correctAnswer & accV > correctAnswer - .5f)
                    {
                        ropeDistance += .2f;
                    }
                    if(accV > correctAnswer)
                    {
                        //theQuestion.SetModalText(PlayerPrefs.GetString("Name") + " drove the van too fast and was already ahead when the helicopter passed the edge. The correct answer is </color>" + correctAnswer.ToString("F2") +"m/s².");
                    }
                     if(accV < correctAnswer)
                    {
                        //theQuestion.SetModalText(PlayerPrefs.GetString("Name") + " drove the van too slow and  was still behind when the helicopter passed the edge. The correct answer is </color>" + correctAnswer.ToString("F2") +"m/s².");
                    }

                }



                if (theSuv.moveSpeed >= 0)
                {
                    theSuv.moveSpeed = 0;
                }


            }
        }

    }
    public void generateProblem()
    {
        theHeart.losslife = false;
        carArrow.SetActive(true);
        chopperArrow.SetActive(true);
        theChopper.transform.position = chopperStartPos;
        theSuv.transform.position = vanStartPos;
        theSuv.transform.rotation = vanstartRot;
        thePlayer.gameObject.SetActive(true);
        theSuv.moveSpeed = 0;
        theSuv.deaccelarating = false;
        resultReady = true;
        pausePos = true;
        ropehere.SetActive(false);
        edgeline.SetActive(true);
        repos = true;
        ragdollReady = true;
        generateDv = Random.Range(22f, 25f);
        generateVelocity = Random.Range(5f, 7f);
        generateAccH = Random.Range(6f, 9f);
        accH = (float)System.Math.Round(generateAccH, 2);
        dv = (float)System.Math.Round(generateDv, 2);
        velocity = (float)System.Math.Round(generateVelocity, 2);
        dx = dh - dv;
        theSubVan.transform.position = new Vector2(edge.transform.position.x + dv, theSubVan.transform.position.y);
        theSubChopper.transform.position = new Vector2(theSubVan.transform.position.x + dx, theSubChopper.transform.position.y);
        theChopper.transform.position = new Vector2(theSuv.transform.position.x + dx, theChopper.transform.position.y);
        
        time = (-velocity + Mathf.Sqrt((velocity * velocity) - (4 * (accH / 2) * (-dh)))) / (2 * (accH / 2));
        generateCorrectAnswer = (2 * (dv - (velocity * time))) / (time * time);
        correctAnswer = (float)System.Math.Round(generateCorrectAnswer, 2);
        carInitials.transform.position = theSubVan.transform.position;
        chopperInitials.transform.position = theSubChopper.transform.position;
        viHtxt.text = ("vi = ") + velocity.ToString("F2") + ("m/s");
        viVtxt.text = ("vi = ") + velocity.ToString("F2") + ("m/s");
        aHtxt.text = ("a = ") + accH.ToString("F2") + ("m/s²");
        aVtxt.text = ("a = ?");
        aVtxt.color = new Color32(188, 10, 0, 255);
        theQuestion.SetQuestion(("<b>") + PlayerPrefs.GetString("Name") + ("</b> is instructed to drive the van off the ledge and hang on into the rope of the helicopter just before it drops, If the helicopter is flying at <b>") + velocity.ToString("F2") + ("</b> m/s while accelarating at  <b>") + accH.ToString("F2") + ("</b> m/s², what should be the accelaration of the van running at  <b>") + velocity.ToString("F2") + ("</b> m/s, so <b>") + PlayerPrefs.GetString("Name") + ("</b> can grab the rope at exactly at the edge of the ledge <b>") + dv.ToString("F2") + ("</b> meters in front of ") + pronoun + (" ?"));




    }
    IEnumerator hover()
    {
        yield return new WaitForSeconds(1);
        theChopper.deaccelaration = false;
        theChopper.accelaration = 10;
        theChopper.accelarating = true;

    }
    public void ragdollSpawn()
    {
        GameObject stick = Instantiate(ragdollPrefab);
        stick.transform.position = stickmanPoint.transform.position;
        stick.transform.localScale = new Vector2(-stick.transform.localScale.x, stick.transform.localScale.y);
    }
    void chopperPause()
    {
        theSubChopper.transform.position = theChopper.transform.position;
        pausePos = false;
    }
     IEnumerator StuntResult()
    {
        resultReady = false;
        yield return new WaitForSeconds(4);
        StartCoroutine(theSimulate.DirectorsCall());
        yield return new WaitForSeconds(1);
        if (accV == correctAnswer)
        {
            theQuestion.ActivateResult((PlayerPrefs.GetString("Name") + " grabbed the rope before the van fell on the water"),true, true);
        }
         if (accV < correctAnswer)
        {
            theQuestion.ActivateResult((PlayerPrefs.GetString("Name") + " drove the van too fast and was already ahead when the helicopter passed the edge. The correct answer is </color>" + correctAnswer.ToString("F2") +"m/s²."),false, false);
        }
         if (accV > correctAnswer)
        {
            theQuestion.ActivateResult((PlayerPrefs.GetString("Name") + " drove the van too fast and was already ahead when the helicopter passed the edge. The correct answer is </color>" + correctAnswer.ToString("F2") +"m/s²."),false, false);
        }
        
        //theQuestion.ToggleModal();
        /*if (accV == correctAnswer)
        {
            theScorer.finalstar();
            if (theHeart.life > currentStar)
            {
                PlayerPrefs.SetInt("AcstarM", theHeart.life);
            }
            if (currentLevel < 4)
            {
                PlayerPrefs.SetInt("level", currentLevel + 1);
            }
        }*/
       

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
