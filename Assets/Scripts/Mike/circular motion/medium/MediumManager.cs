using System.Collections;
using TMPro;
using UnityEngine;
using GameConfig;
public class MediumManager : MonoBehaviour
{
    PlayerCM2 myPlayer;
    Animator playerAnim;
    UnitOf whatIsAsk;
    [SerializeField] TMP_Text directorsSpeech;
    [SerializeField] GameObject directorsBubble, rope, AVelocityIndicator, ragdoll;
    public float distance, stuntTime, elapsed;
    string question, afterStuntMessage, errorMessage, playerName, pronoun, pNoun;
    float playerAnswer, playerSpeed, conveyorSpeed, animSpeed, timingD;
    bool isAnswered, directorIsCalling, isStartOfStunt;
    int stage;
    ConveyorManager conveyor;
    QuestionControllerVThree qc;
    IndicatorManagerV1_1 indicators;
    StageManager sm = new StageManager();

    // Start is called before the first frame update
    void Start()
    {
        qc = FindObjectOfType<QuestionControllerVThree>();
        myPlayer = FindObjectOfType<PlayerCM2>();
        conveyor = FindObjectOfType<ConveyorManager>();
        indicators = FindObjectOfType<IndicatorManagerV1_1>();
        playerAnim = myPlayer.myAnimator;
        playerName = PlayerPrefs.GetString("Name");
        string playerGender = PlayerPrefs.GetString("Gender");
        sm.SetGameLevel(5);
        qc.levelDifficulty = Difficulty.Medium;
        qc.stage = 1;
        if (playerGender == "Male")
        {
            pronoun = "he";
            pNoun = "his";
        }
        else
        {
            pronoun = "she";
            pNoun = "her";
        }
        playerAnim.SetBool("running", true);
        SetUp();
    }

    // Update is called once per frame
    void Update()
    {
        indicators.SetPlayerPosition(myPlayer.transform.position);
        if (directorIsCalling)
            StartCoroutine(DirectorsCall());
        Debug.Log(conveyorSpeed + "cs");
        Debug.Log(playerSpeed + "ps");
        qc.timer = elapsed.ToString("f2") + "s";
        if (isAnswered)
        {
            playerAnim.speed = playerAnswer / 5.6f; // set to 1 before grabbing.
            myPlayer.moveSpeed = playerAnswer - conveyorSpeed;
            elapsed += Time.deltaTime;
            timingD = myPlayer.transform.position.x + 10;
            if (elapsed >= stuntTime)
            {
                // isAnswered =false;
                playerAnim.speed = 1;
                elapsed = stuntTime;
                // myPlayer.moveSpeed = -conveyorSpeed;
                if (playerAnswer == playerSpeed)
                {
                    myPlayer.transform.position = new Vector2(distance - 10, myPlayer.transform.position.y);
                    myPlayer.moveSpeed = 0;
                    playerAnim.SetBool("running", false);
                    playerAnim.speed = 1;
                    //correct
                }
                else
                {
                    myPlayer.gameObject.SetActive(false);
                    ragdoll.transform.position = myPlayer.transform.position;
                    ragdoll.SetActive(true);
                    ragdoll.GetComponent<Rigidbody2D>().velocity = new Vector2(conveyorSpeed, 0);
                    if (playerAnswer > playerSpeed)
                    {
                        myPlayer.transform.position = new Vector2(myPlayer.transform.position.x +0.2f, myPlayer.transform.position.y);
                    }
                    else
                    {
                        myPlayer.transform.position = new Vector2(myPlayer.transform.position.x -0.2f, myPlayer.transform.position.y);
                    }
                }
            }
            indicators.IsRunning(playerAnswer, (playerAnswer - conveyorSpeed), elapsed, timingD);
        }
        indicators.SetPlayerPosition(myPlayer.transform.position);
        if (qc.isSimulating)
            Play();
    }
    void SetUp()
    {
        stage = qc.stage;
        AVelocityIndicator.GetComponent<UnityEngine.U2D.SpriteShapeRenderer>().color = qc.getHexColor(TextColorMode.Given);
        AVelocityIndicator.transform.Find("aVelocityIndicator").gameObject.GetComponent<SpriteRenderer>().color = qc.getHexColor(TextColorMode.Given);
        qc.SetColor(AVelocityIndicator.transform.Find("label").GetComponent<TMP_Text>(), TextColorMode.Given);
        indicators.heightSpawnPnt = new Vector2(-13.3f, -1.15f);
        indicators.timeSpawnPnt = new Vector2(-10, .5f);
        indicators.UnknownIs('v');
        qc.limit = 10.4f;
        while (true)
        {
            float aVelocity = (float)System.Math.Round(Random.Range(150f, 250f), 2);
            Debug.Log(aVelocity);
            stuntTime = (float)System.Math.Round(Random.Range(1.9f, 5f), 2);
            distance = Random.Range(15f, 20f);
            conveyorSpeed = conveyor.SetConveyorSpeed(aVelocity, stuntTime);
            playerSpeed = (float)System.Math.Round(((distance / stuntTime) + conveyorSpeed), 2);
            AVelocityIndicator.transform.Find("label").GetComponent<TMP_Text>().text = aVelocity.ToString("f2") + qc.Unit(UnitOf.angularVelocity);
            if (playerSpeed > 3 && playerSpeed < 10.4f)
                break;
        }
        rope.transform.position = new Vector2(distance - 10, rope.transform.position.y);
        playerAnim.speed = conveyorSpeed / 5.6f; // set to 1 before grabbing.
        indicators.showLines(null, distance, 2.3f, playerSpeed, stuntTime);
        question = playerName + " is instructed to run on a moving conveyor belt and the rope is<b> " + ConveyorManager.angularVelocity + " degrees per second</b>, how fast should " + playerName + " run if " + pronoun + " is to grab the rope exactly after <b>" + stuntTime.ToString("f2") + " seconds</b>?";
        qc.SetQuestion(question);
    }
    void Play()
    {
        qc.isSimulating = false;
        playerAnswer = qc.GetPlayerAnswer();
        directorIsCalling = true;
        isStartOfStunt = true;
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
        }
        else
        {
            directorsBubble.SetActive(true);
            directorsSpeech.text = "Cut!";
            yield return new WaitForSeconds(1f);
            directorsBubble.SetActive(false);
            directorsSpeech.text = "";
        }
    }
}
