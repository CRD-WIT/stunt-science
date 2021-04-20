using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Level_3_Stage_3 : MonoBehaviour
{
    // Start is called before the first frame update

    float height = 5.0f;
    float targetTime = 0f;
    string question;
    public float elapsed;
    public TMP_Text playerNameText, stuntMessageText, timerText, questionText, levelName;
    public GameObject AfterStuntMessage;
    Animator thePlayerAnimation;
    public TMP_InputField playerAnswer;
    bool isSimulating = false;
    public GameObject playerHingeJoint;
    public GameObject thePlayer;
    public GameObject playerHangingFixed;
    public GameObject FirstCamera;
    public GameObject SecondCamera;
    Vector3 thePlayer_position;
    public GameObject accurateCollider;

    public GameObject platformBarTop;

    Vector3 playerOnRopeTransform;

    float timeGiven;
    Vector2 gravityGiven;
    float correctAnswer;
    Vector2 spawnPointValue;
    float distance;
    float distanceGiven;
    void Start()
    {
        // Given        
        timeGiven = (float)System.Math.Round(UnityEngine.Random.Range(0.8f, 1.0f), 2);
        distanceGiven = (float)System.Math.Round(UnityEngine.Random.Range(5.0f, 5.55f), 2);
        gravityGiven = Physics2D.gravity;

        // Formula
        correctAnswer = Mathf.Sqrt(Mathf.Abs((2 * distanceGiven) / gravityGiven.y));

        transform.Find("Annotation1").GetComponent<Annotation>().SetDistance(distanceGiven);
        //transform.Find("Annotation1").GetComponent<Annotation>().SetSpawningPoint(new Vector2(15, playerOnRope.transform.Find("PlayerHingeJoint").transform.position.y - correctAnswer));

        //Debug.Log($"Hinge: {playerOnRope.transform.Find("PlayerHingeJoint").transform.position.y}");
        Debug.Log($"Distance Generated: {distanceGiven}");
        Debug.Log($"Correct Answer: {correctAnswer}");
        Debug.Log($"Correct Answer Rounded: {System.Math.Round(correctAnswer, 2)}");

        //Problem
        levelName.SetText("Free Fall | Stage 3");
        question = $"[name] is hanging on a horizontal pole and [pronoun] is instructed to let go of it, drop down, and hang again to another pole below. If the hands of [name] is exactly <color=red>{distanceGiven}</color> meters above the second pole, <color=purple>how long</color> should [name] fall down before [pronoun] grabs the second pole?";

        if (questionText != null)
        {
            questionText.SetText(question);
        }
        else
        {
            Debug.Log("QuestionText object not loaded.");
        }
        thePlayerAnimation = thePlayer.GetComponent<Animator>();
        thePlayerAnimation.SetBool("isHangingInBar", true);
        thePlayer_position = thePlayer.transform.position;

        distance = transform.Find("Annotation1").GetComponent<Annotation>().distance;

        // playerOnRopeTransform = playerOnRope.transform.position;

        spawnPointValue = transform.Find("Annotation1").GetComponent<Annotation>().SpawnPointValue();
        playerHingeJoint.transform.position = new Vector3(spawnPointValue.x - 1, distanceGiven, 0);
    
        platformBarTop.transform.position = new Vector3(spawnPointValue.x - 9, distanceGiven, 0);
    }

    IEnumerator StuntResult()
    {
        //messageFlag = false;
        yield return new WaitForSeconds(4f);
        AfterStuntMessage.SetActive(true);
    }

    public void StartSimulation()
    {
        isSimulating = true;
    }
    void FixedUpdate()
    {
        float thePlayer_x = thePlayer_position.x;
        float thePlayer_y = thePlayer_position.y;

        if (isSimulating)
        {
            if (playerAnswer.text.Length > 0)
            {
                transform.Find("Annotation1").GetComponent<Annotation>().Hide();
                elapsed += Time.fixedDeltaTime;
                timerText.text = elapsed.ToString("f2") + "s";

                playerHingeJoint.GetComponent<HingeJoint2D>().enabled = false;
                thePlayerAnimation.SetBool("isFalling", true);

                // Correct Answer
                if (System.Math.Round(float.Parse(playerAnswer.text), 2) == System.Math.Round(correctAnswer, 2))
                {
                    Debug.Log("Time is correct!");
                    if (accurateCollider.GetComponent<PlayerColliderEvent>().isCollided)
                    {
                        FirstCamera.SetActive(false);
                        SecondCamera.SetActive(true);

                        thePlayer.SetActive(false);
                        playerHangingFixed.SetActive(true);                       
                        playerHangingFixed.GetComponent<Animator>().SetBool("isHangingInBar", true);                        
                        elapsed-=0.01f;
                        isSimulating = false;
                        stuntMessageText.text = $"<b>Stunt Success!!!</b>\n\n{PlayerPrefs.GetString("Name")} safely grabbed the pole!";
                        StartCoroutine(StuntResult());
                    }                    
                }
                else
                {
                    if (float.Parse(playerAnswer.text) < System.Math.Round(correctAnswer, 2))
                    {
                        if (accurateCollider.GetComponent<PlayerColliderEvent>().isCollided)
                        {
                            Debug.Log("Distance is too short!");
                            isSimulating = false;
                            stuntMessageText.text = $"<b><color=red>Stunt Failed!!!</b>\n\n{PlayerPrefs.GetString("Name")} grabbed the pole too soon.</color>";
                            StartCoroutine(StuntResult());
                        }
                    }
                    else
                    {
                        if (accurateCollider.GetComponent<PlayerColliderEvent>().isCollided)
                        {
                            Debug.Log("Distance is too long!");
                            isSimulating = false;
                            stuntMessageText.text = $"<b><color=red>Stunt Failed!!!</b>\n\n{PlayerPrefs.GetString("Name")} grabbed the pole too late.</color>";
                            StartCoroutine(StuntResult());
                        }
                    }
                }
                 
            }
            else
            {
                Debug.Log("No value was added");
            }
        }
        else
        {
            //platformBarBottom.transform.position = new Vector3(spawnPointValue.x - 9, playerOnRope.transform.Find("PlayerHingeJoint").transform.position.y - distance, 1);            
            isSimulating = false;
            timerText.text = $"{(elapsed).ToString("f2")}s";
        }
    }
}
