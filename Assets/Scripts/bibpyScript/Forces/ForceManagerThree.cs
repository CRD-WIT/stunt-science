using System.Collections;
using UnityEngine;
using TMPro;

public class ForceManagerThree : MonoBehaviour
{
    public TMP_Text debugAnswer;
    AnswerGuards answerGuards = new AnswerGuards();
    public PlayerB thePlayer;
    public QuestionContForces theQuestion;
    public BombScript theBombScript;
    private ForceSimulation theSimulate;
    private ColliderManager theCollider;
    private BombManager theBomb;
    private HeartManager theHeart;
    private ScoreManager theScorer;
    public DirectorManager theDirector;
    float generateAccelaration, accelaration, playerAccelaration, generateForce, force, generateCorrectAnswer, currentPos, totalMass;
    public float correctAnswer, playerAnswer, increaseMass, playerForce;
    public GameObject glassHolder, stickPrefab, stickmanpoint, glassDebri, cameraman, playerSpeech, napsack, speechBubble, playerInitials, action, navigator, zombiePrefab;
    public GameObject[] glassDebriLoc;
    public bool tooWeak, tooStrong, ragdollReady, startAddingMass, crowdExit;
    public bool throwBomb, addingWeight, startRunning, goExit;
    public TMP_Text playerMass, thisWaytxt, acctxt, breakingforcetxt, forcetxt, actiontxt;
    int currentStar, currentLevel;
    string gender, pronoun1, pronoun2;
    public AudioSource glassBreak,thud;

    // Start is called before the first frame update
    void Start()
    {
        theDirector.platformIsOn = DirectorManager.To.Right;
        //thePlayer = FindObjectOfType<Player>();
        //theQuestion.stageNumber = 3;
        theCollider = FindObjectOfType<ColliderManager>();
        theSimulate = FindObjectOfType<ForceSimulation>();
        theBomb = FindObjectOfType<BombManager>();
        theHeart = FindObjectOfType<HeartManager>();
        theScorer = FindObjectOfType<ScoreManager>();
        currentLevel = PlayerPrefs.GetInt("level");
        currentStar = PlayerPrefs.GetInt("FrstarE");

        navigator.transform.position = new Vector2(14.44f, navigator.transform.position.y);
        cameraman.transform.position = new Vector2(35, 5.5f);
        //cameraman.transform.localScale = new Vector2(-cameraman.transform.localScale.x, cameraman.transform.localScale.y);
        thePlayer.transform.localScale = new Vector2(-thePlayer.transform.localScale.x, thePlayer.transform.localScale.y);
        //speechBubble.transform.localScale = new Vector2(-speechBubble.transform.localScale.x, speechBubble.transform.localScale.y);
        thePlayer.exitDown = false;
        thePlayer.brake = false;
        theSimulate.destroyZombies = false;
        thePlayer.myRigidbody.bodyType = RigidbodyType2D.Dynamic;
        thePlayer.myCollider.isTrigger = false;
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
        debugAnswer.SetText($"Answer: {System.Math.Round(correctAnswer, 2)}");
        napsack.SetActive(true);
        currentPos = thePlayer.transform.position.x;
        playerAnswer = ForceSimulation.playerAnswer;
        generateCorrectAnswer = force / accelaration - 70;
        correctAnswer = (float)System.Math.Round(generateCorrectAnswer, 2);
        playerForce = (playerAnswer + 70) * accelaration;
        totalMass = playerAnswer + 70f;

        

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
            playerMass.text = ("m = ") + increaseMass.ToString("F2") + ("kg");
            increaseMass += 40 * Time.fixedDeltaTime;
            if (increaseMass >= totalMass)
            {
                increaseMass = totalMass;
                playerMass.text = ("m = ") + totalMass.ToString("F2") + ("kg");
                startAddingMass = false;
                //playerMass.text = "<color=green>" + increaseMass.ToString("F2") + ("kg</color>");
            }
        }
        if (goExit)
        {
            if (currentPos >= 22.8f)
            {
                thePlayer.moveSpeed = 0;
                thePlayer.godown = true;
            }
        }


