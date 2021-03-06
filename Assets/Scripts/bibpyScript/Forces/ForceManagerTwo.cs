using System.Collections;
using UnityEngine;
using TMPro;

public class ForceManagerTwo : MonoBehaviour
{
    public TMP_Text debugAnswer;
    AnswerGuards answerGuards = new AnswerGuards();
    public PlayerB thePlayer;
    public QuestionContForces theQuestion;
    public DirectorManager theDirector;
    private ForceSimulation theSimulate;
    //private BombManager theBomb;
    //public BombScript theBombScript;
    private ColliderManager theCollider;
    private HeartManager theHeart;
    float generateForce, Force, playerAccelaration, generateMass, mass, generateCorrectAnswer, currentPos;
    public float correctAnswer, playerAnswer, playerForce;
    public GameObject glassHolder, stickPrefab, stickmanpoint, bombHinge, glassDebri, bomb, director, speechBubble, playerInitials, navigator;
    public GameObject[] glassDebriLoc;
    public bool tooWeak, tooStrong, ragdollReady;
    public bool throwBomb;
    public TMP_Text masstxt, acctxt, breakingforcetxt, forcetxt, actiontxt,accPlayertxt;
    string gender, pronoun,pronoun2;
     public AudioSource glassBreak,thud;



    // Start is called before the first frame update
    void Start()
    {
        theDirector.platformIsOn = DirectorManager.To.Left;
        if (gender == "Male")
        {
            pronoun = ("he");
            pronoun2 = "him";
        }
        if (gender == "Female")
        {
            pronoun = ("she");
            pronoun2 = "her";
        }
        //theQuestion.stageNumber = 2;
        theCollider = FindObjectOfType<ColliderManager>();
        theSimulate = FindObjectOfType<ForceSimulation>();
        //theBomb = FindObjectOfType<BombManager>();
        theHeart = FindObjectOfType<HeartManager>();
        GenerateProblem();
        director.transform.position = new Vector2(-5.2f, 2.4f);
        //director.transform.localScale = new Vector2(director.transform.localScale.x, director.transform.localScale.y);
        //speechBubble.transform.localScale = new Vector2(speechBubble.transform.localScale.x, speechBubble.transform.localScale.y);
        thePlayer.exitDown = false;
        thePlayer.brake = false;
        thePlayer.transform.localScale = new Vector2(-thePlayer.transform.localScale.x, thePlayer.transform.localScale.y);
        navigator.transform.position = new Vector2(25.6f, navigator.transform.position.y);
        theSimulate.destroyZombies = false;
        thePlayer.myRigidbody.bodyType = RigidbodyType2D.Dynamic;
        thePlayer.myCollider.isTrigger = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        currentPos = thePlayer.transform.position.x;
        playerAnswer = ForceSimulation.playerAnswer;
        generateCorrectAnswer = Force / mass;
        correctAnswer = (float)System.Math.Round(generateCorrectAnswer, 2);
        playerForce = playerAnswer * mass;

        debugAnswer.SetText($"Answer: {System.Math.Round(correctAnswer, 2)}");

        if (ForceSimulation.simulate == true)
        {
            theSimulate.zombieChase = true;
            if (playerAnswer == correctAnswer)
            {
                forcetxt.text = ("f = ") + Force.ToString("F2") + ("N");
                forcetxt.color = new Color32(107, 0, 176, 255);
                accPlayertxt.color = new Color32(107, 0, 176, 255);
            }
            else
            {
                forcetxt.text = ("f = ") + playerForce.ToString("F2") + ("N");
                forcetxt.color = new Color32(188, 10, 0, 255);
                accPlayertxt.color = new Color32(188, 10, 0, 255);
            }
            forcetxt.gameObject.SetActive(true);
            forcetxt.gameObject.transform.position = new Vector2(thePlayer.transform.position.x + 4, thePlayer.transform.position.y);
            playerInitials.SetActive(false);
            thePlayer.moveSpeed -= playerAnswer * Time.fixedDeltaTime;
            accPlayertxt.gameObject.SetActive(true);
            accPlayertxt.text = "a = "+ playerAnswer.ToString("F2")+ " m/s??";
            if (theCollider.collide == true)
            {
                if (playerAnswer == correctAnswer)
                {
                    glassBreak.Play();
                    actiontxt.text = "Next";
                    theQuestion.answerIsCorrect = true;
                    glassHolder.SetActive(false);
                    //theQuestion.SetModalTitle("Stunt Success");
                    //theQuestion.SetModalText(PlayerPrefs.GetString("Name") + " has broken the glass and successfully thrown the bomb</color>");
                    if (currentPos <= 5.7f)
                    {
                        braking();
                        thePlayer.moveSpeed = 0;
                        thePlayer.transform.position = new Vector2(5.8f, -0.63f);
                        ForceSimulation.simulate = false;
                        StartCoroutine(collision());
                        StartCoroutine(StuntResult());


                    }
                }
                else
                {
                    if (playerAnswer < correctAnswer)
                    {
                        thud.Play();
                        actiontxt.text = "retry";
                        theQuestion.answerIsCorrect = false;
                        if (ragdollReady)
                        {
                            ragdollSpawn();
                            thePlayer.standup = true;
                        }
                        //theBombScript.inPlayer = false;
                        //theQuestion.SetModalTitle("Stunt Failed");
                        //theQuestion.SetModalText(PlayerPrefs.GetString("Name") + " accelerated too slow and unable to break the glass. The correct answer is "+ correctAnswer.ToString("F1") +"m/s??.");
                        tooWeak = true;
                        thePlayer.gameObject.SetActive(false);

                        thePlayer.moveSpeed = 0;
                        StartCoroutine(collision());
                        StartCoroutine(StuntResult());
                        theSimulate.playerDead = true;
                        theHeart.losinglife();
                        ForceSimulation.simulate = false;
                    }
                    if (playerAnswer > correctAnswer)
                    {
                        glassBreak.Play();
                        actiontxt.text = "retry";
                        theQuestion.answerIsCorrect = false;
                        if (ragdollReady)
                        {
                            ragdollSpawn();
                        }
                        //theQuestion.SetModalTitle("Stunt Failed");
                        //theQuestion.SetModalText(PlayerPrefs.GetString("Name") + " accelerated too fast and able to break the glass but also went through it. The correct answer is "+ correctAnswer.ToString("F1") +"m/s??.");
                        tooStrong = true;
                        thePlayer.gameObject.SetActive(false);
                        glassHolder.SetActive(false);
                        throwBomb = true;

                        StartCoroutine(collision());
                        StartCoroutine(StuntResult());
                        ForceSimulation.simulate = false;
                        theHeart.losinglife();
                        theSimulate.playerDead = true;
                    }
                }

            }
        }
    }
    public void GenerateProblem()
    {
        StartCoroutine(createZombies());
        //theBombScript.inPlayer = true;
        generateForce = Random.Range(300, 500);
        Force = (float)System.Math.Round(generateForce, 2);
        generateMass = Random.Range(70f, 75f);
        mass = (float)System.Math.Round(generateMass, 2);
        glassRespawn();
        ragdollReady = true;
        //theBomb.bomb.SetActive(true);
        theSimulate.glassHolder[1].SetActive(true);
        theSimulate.otherGlassHolder[1].SetActive(true);
        thePlayer.transform.position = new Vector2(31, -1f);
        //theBomb.bomb.transform.position = thePlayer.transform.position;
        theQuestion.SetQuestion(("<b>" + PlayerPrefs.GetString("Name") + ("</b> must break another glass wall to escape from the chasing hungry zombies. If  <b>") + PlayerPrefs.GetString("Name") + ("</b> has a mass of  <b>") + mass.ToString("F2") + ("</b> kg and the glass wall has an impact force breaking point of <b>") + Force.ToString("F2") + ("</b> Newtons, how fast should ") + pronoun + (" accelerate towards the glass wall just enough to break it and not go through it?")));
        masstxt.text = ("m = ") + mass.ToString("F2") + ("kg");
        acctxt.text = "a = ?";
        breakingforcetxt.text = ("Breaking Impact Force = ") + Force.ToString("F2") + ("N");
        playerInitials.SetActive(true);
        forcetxt.gameObject.SetActive(false);



    }
    public void ragdollSpawn()
    {
        GameObject stick = Instantiate(stickPrefab);
        stick.transform.position = stickmanpoint.transform.position;
        stick.transform.localScale = new Vector2(-stick.transform.localScale.x, stick.transform.localScale.y);
        stick.transform.rotation = Quaternion.Inverse(stick.transform.rotation);
        ragdollReady = false;
    }
    void braking()
    {
        thePlayer.brake = true;
        thePlayer.exitDown = true;
        thePlayer.myRigidbody.bodyType = RigidbodyType2D.Static;
        thePlayer.myCollider.isTrigger = true;


    }
    IEnumerator collision()
    {
        yield return new WaitForEndOfFrame();
        theCollider.collide = false;

    }
    IEnumerator StuntResult()
    {
        yield return new WaitForSeconds(1f);
        ForceSimulation.simulate = false;
         if(playerAnswer != correctAnswer)
        {
             yield return new WaitForSeconds(1);
             theSimulate.zombieChase = false;
        }
        if (playerAnswer == correctAnswer)
        {
            yield return new WaitForSeconds(4);
            //theQuestion.ActivateResult((PlayerPrefs.GetString("Name") + " run with precise acceleration just to break the glass window without overshooting. Stunt successfully executed"), true, false);
            theQuestion.ActivateResult($"{PlayerPrefs.GetString("Name")} ran with the precise acceleration just to break the glass window without overshooting. Stunt successfully executed!", true, false);
        }
        StartCoroutine(theSimulate.DirectorsCall());
        if (playerAnswer < correctAnswer)
        {
            yield return new WaitForSeconds(2);
            //theQuestion.ActivateResult(PlayerPrefs.GetString("Name")+(" ran into the glass window with little acceleration and unable to break it. The correct answer is <b>")+correctAnswer.ToString("F2")+("  m/s??</b>."), false, false);
            //theQuestion.ActivateResult($"The breaking point of the glass is requires too little force that {PlayerPrefs.GetString("Name")} ran into the glass window and unable to break it. Stunt failed! The correct answer is {correctAnswer.ToString("F2")}m/s??.", false, false);
            theQuestion.ActivateResult($"{PlayerPrefs.GetString("Name")} ran into the glass window with too little acceleration and unable to break it. Stunt failed! The correct answer is {correctAnswer} m/s??.", false, false);

        }
         if (playerAnswer > correctAnswer)
        {
            yield return new WaitForSeconds(2);
            //theQuestion.ActivateResult(PlayerPrefs.GetString("Name")+(" ran into the glass window with too much acceleration and overshoots. The correct answer is <b>")+correctAnswer.ToString("F2")+("  m/s??</b>."), false, false);           
            //theQuestion.ActivateResult($"{PlayerPrefs.GetString("Name")} ran into the glass window with too much acceleration and overshoots through it. Stunt failed! The correct answer is {correctAnswer.ToString("F2")} m/s2", false, false);
            theQuestion.ActivateResult($"{PlayerPrefs.GetString("Name")} ran into the glass window with too much acceleration and overshoots through it. Stunt failed! The correct answer is {correctAnswer} m/s??.", false, false);

        }
       
        //theQuestion.ToggleModal();
    }
    public void glassRespawn()
    {
        GameObject glass1 = Instantiate(glassDebri);
        glass1.transform.position = glassDebriLoc[0].transform.position;
        GameObject glass2 = Instantiate(glassDebri);
        glass2.transform.position = glassDebriLoc[1].transform.position;
        GameObject glass3 = Instantiate(glassDebri);
        glass3.transform.position = glassDebriLoc[2].transform.position;



    }
    public IEnumerator createZombies()
    {
        yield return new WaitForEndOfFrame();
        theSimulate.destroyZombies = false;
        GameObject zombie1 = Instantiate(theSimulate.zombiePrefab);
        zombie1.transform.position = new Vector2(40, -0.58f);
        zombie1.transform.localScale = new Vector2(-zombie1.transform.localScale.x, zombie1.transform.localScale.y);
        GameObject zombie2 = Instantiate(theSimulate.zombiePrefab);
        zombie2.transform.position = new Vector2(33f, -0.58f);
        zombie2.transform.localScale = new Vector2(-zombie2.transform.localScale.x, zombie2.transform.localScale.y);
        GameObject zombie3 = Instantiate(theSimulate.zombiePrefab);
        zombie3.transform.position = new Vector2(35f, -0.58f);
        zombie3.transform.localScale = new Vector2(-zombie3.transform.localScale.x, zombie3.transform.localScale.y);
        GameObject zombie4 = Instantiate(theSimulate.zombiePrefab);
        zombie4.transform.position = new Vector2(38.5f, -0.58f);
        zombie4.transform.localScale = new Vector2(-zombie4.transform.localScale.x, zombie4.transform.localScale.y);
    }

}
