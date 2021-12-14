using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameConfig;
using TMPro;

public class LvlFiveHardManager : MonoBehaviour
{
    UnitOf whatIsAsk;
    [SerializeField]float distance, mechaVelocity, aVelocity, radius, stuntTime, playerVelocity, correctAnswer, elapsed;
    float initialPlayerPos, playerAnswer, camStartPos, mechaPos, adjustedAnswer, velocityY;
    int stage;
    string playerName, playerGender, pronoun, pPronoun, question, messageTxt;
    bool directorIsCalling, isStartOfStunt, isAnswered, isAnswerCorrect, ragdoll = false, playerLanded, isEndOfStunt;
    MechaManager mm;
    QuestionControllerVThree qc;
    IndicatorManagerV1_1 indicators;
    PlayerCM2 myPlayer;
    StageManager sm = new StageManager();
    [SerializeField] GameObject directorsBubble;
    [SerializeField] TMP_Text directorsSpeech;
    [SerializeField] AudioSource lightssfx, camerasfx, actionsfx, cutsfx;
    [SerializeField] HingeJoint2D playerStopper;
    [SerializeField] Camera main;
    Rigidbody2D player;
    
    // Start is called before the first frame update
    void Start()
    {
        stage = 1;
        qc = FindObjectOfType<QuestionControllerVThree>();
        mm = FindObjectOfType<MechaManager>();
        indicators = FindObjectOfType<IndicatorManagerV1_1>();
        myPlayer = FindObjectOfType<PlayerCM2>();
        player = myPlayer.GetComponent<Rigidbody2D>();

        camStartPos = main.transform.position.x;
        sm.SetGameLevel(4);
        playerName = PlayerPrefs.GetString("Name");
        playerGender = PlayerPrefs.GetString("Gender");
        if (playerGender == "Male")
        {
            pronoun = "he";
            pPronoun = "his";
        }
        else
        {
            pronoun = "she";
            pPronoun = "her";
        }
        qc.levelDifficulty = Difficulty.Medium;

        SetUp();
    }

    // Update is called once per frame
    void Update()
    {
        if (directorIsCalling)
            StartCoroutine(DirectorsCall());
        if(isAnswered){
            elapsed += Time.deltaTime;
            switch(stage){
                case 1:
                    mm.SetMechaVelocity(aVelocity, stuntTime, 1.05f);
                    myPlayer.moveSpeed = playerVelocity;
                    float playerPos = myPlayer.transform.position.x + initialPlayerPos;
                    if(playerPos >= playerAnswer){
                        Debug.Log(playerPos);
                        // myPlayer.moveSpeed = 0;
                        elapsed = stuntTime;
                        StartCoroutine(Jump());
                    }
                    if(adjustedAnswer == correctAnswer){
                        isAnswerCorrect =true;
                        messageTxt = "correct";
                    }
                    else{
                        isAnswerCorrect =false;
                        messageTxt = "wrong";
                    }
                break;
                case 2:
                break;
            }
            qc.timer = elapsed.ToString("f2") + "s";
        }
        if(playerLanded){
            float camDistanceFromRobot = mechaPos - camStartPos;
            main.transform.position = new Vector3(mm.transform.position.x - camDistanceFromRobot, main.transform.position.y,-10);
            if(isAnswerCorrect){
                playerStopper.enabled =true;
            }
            else{
                //ragdoll
            }
        }

        if (qc.isSimulating)
            Play();
        else{
            if(qc.nextStage)
                Next();
            else if(qc.retried)
                Reset();
            else{
                qc.nextStage = false;
                qc.retried = false;
            }
        }
        
    }
    void SetUp(){
        playerLanded = false;
        radius = 1.05f;
        float pV, mV, cT, jT, d, t, av, jd;
        switch(stage){
            case 1:
                qc.stage = 1;
                qc.limit=20;
                while(true){
                    pV = (float)System.Math.Round(Random.Range(8f, 10.49f),2);
                    d = (float)System.Math.Round(Random.Range(30f, 33f), 2);
                    t = (float)System.Math.Round(Random.Range(2.5f, 4f),2);
                    av = (float)System.Math.Round(Random.Range(-100, -150f),2);

                    mV = (float)System.Math.Round(mm.MechaVelocity(av*(-1), t, 1.05f), 2);
                    cT = d/(pV+mV);
                    jT = cT -0.5f;
                    jd = pV*jT;
                    if(jd<20)
                        break;
                }
                playerVelocity = pV;
                distance = d;
                stuntTime = t;
                aVelocity = av;
                mechaVelocity = mV;
                correctAnswer = (float)System.Math.Round(jd,2);

                myPlayer.transform.position = new Vector2(-distance+ 12, 0);
                initialPlayerPos = distance -12;
                Debug.Log(initialPlayerPos);
                
                whatIsAsk = UnitOf.distance;
                question = $"{playerName} is intructed to run and jump on top of the robot's wheel. The robot has an engine that has <b>{Mathf.Abs(aVelocity)}{qc.Unit(UnitOf.angularVelocity)}</b> revolution with gear that has a radius of <b>1.05 m</b> is moving towards {pPronoun} and {pronoun} is moving <b>{playerVelocity}{qc.Unit(UnitOf.velocity)}</b> towards the robot. If {playerName} needs to jump 0.5s before the collision, How far does {pronoun} have to run before jumping?";
                //player playerVelocity to jump exactly to the mecha
                break;
            case 2:
                float pVx, pVy, dx = 6.55f, dy = 1.75f;
                d = 7.78f;
                qc.limit = 10.49f;
                while(true){
                    t = (float)System.Math.Round(Random.Range(2.5f, 4f),2);
                    pV = d/t;
                    pVx = dx/t;
                    pVy = dy/t;
                    av = (float)System.Math.Round(Random.Range(-150, -200), 2);
                    if(pV<10.5f)
                        break;
                }
                stuntTime = t;
                aVelocity = av;
                mechaVelocity = mv;
                correctAnswer = (float)System.Math.Round(pV, 2);
                break;
            case 3:
                break;
        }
        qc.SetQuestion(question);
    }
    void Play(){
        playerAnswer = qc.GetPlayerAnswer();
        adjustedAnswer = qc.AnswerTolerance(correctAnswer);
        qc.isSimulating = false;
        isStartOfStunt = true;
        directorIsCalling = true;
    }
    void Reset(){
        SetUp();
    }
    void Next(){
        SetUp();
    }
    
