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
    [SerializeField] GameObject directorsBubble, rope;
    public float distance, stuntTime, elapsed;
    string question, afterStuntMessage, errorMessage, playerName, pronoun, pNoun;
    float playerAnswer, playerSpeed, conveyorSpeed, animSpeed;
    bool isAnswered, directorIsCalling, isStartOfStunt;
    ConveyorManager conveyor;
    QuestionControllerVThree qc;
    IndicatorManagerV1_1 labels;

    // Start is called before the first frame update
    void Start()
    {
        qc = FindObjectOfType<QuestionControllerVThree>();
        myPlayer = FindObjectOfType<PlayerCM2>();
        conveyor = FindObjectOfType<ConveyorManager>();
        labels = FindObjectOfType<IndicatorManagerV1_1>();
        playerAnim = myPlayer.myAnimator;
        playerName = PlayerPrefs.GetString("Name");
        string playerGender = PlayerPrefs.GetString("Gender");
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
        labels.SetPlayerPosition(myPlayer.transform.position);
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
            if (elapsed >= stuntTime)
            {
                playerAnim.speed =1;
                elapsed = stuntTime;
                myPlayer.moveSpeed = -conveyorSpeed;
                playerAnim.SetBool("running", false);
                if(playerAnswer == playerSpeed){
                    playerAnim.speed = 1;
                    //correct
                }
            }
        }
        if (qc.isSimulating)
            Play();
    }
    void SetUp()
    {
        labels.heightSpawnPnt = new Vector2(-13.3f, -1.15f);
        labels.timeSpawnPnt = new Vector2(-10, .5f);
        labels.UnknownIs('v');
        qc.limit = 10.4f;
        while (true)
        {
            float aVelocity = (float)System.Math.Round(Random.Range(150f, 250f), 2);
            Debug.Log(aVelocity);
            stuntTime = (float)System.Math.Round(Random.Range(1.9f, 5f), 2);
            distance = Random.Range(15f, 20f);
            conveyorSpeed = conveyor.SetConveyorSpeed(aVelocity, stuntTime);
            playerSpeed = (distance / stuntTime) + conveyorSpeed;
            if (playerSpeed > 3 && playerSpeed < 10.4f)
                break;
        }
        rope.transform.position = new Vector2(distance - 10, rope.transform.position.y);
        playerAnim.speed = conveyorSpeed / 5.6f; // set to 1 before grabbing.
        labels.showLines(null, distance, 2.3f, playerSpeed, stuntTime);
        question = playerName + " is instructed to run on a moving conveyor belt and the rope is " + ConveyorManager.angularVelocity + "degrees per second, how fast should " + playerName + " run if " + pronoun + " is to grab the rope exactly after " + stuntTime.ToString("f2") + " seconds?";
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
