using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ForceManagerThree : MonoBehaviour
{
    public Player thePlayer;
    public BombScript theBombScript;
    private ForceSimulation theSimulate;
    private ColliderManager theCollider;
    private BombManager theBomb;
    private HeartManager theHeart;
    private ScoreManager theScorer;
    float generateAccelaration, accelaration, playerAccelaration, generateForce, force, generateCorrectAnswer, currentPos;
    public float correctAnswer, playerAnswer,increaseMass, playerForce;
    public GameObject glassHolder, stickPrefab, stickmanpoint, afterStuntMessage, retry, next, glassDebri, cameraman, playerSpeech, napsack, wordedBoard, speechBubble, playerInitials;
    public GameObject[] glassDebriLoc;
    public bool tooWeak, tooStrong, ragdollReady, startAddingMass, crowdExit;
    public bool throwBomb, addingWeight, startRunning, goExit;
    public TMP_Text stuntMessageTxt, playerMass, thisWaytxt, acctxt, breakingforcetxt, forcetxt;
    int currentStar, currentLevel;
    string gender, pronoun1, pronoun2;




    // Start is called before the first frame update
    void Start()
    {
        //thePlayer = FindObjectOfType<Player>();
        theCollider = FindObjectOfType<ColliderManager>();
        theSimulate = FindObjectOfType<ForceSimulation>();
        theBomb = FindObjectOfType<BombManager>();
        theHeart = FindObjectOfType<HeartManager>();
        theScorer = FindObjectOfType<ScoreManager>();
        currentLevel = PlayerPrefs.GetInt("level");
        currentStar = PlayerPrefs.GetInt("FrstarE");
        
        wordedBoard.transform.position = new Vector2(8.9f, wordedBoard.transform.position.y);
        cameraman.transform.position = new Vector2(35, 5.5f);
        cameraman.transform.localScale = new Vector2(-cameraman.transform.localScale.x, cameraman.transform.localScale.y);
        thePlayer.transform.localScale = new Vector2(-thePlayer.transform.localScale.x, thePlayer.transform.localScale.y);
        speechBubble.transform.localScale = new Vector2(-speechBubble.transform.localScale.x, speechBubble.transform.localScale.y);
        gender = PlayerPrefs.GetString("Gender");
         if (gender == "Male")
        {
            pronoun1 = ("he");
            pronoun2 = ("his");
        }
        if (gender == "Female")
        {
            pronoun1 = ("she");
            pronoun2 = ("her");
        }
        GenerateProblem();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        napsack.SetActive(true);
        currentPos = thePlayer.transform.position.x;
        playerAnswer = ForceSimulation.playerAnswer;
        generateCorrectAnswer = force / accelaration - 70;
        correctAnswer = (float)System.Math.Round(generateCorrectAnswer, 2);
        playerForce = (playerAnswer + 70) * accelaration;
        
        if (addingWeight)
        {
            StartCoroutine(addingMass());
            addingWeight = false;
           
        }

        if (ForceSimulation.simulate == true)
        {
            thePlayer.moveSpeed = 3;
        }
        if (startAddingMass)
        {
            playerMass.text = increaseMass.ToString("F2") + ("kg");
            increaseMass += 50 * Time.fixedDeltaTime;
            if(increaseMass >= playerAnswer + 70)
            {
                startAddingMass = false;
                increaseMass = playerAnswer + 70;
                //playerMass.text = "<color=green>" + increaseMass.ToString("F2") + ("kg</color>");
            }
        }
        if (goExit)
        {
            if(currentPos >= 22.8f)
            {
                thePlayer.moveSpeed = 0;
                thePlayer.godown = true;
            }
        }


        if (startRunning)
        {
             if(playerAnswer == correctAnswer)
            {
                forcetxt.text = ("f = ")+ force.ToString("F2")+("N");
            }
            if(playerAnswer != correctAnswer) 
            {
                forcetxt.text = ("f = ")+ playerForce.ToString("F2")+("N");
            }
            forcetxt.gameObject.SetActive(true);
            forcetxt.gameObject.transform.position = new Vector2(thePlayer.transform.position.x - 4,thePlayer.transform.position.y);  
            playerInitials.SetActive(false);
            ForceSimulation.simulate = false;
            thePlayer.moveSpeed += accelaration * Time.fixedDeltaTime;
            if (theCollider.collide == true)
            {
                if (playerAnswer == correctAnswer)
                {
                    stuntMessageTxt.text = "<b><color=green>Your Answer is Correct!!!</b>\n\n" + PlayerPrefs.GetString("Name") + " has broken the glass</color>";
                    glassHolder.SetActive(false);
                    StartCoroutine(thiswaySpeech());

                    if (currentPos >= 22)
                    {
                        startRunning = false;
                        StartCoroutine(braking());
                        thePlayer.moveSpeed = 0;
                        thePlayer.transform.position = new Vector2(22, 2.5f);
                        StartCoroutine(collision());


                    }
                }
                if (playerAnswer < correctAnswer)
                {
                    stuntMessageTxt.text = "<b><color=red>Stunt Failed!!!</b>\n\n" + " the glass was too tough for </color>" + PlayerPrefs.GetString("Name") + ", and unable to break the glass. The correct answer is " + correctAnswer.ToString("F1") + "Newtons.";
                    tooWeak = true;
                    thePlayer.gameObject.SetActive(false);
                    if (ragdollReady)
                    {
                        ragdollSpawn();
                        thePlayer.standup = true;
                    }
                    thePlayer.moveSpeed = 0;
                    retry.SetActive(true);
                    StartCoroutine(collision());
                    StartCoroutine(StuntResult());
                    theSimulate.playerDead = true;
                    startRunning = false;
                    theHeart.losinglife();
                }
                if (playerAnswer > correctAnswer)
                {
                    stuntMessageTxt.text = "<b><color=red>Stunt Failed!!!</b>\n\n" + " the glass was too weak for </color>" + PlayerPrefs.GetString("Name") + ", able to break the glass but also went through it. The correct answer is " + correctAnswer.ToString("F1") + "Newtons.";
                    tooStrong = true;
                    thePlayer.gameObject.SetActive(false);
                    glassHolder.SetActive(false);
                    throwBomb = true;
                    thePlayer.moveSpeed = 0;
                    thePlayer.standup = true;
                    if (ragdollReady)
                    {
                        ragdollSpawn();
                    }
                    StartCoroutine(collision());
                    retry.SetActive(true);
                    StartCoroutine(StuntResult());
                    theSimulate.playerDead = true;
                    startRunning = false;
                    theHeart.losinglife();
                }
            }
        }

    }
    public void GenerateProblem()
    {
        increaseMass = 70;
        playerMass.text = increaseMass.ToString("F2") + ("kg");
        playerMass.gameObject.SetActive(true);
        theCollider.braker.SetActive(true);
        generateAccelaration = Random.Range(3f, 3.5f);
        accelaration = (float)System.Math.Round(generateAccelaration, 2);
        generateForce = Random.Range(250, 300);
        force = (float)System.Math.Round(generateForce, 2);
        theBomb.glassHolder[2].SetActive(true);
        theBomb.otherGlassHolder[0].SetActive(true);
        ragdollReady = true;
        theBomb.bomb.SetActive(true);
        theBomb.bomb.transform.position = thePlayer.transform.position;
        theBomb.followRagdoll = false;
        thePlayer.transform.position = new Vector2(0, 3.2f);
        theBombScript.gameObject.transform.position = new Vector2(7.8f, 1.5f);
        glassRespawn();
        ForceSimulation.question = ((PlayerPrefs.GetString("Name") + ("</b> cannot find the third bomb to throw out and decided to help the trapped persons inside the building get out instead by breaking the glass and letting them slide down the rope outside, If  <b>") + PlayerPrefs.GetString("Name") + ("</b> runs with the accelaration of  <b>") + accelaration.ToString("F2") + ("</b> m/s² and the glass breaks at an impact force of <b>") + force.ToString("F2") + ("</b> N, how much  mass ")+ pronoun1 + (" should add to ")+ pronoun2 + (" 70kg body in order to run into the glass and break it without overshooting?")));
        acctxt.text = ("a = ")+ accelaration.ToString("F2")+ ("m/s²");
        breakingforcetxt.text = ("Breaking Impact Force = ")+ force.ToString("F2");
        playerInitials.SetActive(true);
        forcetxt.gameObject.SetActive(false);
    



    }
    public void ragdollSpawn()
    {
        GameObject stick = Instantiate(stickPrefab);
        stick.transform.position = stickmanpoint.transform.position;
        ragdollReady = false;
    }
    IEnumerator braking()
    {
        thePlayer.brake = true;
        thePlayer.thisway = true;
        crowdExit = true;
        yield return new WaitForSeconds(1);
        //throwBomb = true;
        thePlayer.brake = false;
        theBombScript.inPlayer = false;
        yield return new WaitForSeconds(5);
        goExit = true;
        thePlayer.thisway = false;
        thePlayer.moveSpeed = 5;
        yield return new WaitForSeconds(1);
        goExit = false;
        thePlayer.godown = false;
        yield return new WaitForSeconds(1.2f);
        theBomb.explodebomb = true;
        thePlayer.moveSpeed = 5;
        yield return new WaitForSeconds(1.5f);
        thePlayer.moveSpeed = 0;
        thePlayer.happy = true;
        StartCoroutine(StuntResult());
    }
    IEnumerator collision()
    {
        yield return new WaitForEndOfFrame();
        theCollider.collide = false;

    }
    IEnumerator StuntResult()
    {
        ForceSimulation.simulate = false;
        yield return new WaitForSeconds(1);
        StartCoroutine(theSimulate.DirectorsCall());
        yield return new WaitForSeconds(3);
        afterStuntMessage.SetActive(true);
        if(playerAnswer == correctAnswer)
        {
            theScorer.finalstar();
            if(theHeart.life > currentStar)
            {
                PlayerPrefs.SetInt("FrstarE", theHeart.life);
            }
            if (currentLevel < 16)
            {
                PlayerPrefs.SetInt("level", currentLevel + 1);
            }
            //afterStuntMessage.SetActive(false);
        }

    }
    public void glassRespawn()
    {
        GameObject glass1 = Instantiate(glassDebri);
        glass1.transform.position = glassDebriLoc[0].transform.position;
        GameObject glass2 = Instantiate(glassDebri);
        glass2.transform.position = glassDebriLoc[1].transform.position;
        glass2.transform.rotation = glassDebriLoc[1].transform.rotation;
        GameObject glass3 = Instantiate(glassDebri);
        glass3.transform.position = glassDebriLoc[2].transform.position;
        glass3.transform.rotation = glassDebriLoc[2].transform.rotation;
        GameObject glass4 = Instantiate(glassDebri);
        glass4.transform.position = glassDebriLoc[3].transform.position;
        glass4.transform.rotation = glassDebriLoc[3].transform.rotation;



    }
    IEnumerator addingMass()
    {
        yield return new WaitForSeconds(1.5f);
        startAddingMass = true;
        yield return new WaitForSeconds(2f);
        thePlayer.addweights = false;
        yield return new WaitForSeconds(2);
        playerMass.gameObject.SetActive(false);
        startRunning = true;
    }
    IEnumerator thiswaySpeech()
    {
        yield return new WaitForSeconds(1.2f);
        playerSpeech.SetActive(true);
        thisWaytxt.text = ("this way!!!");
        yield return new WaitForSeconds(1);
        playerSpeech.SetActive(false);
        yield return new WaitForSeconds(1);
        playerSpeech.SetActive(true);
        yield return new WaitForSeconds(1);
        playerSpeech.SetActive(false);
        yield return new WaitForSeconds(1);
        playerSpeech.SetActive(true);
        yield return new WaitForSeconds(1);
        playerSpeech.SetActive(false);

    }

}