    public IEnumerator DirectorsCall()
    {
        directorIsCalling = false;
        if (isStartOfStunt)
        {
            directorsBubble.SetActive(true);
            directorsSpeech.text = "Lights!";
            // lightssfx.Play();
            yield return new WaitForSeconds(0.75f);
            directorsSpeech.text = "Camera!";
            // camerasfx.Play();
            yield return new WaitForSeconds(0.50f);
            yield return new WaitForSeconds(0.25f);
            directorsSpeech.text = "Action!";
            // actionsfx.Play();
            yield return new WaitForSeconds(0.75f);
            directorsSpeech.text = "";
            directorsBubble.SetActive(false);
            isAnswered = true;
            isStartOfStunt = false;
        }
        else
        {
            // if (stage != 3)
            // {
            //     if (isAnswerCorrect)
            //         myPlayer.happy = true;
            //     else
            //     {
            //         if (ragdoll)
            //         {
            //             myPlayer.lost = false;
            //             myPlayer.standup = false;
            //             myPlayer.ToggleTrigger();
            //         }
            //         else
            //         {
            //             myPlayer.lost = true;
            //             myPlayer.standup = true;
            //         }
            //     }
            // }
            directorsBubble.SetActive(true);
            directorsSpeech.text = "Cut!";
            // cutsfx.Play();
            yield return new WaitForSeconds(1f);
            directorsBubble.SetActive(false);
            directorsSpeech.text = "";
        }
    }
    IEnumerator Jump(){
        myPlayer.jumpforce = 2;
        myPlayer.jump();
        yield return new WaitForSeconds(0.4f);
        myPlayer.moveSpeed = 0;
        myPlayer.jumpforce = 0;
        playerLanded = true;
        isAnswered = false;
        myPlayer.walking = true;
        mechaPos = mm.transform.position.x;
        StartCoroutine(StuntResult());
    }
    IEnumerator StuntResult()
    {
        isEndOfStunt = false;
        // yield return new WaitForSeconds(0.25f);
        directorIsCalling = true;
        isStartOfStunt = false;
        yield return new WaitForSeconds(1f);
        qc.ActivateResult(messageTxt, isAnswerCorrect);
    }
}

/*
Required    |   stage   |   radius
----------------------------------
distance    |   1       |   1.05
velocity    |   2       |   0.775
time        |   3       |   0.575
*/
