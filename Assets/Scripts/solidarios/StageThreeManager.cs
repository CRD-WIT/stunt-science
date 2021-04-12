using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StageThreeManager : MonoBehaviour
{
    // Start is called before the first frame update
    string levelName = "Free Fall";
    float height = 5.0f;
    float targetTime = 0f;
    string question = $"[name] is hanging from a rope and [pronoun] is instructed to let go of it, drop down, and hang again to the horizontal pole below. If [name] will let go ang grab the pole after exactly [t] seconds, at what distance should [pronoun] hands above the pole before letting go?";
    public float elapsed;
    public TMP_Text playerNameText, messageText, timerText, questionText;
    public GameObject AfterStuntMessage;
    public GameObject dimensionLine;
    Animator thePlayerAnimation;
    bool isSimulating = false;
    public GameObject playerHingeJoint;
    public GameObject thePlayer;
    public GameObject playerHangingFixed;
    public GameObject FirstCamera;
    public GameObject SecondCamera;
    Vector3 thePlayer_position;
    public GameObject accurateCollider;

    public GameObject platformBar;

    Vector3 platformBar_position;

    void Start()
    {
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
        platformBar_position = platformBar.transform.position;
        thePlayerAnimation.SetBool("isHanging", true);
        thePlayer_position = thePlayer.transform.position;
    }

    public void StartSimulation()
    {
        isSimulating = true;
    }

    void FixedUpdate()
    {
        float thePlayer_x = thePlayer_position.x;
        float thePlayer_y = thePlayer_position.y;
        Vector3 playerHangingFixed_position = playerHangingFixed.transform.position;
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
                playerHangingFixed.transform.position = new Vector3(thePlayer_x, platformBar_position.y - 1.59f, 1);
                thePlayer.SetActive(false);
                playerHangingFixed.SetActive(true);
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