        if (startRunning)
        {
            theSimulate.zombieChase = true;
            if (playerAnswer == correctAnswer)
            {
                forcetxt.text = ("f = ") + force.ToString("F2") + ("N");
                forcetxt.color = new Color32(107, 0, 176, 255);
            }
            else
            {
                forcetxt.text = ("f = ") + playerForce.ToString("F2") + ("N");
                forcetxt.color = new Color32(188, 10, 0, 255);
            }
            forcetxt.gameObject.SetActive(true);
            forcetxt.gameObject.transform.position = new Vector2(thePlayer.transform.position.x - 4, thePlayer.transform.position.y);
            playerInitials.SetActive(false);
            ForceSimulation.simulate = false;
            thePlayer.moveSpeed += accelaration * Time.fixedDeltaTime;
            if (theCollider.collide == true)
            {
                if (playerAnswer == correctAnswer)
                {
                    glassBreak.Play();
                    theQuestion.answerIsCorrect = true;
                    //action.SetActive(false);
                    //theQuestion.SetModalText(PlayerPrefs.GetString("Name") + " has broken the glass and succesfully escaped from the explosion </color>");
                    glassHolder.SetActive(false);
                    //StartCoroutine(thiswaySpeech());

                    if (currentPos >= 22)
                    {
                        startRunning = false;
                        StartCoroutine(braking());
                        thePlayer.moveSpeed = 0;
                        thePlayer.transform.position = new Vector2(22, 2.5f);
                        StartCoroutine(collision());


                    }
                }
                else
                {
                    if (playerAnswer < correctAnswer)
                    {
                        thud.Play();
                        actiontxt.text = "retry";
                        theQuestion.answerIsCorrect = false;
                        //theQuestion.SetModalText(PlayerPrefs.GetString("Name") + ", and unable to break the glass. The correct answer is " + correctAnswer.ToString("F1") + "Newtons.");
                        tooWeak = true;
                        thePlayer.gameObject.SetActive(false);
                        if (ragdollReady)
                        {
                            ragdollSpawn();
                            thePlayer.standup = true;
                        }
                        thePlayer.moveSpeed = 0;
                        StartCoroutine(collision());
                        StartCoroutine(StuntResult());
                        theSimulate.playerDead = true;
                        startRunning = false;
                        theHeart.losinglife();
                    }
                    if (playerAnswer > correctAnswer)
                    {
                         glassBreak.Play();
                        actiontxt.text = "retry";
                        theQuestion.answerIsCorrect = false;
                        //theQuestion.SetModalText(PlayerPrefs.GetString("Name") + ", able to break the glass but also went through it. The correct answer is " + correctAnswer.ToString("F1") + "Newtons.");
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
                        StartCoroutine(StuntResult());
                        theSimulate.playerDead = true;
                        startRunning = false;
                        theHeart.losinglife();
                    }
                }
            }
        }

    }
    public void GenerateProblem()
    {
        StartCoroutine(createZombies());
        increaseMass = 70;
        playerMass.text = ("m = ") + increaseMass.ToString("F2") + ("kg");
        playerMass.gameObject.SetActive(true);
        theCollider.braker.SetActive(true);
        generateAccelaration = Random.Range(3f, 3.5f);
        accelaration = (float)System.Math.Round(generateAccelaration, 2);
        generateForce = Random.Range(300, 500);
        force = (float)System.Math.Round(generateForce, 2);
        theSimulate.glassHolder[2].SetActive(true);
        theSimulate.otherGlassHolder[0].SetActive(true);
        ragdollReady = true;
        //theBomb.bomb.SetActive(true);
        //theBomb.bomb.transform.position = thePlayer.transform.position;
        //theBomb.followRagdoll = false;
        thePlayer.transform.position = new Vector2(0, 2.59f);
        //theBombScript.gameObject.transform.position = new Vector2(7.8f, 1.5f);
        glassRespawn();
        theQuestion.SetQuestion(("<b>" + PlayerPrefs.GetString("Name") + ("</b> still chased by these hungry zombies and need to break another glass wall to escape from them, If  <b>") + PlayerPrefs.GetString("Name") + ("</b> runs with the accelaration of  <b>") + accelaration.ToString("F2") + ("</b> m/s?? and the glass breaks at an impact force of <b>") + force.ToString("F2") + ("</b> N, how much  mass ") + pronoun1 + (" should add to ") + pronoun2 + (" 70kg body in order to run into the glass and break it without overshooting?")));
        acctxt.text = ("a = ") + accelaration.ToString("F2") + ("m/s??");
        breakingforcetxt.text = ("Breaking Impact Force = ") + force.ToString("F2");
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
        //thePlayer.thisway = true;
        //crowdExit = true;
        yield return new WaitForSeconds(.5f);
        //throwBomb = true;
        thePlayer.brake = false;
        //theBombScript.inPlayer = false;
        thePlayer.toJump = true;
        yield return new WaitForSeconds(.7f);
        thePlayer.transform.position = new Vector2(23.2f, thePlayer.transform.position.y + 1.3f);
        thePlayer.climb = true;
        //thePlayer.transform.localScale = new Vector2(-thePlayer.transform.localScale.x, thePlayer.transform.localScale.y);
        thePlayer.moveSpeedY = 1.5f;
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
        if(playerAnswer != correctAnswer)
        {
             yield return new WaitForSeconds(1);
             theSimulate.zombieChase = false;
        }
        if (playerAnswer == correctAnswer)
        {
            yield return new WaitForSeconds(4);
            //theQuestion.ActivateResult((PlayerPrefs.GetString("Name") + " run with precise mass need to break the glass window without overshooting. Stunt succesfully executed"), true, true);
            theQuestion.ActivateResult($"{PlayerPrefs.GetString("Name")} ran into the glass window with the precise mass needed to break it without overshooting. Stunt successfully executed!", true, true);
        }
        StartCoroutine(theSimulate.DirectorsCall());
        if (playerAnswer < correctAnswer)
        {
            yield return new WaitForSeconds(2);
            
            //theQuestion.ActivateResult(PlayerPrefs.GetString("Name")+" ran into the glass window carrying too little mass with "+pronoun2+" and unable to break it. The correct answer is <b>"+correctAnswer.ToString("F2")+("  kg</b>."), false, false);
            theQuestion.ActivateResult($"{PlayerPrefs.GetString("Name")} ran into the glass window carrying too little mass with {pronoun2} and unable to break it. Stunt failed! The correct answer is {correctAnswer.ToString("F2")}kgs", false, false);

        }
         if (playerAnswer > correctAnswer)
        {
            yield return new WaitForSeconds(2);
           
            //theQuestion.ActivateResult(PlayerPrefs.GetString("Name")+" ran into the glass window carrying too much mass with "+pronoun2+" and overshoots through it. The correct answer is <b>"+correctAnswer.ToString("F2")+("  kg</b>."), false, false);
            theQuestion.ActivateResult($"{PlayerPrefs.GetString("Name")} ran into the glass window carrying too much mass with {pronoun2} and overshoots through it. The correct answer is {correctAnswer.ToString("F2")}kgs", false, false);

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
    /*IEnumerator thiswaySpeech()
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

    }*/
    public IEnumerator createZombies()
    {
        yield return new WaitForEndOfFrame();
        theSimulate.destroyZombies = false;
        GameObject zombie1 = Instantiate(theSimulate.zombiePrefab);
        zombie1.transform.position = new Vector2(-9, 3.86f);
        GameObject zombie2 = Instantiate(theSimulate.zombiePrefab);
        zombie2.transform.position = new Vector2(-11.5f, 3.86f);
        GameObject zombie3 = Instantiate(theSimulate.zombiePrefab);
        zombie3.transform.position = new Vector2(-13.6f, 3.86f);
        GameObject zombie4 = Instantiate(theSimulate.zombiePrefab);
        zombie4.transform.position = new Vector2(-15, 3.86f);
    }

}
