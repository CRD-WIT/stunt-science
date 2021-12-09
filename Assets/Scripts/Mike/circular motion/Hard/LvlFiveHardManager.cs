using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameConfig;
using TMPro;

public class LvlFiveHardManager : MonoBehaviour
{
    UnitOf whatIsAsk;
    [SerializeField]float distance, mechaVelocity, aVelocity, radius, stuntTime, playerVelocity, correctAnswer, elapsed;
    float initialPlayerPos, playerAnswer;
    int stage;
    string playerName, playerGender, pronoun, pPronoun, question, message;
    bool directorIsCalling, isStartOfStunt, isAnswered, isAnswerCorrect, ragdoll = false, playerLanded;
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
    
    // Start is called before the first frame update
    void Start()
    {
        stage = 1;
        qc = FindObjectOfType<QuestionControllerVThree>();
        mm = FindObjectOfType<MechaManager>();
        indicators = FindObjectOfType<IndicatorManagerV1_1>();
        myPlayer = FindObjectOfType<PlayerCM2>();

        sm.SetGameLevel(5);
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
            mm.SetMechaVelocity(aVelocity, stuntTime);
            myPlayer.moveSpeed = playerVelocity;
            float playerPos = myPlayer.transform.position.x + initialPlayerPos;
            if(playerPos >= playerAnswer){
                Debug.Log(playerPos);
                // myPlayer.moveSpeed = 0;
                elapsed = stuntTime;
                StartCoroutine(Jump());
            }
            if(playerAnswer == correctAnswer){
                isAnswerCorrect =true;
                message = "correct";
            }
        }
        if(playerLanded){
            if(isAnswerCorrect){
                playerStopper.enabled =true;
                if(myPlayer.transform.position.x>main.transform.position.x)
                    main.transform.position = new Vector3(main.transform.position.x + 0.1f, main.transform.position.y,-10);
                else
                    main.transform.position = new Vector3(myPlayer.transform.position.x, main.transform.position.y,-10);
            }
        }
        if (qc.isSimulating)
            Play();
        
    }
    void SetUp(){
        playerLanded = false;
        radius = 1.05f;
        float pV, mV, cT, jT, d, t, av, jd;
        switch(stage){
            case 1:
                qc.limit=20;
                while(true){
                    pV = (float)System.Math.Round(Random.Range(8f, 10.49f),2);
                    d = (float)System.Math.Round(Random.Range(30f, 33f), 2);
                    t = (float)System.Math.Round(Random.Range(2.5f, 4f),2);
                    av = (float)System.Math.Round(Random.Range(-100, -150f),2);

                    mV = (float)System.Math.Round(mm.MechaVelocity(av*(-1), t), 2);
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
                break;
            case 3:
                break;
        }
        qc.SetQuestion(question);
    }
    void Play(){
        playerAnswer = qc.GetPlayerAnswer();
        qc.isSimulating = false;
        isStartOfStunt = true;
        directorIsCalling = true;
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
            if (stage != 3)
            {
                if (isAnswerCorrect)
                    myPlayer.happy = true;
                else
                {
                    if (ragdoll)
                    {
                        myPlayer.lost = false;
                        myPlayer.standup = false;
                        myPlayer.ToggleTrigger();
                    }
                    else
                    {
                        myPlayer.lost = true;
                        myPlayer.standup = true;
                    }
                }
            }
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
    }
}
