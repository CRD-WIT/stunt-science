using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StageThreeManager : MonoBehaviour
{
    // Start is called before the first frame update

    float height = 5.0f;
    float targetTime = 0f;
    string question;
    public float elapsed;
    public TMP_Text playerNameText, messageText, timerText, questionText, levelName;
    public GameObject AfterStuntMessage;
    public GameObject dimensionLine;
    Animator thePlayerAnimation;
    public GameObject playerOnRope;
    bool isSimulating = false;
    public GameObject playerHingeJoint;
    public GameObject thePlayer;
    public GameObject playerHangingFixed;
    public GameObject FirstCamera;
    public GameObject SecondCamera;
    Vector3 thePlayer_position;
    public GameObject accurateCollider;

    public GameObject platformBar;

    Vector3 playerOnRopeTransform;

    float timeProblem;

    void Start()
    {
        levelName.SetText("Free Fall | Stage 1");
        timeProblem = (float)System.Math.Round(UnityEngine.Random.Range(0.8f, 1.0f), 2);

        question = $"[name] is hanging from a rope and [pronoun] is instructed to let go of it, drop down, and hang again to the horizontal pole below. If [name] will let go ang grab the pole after exactly <color=purple>{timeProblem} sec</color>, at what <color=red>distance</color> should [pronoun] hands above the pole before letting go?";
        if (questionText != null)
        {
            questionText.SetText(question);
        }
        else
        {
            Debug.Log("QuestionText object not loaded.");
        }
        thePlayerAnimation = thePlayer.GetComponent<Animator>();

        if (dimensionLine != null)
        {
            dimensionLine.SetActive(true);
        }
        thePlayerAnimation.SetBool("isHanging", true);
        thePlayer_position = thePlayer.transform.position;
        playerOnRopeTransform = playerOnRope.transform.position;
    }

    public void StartSimulation()
    {
        isSimulating = true;
    }
    void FixedUpdate()
    {
        Vector2 spawnPointValue = transform.Find("Annotation1").GetComponent<Annotation>().SpawnPointValue();
        float distance = transform.Find("Annotation1").GetComponent<Annotation>().distance;
        platformBar.transform.position = new Vector3(spawnPointValue.x - 9, spawnPointValue.y, 1);
        float thePlayer_x = thePlayer_position.x;
        float thePlayer_y = thePlayer_position.y;


        if (isSimulating)
        {
            elapsed += Time.fixedDeltaTime;
            timerText.text = elapsed.ToString("f2") + "s";

            playerHingeJoint.GetComponent<HingeJoint2D>().enabled = false;
            thePlayerAnimation.SetBool("isFalling", true);
            if (accurateCollider.GetComponent<StageThreePlayerScript>().isCollided)
            {
                FirstCamera.SetActive(false);
                SecondCamera.SetActive(true);
               
                thePlayer.SetActive(false);
                playerHangingFixed.SetActive(true);
                playerHangingFixed.transform.position = new Vector3(spawnPointValue.x - 0.2f, platformBar.transform.position.y-1.5f, 1);
                platformBar.GetComponent<Animator>().SetBool("collided", true);
                playerHangingFixed.GetComponent<Animator>().SetBool("isHangingInBar", true);
                isSimulating = false;
            }
            // dimensionLine.SetActive(false);
        }
        else
        {
            timerText.text = elapsed.ToString("f2") + "s";
        }


    }
}
