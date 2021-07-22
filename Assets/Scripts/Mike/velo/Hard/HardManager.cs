using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using GameConfig;

public class HardManager : MonoBehaviour
{
    BossScript boss;
    PlayerV1_1 myPlayer;
    QuestionControllerVThree qc;
    public GameObject directorsBubble, bossHead;
    public TMP_Text directorsSpeech;
    float x, y, bossV, playerAnswer, stuntTime, elapsed, distance;
    bool isAnswered, isEndOfStunt, isStartOfStunt, directorIsCalling, isAnswerCorrect;
    string messageTxt;
    Rigidbody2D bossRB;
    // Start is called before the first frame update
    void Start()
    {
        boss = FindObjectOfType<BossScript>();
        myPlayer = FindObjectOfType<PlayerV1_1>();
        qc = FindObjectOfType<QuestionControllerVThree>();
        bossRB = bossHead.GetComponent<Rigidbody2D>();
        SetUp();
    }

    // Update is called once per frame
    void Update()
    {
        // boss.VelocityOfTheHead(x, y, -bossV);
        if(isAnswered){
            bossRB.constraints = RigidbodyConstraints2D.None;
            bossRB.constraints = RigidbodyConstraints2D.FreezeRotation;

            boss.SetVelocityOfTheHead(x, y, bossV);

            playerAnswer = qc.GetPlayerAnswer();
            qc.timer = elapsed.ToString("f2") + "s";
            elapsed += Time.deltaTime;
            if(elapsed >= stuntTime){
                boss.SetVelocityOfTheHead(x, y, 0);
            }
        }
        if (isEndOfStunt)
        {
            StartCoroutine(StuntResult());
        }
        if (qc.isSimulating)
            Play();
        else
        {
            if (qc.nextStage)
                StartCoroutine(Next());
            else if (qc.retried)
                StartCoroutine(Retry());
            else
            {
                qc.nextStage = false;
                qc.retried = false;
            }
        }
    }
    void SetUp()
    {
        x = Random.Range(3f, 10f);
        y = Random.Range(2f, 5f);
        bossV = -Random.Range(8f, 10f);
        stuntTime = boss.t;
        elapsed=0;
        distance= Mathf.Sqrt((x*x)+(y*y));
        boss.SetVelocityOfTheHead(x, y, 0);
        qc.limit=10;
    }
    void Play()
    {
        qc.isSimulating = false;
        isStartOfStunt = true;
        directorIsCalling = true;
        isAnswered =true;
    }
    IEnumerator Retry()
    {
        // Destroy(pShadow);
        // Destroy(b1Shadow);
        // Destroy(b2Shadow);
        // FallingBoulders.isRumbling = false;
        qc.retried = false;
        // PrefabDestroyer.end = true;
        // StartCoroutine(life.endBGgone());
        yield return new WaitForSeconds(3);
        // myPlayer.ToggleTrigger();
        // myPlayer.transform.position = new Vector2(0, boulder.transform.position.y);
        myPlayer.moveSpeed = 0;
        playerAnswer = 0;
        // RumblingManager.isCrumbling = false;
        SetUp();
    }
    IEnumerator Next()
    {
        // Destroy(pShadow);
        // Destroy(b1Shadow);
        // Destroy(b2Shadow);
        // FallingBoulders.isRumbling = false;
        qc.nextStage = false;
        myPlayer.happy = false;
        // PrefabDestroyer.end = true;
        yield return new WaitForSeconds(3f);
        // StartCoroutine(life.endBGgone());
        yield return new WaitForSeconds(2.8f);
        // myPlayer.transform.position = new Vector2(0, boulder.transform.position.y);
        myPlayer.moveSpeed = 0;
        playerAnswer = 0;
        // RumblingManager.isCrumbling = false;
        SetUp();
    }
    public IEnumerator DirectorsCall()
    {
        directorIsCalling = false;
        if (isStartOfStunt)
        {
            isStartOfStunt = false;
            directorsBubble.SetActive(true);
            directorsSpeech.text = "Lights!";
            yield return new WaitForSeconds(0.75f);
            directorsSpeech.text = "Camera!";
            yield return new WaitForSeconds(0.75f);
            directorsSpeech.text = "Action!";
            yield return new WaitForSeconds(0.75f);
            directorsSpeech.text = "";
            directorsBubble.SetActive(false);
            isAnswered = true;
            // FallingBoulders.isRumbling = true;
        }
        else
        {
            // yield return new WaitForSeconds((35 / playerVelocity) - stuntTime);
            directorsBubble.SetActive(true);
            directorsSpeech.text = "Cut!";
            // RumblingManager.isCrumbling = true;
            yield return new WaitForSeconds(1f);
            directorsBubble.SetActive(false);
            directorsSpeech.text = "";

            // boulderVelocity = 0;
        }
    }
    IEnumerator StuntResult()
    {
        isEndOfStunt = false;
        yield return new WaitForSeconds(1f);
        directorIsCalling = true;
        isStartOfStunt = false;
        // boulderRB.sharedMaterial.bounciness = 0;
        // boulder2RB.sharedMaterial.bounciness = 0;
        // boulderRB.sharedMaterial.friction = 0.8f;
        // boulder2RB.sharedMaterial.friction = 0.8f;
        // RumblingManager.shakeON = false;
        // RumblingManager.isCrumbling = true;
        yield return new WaitForSeconds(3);
        qc.ActivateResult(messageTxt, isAnswerCorrect);
    }
}
