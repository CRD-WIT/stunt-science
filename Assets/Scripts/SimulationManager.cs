using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SimulationManager : MonoBehaviour
{
    public GameObject directorsBubble, ragdollSpawn;
    public VelocityEasyStage1 VelocityEasyStage1;
    public StageTwoManager theManager2;
    public VelocityEasyStage3 StageThreeManager;
    public PlayerV1_1 thePlayer;
    public TMP_Text diretorsSpeech;
    public static float playerAnswer;
    public static bool isAnswered, isAnswerCorrect, directorIsCalling, isStartOfStunt, playerDead, isRagdollActive, stage3Flag;
    private HeartManager theHeart;
    QuestionControllerVThree qc;
    IndicatorManagerV1_1 dimLine;
     public AudioSource lightssfx, camerasfx, actionsfx, cutsfx;
    
    // Start is called before the first frame update
    void Start()
    {
        qc = FindObjectOfType<QuestionControllerVThree>();
        thePlayer = FindObjectOfType<PlayerV1_1>();
        theHeart = FindObjectOfType<HeartManager>();
        dimLine = FindObjectOfType<IndicatorManagerV1_1>();
        //destroyBoulders = FindObjectOfType<PrefabDestroyer>();
        //theHeart.life = PlayerPrefs.GetInt("life");=
        qc.stage = 1;
    }
    // Update is called once per frame
    public void FixedUpdate()
    {
        if (qc.isSimulating)
        {
            if (qc.stage == 3)
            {
                stage3Flag = true;
                playerAnswer = qc.GetPlayerAnswer();
                if (thePlayer.transform.position.x < (40 - playerAnswer))
                {
                    thePlayer.moveSpeed = 1.99f;
                }
                else
                {
                    thePlayer.moveSpeed = 0;
                    isStartOfStunt = true;
                    directorIsCalling = true;
                    stage3Flag = false;
                }
            }
            else
            {
                isStartOfStunt = true;
                directorIsCalling = true;
            }
        }
        //levelText.text = sm.GetGameLevel();
        //questionTextBox.SetText(question);

        if (directorIsCalling)
        {
            StartCoroutine(DirectorsCall());
        }
        else
        {
            directorIsCalling = false;
        }
        if (qc.nextStage)
            StartCoroutine(ExitStage());
        if (qc.retried)
            StartCoroutine(ReloadStage());
    }
    public IEnumerator DirectorsCall()
    {
        qc.isSimulating = false;
        directorIsCalling = false;
        if (isStartOfStunt)
        {
            directorsBubble.SetActive(true);
            lightssfx.Play();
            diretorsSpeech.text = "Lights!";
            yield return new WaitForSeconds(0.75f);
            camerasfx.Play();
            diretorsSpeech.text = "Camera!";
            yield return new WaitForSeconds(0.75f);
            actionsfx.Play();
            diretorsSpeech.text = "Action!";
            yield return new WaitForSeconds(0.75f);
            diretorsSpeech.text = "";
            directorsBubble.SetActive(false);
            isAnswered = true;
        }
        else
        {
            RumblingManager.shakeON = false;
            yield return new WaitForSeconds(1.75f);
            directorsBubble.SetActive(true);
            cutsfx.Play();
            diretorsSpeech.text = "Cut!";
            yield return new WaitForSeconds(1f);
            directorsBubble.SetActive(false);
            diretorsSpeech.text = "";
            if (isAnswerCorrect)
            {
                if (qc.stage == 3)
                    thePlayer.slide = true;
                else
                    thePlayer.happy = true;
            }
            else
            {
                thePlayer.standup = true;
            }
        }
    }
    IEnumerator ReloadStage()
    {
        // dimLine.showLines(null,null,null,0,0);
        qc.limit = 0;
        qc.retried = false;
        thePlayer.SetEmotion("");
        ragdollSpawn.SetActive(false);
        PrefabDestroyer.destroyPrefab = true;
        yield return new WaitForSeconds(3f);
        theHeart.startbgentrance();
        thePlayer.transform.position = new Vector2(0f, thePlayer.transform.position.y);
        thePlayer.moveSpeed = 0;
        playerAnswer = 0;
        RumblingManager.isCrumbling = false;
        if (qc.stage == 1)
        {
            theManager2.gameObject.SetActive(false);
            StageThreeManager.gameObject.SetActive(false);
            VelocityEasyStage1.gameObject.SetActive(true);
            VelocityEasyStage1.VelocityEasyStage1SetUp();
        }
        else if (qc.stage == 2)
        {
            VelocityEasyStage1.gameObject.SetActive(false);
            StageThreeManager.gameObject.SetActive(false);
            theManager2.gameObject.SetActive(true);
            theManager2.reset();
        }
        else
        {
            VelocityEasyStage1.gameObject.SetActive(false);
            theManager2.gameObject.SetActive(false);
            StageThreeManager.gameObject.SetActive(true);
            StageThreeManager.Stage3SetUp();
        }
    }
    IEnumerator ExitStage()
    {
        // dimLine.showLines(null,null,null,0,0);
        qc.limit = 0;
        qc.nextStage = false;
        VelocityEasyStage1.gameObject.SetActive(false);
        theManager2.gameObject.SetActive(false);
        thePlayer.SetEmotion("");
        ragdollSpawn.SetActive(false);
        PrefabDestroyer.destroyPrefab = true;
        thePlayer.moveSpeed = 5;
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(theHeart.endBGgone());
        yield return new WaitForSeconds(2.8f);
        theHeart.startbgentrance();
        thePlayer.transform.position = new Vector2(0f, thePlayer.transform.position.y);
        thePlayer.moveSpeed = 0;
        playerAnswer = 0;
        RumblingManager.isCrumbling = false;
        if (qc.stage == 2)
        {
            theManager2.gameObject.SetActive(true);
            theManager2.generateProblem();
        }
        if (qc.stage == 3)
        {
            StageThreeManager.gameObject.SetActive(true);
            StageThreeManager.Stage3SetUp();
        }
    }
    IEnumerator resetPrefab()
    {
        PrefabDestroyer.end = true;
        yield return new WaitForEndOfFrame();
        PrefabDestroyer.end = false;
    }
}
